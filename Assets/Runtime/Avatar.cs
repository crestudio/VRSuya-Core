using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

using VRC.SDKBase;
using VRC.SDK3.Avatars.Components;
using static VRC.SDK3.Avatars.Components.VRCAvatarDescriptor;

/*
 * VRSuya Core
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 */

namespace VRSuya.Core {

	public class Avatar {

		public enum AvatarAuthor {
			General,
			ChocolateRice,
			JINGO,
			Komado,
			Plusone
		}

		public enum AvatarType {
			None, General,
			Airi, Aldina, Angura, Anon, Anri, Ash,
			Chiffon, Chise, Chocolat, Cygnet,
			Eku, Emmelie, EYO,
			Firina, Flare, Fuzzy,
			Glaze, Grus,
			Hakka,
			IMERIS,
			Karin, Kikyo, Kipfel, Kokoa, Koyuki, KUMALY, Kuronatu,
			Lapwing, Lazuli, Leefa, Leeme, Lime, LUMINA, Lunalitt,
			Mafuyu, Maki, Mamehinata, MANUKA, Mariel, Marron, Maya, MAYO, Merino, Milfy, Milk, Milltina, Minahoshi, Minase, Mint, Mir, Misaki, Mishe, Moe,
			Nayu, Nehail, Nochica,
			Platinum, Plum, Pochimaru,
			Quiche,
			Rainy, Ramune, Ramune_Old, RINDO, Rokona, Rue, Rurune, Rusk,
			SELESTIA, Sephira, Shami, Shinano, Shinra, SHIRAHA, Shiratsume, Sio, Sue, Sugar, Suzuhana,
			Tien, TubeRose,
			Ukon, Usasaki, Uzuki,
			VIVH,
			Wolferia,
			Yoll, YUGI_MIYO, Yuuko
			// 검색용 신규 아바타 추가 위치
		}

		public static readonly string[] HeadGameObjectNames = new string[] { "Body", "Head", "Face" };
		public static readonly string[] CheekLeftBoneNames = new string[] {
			"Cheek_L", "Cheek.L", "Cheek1_L", "Cheek_Root_L", "Cheek_root_L", "Hoppe.L", "ho_L"
		};
		public static readonly string[] CheekRightBoneNames = new string[] {
			"Cheek_R", "Cheek.R", "Cheek1_R", "Cheek_Root_R", "Cheek_root_R", "Hoppe.R", "ho_R"
		};
		public static readonly string[] CheekBoneNames = CheekLeftBoneNames.Concat(CheekRightBoneNames).ToArray();

		public string GetAvatarName(string TargetString) {
			foreach (string AvatarName in GetAvatarNames()) {
				if (TargetString.Contains(AvatarName, StringComparison.OrdinalIgnoreCase)) return AvatarName;
			}
			return null;
		}

		public string[] GetAvatarNames() {
			return Enum.GetNames(typeof(AvatarType));
		}

		public GameObject GetAvatarGameObject(GameObject TargetGameObject = null) {
			if (TargetGameObject == null) {
				return GetVRCAvatarDescriptor().gameObject;
			} else {
				GameObject RootGameObject = TargetGameObject.transform.root.gameObject;
				VRC_AvatarDescriptor TargetAvatarDescriptor = RootGameObject.GetComponent<VRC_AvatarDescriptor>();
				if (TargetAvatarDescriptor) {
					return RootGameObject;
				} else {
					return null;
				}
			}
		}

		public VRC_AvatarDescriptor GetVRCAvatarDescriptor() {
			VRC_AvatarDescriptor TargetAvatarDescriptor = GetAvatarDescriptorFromVRCSDKBuilder();
			if (!TargetAvatarDescriptor) TargetAvatarDescriptor = GetAvatarDescriptorFromSelection();
			if (!TargetAvatarDescriptor) TargetAvatarDescriptor = GetAvatarDescriptorFromVRCTool();
			return TargetAvatarDescriptor;
		}

		VRC_AvatarDescriptor GetAvatarDescriptorFromVRCSDKBuilder() {
			return null;
		}

		VRC_AvatarDescriptor GetAvatarDescriptorFromSelection() {
			GameObject[] SelectedGameObjects = Selection.gameObjects;
			if (SelectedGameObjects.Length == 1) {
				VRC_AvatarDescriptor TargetVRCAvatarDescriptor = SelectedGameObjects[0].GetComponent<VRC_AvatarDescriptor>();
				if (TargetVRCAvatarDescriptor) {
					return TargetVRCAvatarDescriptor;
				} else {
					return null;
				}
			} else if (SelectedGameObjects.Length > 1) {
				VRC_AvatarDescriptor TargetVRCAvatarDescriptor = SelectedGameObjects
					.Where(SelectedGameObject => SelectedGameObject.activeInHierarchy == true)
					.Select(SelectedGameObject => SelectedGameObject.GetComponent<VRC_AvatarDescriptor>()).ToArray()[0];
				if (TargetVRCAvatarDescriptor) {
					return TargetVRCAvatarDescriptor;
				} else {
					return null;
				}
			} else {
				return null;
			}
		}

		VRC_AvatarDescriptor GetAvatarDescriptorFromVRCTool() {
			VRC_AvatarDescriptor[] AllVRCAvatarDescriptor = VRC.Tools.FindSceneObjectsOfTypeAll<VRC_AvatarDescriptor>().ToArray();
			if (AllVRCAvatarDescriptor.Length > 0) {
				return AllVRCAvatarDescriptor.Where(Avatar => Avatar.gameObject.activeInHierarchy).ToArray()[0];
			} else {
				return null;
			}
		}

		public AnimatorController GetAnimatorController(GameObject AvatarGameObject, AnimLayerType LayerType) {
			AvatarGameObject.TryGetComponent(out VRCAvatarDescriptor AvatarDescriptor);
			if (AvatarDescriptor) {
				CustomAnimLayer TargetLayer = AvatarDescriptor.baseAnimationLayers.FirstOrDefault(Item => Item.type == LayerType);
				if (!TargetLayer.isDefault) {
					return TargetLayer.animatorController as AnimatorController;
				}
			}
			return null;
		}

		public static List<HumanBodyBones> GetHumanBoneList() {
			return Enum.GetValues(typeof(HumanBodyBones)).Cast<HumanBodyBones>().ToList();
		}

		public GameObject GetHeadGameObject(GameObject AvatarGameObject) {
			AvatarGameObject.TryGetComponent(out VRCAvatarDescriptor AvatarDescriptor);
			if (AvatarDescriptor) {
				if (AvatarDescriptor.VisemeSkinnedMesh) {
					return AvatarDescriptor.VisemeSkinnedMesh.gameObject;
				}
				if (AvatarDescriptor.customEyeLookSettings.eyelidsSkinnedMesh) {
					return AvatarDescriptor.customEyeLookSettings.eyelidsSkinnedMesh.gameObject;
				}
			}
			AvatarGameObject.TryGetComponent(out UnityEngine.Animator AvatarAnimator);
			if (AvatarAnimator) {
				Transform HeadTransform = AvatarAnimator.GetBoneTransform(HumanBodyBones.Head);
				if (HeadTransform) {
					SkinnedMeshRenderer HeadSkinnedMeshrenderer = AvatarGameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true)
						.Where(Item => HeadGameObjectNames.Contains(Item.gameObject.name, StringComparer.OrdinalIgnoreCase))
						.FirstOrDefault(Item => Item.bones.Contains(HeadTransform));
					if (HeadSkinnedMeshrenderer) {
						return HeadSkinnedMeshrenderer.gameObject;
					}
				}
			}
			return null;
		}

		public string[] GetBlendshapeNameList(SkinnedMeshRenderer TargetSkinnedMeshRenderer) {
			List<string> newBlendshapeNameList = new List<string>();
			Mesh TargetMesh = TargetSkinnedMeshRenderer.sharedMesh;
			if (TargetMesh.blendShapeCount > 0) {
				for (int Index = 0; Index < TargetMesh.blendShapeCount; Index++) {
					newBlendshapeNameList.Add(TargetMesh.GetBlendShapeName(Index));
				}
			}
			return newBlendshapeNameList.ToArray();
		}

		public Transform GetAvatarAnchorOverride(GameObject AvatarGameObject) {
			GameObject HeadGameObject = GetHeadGameObject(AvatarGameObject);
			if (HeadGameObject) {
				SkinnedMeshRenderer HeadSkinnedMeshRenderer = HeadGameObject.GetComponent<SkinnedMeshRenderer>();
				if (HeadSkinnedMeshRenderer) {
					if (HeadSkinnedMeshRenderer.probeAnchor) {
						return HeadSkinnedMeshRenderer.probeAnchor;
					}
				}
			}
			return AvatarGameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true)
				.Select(Item => Item.probeAnchor)
				.Where(Item => Item != null)
				.GroupBy(Item => Item)
				.OrderByDescending(Item => Item.Count())
				.FirstOrDefault()?
				.Key;
		}

		public Transform GetAvatarRootBone(GameObject AvatarGameObject) {
			AvatarGameObject.TryGetComponent(out UnityEngine.Animator AvatarAnimator);
			if (AvatarAnimator != null) {
				return AvatarAnimator.GetBoneTransform(HumanBodyBones.Hips);
			}
			return null;
		}

		public AnimationClip GetStandingAnimation(AnimatorController TargetAnimator) {
			AnimatorState StandingState = GetStandingState(TargetAnimator);
			if (StandingState) {
				if (StandingState.motion && StandingState.motion is BlendTree) {
					BlendTree StandingBlendTree = StandingState.motion as BlendTree;
					ChildMotion[] StandingMotion = StandingBlendTree.children.Where(Item => Item.position == new Vector2(0f, 0f)).ToArray();
					if (StandingMotion.Length > 0) {
						if (StandingMotion[0].motion && StandingMotion[0].motion is AnimationClip) {
							return StandingMotion[0].motion as AnimationClip;
						}
					}
				}
			}
			return null;
		}

		public AnimatorState GetStandingState(AnimatorController TargetAnimator) {
			if (TargetAnimator.layers.Length > 0) {
				Animator AnimatorInstance = new Animator();
				AnimatorState[] AllAnimatorStates = AnimatorInstance.GetAllStates(TargetAnimator.layers[0].stateMachine);
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

		public AnimationClip[] GetAllAvatarAnimationClips(GameObject AvatarGameObject) {
			List<AnimatorController> AvatarAnimatorControllers = new List<AnimatorController>();
			List<AnimationClip> AvatarAnimationClips = new List<AnimationClip>();
			AvatarAnimatorControllers.Add(GetAnimatorController(AvatarGameObject, AnimLayerType.Action));
			AvatarAnimatorControllers.Add(GetAnimatorController(AvatarGameObject, AnimLayerType.Additive));
			AvatarAnimatorControllers.Add(GetAnimatorController(AvatarGameObject, AnimLayerType.Base));
			AvatarAnimatorControllers.Add(GetAnimatorController(AvatarGameObject, AnimLayerType.FX));
			AvatarAnimatorControllers.Add(GetAnimatorController(AvatarGameObject, AnimLayerType.Gesture));
			AvatarAnimatorControllers.Add(GetAnimatorController(AvatarGameObject, AnimLayerType.IKPose));
			AvatarAnimatorControllers.Add(GetAnimatorController(AvatarGameObject, AnimLayerType.Sitting));
			AvatarAnimatorControllers.Add(GetAnimatorController(AvatarGameObject, AnimLayerType.TPose));
			foreach (AnimatorController TargetAnimator in AvatarAnimatorControllers) {
				AvatarAnimationClips.AddRange(GetAllAnimationClips(TargetAnimator));
			}
			return AvatarAnimationClips.Distinct().ToArray();
		}

		public AnimationClip[] GetAllAnimationClips(AnimatorController TargetAnimator) {
			if (!TargetAnimator) return new AnimationClip[0];
			VRSuya.Core.Animator AnimatorInstance = new VRSuya.Core.Animator();
			List<AnimationClip> AnimatorAnimationClips = new List<AnimationClip>();
			List<Motion> AnimatorMotionList = AnimatorInstance.GetAllAnimatorStates(TargetAnimator)
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

		public AnimationClip[] GetAllAnimationClipsFromBlendTree(BlendTree TargetBlendTree) {
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
	}
}