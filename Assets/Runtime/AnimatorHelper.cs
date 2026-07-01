#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor.Animations;
using UnityEngine;

using static VRC.SDK3.Avatars.Components.VRCAvatarDescriptor;

/*
 * VRSuya Core
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 */

namespace VRSuya.Core {

	public static class AnimatorHelper {

		public static void AddParameter(AnimatorController TargetController, AnimatorControllerParameter TargetParameter) {
			AnimatorControllerParameter NewParameter = new AnimatorControllerParameter {
				defaultBool = TargetParameter.defaultBool,
				defaultFloat = TargetParameter.defaultFloat,
				defaultInt = TargetParameter.defaultInt,
				name = TargetParameter.name,
				type = TargetParameter.type
			};
			TargetController.AddParameter(NewParameter);
		}

		static void CollectAnimatorComponent<TargetComponent>(AnimatorStateMachine TargetStateMachine, List<TargetComponent> TargetComponents) where TargetComponent : StateMachineBehaviour {
			foreach (StateMachineBehaviour TargetBehaviour in TargetStateMachine.behaviours) {
				if (TargetBehaviour is TargetComponent TargetStateMachineBehaviour) {
					TargetComponents.Add(TargetStateMachineBehaviour);
				}
			}
			foreach (ChildAnimatorState ChildState in TargetStateMachine.states) {
				AnimatorState TargetState = ChildState.state;
				if (!TargetState) continue;
				foreach (StateMachineBehaviour TargetBehaviour in TargetState.behaviours) {
					if (TargetBehaviour is TargetComponent TargetStateMachineBehaviourl) {
						TargetComponents.Add(TargetStateMachineBehaviourl);
					}
				}
			}
			foreach (ChildAnimatorStateMachine ChildStateMachine in TargetStateMachine.stateMachines) {
				AnimatorStateMachine ChildSubStateMachine = ChildStateMachine.stateMachine;
				if (!ChildSubStateMachine) continue;
				CollectAnimatorComponent(ChildSubStateMachine, TargetComponents);
			}
		}

		public static void CopyTransitions(AnimatorStateMachine TargetStateMachine, AnimatorStateMachine OldStateMachine) {
			AnimatorStateMachine[] OldAnimatorStateMachines = GetAllStateMachines(OldStateMachine);
			AnimatorStateMachine[] NewAnimatorStateMachines = GetAllStateMachines(TargetStateMachine);
			AnimatorState[] OldAnimatorStates = GetAllStates(OldStateMachine);
			AnimatorState[] NewAnimatorStates = GetAllStates(TargetStateMachine);
			foreach (AnimatorState TargetState in NewAnimatorStates) {
				AnimatorState ExistState = Array.Find(OldAnimatorStates, AnimatorState => AnimatorState.name == TargetState.name);
				AnimatorStateTransition[] OldStateTransitions = ExistState.transitions;
				if (OldStateTransitions.Length > 0) {
					AnimatorStateTransition[] NewTransitions = new AnimatorStateTransition[OldStateTransitions.Length];
					for (int Index = 0; Index < OldStateTransitions.Length; Index++) {
						if (OldStateTransitions[Index].destinationState != null) {
							AnimatorState NewDestinationState = Array.Find(NewAnimatorStates, AnimatorState => AnimatorState.name == OldStateTransitions[Index].destinationState.name);
							NewTransitions[Index] = DuplicateTransition(OldStateTransitions[Index], NewDestinationState, null);
						} else if (OldStateTransitions[Index].destinationStateMachine != null) {
							AnimatorStateMachine NewDestinationExistStateMachine = Array.Find(NewAnimatorStateMachines, ExistStateMachine => ExistStateMachine.name == OldStateTransitions[Index].destinationStateMachine.name);
							NewTransitions[Index] = DuplicateTransition(OldStateTransitions[Index], null, NewDestinationExistStateMachine);
						}
					}
					TargetState.transitions = NewTransitions;
				}
			}
		}

		public static AnimatorControllerLayer DuplicateAnimatorLayer(AnimatorControllerLayer TargetAnimatorLayer) {
			AnimatorControllerLayer NewAnimatorLayer = new AnimatorControllerLayer {
				avatarMask = TargetAnimatorLayer.avatarMask,
				blendingMode = TargetAnimatorLayer.blendingMode,
				defaultWeight = TargetAnimatorLayer.defaultWeight,
				iKPass = TargetAnimatorLayer.iKPass,
				name = TargetAnimatorLayer.name,
				stateMachine = DuplicateStateMachine(TargetAnimatorLayer.stateMachine),
				syncedLayerAffectsTiming = TargetAnimatorLayer.syncedLayerAffectsTiming,
				syncedLayerIndex = TargetAnimatorLayer.syncedLayerIndex
			};
			return NewAnimatorLayer;
		}

		public static AnimatorControllerLayer[] DuplicateAnimatorLayers(AnimatorController TargetController, AnimatorControllerLayer[] TargetLayers) {
			AnimatorControllerLayer[] NewAnimatorLayers = new AnimatorControllerLayer[TargetController.layers.Length + TargetLayers.Length];
			Array.Copy(TargetController.layers, NewAnimatorLayers, TargetController.layers.Length);
			for (int Index = 0; Index < TargetLayers.Length; Index++) {
				AnimatorControllerLayer NewAnimatorLayer = DuplicateAnimatorLayer(TargetLayers[Index]);
				NewAnimatorLayers[TargetController.layers.Length + Index] = NewAnimatorLayer;
			}
			return NewAnimatorLayers;
		}

		public static AnimatorState DuplicateAnimatorState(AnimatorState TargetAnimatorState) {
			AnimatorState NewState = new AnimatorState {
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
			return NewState;
		}

		public static ChildAnimatorState[] DuplicateChildAnimatorState(ChildAnimatorState[] TargetChildAnimatorStates) {
			ChildAnimatorState[] NewChildAnimatorStates = new ChildAnimatorState[TargetChildAnimatorStates.Length];
			for (int Index = 0; Index < TargetChildAnimatorStates.Length; Index++) {
				ChildAnimatorState NewChildAnimatorState = new ChildAnimatorState {
					position = TargetChildAnimatorStates[Index].position,
					state = DuplicateAnimatorState(TargetChildAnimatorStates[Index].state)
				};
				NewChildAnimatorStates[Index] = NewChildAnimatorState;
			}
			return NewChildAnimatorStates;
		}

		public static ChildAnimatorStateMachine[] DuplicateChildStateMachine(ChildAnimatorStateMachine[] TargetChildStateMachines) {
			ChildAnimatorStateMachine[] NewChildStateMachines = new ChildAnimatorStateMachine[TargetChildStateMachines.Length];
			for (int Index = 0; Index < TargetChildStateMachines.Length; Index++) {
				ChildAnimatorStateMachine NewChildStateMachine = new ChildAnimatorStateMachine {
					position = TargetChildStateMachines[Index].position,
					stateMachine = DuplicateStateMachine(TargetChildStateMachines[Index].stateMachine)
				};
				NewChildStateMachines[Index] = NewChildStateMachine;
			}
			return NewChildStateMachines;
		}

		public static AnimatorCondition[] DuplicateConditions(AnimatorCondition[] TargetConditions) {
			AnimatorCondition[] NewConditions = new AnimatorCondition[TargetConditions.Length];
			for (int Index = 0; Index < TargetConditions.Length; Index++) {
				AnimatorCondition NewCondition = new AnimatorCondition {
					mode = TargetConditions[Index].mode,
					threshold = TargetConditions[Index].threshold,
					parameter = TargetConditions[Index].parameter
				};
				NewConditions[Index] = NewCondition;
			}
			return NewConditions;
		}

		public static AnimatorStateMachine DuplicateStateMachine(AnimatorStateMachine TargetStateMachine) {
			AnimatorStateMachine NewStateMachines = new AnimatorStateMachine {
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
			CopyTransitions(NewStateMachines, TargetStateMachine);
			return NewStateMachines;
		}

		public static AnimatorStateTransition DuplicateTransition(AnimatorStateTransition TargetStateTransition, AnimatorState TargetAnimatorState, AnimatorStateMachine TargetAnimatorStateMachine) {
			AnimatorStateTransition NewTransition = new AnimatorStateTransition {
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
			return NewTransition;
		}

		public static AnimationClip[] GetAllAnimationClips(AnimatorController TargetAnimator) {
			if (!TargetAnimator) return new AnimationClip[0];
			List<AnimationClip> AnimatorAnimationClips = new List<AnimationClip>();
			List<Motion> AnimatorMotionList = GetAllAnimatorStates(TargetAnimator)
				.Where(Item => Item.motion != null)
				.Select(Item => Item.motion)
				.ToList();
			List<BlendTree> ChildBlendTrees = AnimatorMotionList.Where(Item => Item is BlendTree).Select(Item => Item as BlendTree).ToList();
			AnimatorAnimationClips.AddRange(AnimatorMotionList.Where(Item => Item is AnimationClip).Select(Item => Item as AnimationClip));
			foreach (BlendTree TargetChildBlendTree in ChildBlendTrees) {
				AnimatorAnimationClips.AddRange(GetAllAnimationClipsFromBlendTree(TargetChildBlendTree));
			}
			return AnimatorAnimationClips.Distinct().ToArray();
		}

		public static AnimationClip[] GetAllAnimationClipsFromBlendTree(BlendTree TargetBlendTree) {
			if (!TargetBlendTree) return new AnimationClip[0];
			List<AnimationClip> BlendTreeAnimationClips = new List<AnimationClip>();
			List<Motion> BlendTreeMotions = TargetBlendTree.children.Where(Item => Item.motion != null).Select(Item => Item.motion).ToList();
			List<BlendTree> ChildBlendTrees = BlendTreeMotions.Where(Item => Item is BlendTree).Select(Item => Item as BlendTree).ToList();
			BlendTreeAnimationClips.AddRange(BlendTreeMotions.Where(Item => Item is AnimationClip).Select(Item => Item as AnimationClip));
			foreach (BlendTree TargetChildBlendTree in ChildBlendTrees) {
				BlendTreeAnimationClips.AddRange(GetAllAnimationClipsFromBlendTree(TargetChildBlendTree));
			}
			return BlendTreeAnimationClips.Distinct().ToArray();
		}

		public static AnimatorState[] GetAllAnimatorStates(AnimatorController TargetAnimatorController) {
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

		public static AnimationClip[] GetAllAvatarAnimationClips(GameObject AvatarGameObject) {
			List<AnimatorController> AvatarAnimatorControllers = new List<AnimatorController>();
			List<AnimationClip> AvatarAnimationClips = new List<AnimationClip>();
			AvatarAnimatorControllers.Add(AvatarUtility.GetAnimatorController(AvatarGameObject, AnimLayerType.Action));
			AvatarAnimatorControllers.Add(AvatarUtility.GetAnimatorController(AvatarGameObject, AnimLayerType.Additive));
			AvatarAnimatorControllers.Add(AvatarUtility.GetAnimatorController(AvatarGameObject, AnimLayerType.Base));
			AvatarAnimatorControllers.Add(AvatarUtility.GetAnimatorController(AvatarGameObject, AnimLayerType.FX));
			AvatarAnimatorControllers.Add(AvatarUtility.GetAnimatorController(AvatarGameObject, AnimLayerType.Gesture));
			AvatarAnimatorControllers.Add(AvatarUtility.GetAnimatorController(AvatarGameObject, AnimLayerType.IKPose));
			AvatarAnimatorControllers.Add(AvatarUtility.GetAnimatorController(AvatarGameObject, AnimLayerType.Sitting));
			AvatarAnimatorControllers.Add(AvatarUtility.GetAnimatorController(AvatarGameObject, AnimLayerType.TPose));
			foreach (AnimatorController TargetAnimator in AvatarAnimatorControllers) {
				AvatarAnimationClips.AddRange(GetAllAnimationClips(TargetAnimator));
			}
			return AvatarAnimationClips.Distinct().ToArray();
		}

		public static BlendTree[] GetAllBlendTrees(AnimatorState[] TargetAnimatorStates) {
			BlendTree[] BlendTrees = new BlendTree[0];
			foreach (AnimatorState TargetAnimatorState in TargetAnimatorStates) {
				if (TargetAnimatorState.motion is BlendTree TargetBlendTree) {
					BlendTrees = BlendTrees.Concat(GetSubBlendTrees(TargetBlendTree)).ToArray();
				}
			}
			return BlendTrees;
		}

		public static AnimatorStateMachine[] GetAllStateMachines(AnimatorStateMachine TargetStateMachine) {
			AnimatorStateMachine[] StateMachines = new AnimatorStateMachine[] { TargetStateMachine };
			if (TargetStateMachine.stateMachines.Length > 0) {
				foreach (var TargetChildStateMachine in TargetStateMachine.stateMachines) {
					StateMachines = StateMachines.Concat(GetAllStateMachines(TargetChildStateMachine.stateMachine)).ToArray();
				}
			}
			return StateMachines;
		}

		public static AnimatorState[] GetAllStates(AnimatorStateMachine TargetStateMachine) {
			AnimatorState[] States = TargetStateMachine.states.Select(ExistChildState => ExistChildState.state).ToArray();
			if (TargetStateMachine.stateMachines.Length > 0) {
				foreach (var TargetChildStatetMachine in TargetStateMachine.stateMachines) {
					States = States.Concat(GetAllStates(TargetChildStatetMachine.stateMachine)).ToArray();
				}
			}
			return States;
		}

		public static AnimatorStateTransition[] GetAllTransitions(AnimatorController TargetAnimatorController) {
			AnimatorState[] AllAnimatorState = GetAllAnimatorStates(TargetAnimatorController);
			return AllAnimatorState.SelectMany(Item => Item.transitions).ToArray();
		}

		public static TargetComponent[] GetAnimatorComponent<TargetComponent>(AnimatorStateMachine TargetStateMachine) where TargetComponent : StateMachineBehaviour {
			List<TargetComponent> TargetComponents = new List<TargetComponent>();
			CollectAnimatorComponent(TargetStateMachine, TargetComponents);
			return TargetComponents.ToArray();
		}

		public static AnimatorState GetStandingState(AnimatorController TargetAnimator) {
			if (TargetAnimator.layers.Length > 0) {
				AnimatorState[] AllAnimatorStates = GetAllStates(TargetAnimator.layers[0].stateMachine);
				AnimatorState StandingState = AllAnimatorStates.FirstOrDefault(Item => Item.name == "Standing");
				if (StandingState) return StandingState;
				StandingState = AllAnimatorStates.FirstOrDefault(Item => Item.name.Contains("Stand", StringComparison.OrdinalIgnoreCase));
				if (StandingState) return StandingState;
				if (TargetAnimator.layers[0].stateMachine.defaultState) {
					return TargetAnimator.layers[0].stateMachine.defaultState;
				}
			}
			return null;
		}

		static BlendTree[] GetSubBlendTrees(BlendTree TargetBlendTree) {
			BlendTree[] BlendTrees = new BlendTree[0];
			BlendTrees = BlendTrees.Concat(new BlendTree[] { TargetBlendTree }).ToArray();
			foreach (ChildMotion ChildMotion in TargetBlendTree.children) {
				if (ChildMotion.motion is BlendTree ChildBlendTree) {
					BlendTrees = BlendTrees.Concat(GetSubBlendTrees(ChildBlendTree)).ToArray();
				}
			}
			return BlendTrees;
		}

		public static bool IsAnimatorWriteDefaults(AnimatorController TargetAnimator) {
			AnimatorState[] AvatarAnimatorState = GetAllAnimatorStates(TargetAnimator);
			bool[] WriteDefaults = AvatarAnimatorState.Select(Item => Item.writeDefaultValues).ToArray();
			int WriteDefaultsOffCount = WriteDefaults.Where(Item => Item == false).Count();
			if (WriteDefaults.Length > 0) {
				if ((float)WriteDefaultsOffCount / WriteDefaults.Length <= 0.5) {
					return true;
				} else {
					return false;
				}
			}
			return true;
		}
	}
}
#endif