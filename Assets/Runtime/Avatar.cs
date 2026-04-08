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

	[ExecuteInEditMode]
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
			Mafuyu, Maki, Mamehinata, MANUKA, Mariel, Marron, Maya, MAYO, Merino, Milfy, Milk, Milltina, Minahoshi, Minase, Mint, Mir, Mishe, Moe,
			Nayu, Nehail, Nochica,
			Platinum, Plum, Pochimaru,
			Quiche,
			Rainy, Ramune, Ramune_Old, RINDO, Rokona, Rue, Rurune, Rusk,
			SELESTIA, Sephira, Shinano, Shinra, SHIRAHA, Shiratsume, Sio, Sue, Sugar, Suzuhana,
			Tien, TubeRose,
			Ukon, Usasaki, Uzuki,
			VIVH,
			Wolferia,
			Yoll, YUGI_MIYO, Yuuko
			// 검색용 신규 아바타 추가 위치
		}

		public static readonly string[] HeadGameObjectNames = new string[] { "Body", "Head", "Face" };

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
			foreach (SkinnedMeshRenderer TargetSkinnedMeshRenderer in AvatarGameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true)) {
				if (Array.Exists(HeadGameObjectNames, Item => TargetSkinnedMeshRenderer.gameObject.name == Item)) {
					if (Array.Exists(TargetSkinnedMeshRenderer.bones, Bone => AvatarGameObject.GetComponent<UnityEngine.Animator>().GetBoneTransform(HumanBodyBones.Head) == Bone)) {
						return TargetSkinnedMeshRenderer.gameObject;
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
			Transform AvatarAnchorOverride = null;
			GameObject HeadGameObject = GetHeadGameObject(AvatarGameObject);
			if (HeadGameObject) {
				SkinnedMeshRenderer HeadSkinnedMeshRenderer = HeadGameObject.GetComponent<SkinnedMeshRenderer>();
				if (HeadSkinnedMeshRenderer.probeAnchor) {
					AvatarAnchorOverride = HeadSkinnedMeshRenderer.probeAnchor;
					return AvatarAnchorOverride;
				}
			}
			UnityEngine.Animator AvatarAnimator = AvatarGameObject.GetComponent<UnityEngine.Animator>();
			if (AvatarAnimator.GetBoneTransform(HumanBodyBones.Head)) {
				AvatarAnchorOverride = AvatarAnimator.GetBoneTransform(HumanBodyBones.Head);
			} else if (AvatarAnimator.GetBoneTransform(HumanBodyBones.Hips)) {
				AvatarAnchorOverride = AvatarAnimator.GetBoneTransform(HumanBodyBones.Hips);
			}
			return AvatarAnchorOverride;
		}
	}
}