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

		/// <summary>주어진 String에서 아바타 이름을 추출하여 반환합니다</summary>
		/// <returns>아바타 이름 또는 null 값</returns>
		public string GetAvatarName(string TargetString) {
			foreach (string AvatarName in GetAvatarNames()) {
				if (TargetString.Contains(AvatarName, StringComparison.OrdinalIgnoreCase)) return AvatarName;
			}
			return null;
		}

		/// <summary>Avatar ENUM의 모든 요소를 string[]으로 반환합니다.</summary>
		/// <returns>ENUM 구성 요소 이름의 배열</returns>
		public string[] GetAvatarNames() {
			return Enum.GetNames(typeof(AvatarType));
		}

		/// <summary>Scene에서 조건에 맞는 VRC AvatarDescriptor 컴포넌트 아바타 1개를 반환합니다.</summary>
		/// <returns>조건에 맞는 VRC 아바타</returns>
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

		/// <summary>Scene에서 조건에 맞는 VRC AvatarDescriptor 컴포넌트 아바타 1개를 반환합니다.</summary>
		/// <returns>조건에 맞는 VRC 아바타</returns>
		public VRC_AvatarDescriptor GetVRCAvatarDescriptor() {
			VRC_AvatarDescriptor TargetAvatarDescriptor = GetAvatarDescriptorFromVRCSDKBuilder();
			if (!TargetAvatarDescriptor) TargetAvatarDescriptor = GetAvatarDescriptorFromSelection();
			if (!TargetAvatarDescriptor) TargetAvatarDescriptor = GetAvatarDescriptorFromVRCTool();
			return TargetAvatarDescriptor;
		}

		/// <summary>VRCSDK Builder에서 활성화 상태인 VRC 아바타를 반환합니다.</summary>
		/// <returns>VRCSDK Builder에서 활성화 상태인 VRC 아바타</returns>
		VRC_AvatarDescriptor GetAvatarDescriptorFromVRCSDKBuilder() {
			return null;
		}

		/// <summary>Unity 하이어라키에서 선택한 GameObject 중에서 VRC AvatarDescriptor 컴포넌트가 존재하는 아바타를 1개를 반환합니다.</summary>
		/// <returns>선택 중인 VRC 아바타</returns>
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

		/// <summary>Scene에서 활성화 상태인 VRC AvatarDescriptor 컴포넌트가 존재하는 아바타를 1개를 반환합니다.</summary>
		/// <returns>Scene에서 활성화 상태인 VRC 아바타</returns>
		VRC_AvatarDescriptor GetAvatarDescriptorFromVRCTool() {
			VRC_AvatarDescriptor[] AllVRCAvatarDescriptor = VRC.Tools.FindSceneObjectsOfTypeAll<VRC_AvatarDescriptor>().ToArray();
			if (AllVRCAvatarDescriptor.Length > 0) {
				return AllVRCAvatarDescriptor.Where(Avatar => Avatar.gameObject.activeInHierarchy).ToArray()[0];
			} else {
				return null;
			}
		}

		/// <summary>아바타의 지정한 레이어 애니메이터 컨트롤러를 찾아서 반환합니다.</summary>
		/// <param name="AvatarGameObject">VRChat 아바타 GameObject</param>
		/// <param name="LayerType">VRChat 애니메이터 타입</param>
		/// <returns>아바타의 지정한 레이어 애니메이터 컨트롤러</returns>
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

		/// <summary>HumanBodyBones의 하위 본 목록들을 반환합니다.</summary>
		/// <returns>HumanBodyBones 목록</returns>
		public static List<HumanBodyBones> GetHumanBoneList() {
			return Enum.GetValues(typeof(HumanBodyBones)).Cast<HumanBodyBones>().ToList();
		}

		/// <summary>요청한 아바타의 머리 GameObject를 가져옵니다</summary>
		/// <param name="AvatarGameObject">VRChat 아바타 GameObject</param>
		/// <returns>아바타의 머리 GameObject</returns>
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

		/// <summary>요청한 SkinnedMeshRenderer에서 BlendShape 목록을 모두 불러옵니다.</summary>
		/// <param name="TargetSkinnedMeshRenderer">요청할 SkinnedMeshRenderer</param>
		/// <returns>BlendShape 이름 어레이</returns>
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

		/// <summary>아바타의 대표 AnchorOverride 포인트를 획득하는 메소드 입니다.</summary>
		/// <returns>기준이 되는 AnchorOverride 트랜스폼</returns>
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