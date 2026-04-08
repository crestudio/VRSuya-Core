#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor.Animations;

/*
 * VRSuya Core
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 */

namespace VRSuya.Core {

	[ExecuteInEditMode]
	public class Animator {

		public void AddParameter(AnimatorController TargetController, AnimatorControllerParameter TargetParameter) {
			AnimatorControllerParameter newParameter = new AnimatorControllerParameter {
				defaultBool = TargetParameter.defaultBool,
				defaultFloat = TargetParameter.defaultFloat,
				defaultInt = TargetParameter.defaultInt,
				name = TargetParameter.name,
				type = TargetParameter.type
			};
			TargetController.AddParameter(newParameter);
		}

		public AnimatorControllerLayer[] DuplicateAnimatorLayers(AnimatorController TargetController, AnimatorControllerLayer[] TargetLayers) {
			AnimatorControllerLayer[] newAnimatorLayers = new AnimatorControllerLayer[TargetController.layers.Length + TargetLayers.Length];
			Array.Copy(TargetController.layers, newAnimatorLayers, TargetController.layers.Length);
			for (int Index = 0; Index < TargetLayers.Length; Index++) {
				AnimatorControllerLayer newAnimatorLayer = DuplicateAnimatorLayer(TargetLayers[Index]);
				newAnimatorLayers[TargetController.layers.Length + Index] = newAnimatorLayer;
			}
			return newAnimatorLayers;
		}

		public AnimatorControllerLayer DuplicateAnimatorLayer(AnimatorControllerLayer TargetAnimatorLayer) {
			AnimatorControllerLayer newAnimatorLayer = new AnimatorControllerLayer {
				avatarMask = TargetAnimatorLayer.avatarMask,
				blendingMode = TargetAnimatorLayer.blendingMode,
				defaultWeight = TargetAnimatorLayer.defaultWeight,
				iKPass = TargetAnimatorLayer.iKPass,
				name = TargetAnimatorLayer.name,
				stateMachine = DuplicateStateMachine(TargetAnimatorLayer.stateMachine),
				syncedLayerAffectsTiming = TargetAnimatorLayer.syncedLayerAffectsTiming,
				syncedLayerIndex = TargetAnimatorLayer.syncedLayerIndex
			};
			return newAnimatorLayer;
		}

		public AnimatorStateMachine DuplicateStateMachine(AnimatorStateMachine TargetStateMachine) {
			AnimatorStateMachine newStateMachines = new AnimatorStateMachine {
				anyStatePosition = TargetStateMachine.anyStatePosition,
				behaviours = TargetStateMachine.behaviours,
				entryPosition = TargetStateMachine.entryPosition,
				exitPosition = TargetStateMachine.exitPosition,
				parentStateMachinePosition = TargetStateMachine.parentStateMachinePosition,
				stateMachines = DuplicateChildStateMachine(TargetStateMachine.stateMachines),
				states = DuplicateChildAnimatorState(TargetStateMachine.states),
				hideFlags = TargetStateMachine.hideFlags,
				name = TargetStateMachine.name
			};
			CopyTransitions(newStateMachines, TargetStateMachine);
			return newStateMachines;
		}

		public void CopyTransitions(AnimatorStateMachine TargetStateMachine, AnimatorStateMachine OldStateMachine) {
			AnimatorStateMachine[] OldAnimatorStateMachines = GetAllStateMachines(OldStateMachine);
			AnimatorStateMachine[] NewAnimatorStateMachines = GetAllStateMachines(TargetStateMachine);
			AnimatorState[] OldAnimatorStates = GetAllStates(OldStateMachine);
			AnimatorState[] NewAnimatorStates = GetAllStates(TargetStateMachine);
			foreach (AnimatorState TargetState in NewAnimatorStates) {
				AnimatorState ExistState = Array.Find(OldAnimatorStates, AnimatorState => AnimatorState.name == TargetState.name);
				AnimatorStateTransition[] OldStateTransitions = ExistState.transitions;
				if (OldStateTransitions.Length > 0) {
					AnimatorStateTransition[] newTransitions = new AnimatorStateTransition[OldStateTransitions.Length];
					for (int Index = 0; Index < OldStateTransitions.Length; Index++) {
						if (OldStateTransitions[Index].destinationState != null) {
							AnimatorState newDestinationState = Array.Find(NewAnimatorStates, AnimatorState => AnimatorState.name == OldStateTransitions[Index].destinationState.name);
							newTransitions[Index] = DuplicateTransition(OldStateTransitions[Index], newDestinationState, null);
						} else if (OldStateTransitions[Index].destinationStateMachine != null) {
							AnimatorStateMachine newDestinationExistStateMachine = Array.Find(NewAnimatorStateMachines, ExistStateMachine => ExistStateMachine.name == OldStateTransitions[Index].destinationStateMachine.name);
							newTransitions[Index] = DuplicateTransition(OldStateTransitions[Index], null, newDestinationExistStateMachine);
						}
					}
					TargetState.transitions = newTransitions;
				}
			}
		}

		public AnimatorStateTransition[] GetAllTransitions(AnimatorController TargetAnimatorController) {
			AnimatorState[] AllAnimatorState = GetAllAnimatorStates(TargetAnimatorController);
			return AllAnimatorState.SelectMany(Item => Item.transitions).ToArray();
		}

		public AnimatorState[] GetAllAnimatorStates(AnimatorController TargetAnimatorController) {
			List<AnimatorStateMachine> AllAnimatorStateMachine = new List<AnimatorStateMachine>();
			List<AnimatorState> AllAnimatorState = new List<AnimatorState>();
			foreach (AnimatorControllerLayer TargetAnimatorLayer in TargetAnimatorController.layers) {
				AllAnimatorStateMachine.AddRange(GetAllStateMachines(TargetAnimatorLayer.stateMachine).ToList());
			}
			foreach (AnimatorStateMachine TargetAnimatorStateMachine in AllAnimatorStateMachine) {
				AllAnimatorState.AddRange(GetAllStates(TargetAnimatorStateMachine).ToList());
			}
			return AllAnimatorState.ToArray();
		}

		public BlendTree[] GetAllBlendTrees(AnimatorState[] TargetAnimatorStates) {
			BlendTree[] BlendTrees = new BlendTree[0];
			foreach (AnimatorState TargetAnimatorState in TargetAnimatorStates) {
				if (TargetAnimatorState.motion is BlendTree TargetBlendTree) {
					BlendTrees = BlendTrees.Concat(GetSubBlendTrees(TargetBlendTree)).ToArray();
				}
			}
			return BlendTrees;
		}

		BlendTree[] GetSubBlendTrees(BlendTree TargetBlendTree) {
			BlendTree[] BlendTrees = new BlendTree[0];
			BlendTrees = BlendTrees.Concat(new BlendTree[] { TargetBlendTree }).ToArray();
			foreach (ChildMotion ChildMotion in TargetBlendTree.children) {
				if (ChildMotion.motion is BlendTree ChildBlendTree) {
					BlendTrees = BlendTrees.Concat(GetSubBlendTrees(ChildBlendTree)).ToArray();
				}
			}
			return BlendTrees;
		}

		public AnimatorState[] GetAllStates(AnimatorStateMachine TargetStateMachine) {
			AnimatorState[] States = TargetStateMachine.states.Select(ExistChildState => ExistChildState.state).ToArray();
			if (TargetStateMachine.stateMachines.Length > 0) {
				foreach (var TargetChildStatetMachine in TargetStateMachine.stateMachines) {
					States = States.Concat(GetAllStates(TargetChildStatetMachine.stateMachine)).ToArray();
				}
			}
			return States;
		}

		public AnimatorStateMachine[] GetAllStateMachines(AnimatorStateMachine TargetStateMachine) {
			AnimatorStateMachine[] StateMachines = new AnimatorStateMachine[] { TargetStateMachine };
			if (TargetStateMachine.stateMachines.Length > 0) {
				foreach (var TargetChildStateMachine in TargetStateMachine.stateMachines) {
					StateMachines = StateMachines.Concat(GetAllStateMachines(TargetChildStateMachine.stateMachine)).ToArray();
				}
			}
			return StateMachines;
		}

		public ChildAnimatorStateMachine[] DuplicateChildStateMachine(ChildAnimatorStateMachine[] TargetChildStateMachines) {
			ChildAnimatorStateMachine[] newChildStateMachines = new ChildAnimatorStateMachine[TargetChildStateMachines.Length];
			for (int Index = 0; Index < TargetChildStateMachines.Length; Index++) {
				ChildAnimatorStateMachine newChildStateMachine = new ChildAnimatorStateMachine {
					position = TargetChildStateMachines[Index].position,
					stateMachine = DuplicateStateMachine(TargetChildStateMachines[Index].stateMachine)
				};
				newChildStateMachines[Index] = newChildStateMachine;
			}
			return newChildStateMachines;
		}

		public ChildAnimatorState[] DuplicateChildAnimatorState(ChildAnimatorState[] TargetChildAnimatorStates) {
			ChildAnimatorState[] newChildAnimatorStates = new ChildAnimatorState[TargetChildAnimatorStates.Length];
			for (int Index = 0; Index < TargetChildAnimatorStates.Length; Index++) {
				ChildAnimatorState newChildAnimatorState = new ChildAnimatorState {
					position = TargetChildAnimatorStates[Index].position,
					state = DuplicateAnimatorState(TargetChildAnimatorStates[Index].state)
				};
				newChildAnimatorStates[Index] = newChildAnimatorState;
			}
			return newChildAnimatorStates;
		}

		public AnimatorState DuplicateAnimatorState(AnimatorState TargetAnimatorState) {
			AnimatorState newState = new AnimatorState {
				behaviours = TargetAnimatorState.behaviours,
				cycleOffset = TargetAnimatorState.cycleOffset,
				cycleOffsetParameter = TargetAnimatorState.cycleOffsetParameter,
				cycleOffsetParameterActive = TargetAnimatorState.cycleOffsetParameterActive,
				iKOnFeet = TargetAnimatorState.iKOnFeet,
				mirror = TargetAnimatorState.mirror,
				mirrorParameter = TargetAnimatorState.mirrorParameter,
				mirrorParameterActive = TargetAnimatorState.mirrorParameterActive,
				motion = TargetAnimatorState.motion,
				speed = TargetAnimatorState.speed,
				speedParameter = TargetAnimatorState.speedParameter,
				speedParameterActive = TargetAnimatorState.speedParameterActive,
				tag = TargetAnimatorState.tag,
				timeParameter = TargetAnimatorState.timeParameter,
				timeParameterActive = TargetAnimatorState.timeParameterActive,
				writeDefaultValues = TargetAnimatorState.writeDefaultValues,
				hideFlags = TargetAnimatorState.hideFlags,
				name = TargetAnimatorState.name
			};
			return newState;
		}

		public AnimatorStateTransition DuplicateTransition(AnimatorStateTransition TargetStateTransition, AnimatorState TargetAnimatorState, AnimatorStateMachine TargetAnimatorStateMachine) {
			AnimatorStateTransition newTransition = new AnimatorStateTransition {
				canTransitionToSelf = TargetStateTransition.canTransitionToSelf,
				duration = TargetStateTransition.duration,
				exitTime = TargetStateTransition.exitTime,
				hasExitTime = TargetStateTransition.hasExitTime,
				hasFixedDuration = TargetStateTransition.hasFixedDuration,
				interruptionSource = TargetStateTransition.interruptionSource,
				offset = TargetStateTransition.offset,
				orderedInterruption = TargetStateTransition.orderedInterruption,
				conditions = DuplicateConditions(TargetStateTransition.conditions),
				destinationState = TargetAnimatorState,
				destinationStateMachine = TargetAnimatorStateMachine,
				isExit = TargetStateTransition.isExit,
				mute = TargetStateTransition.mute,
				solo = TargetStateTransition.solo,
				hideFlags = TargetStateTransition.hideFlags,
				name = TargetStateTransition.name
			};
			return newTransition;
		}

		public AnimatorCondition[] DuplicateConditions(AnimatorCondition[] TargetConditions) {
			AnimatorCondition[] newConditions = new AnimatorCondition[TargetConditions.Length];
			for (int Index = 0; Index < TargetConditions.Length; Index++) {
				AnimatorCondition newCondition = new AnimatorCondition {
					mode = TargetConditions[Index].mode,
					threshold = TargetConditions[Index].threshold,
					parameter = TargetConditions[Index].parameter
				};
				newConditions[Index] = newCondition;
			}
			return newConditions;
		}

		public bool IsAnimatorWriteDefaults(AnimatorController TargetAnimator) {
			AnimatorState[] AvatarAnimatorState = GetAllAnimatorStates(TargetAnimator);
			bool[] WriteDefaults = AvatarAnimatorState.Select(Item => Item.writeDefaultValues).ToArray();
			int WriteDefaultsOffCount = WriteDefaults.Where(Item => Item == false).Count();
			if (WriteDefaults.Length > 0) {
				if ((WriteDefaultsOffCount / WriteDefaults.Length) <= 0.5) {
					return true;
				} else {
					return false;
				}
			}
			return true;
		}

		public bool ContainWords(string OriginalString, string TargetString) {
			string[] Words = TargetString.Split(' ');
			return Words.All(Word => OriginalString.Contains(Word));
		}
	}
}
#endif