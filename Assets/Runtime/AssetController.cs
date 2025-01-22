#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEngine;

using VRC.SDK3.Avatars.ScriptableObjects;
using VRSuya.Core.Avatar;

/*
 * VRSuya Core
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 */

namespace VRSuya.Core.Asset {

	[ExecuteInEditMode]
	public static class AssetController {

		public enum AssetType {
			AnimatorController,
			Scene,
			VRCMenu,
			VRCParameter
		}

		/// <summary>요청한 타입의 파일 GUID 어레이를 가져옵니다</summary>
		/// <returns>요청한 타입의 파일 GUID 어레이</returns>
		public static string[] GetAssetGUIDs(AssetType TargetType) {
			List<string> AssetGUIDs = new List<string>();
			string SearchWord = string.Empty;
			string SearchPath = "Assets/";
			switch (TargetType) {
				case AssetType.AnimatorController:
					SearchWord = "t:AnimatorController";
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

		/// <summary>요청한 GUID를 파일 이름으로 반환합니다. 2번째 인자는 확장명 포함 여부를 결정합니다.</summary>
		/// <returns>파일 이름</returns>
		public static string GUIDToAssetName(string GUID, bool OnlyFileName) {
			string FileName = "";
			FileName = AssetDatabase.GUIDToAssetPath(GUID).Split('/')[AssetDatabase.GUIDToAssetPath(GUID).Split('/').Length - 1];
			if (OnlyFileName) FileName = FileName.Split('.')[0];
			return FileName;
		}

		/// <summary>파일 이름에서 아바타 이름을 찾아 아바타 이름을 반환합니다.</summary>
		/// <param name="OriginalFileName">원본 파일 이름</param>
		/// <returns>아바타 이름</returns>
		public static string GetAvatarName(string OriginalFileName) {
			string[] FileNameParts = OriginalFileName.Split('_');
			for (int Index = FileNameParts.Length - 1; Index >= 0; Index--) {
				if (IsAvatarName(FileNameParts[Index])) {
					return FileNameParts[Index];
				}
			}
			return null;
		}

		/// <summary>파일 이름에서 아바타 이름을 찾아 다른 이름으로 바꾼 문자열을 반환합니다.</summary>
		/// <param name="OriginalFileName">원본 파일 이름</param>
		/// <param name="NewAvatarName">새로운 아바타 이름</param>
		/// <returns>변경된 파일 이름</returns>
		public static string ReplaceAvatarName(string OriginalFileName, string NewAvatarName) {
			string[] FileNameParts = OriginalFileName.Split('_');
			for (int Index = FileNameParts.Length - 1; Index >= 0; Index--) {
				if (IsAvatarName(FileNameParts[Index])) {
					FileNameParts[Index] = NewAvatarName;
					break;
				}
			}
			return string.Join("_", FileNameParts);
		}

		/// <summary>문자열이 아바타 이름인지 판단하는 메서드</summary>
		/// <param name="TargetWord">검사할 문자열</param>
		/// <returns>아바타 이름 여부</returns>
		public static bool IsAvatarName(string TargetWord) {
			string[] AvatarNames = AvatarController.GetAvatarNames();
			return AvatarNames.Contains(TargetWord);
		}
	}
}
#endif