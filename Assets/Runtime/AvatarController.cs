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

		/// <summary>�ƹ�Ÿ�� ������ ���̾� �ִϸ����� ��Ʈ�ѷ��� ã�Ƽ� ��ȯ�մϴ�.</summary>
		/// <returns>�ƹ�Ÿ�� ������ ���̾� �ִϸ����� ��Ʈ�ѷ�</returns>
		public static AnimatorController GetAnimatorController(GameObject AvatarGameObject, VRCAvatarDescriptor.AnimLayerType LayerType) {
			AvatarGameObject.TryGetComponent(typeof(VRCAvatarDescriptor), out Component AvatarDescriptor);
			if (AvatarDescriptor) {
				VRCAvatarDescriptor.CustomAnimLayer TargetAnimatorController = Array.Find(AvatarDescriptor.GetComponent<VRCAvatarDescriptor>().baseAnimationLayers, AnimationLayer => AnimationLayer.type == LayerType);
				return (AnimatorController)TargetAnimatorController.animatorController;
			}
			return null;
		}

		/// <summary>HumanBodyBones�� ���� �� ��ϵ��� ��ȯ�մϴ�.</summary>
		/// <returns>HumanBodyBones ���</returns>
		public static List<HumanBodyBones> GetHumanBoneList() {
			return Enum.GetValues(typeof(HumanBodyBones)).Cast<HumanBodyBones>().ToList();
		}
	}
}
#endif