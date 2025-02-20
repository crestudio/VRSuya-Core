#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor.Animations;

using VRC.SDK3.Avatars.Components;

/*
 * VRSuya Core
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 */

namespace VRSuya.Core {

	[ExecuteInEditMode]
	public class Avatar {

		public enum Avatar {
			Airi, Aldina, Angura, Anon, Anri, Ash,
			Chiffon, Chocolat, Cygnet,
			Emmelie, EYO,
			Firina, Fuzzy,
			Glaze, Grus,
			Hakka,
			IMERIS,
			Karin, Kikyo, Kokoa, Koyuki, Kuronatu,
			Lapwing, Leefa, Leeme, Lime, Lunalitt,
			Maki, Mamehinata, MANUKA, Mariel, Marron, Maya, Merino, Milk, Milltina, Minahoshi, Minase, Mint, Mir, Mishe, Moe,
			Nayu,
			Platinum,
			Quiche,
			Rainy, Ramune_Old, RINDO, Rue, Rusk,
			SELESTIA, Sephira, Shinano, Shinra, Sio, Sue, Sugar, Suzuhana,
			Tien, TubeRose,
			Ukon, Usasaki, Uzuki,
			Wolferia,
			Yoll, YUGI_MIYO, Yuuko
		}

		/// <summary>Avatar ENUM의 모든 요소를 string[]으로 반환합니다.</summary>
		/// <returns>ENUM 구성 요소 이름의 배열</returns>
		public string[] GetAvatarNames() {
			return Enum.GetNames(typeof(Avatar));
		}

		/// <summary>아바타의 지정한 레이어 애니메이터 컨트롤러를 찾아서 반환합니다.</summary>
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
		public List<HumanBodyBones> GetHumanBoneList() {
			return Enum.GetValues(typeof(HumanBodyBones)).Cast<HumanBodyBones>().ToList();
		}
	}
}
#endif