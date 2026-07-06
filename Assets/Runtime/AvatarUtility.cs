#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;

using VRC.SDKBase;
using VRC.SDK3.Avatars.Components;
using static VRC.SDK3.Avatars.Components.VRCAvatarDescriptor;

/*
 * VRSuya Core
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 */

namespace VRSuya.Core {

	public static class AvatarUtility {

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

		public static readonly string[] ToeLeftBoneNames = new string[] {
			"Toe.L", "Toe_L" , "Toes.L", "Toes_L", "Left Toe", "LeftToeBase", "LeftToes", "foot.L.001", "Foot.L.002", "Bese_L"
		};
		public static readonly string[] ToeRightBoneNames = new string[] {
			"Toe.R", "Toe_R", "Toes.R", "Toes_L", "Right Toe", "RightToeBase", "RightToes", "foot.R.001", "Foot.R.002", "Bese_R"
		};
		public static readonly string[] HumanoidToeBoneNames = ToeLeftBoneNames.Concat(ToeRightBoneNames).ToArray();

		public static readonly Dictionary<string, string[]> ToeBoneDictionary = new Dictionary<string, string[]>() {
			{ "ThumbToe1_L", new string[] { "ThumbToe1_L", "Toe_Thumb_Proximal_L" } },
			{ "ThumbToe1_R", new string[] { "ThumbToe1_R", "Toe_Thumb_Proximal_R" } },
			{ "IndexToe1_L", new string[] { "IndexToe1_L", "Toe_Index_Proximal_L" } },
			{ "IndexToe1_R", new string[] { "IndexToe1_R", "Toe_Index_Proximal_R" } },
			{ "MiddleToe1_L", new string[] { "MiddleToe1_L", "Toe_Middle_Proximal_L" } },
			{ "MiddleToe1_R", new string[] { "MiddleToe1_R", "Toe_Middle_Proximal_R" } },
			{ "RingToe1_L", new string[] { "RingToe1_L", "Toe_Ring_Proximal_L" } },
			{ "RingToe1_R", new string[] { "RingToe1_R", "Toe_Ring_Proximal_R" } },
			{ "LittleToe1_L", new string[] { "LittleToe1_L", "Toe_Little_Proximal_L" } },
			{ "LittleToe1_R", new string[] { "LittleToe1_R", "Toe_Little_Proximal_R" } }
		};
		public static readonly string[] ToeBoneNames = ToeBoneDictionary.Values.SelectMany(Item => Item).ToArray();

		public static AnimatorController GetAnimatorController(GameObject AvatarGameObject, AnimLayerType LayerType) {
			AvatarGameObject.TryGetComponent(out VRCAvatarDescriptor TargetAvatarDescriptor);
			if (TargetAvatarDescriptor) {
				CustomAnimLayer TargetLayer = TargetAvatarDescriptor.baseAnimationLayers.FirstOrDefault(Item => Item.type == LayerType);
				if (!TargetLayer.isDefault) {
					return TargetLayer.animatorController as AnimatorController;
				}
			}
			return null;
		}

		public static GameObject GetArmatureGameObject(GameObject AvatarGameObject) {
			AvatarGameObject.TryGetComponent(out Animator TargetAnimator);
			if (TargetAnimator) {
				Transform HipsTransform = TargetAnimator.GetBoneTransform(HumanBodyBones.Hips);
				if (HipsTransform) {
					Transform ArmatureTransform = HipsTransform.parent;
					if (ArmatureTransform) {
						return ArmatureTransform.gameObject;
					}
				}
			}
			return null;
		}

		public static Transform GetAvatarAnchorOverride(GameObject AvatarGameObject) {
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

		public static VRC_AvatarDescriptor GetAvatarDescriptor() {
			VRC_AvatarDescriptor TargetAvatarDescriptor = GetAvatarDescriptorFromVRCSDKBuilder();
			if (!TargetAvatarDescriptor) TargetAvatarDescriptor = GetAvatarDescriptorFromSelection();
			if (!TargetAvatarDescriptor) TargetAvatarDescriptor = GetAvatarDescriptorFromVRCTool();
			return TargetAvatarDescriptor;
		}

		static VRC_AvatarDescriptor GetAvatarDescriptorFromSelection() {
			GameObject[] SelectedGameObjects = Selection.gameObjects;
			if (SelectedGameObjects.Length == 1) {
				return SelectedGameObjects[0].GetComponent<VRC_AvatarDescriptor>();
			} else if (SelectedGameObjects.Length > 1) {
				return SelectedGameObjects
					.Where(Item => Item.activeInHierarchy == true)
					.Select(Item => Item.GetComponent<VRC_AvatarDescriptor>())
					.FirstOrDefault(Item => Item != null);
			}
			return null;
		}

		static VRC_AvatarDescriptor GetAvatarDescriptorFromVRCSDKBuilder() {
			return null;
		}

		static VRC_AvatarDescriptor GetAvatarDescriptorFromVRCTool() {
			return VRC.Tools.FindSceneObjectsOfTypeAll<VRC_AvatarDescriptor>()
				.FirstOrDefault(Item => Item.gameObject.activeInHierarchy == true);
		}

		public static GameObject GetAvatarGameObject(GameObject TargetGameObject = null) {
			if (!TargetGameObject) {
				VRC_AvatarDescriptor TargetAvatarDescriptor = GetAvatarDescriptor();
				if (TargetAvatarDescriptor) {
					return TargetAvatarDescriptor.gameObject;
				}
			} else {
				GameObject RootGameObject = TargetGameObject.transform.root.gameObject;
				VRC_AvatarDescriptor TargetAvatarDescriptor = RootGameObject.GetComponent<VRC_AvatarDescriptor>();
				if (TargetAvatarDescriptor) {
					return RootGameObject;
				}
			}
			return null;
		}

		public static GameObject[] GetAvatarGameObjects() {
			return EditorSceneManager.GetActiveScene().GetRootGameObjects()
				.Where(Item => Item.GetComponent<VRC_AvatarDescriptor>() != null)
				.ToArray();
		}

		public static string GetAvatarName(string TargetString) {
			foreach (string AvatarName in GetAvatarNames()) {
				if (TargetString.Contains(AvatarName, StringComparison.OrdinalIgnoreCase)) return AvatarName;
			}
			return null;
		}

		public static string[] GetAvatarNames() {
			return Enum.GetNames(typeof(AvatarType));
		}

		public static Transform GetAvatarRootBone(GameObject AvatarGameObject) {
			AvatarGameObject.TryGetComponent(out UnityEngine.Animator AvatarAnimator);
			if (AvatarAnimator) {
				return AvatarAnimator.GetBoneTransform(HumanBodyBones.Hips);
			}
			return null;
		}

		public static GameObject GetHeadGameObject(GameObject AvatarGameObject) {
			AvatarGameObject.TryGetComponent(out VRCAvatarDescriptor TargetAvatarDescriptor);
			if (TargetAvatarDescriptor) {
				if (TargetAvatarDescriptor.VisemeSkinnedMesh) {
					return TargetAvatarDescriptor.VisemeSkinnedMesh.gameObject;
				}
				if (TargetAvatarDescriptor.customEyeLookSettings.eyelidsSkinnedMesh) {
					return TargetAvatarDescriptor.customEyeLookSettings.eyelidsSkinnedMesh.gameObject;
				}
			}
			AvatarGameObject.TryGetComponent(out UnityEngine.Animator TargetAnimator);
			if (TargetAnimator) {
				Transform LeftEyeTransform = TargetAnimator.GetBoneTransform(HumanBodyBones.LeftEye);
				Transform RightEyeTransform = TargetAnimator.GetBoneTransform(HumanBodyBones.RightEye);
				if (LeftEyeTransform && RightEyeTransform) {
					SkinnedMeshRenderer HeadSkinnedMeshrenderer = AvatarGameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true)
						.FirstOrDefault(Item => Item.bones.Contains(LeftEyeTransform) && Item.bones.Contains(LeftEyeTransform));
					if (HeadSkinnedMeshrenderer) {
						return HeadSkinnedMeshrenderer.gameObject;
					}
				}
				Transform HeadTransform = TargetAnimator.GetBoneTransform(HumanBodyBones.Head);
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

		public static AnimationClip GetStandingAnimation(AnimatorController TargetAnimator) {
			AnimatorState StandingState = AnimatorHelper.GetStandingState(TargetAnimator);
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

		public static bool HasEndBone(GameObject AvatarGameObject) {
			GameObject ArmatureGameObject = GetArmatureGameObject(AvatarGameObject);
			Transform[] EndTransforms = ArmatureGameObject.GetComponentsInChildren<Transform>(true).Where(Item => Item.childCount == 0).ToArray();
			int EndBoneCount = EndTransforms.Where(Item => Item.name.Contains("_end")).Count();
			if (EndTransforms.Length > 0) {
				if ((float)EndBoneCount / EndTransforms.Length >= 0.75) {
					return true;
				}
			}
			return false;
		}
	}
}
#endif