#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;
using UnityEngine;

using VRC.SDK3.Avatars.ScriptableObjects;

/*
 * VRSuya Core
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 */

namespace VRSuya.Core {

	[ExecuteInEditMode]
	public class Asset {

		public enum AssetType {
			AnimatorController,
			Prefab,
			Scene,
			VRCMenu,
			VRCParameter
		}

		public string[] GetAssetGUIDs(AssetType TargetType) {
			List<string> AssetGUIDs = new List<string>();
			string SearchWord = string.Empty;
			string SearchPath = "Assets/";
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
			AssetGUIDs = AssetDatabase.FindAssets(SearchWord, new[] { SearchPath }).ToList();
			if (TargetType == AssetType.VRCMenu || TargetType == AssetType.VRCParameter) {
				for (int Index = AssetGUIDs.Count - 1; Index >= 0; Index--) {
					switch (TargetType) {
						case AssetType.VRCMenu:
							VRCExpressionsMenu VRCMenuAsset = AssetDatabase.LoadAssetAtPath<VRCExpressionsMenu>(AssetDatabase.GUIDToAssetPath(AssetGUIDs[Index]));
							if (!VRCMenuAsset || VRCMenuAsset.GetType() != typeof(VRCExpressionsMenu)) {
								AssetGUIDs.RemoveAt(Index);
							}
							break;
						case AssetType.VRCParameter:
							VRCExpressionParameters VRCParameterAsset = AssetDatabase.LoadAssetAtPath<VRCExpressionParameters>(AssetDatabase.GUIDToAssetPath(AssetGUIDs[Index]));
							if (!VRCParameterAsset || VRCParameterAsset.GetType() != typeof(VRCExpressionParameters)) {
								AssetGUIDs.RemoveAt(Index);
							}
							break;
					}
				}
			}
			return AssetGUIDs.ToArray();
		}

		public string GUIDToAssetName(string GUID, bool OnlyFileName) {
			string FileName = string.Empty;
			FileName = AssetDatabase.GUIDToAssetPath(GUID).Split('/')[AssetDatabase.GUIDToAssetPath(GUID).Split('/').Length - 1];
			if (OnlyFileName) FileName = FileName.Split('.')[0];
			return FileName;
		}

		public string GetAvatarName(string OriginalFileName) {
			Avatar AvatarInstance = new Avatar();
			string[] FileNameParts = OriginalFileName.Split('_');
			string[] AvatarNames = AvatarInstance.GetAvatarNames();
			for (int Index = FileNameParts.Length - 1; Index >= 0; Index--) {
				if (AvatarNames.Contains(FileNameParts[Index])) {
					return FileNameParts[Index];
				}
			}
			return null;
		}

		public string ReplaceAvatarName(string OriginalFileName, string NewAvatarName) {
			Avatar AvatarInstance = new Avatar();
			string[] FileNameParts = OriginalFileName.Split('_');
			string[] AvatarNames = AvatarInstance.GetAvatarNames();
			for (int Index = FileNameParts.Length - 1; Index >= 0; Index--) {
				if (AvatarNames.Contains(FileNameParts[Index])) {
					FileNameParts[Index] = NewAvatarName;
					break;
				}
			}
			return string.Join("_", FileNameParts);
		}

		public GameObject ExportPrefab(GameObject TargetGameObject, string TargetAssetPath, string TargetAssetName) {
			string FullExportPath = Path.Combine(TargetAssetPath, $"{TargetAssetName}.prefab");
			FullExportPath = FullExportPath.Replace("\\", "/");
			if (!Directory.Exists(TargetAssetPath)) {
				Directory.CreateDirectory(TargetAssetPath);
				AssetDatabase.Refresh();
			}
			GameObject CreatedPrefab = PrefabUtility.SaveAsPrefabAsset(TargetGameObject, FullExportPath, out bool PrefabCreated);
			if (PrefabCreated) {
				Debug.Log($"[VRSuya] {TargetAssetName} 프리팹을 성공적으로 내보내기 하였습니다");
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
			return CreatedPrefab;
		}

		public bool ContainAnimationClip(Object[] TargetObjects) {
			return TargetObjects
				.Select(Item => AssetDatabase.GetAssetPath(Item).EndsWith(".anim"))
				.Contains(true);
		}

		public bool ContainAnimatorController(Object[] TargetObjects) {
			return TargetObjects
				.Select(Item => AssetDatabase.GetAssetPath(Item).EndsWith(".controller"))
				.Contains(true);
		}

		public bool ContainPrefab(Object[] TargetObjects) {
			return TargetObjects
				.Select(Item => AssetDatabase.GetAssetPath(Item).EndsWith(".prefab"))
				.Contains(true);
		}

		public bool ContainScene(Object[] TargetObjects) {
			return TargetObjects
				.Select(Item => AssetDatabase.GetAssetPath(Item).EndsWith(".unity"))
				.Contains(true);
		}
	}
}
#endif