#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

using VRC.SDKBase;
using VRC.SDK3.Avatars.Components;

/*
 * VRSuya Core
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 */

namespace VRSuya.Core {

	[ExecuteInEditMode]
	public class Avatar {

		public enum AvatarType {
			Airi, Aldina, Angura, Anon, Anri, Ash,
			Chiffon, Chise, Chocolat, Cygnet,
			Emmelie, EYO,
			Firina, Fuzzy,
			Glaze, Grus,
			Hakka,
			IMERIS,
			Karin, Kikyo, Kipfel, Kokoa, Koyuki, Kuronatu,
			Lapwing, Leefa, Leeme, Lime, Lunalitt,
			Maki, Mamehinata, MANUKA, Mariel, Marron, Maya, Merino, Milfy, Milk, Milltina, Minahoshi, Minase, Mint, Mir, Mishe, Moe,
			Nayu,
			Platinum,
			Quiche,
			Rainy, Ramune_Old, RINDO, Rue, Rusk,
			SELESTIA, Sephira, Shinano, Shinra, Sio, Sue, Sugar, Suzuhana,
			Tien, TubeRose,
			Ukon, Usasaki, Uzuki,
			Wolferia,
			Yoll, YUGI_MIYO, Yuuko
			// 검색용 신규 아바타 추가 위치
		}

		public static readonly string[] HeadGameObjectNames = new string[] { "Body", "Head", "Face" };

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
		public AnimatorController GetAnimatorController(GameObject AvatarGameObject, VRCAvatarDescriptor.AnimLayerType LayerType) {
			AvatarGameObject.TryGetComponent(typeof(VRCAvatarDescriptor), out Component AvatarDescriptor);
			if (AvatarDescriptor) {
				VRCAvatarDescriptor.CustomAnimLayer TargetAnimatorController = Array.Find(AvatarDescriptor.GetComponent<VRCAvatarDescriptor>().baseAnimationLayers, AnimationLayer => AnimationLayer.type == LayerType);
				return (AnimatorController)TargetAnimatorController.animatorController;
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
#endif