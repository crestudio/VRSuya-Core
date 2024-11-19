#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

using VRC.SDK3.Avatars.Components;

/*
 * VRSuya Core
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 */

namespace VRSuya.Core.Avatar {

	[ExecuteInEditMode]
	public static class AvatarController {

		/// <summary>아바타의 지정한 레이어 애니메이터 컨트롤러를 찾아서 반환합니다.</summary>
		/// <returns>아바타의 지정한 레이어 애니메이터 컨트롤러</returns>
		public static AnimatorController GetAnimatorController(GameObject AvatarGameObject, VRCAvatarDescriptor.AnimLayerType LayerType) {
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
	}
}
#endif