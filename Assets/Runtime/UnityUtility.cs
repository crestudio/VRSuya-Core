#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/*
 * VRSuya Core
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 */

namespace VRSuya.Core {

	public static class UnityUtility {

		public static string ColorToHex(Color TargetColor, bool IncludeAlpha = false) {
			int R = Mathf.RoundToInt(TargetColor.r * 255);
			int G = Mathf.RoundToInt(TargetColor.g * 255);
			int B = Mathf.RoundToInt(TargetColor.b * 255);
			int A = Mathf.RoundToInt(TargetColor.a * 255);
			return IncludeAlpha ?
				$"#{R:X2}{G:X2}{B:X2}{A:X2}" :
				$"#{R:X2}{G:X2}{B:X2}";
		}

		public static bool ContainWords(string OriginalString, string TargetString) {
			string[] Words = TargetString.Split(' ');
			return Words.All(Word => OriginalString.Contains(Word));
		}

		public static Vector3 ConvertRGBToHSV(Color TargetColor) {
			float H, S, V;
			Color.RGBToHSV(TargetColor, out H, out S, out V);
			return new Vector3(H, S, V);
		}

		public static string[] GetBlendshapeNameList(SkinnedMeshRenderer TargetSkinnedMeshRenderer) {
			List<string> NewBlendshapeNameList = new List<string>();
			Mesh TargetMesh = TargetSkinnedMeshRenderer.sharedMesh;
			if (TargetMesh.blendShapeCount > 0) {
				for (int Index = 0; Index < TargetMesh.blendShapeCount; Index++) {
					NewBlendshapeNameList.Add(TargetMesh.GetBlendShapeName(Index));
				}
			}
			return NewBlendshapeNameList.ToArray();
		}

		public static string GetHierarchyPath(GameObject TargetGameObject) {
			if (!TargetGameObject) return string.Empty;
			StringBuilder HierarchyPathStringBuilder = new StringBuilder();
			Transform CurrentTransform = TargetGameObject.transform;
			while (CurrentTransform != null) {
				if (HierarchyPathStringBuilder.Length > 0) HierarchyPathStringBuilder.Insert(0, "/");
				HierarchyPathStringBuilder.Insert(0, CurrentTransform.gameObject.name);
				if (!CurrentTransform.parent) break;
				CurrentTransform = CurrentTransform.parent;
			}
			return HierarchyPathStringBuilder.ToString();
		}

		public static List<HumanBodyBones> GetHumanBoneList() {
			return Enum.GetValues(typeof(HumanBodyBones)).Cast<HumanBodyBones>().ToList();
		}

		public static TargetComponent GetOrCreateComponent<TargetComponent>(GameObject TargetGameObject) where TargetComponent : Component {
			TargetComponent Component = TargetGameObject.GetComponent<TargetComponent>();
			if (!Component) Component = TargetGameObject.AddComponent<TargetComponent>();
			return Component;
		}

		public static GameObject GetSourcePrefab(GameObject TargetGameObject) {
			if (!TargetGameObject) return null;
			if (PrefabUtility.GetPrefabInstanceStatus(TargetGameObject) == PrefabInstanceStatus.NotAPrefab) return null;
			GameObject TargetPrefabGameObject = PrefabUtility.GetNearestPrefabInstanceRoot(TargetGameObject);
			if (!TargetPrefabGameObject) return null;
			return PrefabUtility.GetCorrespondingObjectFromSource(TargetPrefabGameObject);
		}

		public static Color HexToColor(string HEXColorCode) {
			if (HEXColorCode.StartsWith("#")) HEXColorCode = HEXColorCode.Substring(1);
			if (HEXColorCode.Length != 6) return Color.black;
			float R = int.Parse(HEXColorCode.Substring(0, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
			float G = int.Parse(HEXColorCode.Substring(2, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
			float B = int.Parse(HEXColorCode.Substring(4, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
			return new Color(R, G, B);
		}

		public static int InitializeUndoGroup(string TargetUndoName) {
			Undo.IncrementCurrentGroup();
			Undo.SetCurrentGroupName(TargetUndoName);
			return Undo.GetCurrentGroup();
		}

		public static bool IsColorCloseToReference(Color TargetColor, Color ReferenceColor, float ToleranceSquared) {
			float DistanceSquared = (TargetColor.r - ReferenceColor.r) * (TargetColor.r - ReferenceColor.r) +
									(TargetColor.g - ReferenceColor.g) * (TargetColor.g - ReferenceColor.g) +
									(TargetColor.b - ReferenceColor.b) * (TargetColor.b - ReferenceColor.b);
			return DistanceSquared < ToleranceSquared;
		}

		public static bool IsPrefabEditingMode() {
			return PrefabStageUtility.GetCurrentPrefabStage() != null;
		}

		public static bool IsVariantModelPrefab(GameObject TargetGameObject) {
			if (!PrefabUtility.IsPartOfVariantPrefab(TargetGameObject)) return false;
			GameObject PrefabSourceGameObject = TargetGameObject;
			string PrefabSourceAssetPath = AssetDatabase.GetAssetPath(TargetGameObject);
			while (PrefabSourceGameObject) {
				string CurrentAssetPath = AssetDatabase.GetAssetPath(PrefabSourceGameObject);
				if (!string.IsNullOrEmpty(CurrentAssetPath)) PrefabSourceAssetPath = CurrentAssetPath;
				GameObject ParentPrefabSourceGameObject = PrefabUtility.GetCorrespondingObjectFromSource(PrefabSourceGameObject);
				if (!ParentPrefabSourceGameObject) break;
				PrefabSourceGameObject = ParentPrefabSourceGameObject;
			}
			if (string.IsNullOrEmpty(PrefabSourceAssetPath)) return false;
			return PrefabSourceAssetPath.EndsWith(".fbx", StringComparison.OrdinalIgnoreCase);
		}

		public static bool SetBlendshapeValue(SkinnedMeshRenderer TargetSkinnedMeshRenderer, string TargetBlendShapeName, float NewValue) {
			int TargetIndex = Array.IndexOf(GetBlendshapeNameList(TargetSkinnedMeshRenderer), TargetBlendShapeName);
			if (TargetIndex != -1) {
				float OldValue = TargetSkinnedMeshRenderer.GetBlendShapeWeight(TargetIndex);
				if (!Mathf.Approximately(OldValue, NewValue)) {
					TargetSkinnedMeshRenderer.SetBlendShapeWeight(TargetIndex, NewValue);
					EditorUtility.SetDirty(TargetSkinnedMeshRenderer);
					return true;
				}
			}
			return false;
		}

		public static void SetMirroredTransform(Transform TargetTransform, Transform TargetMirroredTransform) {
			if (!TargetTransform || !TargetMirroredTransform) return;
			Vector3 MirroredLocalPosition = new Vector3(
				-TargetTransform.localPosition.x,
				TargetTransform.localPosition.y,
				TargetTransform.localPosition.z
			);
			Quaternion MirroredLocalRotation = new Quaternion(
				TargetTransform.rotation.x,
				-TargetTransform.rotation.y,
				-TargetTransform.rotation.z,
				TargetTransform.rotation.w
			);
			TargetMirroredTransform.localPosition = MirroredLocalPosition;
			TargetMirroredTransform.localRotation = MirroredLocalRotation;
			TargetMirroredTransform.localScale = TargetTransform.transform.localScale;
			return;
		}
	}
}
#endif