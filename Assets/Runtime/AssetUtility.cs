#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

using static VRC.SDK3.Avatars.Components.VRCAvatarDescriptor;
using VRC.SDK3.Avatars.ScriptableObjects;

using Object = UnityEngine.Object;

/*
 * VRSuya Core
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 */

namespace VRSuya.Core {

	public static class AssetUtility {

		public enum AssetType {
			AnimatorController,
			Prefab,
			Scene,
			VRCMenu,
			VRCParameter
		}

		public static bool ContainAnimationClip(Object[] TargetObjects) {
			return TargetObjects
				.Select(Item => AssetDatabase.GetAssetPath(Item).EndsWith(".anim"))
				.Contains(true);
		}

		public static bool ContainAnimatorController(Object[] TargetObjects) {
			return TargetObjects
				.Select(Item => AssetDatabase.GetAssetPath(Item).EndsWith(".controller"))
				.Contains(true);
		}

		public static bool ContainAsset(Object[] TargetObjects) {
			return TargetObjects
				.Select(Item => AssetDatabase.GetAssetPath(Item).EndsWith(".asset"))
				.Contains(true);
		}

		public static bool ContainPrefab(Object[] TargetObjects) {
			return TargetObjects
				.Select(Item => AssetDatabase.GetAssetPath(Item).EndsWith(".prefab"))
				.Contains(true);
		}

		public static bool ContainScene(Object[] TargetObjects) {
			return TargetObjects
				.Select(Item => AssetDatabase.GetAssetPath(Item).EndsWith(".unity"))
				.Contains(true);
		}

		public static GameObject ExportPrefab(GameObject TargetGameObject, string TargetAssetPath, string TargetAssetName) {
			string FullExportPath = Path.Combine(TargetAssetPath, $"{TargetAssetName}.prefab");
			FullExportPath = FullExportPath.Replace("\\", "/");
			if (!Directory.Exists(TargetAssetPath)) {
				Directory.CreateDirectory(TargetAssetPath);
				AssetDatabase.Refresh();
			}
			GameObject CreatedPrefab = PrefabUtility.SaveAsPrefabAsset(TargetGameObject, FullExportPath, out bool PrefabCreated);
			if (PrefabCreated) {
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
			return CreatedPrefab;
		}

		public static Material[] GetAnimationMaterials(GameObject TargetGameObject) {
			List<Material> AnimationMaterials = new List<Material>();
			AnimatorController AvatarFXAnimator = AvatarUtility.GetAnimatorController(TargetGameObject, AnimLayerType.FX);
			if (AvatarFXAnimator) {
				AnimationClip[] AllAnimationClips = AnimatorHelper.GetAllAnimationClips(AvatarFXAnimator);
				AnimationMaterials.AddRange(AllAnimationClips
					.SelectMany(TargetAnimationClip => AnimationUtility.GetObjectReferenceCurveBindings(TargetAnimationClip)
						.Where(TargetBinding => TargetBinding.type == typeof(SkinnedMeshRenderer) || TargetBinding.type == typeof(MeshRenderer))
						.SelectMany(TargetBinding => AnimationUtility.GetObjectReferenceCurve(TargetAnimationClip, TargetBinding) ?? new ObjectReferenceKeyframe[0])
						.Where(TargetKeyframe => TargetKeyframe.value is Material)
						.Select(TargetKeyframe => TargetKeyframe.value as Material)
					)
				);
			}
			AnimationMaterials = AnimationMaterials.Where(Item => Item != null).Distinct().OrderBy(Item => Item.name).ToList();
			return AnimationMaterials.ToArray();
		}

		public static string[] GetAssetGUIDs(AssetType TargetType) {
			string SearchWord = string.Empty;
			switch (TargetType) {
				case AssetType.AnimatorController:
					SearchWord = "t:AnimatorController";
					break;
				case AssetType.Prefab:
					SearchWord = "t:Prefab";
					break;
				case AssetType.Scene:
					SearchWord = "t:Scene";
					break;
				case AssetType.VRCMenu:
				case AssetType.VRCParameter:
					SearchWord = "glob:\"*.asset\"";
					break;
			}
			List<string> AssetGUIDs = AssetDatabase.FindAssets(SearchWord, new[] { "Assets/" }).ToList();
			if (TargetType == AssetType.VRCMenu || TargetType == AssetType.VRCParameter) {
				for (int Index = AssetGUIDs.Count - 1; Index >= 0; Index--) {
					switch (TargetType) {
						case AssetType.VRCMenu:
							VRCExpressionsMenu VRCMenuAsset = AssetDatabase.LoadAssetAtPath<VRCExpressionsMenu>(AssetDatabase.GUIDToAssetPath(AssetGUIDs[Index]));
							if (!VRCMenuAsset || VRCMenuAsset is not VRCExpressionsMenu) {
								AssetGUIDs.RemoveAt(Index);
							}
							break;
						case AssetType.VRCParameter:
							VRCExpressionParameters VRCParameterAsset = AssetDatabase.LoadAssetAtPath<VRCExpressionParameters>(AssetDatabase.GUIDToAssetPath(AssetGUIDs[Index]));
							if (!VRCParameterAsset || VRCParameterAsset is not VRCExpressionParameters) {
								AssetGUIDs.RemoveAt(Index);
							}
							break;
					}
				}
			}
			return AssetGUIDs.ToArray();
		}

		public static string GetAssetName(string TargetPath, bool OnlyFileName) {
			return OnlyFileName ? Path.GetFileNameWithoutExtension(TargetPath) : Path.GetFileName(TargetPath);
		}

		public static string GetAvatarName(string TargetAssetName) {
			string[] AssetNameParts = TargetAssetName.Split('_');
			string[] AvatarNames = AvatarUtility.GetAvatarNames();
			return AssetNameParts.FirstOrDefault(Item => AvatarNames.Contains(Item, StringComparer.OrdinalIgnoreCase));
		}

		public static string GetDirectoryPath(string TargetAssetPath) {
			return Path.GetDirectoryName(TargetAssetPath).Replace('\\', '/');
		}

		public static Texture2D[] GetMaterialTextures(Material TargetMaterial) {
			List<Texture2D> NewTexture2Ds = new List<Texture2D>();
			if (TargetMaterial) {
				Shader TargetShader = TargetMaterial.shader;
				int PropertyCount = ShaderUtil.GetPropertyCount(TargetShader);
				for (int Index = 0; Index < PropertyCount; Index++) {
					if (ShaderUtil.GetPropertyType(TargetShader, Index) == ShaderUtil.ShaderPropertyType.TexEnv) {
						string PropertyName = ShaderUtil.GetPropertyName(TargetShader, Index);
						Texture MaterialTexture = TargetMaterial.GetTexture(PropertyName);
						if (MaterialTexture && MaterialTexture is Texture2D) {
							NewTexture2Ds.Add(MaterialTexture as Texture2D);
						}
					}
				}
			}
			return NewTexture2Ds.Distinct().OrderBy(Item => Item.name).ToArray();
		}

		public static string GetUnityAssetPath(string TargetFilePath) {
			string AssetPath = Path.GetFullPath(TargetFilePath).Replace('\\', '/');
			string UnityProjectAssetsPath = Path.GetFullPath(Application.dataPath + "/..").Replace('\\', '/');
			return AssetPath.Substring(UnityProjectAssetsPath.Length + 1);
		}

		public static string GUIDToAssetName(string TargetGUID, bool OnlyFileName) {
			return GetAssetName(AssetDatabase.GUIDToAssetPath(TargetGUID), OnlyFileName);
		}

		public static bool IsTextureNormalMap(Texture2D TargetTexture) {
			if (!TargetTexture) return false;
			Color ReferenceColor = new Color(0.5f, 0.5f, 1.0f);
			float Tolerance = 0.3f;
			Color[] Pixels = TargetTexture.GetPixels();
			int ClosePixelCount = 0;
			float ToleranceSquared = Tolerance * Tolerance;
			foreach (Color Pixel in Pixels) {
				if (UnityUtility.IsColorCloseToReference(Pixel, ReferenceColor, ToleranceSquared)) {
					ClosePixelCount++;
				}
			}
			float ClosePixelRatio = (float)ClosePixelCount / Pixels.Length;
			return ClosePixelRatio > 0.5f;
		}

		public static void PingAsset(string TargetAssetPath) {
			if (string.IsNullOrEmpty(TargetAssetPath)) return;
			Object TargetAssetObject = AssetDatabase.LoadAssetAtPath<Object>(TargetAssetPath);
			if (!TargetAssetObject) return;
			EditorGUIUtility.PingObject(TargetAssetObject);
			Selection.activeObject = TargetAssetObject;
		}

		public static string ReplaceAvatarName(string TargetAssetName, string NewAvatarName) {
			string[] AssetNameParts = TargetAssetName.Split('_');
			string[] AvatarNames = AvatarUtility.GetAvatarNames();
			for (int Index = AssetNameParts.Length - 1; Index >= 0; Index--) {
				if (AvatarNames.Contains(AssetNameParts[Index], StringComparer.OrdinalIgnoreCase)) {
					AssetNameParts[Index] = NewAvatarName;
					break;
				}
			}
			return string.Join("_", AssetNameParts);
		}
	}
}
#endif