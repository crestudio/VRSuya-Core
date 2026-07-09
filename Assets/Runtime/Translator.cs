#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEngine;

using static VRSuya.Core.AvatarUtility;

/*
 * VRSuya Core
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 */

namespace VRSuya.Core {

	public static class Translator {

		public static readonly string[] LanguageOption = new string[] { "English", "한국어", "日本語" };
		public static int LanguageIndex = 0;

		static Translator() {
			LanguageIndex = GetSystemLanguage();
		}

		static int GetSystemLanguage() {
			SystemLanguage TargetLanguage = Application.systemLanguage;
			switch (TargetLanguage) {
				case SystemLanguage.Korean:
					return 1;
				case SystemLanguage.Japanese:
					return 2;
				default:
					return 0;
			}
		}

		public static string GetTranslatedString(string TargetString) {
			string NewString = TargetString;
			switch (LanguageIndex) {
				case 0:
					if (String_English.ContainsKey(TargetString)) NewString = String_English[TargetString];
					break;
				case 1:
					if (String_Korean.ContainsKey(TargetString)) NewString = String_Korean[TargetString];
					break;
				case 2:
					if (String_Japanese.ContainsKey(TargetString)) NewString = String_Japanese[TargetString];
					break;
			}
			return NewString;
		}

		public static string[] GetAvatarAuthorName(SerializedProperty TargetProperty) {
			int AvatarAuthorCount = TargetProperty.arraySize;
			AvatarAuthor[] AvatarAuthorNames = new AvatarAuthor[AvatarAuthorCount];
			for (int Index = 0; Index < AvatarAuthorCount; Index++) {
				SerializedProperty ArrayItem = TargetProperty.GetArrayElementAtIndex(Index);
				string AvatarAuthorEnumName = ArrayItem.enumNames[ArrayItem.enumValueIndex];
				AvatarAuthorNames[Index] = (AvatarAuthor)Enum.Parse(typeof(AvatarAuthor), AvatarAuthorEnumName);
			}
			return AvatarAuthorNames
				.Where((AvatarAuthorName) => AvatarAuthorNameList.ContainsKey(AvatarAuthorName))
				.Select((AvatarAuthorName) => AvatarAuthorNameList[AvatarAuthorName][LanguageIndex])
				.ToArray();
		}

		public static string[] GetAvatarName(SerializedProperty TargetProperty) {
			int AvatarNameCount = TargetProperty.arraySize;
			AvatarType[] AvatarNames = new AvatarType[AvatarNameCount];
			for (int Index = 0; Index < AvatarNameCount; Index++) {
				SerializedProperty ArrayItem = TargetProperty.GetArrayElementAtIndex(Index);
				string AvatarEnumName = ArrayItem.enumNames[ArrayItem.enumValueIndex];
				AvatarNames[Index] = (AvatarType)Enum.Parse(typeof(AvatarType), AvatarEnumName);
			}
			return AvatarNames
				.Where((AvatarName) => AvatarNameList.ContainsKey(AvatarName))
				.Select((AvatarName) => AvatarNameList[AvatarName][LanguageIndex])
				.ToArray();
		}

		public static string[] GetLanguageOption() {
			switch (LanguageIndex) {
				case 0:
					return new string[] { "Auto", "English", "Korean", "Japanese" };
				case 1:
					return new string[] { "자동", "영어", "한국어", "일본어" };
				case 2:
					return new string[] { "自動", "英語", "韓国語", "日本語" };
				default:
					return new string[] { "Auto", "English", "Korean", "Japanese" };
			}
		}

		public static string[] GetPhysBoneOption() {
			switch (LanguageIndex) {
				case 0:
					return new string[] { "Cheek", "Toe" };
				case 1:
					return new string[] { "볼", "발가락" };
				case 2:
					return new string[] { "頬", "足指" };
				default:
					return new string[] { "Cheek", "Toe" };
			}
		}

		// 영어 사전 데이터
		static readonly Dictionary<string, string> String_English = new Dictionary<string, string>() {
			{ "String_Active", "Active" },
			{ "String_Add", "Add" },
			{ "String_After", "After" },
			{ "String_AnimationClip", "Animation Clip" },
			{ "String_AnimationOrigin", "Animation Origin" },
			{ "String_AnimationStrength", "Animation Strength" },
			{ "String_Apply", "Apply" },
			{ "String_Avatar", "Avatar" },
			{ "String_AvatarAuthor", "Avatar Author" },
			{ "String_AvatarOrigin", "Avatar Origin" },
			{ "String_Before", "Before" },
			{ "String_BlendShape", "Blendshape" },
			{ "String_Browse", "Browse" },
			{ "String_Close", "Close" },
			{ "String_FXLayer", "FX Layer" },
			{ "String_GetPosition", "Get cheek bone position" },
			{ "String_HeadMesh", "Face Mesh" },
			{ "String_Hide", "Hide" },
			{ "String_Language", "Language" },
			{ "String_Material", "Materials" },
			{ "String_NewAvatar", "New Avatar" },
			{ "String_Okay", "OK" },
			{ "String_OldAvatar", "Original Avatar" },
			{ "String_Parameter", "Parameters" },
			{ "String_Path", "Path" },
			{ "String_Refresh", "Refresh" },
			{ "String_Reload", "Reload" },
			{ "String_Remove", "Remove" },
			{ "String_Replace", "Replace" },
			{ "String_Save", "Save" },
			{ "String_Show", "Show" },
			{ "String_Strength", "Strength" },
			{ "String_Texture", "Textures" },
			{ "String_Undo", "Undo" },
			{ "String_Update", "Update" },

			// AnimatedHairPhysBone
			{ "String_PhysBoneName", "PhysBone Name" },

			// AnimatedPhysBone
			{ "String_AnimatedPhysBone", "When uploading the avatar, the Animated property of PhysBone components on cheek bones will be enabled." },

			// AnimatorView
			{ "String_FollowGameObject", "Follow GameObject" },
			{ "String_LockRotation", "Lock Camera Rotation" },

			// AvatarPatcher
			{ "String_AvatarPatcher", "This tool is currently under development, We're working to release an update as soon as possible" },
			{ "COMPLETED_PATCH", "The {0} avatar has been patched" },

			// AvatarRebuilder
			{ "String_AvatarRebuilder", "This tool is used to replace avatar models that have been patched in Blender\nIf you want to patch your avatar directly in Unity, please use the following menu instead of this tool\nTools → VRSuya → Installer → HDiffPatcher" },
			{ "NO_NEW_ANIMATOR", "Not found Animator Component in the New Avatar" },
			{ "NO_NEW_AVATAR", "No New Avatar is selected" },
			{ "NO_MATCHED_SKINNEDMESHRENDERERS", "No matching SkinnedMeshRenderer was found" },
			{ "NO_OLD_ANIMATOR", "Not found Animator Component in the Original Avatar" },
			{ "NO_OLD_AVATAR_SCENE", "The avatar is not placed in the scene" },
			{ "NO_OLD_AVATAR", "No Avatar is selected" },
			{ "SAME_AVATAR", "Same as the original avatar! Select a new GameObject of the same avatar" },

			// AvatarScaler
			{ "String_AvatarHeight", "Avatar Height (cm)" },

			// AvatarSettingUpdater
			{ "String_AvatarSettingUpdater", "The latest VRSuya items are now configured automatically during avatar upload, similar to Modular Avatar-compatible items. AvatarSettingUpdater is no longer used." },
			{ "String_OpenBOOTH", "Open BOOTH" },

			// ChangeStandingPose
			{ "String_ChangeStandingPose", "Replaces the default VRChat standing pose in the Action Layer with the avatar's standing pose." },

			// ConstraintConnector
			{ "String_LeftHand", "Left Hand" },
			{ "String_RightHand", "Right Hand" },

			// FixFacialAnimation
			{ "String_AddBlink", "Add Blink Shapekey" },
			{ "String_AddLayerControl", "Add Layer Control" },
			{ "String_GetAvatarData", "Get Avatar Data" },
			{ "String_LayerIndex", "Layer Index" },

			// ForceOnWriteDefaults
			{ "String_ForceOnWriteDefaults", "Sets Write Defaults to ON for the FX layer.\nSome avatar gimmicks may not function correctly as a result. If this occurs, removing this component will resolve the issue; however, facial expression animations provided by VRSuya items may no longer work correctly." },

			// HDiffPatcher
			{ "String_HDiffPatcher", "Patches can only be applied to the original avatar model file\nIf the model file has been modified (like facial patch), use the AvatarPatcher add-on in Blender to patch the modified model, then replace the avatar model using AvatarRebuilder" },
			{ "String_PatchData", "Patch Data" },
			{ "String_ReplaceAfterPatch", "Replace Avatar After Patching" },
			{ "ERROR_CONSOLE", "An error occurred while patching the avatar! Please check the error message in the Unity Console window" },
			{ "ERROR_FAILEDRUN", "Failed to launch HDiffPatch" },
			{ "ERROR_FBX", "The source file path is invalid" },
			{ "ERROR_HDIFF", "The HDiff patch file path is invalid" },
			{ "ERROR_NOHDIFFPATCH", "Could not find the HDiffPatch executable" },
			{ "ERROR_NOPERMISSION", "Failed to grant execute permission to HDiffPatch" },
			{ "ERROR_OUTPUTPATH", "Exporting is only supported within the Unity project's Assets folder, Please select a different path" },
			{ "ERROR_PLATFORM", "HDiffPatch supports Windows, macOS, and Linux only" },
			{ "ERROR_TIMEDOUT", "The HDiffPatch process timed out" },
			{ "NOT_MATCH", "Failed to apply the HDiff patch because the selected source file does not match the patch, Please select the correct source file" },

			// MenuSelector
			{ "String_MenuLanguage", "Menu Language" },

			// PhysBoneConnector
			{ "String_PhysBoneType", "PhysBone Type" },
			{ "String_LeftCheek", "Left Cheek" },
			{ "String_RightCheek", "Right Cheek" },
			{ "String_LeftToe", "Left Toe" },
			{ "String_RightToe", "Right Toe" },
			{ "String_LeftThumbToe", "Left Thumb Toe" },
			{ "String_RightThumbToe", "Right Thumb Toe" },
			{ "String_LeftIndexToe", "Left Index Toe" },
			{ "String_RightIndexToe", "Right Index Toe" },
			{ "String_LeftMiddleToe", "Left Middle Toe" },
			{ "String_RightMiddleToe", "Right Middle Toe" },
			{ "String_LeftRingToe", "Left Ring Toe" },
			{ "String_RightRingToe", "Right Ring Toe" },
			{ "String_LeftLittleToe", "Left Little Toe" },
			{ "String_RightLittleToe", "Right Little Toe" },

			// RemoveAnimatorLayer
			{ "String_LayerName", "Layer Name" },

			// RemoveFXMask
			{ "String_RemoveFXMask", "Removes the mask if one is assigned to the FX layer." },

			// RemovePhysBone
			{ "String_RemovePhysBone", "When uploading the avatar, PhysBone components on cheek bones will be removed." },

			// TextureReplacer
			{ "String_Null", "Clearing the item will remove the texture from the material" },
			{ "NO_DATA", "The texture cannot be found in the specified object" },

			// 성공 코드
			{ "COMPLETED_GETPOSITION", "Imported cheek bone origin position" },
			{ "COMPLETED_UPDATE", "Updated the offset of the animation clip" },

			// 에러 코드
			{ "NO_ANIMATOR", "Not found Animator Component in the Avatar!" },
			{ "NO_ANIMSHAPEKEY", "There are no face-related shape keys in the FX layer animation" },
			{ "NO_CHEEKBONE", "Not found any cheek bone in the Avatar!" },
			{ "NO_CLIPS", "There is no animation clip to update!" },
			{ "NO_FACEMESH", "Face mesh not found" },
			{ "NO_PREFAB_MODE", "This operation is not available in Prefab Mode" },
			{ "NO_SHAPEKEY", "No shapekeys with values set" }
		};

		// 한국어 사전 데이터
		static readonly Dictionary<string, string> String_Korean = new Dictionary<string, string>() {
			{ "String_Active", "활성화" },
			{ "String_Add", "추가" },
			{ "String_After", "변경 후" },
			{ "String_AnimationClip", "애니메이션 클립" },
			{ "String_AnimationOrigin", "애니메이션 본 원점" },
			{ "String_AnimationStrength", "애니메이션 강도" },
			{ "String_Apply", "적용" },
			{ "String_Avatar", "아바타" },
			{ "String_AvatarAuthor", "아바타 제작자" },
			{ "String_AvatarOrigin", "아바타 볼 원점" },
			{ "String_Before", "변경 전" },
			{ "String_BlendShape", "쉐이프키" },
			{ "String_Browse", "찾아보기" },
			{ "String_Close", "닫기" },
			{ "String_FXLayer", "FX 레이어" },
			{ "String_GetPosition", "볼 데이터 가져오기" },
			{ "String_HeadMesh", "얼굴 메쉬" },
			{ "String_Hide", "숨기기" },
			{ "String_Language", "언어" },
			{ "String_Material", "머테리얼" },
			{ "String_NewAvatar", "신규 아바타" },
			{ "String_Okay", "확인" },
			{ "String_OldAvatar", "원본 아바타" },
			{ "String_Parameter", "파라메터" },
			{ "String_Path", "경로" },
			{ "String_Refresh", "새로 고침" },
			{ "String_Reload", "다시 불러오기" },
			{ "String_Remove", "삭제" },
			{ "String_Replace", "교체" },
			{ "String_Save", "저장" },
			{ "String_Show", "표시" },
			{ "String_Strength", "강도" },
			{ "String_Texture", "텍스쳐" },
			{ "String_Undo", "실행 취소" },
			{ "String_Update", "업데이트" },

			// AnimatedHairPhysBone
			{ "String_PhysBoneName", "PhysBone 이름" },

			// AnimatedPhysBone
			{ "String_AnimatedPhysBone", "아바타 업로드 할 때, 볼 본의 PhysBone 컴포넌트의 Animated 속성을 활성화 합니다" },

			// AnimatorView
			{ "String_FollowGameObject", "GameObject 추적" },
			{ "String_LockRotation", "카메라 회전 고정" },

			// AvatarPatcher
			{ "String_AvatarPatcher", "현재 프로그램은 개발 중입니다, 빠르게 업데이트 할 수 있도록 하겠습니다" },
			{ "COMPLETED_PATCH", "{0} 아바타를 패치하였습니다" },

			// AvatarRebuilder
			{ "String_AvatarRebuilder", "Blender에서 패치한 아바타 모델을 교체하기 위한 프로그램 입니다\nUnity에서 바로 아바타를 패치하시려면, 현재 프로그램 대신 아래의 메뉴를 이용하여 아바타 모델 패치를 진행해 주세요\nTools → VRSuya → Installer → HDiffPatcher" },
			{ "NO_NEW_ANIMATOR", "새 아바타에서 애니메이터를 찾을 수 없습니다" },
			{ "NO_NEW_AVATAR", "새 아바타가 지정되지 않았습니다" },
			{ "NO_MATCHED_SKINNEDMESHRENDERERS", "서로 매치가 되는 SkinnedMeshRenderer가 존재하지 않습니다" },
			{ "NO_OLD_ANIMATOR", "원본 아바타에서 애니메이터를 찾을 수 없습니다" },
			{ "NO_OLD_AVATAR_SCENE", "아바타가 Scene에 위치하고 있지 않습니다" },
			{ "NO_OLD_AVATAR", "아바타가 지정되지 않았습니다" },
			{ "SAME_AVATAR", "원본과 같은 아바타입니다, 복구하려는 아바타와 같은 종류의 아바타를 만들어 넣어주세요" },

			// AvatarScaler
			{ "String_AvatarHeight", "아바타 키 (cm)" },

			// AvatarSettingUpdater
			{ "String_AvatarSettingUpdater", "최신 VRSuya 아이템은 모듈러 아바타 대응 아이템처럼 이제 아바타 업로드시에 자동으로 설정합니다, AvatarSettingUpdater는 더 이상 사용하지 않습니다" },
			{ "String_OpenBOOTH", "BOOTH 열기" },

			// ChangeStandingPose
			{ "String_ChangeStandingPose", "액션 레이어의 기본 VRChat 스탠드 포즈를 아바타의 스탠드 포즈로 바꿉니다" },

			// ConstraintConnector
			{ "String_LeftHand", "왼손" },
			{ "String_RightHand", "오른손" },

			// FixFacialAnimation
			{ "String_AddBlink", "블링크 쉐이프키 추가" },
			{ "String_AddLayerControl", "레이어 컨트롤 추가" },
			{ "String_GetAvatarData", "아바타 데이터 업데이트" },
			{ "String_LayerIndex", "레이어 인덱스" },

			// ForceOnWriteDefaults
			{ "String_ForceOnWriteDefaults", "FX 레이어를 Write Defaults를 ON으로 설정합니다\n일부 아바타의 기믹이 제대로 동작하지 않을 수 있습니다, 이러한 경우에는 현재 컴포넌트를 제거하면 문제가 해결이 되나 VRSuya 아이템의 표정 애니메이션이 제대로 동작하지 않습니다" },

			// HDiffPatcher
			{ "String_HDiffPatcher", "순정 아바타 모델 파일만 패치를 적용할 수 있습니다\n페이셜 패치 등으로 모델 파일을 수정한 경우에는 Blender에서 AvatarPatcher 애드온으로 수정된 모델으로 패치를 진행한 모델 파일로 AvatarRebuilder에서 교체 작업을 해야 합니다" },
			{ "String_PatchData", "패치 데이터" },
			{ "String_ReplaceAfterPatch", "패치 후 아바타 교체" },
			{ "ERROR_CONSOLE", "아바타 패치 도중 에러가 발생하였습니다, Unity의 Console 창에서 오류 메시지를 확인해 주세요" },
			{ "ERROR_FAILEDRUN", "HDiffPatch를 실행하는데 실패하였습니다" },
			{ "ERROR_FBX", "원본 파일의 경로가 올바르지 않습니다" },
			{ "ERROR_HDIFF", "HDiff 패치 파일의 경로가 올바르지 않습니다" },
			{ "ERROR_NOHDIFFPATCH", "HDiffPatch 실행 파일을 찾을 수 없습니다" },
			{ "ERROR_NOPERMISSION", "HDiffPatch 실행 권한 부여에 실패하였습니다" },
			{ "ERROR_OUTPUTPATH", "Unity 프로젝트의 Assets 폴더 내부로만 내보내기를 할 수 있습니다, 다시 경로를 지정해 주세요" },
			{ "ERROR_PLATFORM", "HDiffPatch는 윈도우, 맥, 리눅스만 지원합니다" },
			{ "ERROR_TIMEDOUT", "HDiffPatch 프로세스의 작업 시간을 초과하였습니다" },
			{ "NOT_MATCH", "선택한 원본 파일이 패치 데이터와 일치하지 않아 HDiff 패치를 적용할 수 없었습니다, 올바른 원본 파일을 선택해 주세요" },

			// MenuSelector
			{ "String_MenuLanguage", "메뉴 언어" },

			// PhysBoneConnector
			{ "String_PhysBoneType", "PhysBone 종류" },
			{ "String_LeftCheek", "왼쪽 볼" },
			{ "String_RightCheek", "오른쪽 볼" },
			{ "String_LeftToe", "왼쪽 발가락" },
			{ "String_RightToe", "오른쪽 발가락" },
			{ "String_LeftThumbToe", "왼쪽 엄지 발가락" },
			{ "String_RightThumbToe", "오른쪽 엄지 발가락" },
			{ "String_LeftIndexToe", "왼쪽 검지 발가락" },
			{ "String_RightIndexToe", "오른쪽 검지 발가락" },
			{ "String_LeftMiddleToe", "왼쪽 중지 발가락" },
			{ "String_RightMiddleToe", "오른쪽 중지 발가락" },
			{ "String_LeftRingToe", "왼쪽 약지 발가락" },
			{ "String_RightRingToe", "오른쪽 약지 발가락" },
			{ "String_LeftLittleToe", "왼쪽 소지 발가락" },
			{ "String_RightLittleToe", "오른쪽 소지 발가락" },

			// RemoveAnimatorLayer
			{ "String_LayerName", "레이어 이름" },

			// RemoveFXMask
			{ "String_RemoveFXMask", "FX 레이어에 마스크가 할당되어 있는 경우 마스크를 삭제를 합니다" },

			// RemovePhysBone
			{ "String_RemovePhysBone", "아바타 업로드 할 때, 볼 본의 PhysBone 컴포넌트들을 제거합니다" },

			// TextureReplacer
			{ "String_Null", "항목을 비우면 해당 텍스쳐를 머테리얼에서 제거합니다" },
			{ "NO_DATA", "해당 오브젝트에서 텍스쳐를 찾을 수 없습니다" },

			// 성공 코드
			{ "COMPLETED_GETPOSITION", "볼 위치 데이터를 가져왔습니다" },
			{ "COMPLETED_UPDATE", "애니메이션 클립의 오프셋을 업데이트 하였습니다" },

			// 에러 코드
			{ "NO_ANIMATOR", "아바타에서 애니메이터를 찾을 수 없습니다!" },
			{ "NO_ANIMSHAPEKEY", "FX 레이어의 애니메이션에서 얼굴 관련 쉐이프키가 없습니다" },
			{ "NO_CHEEKBONE", "아바타에서 볼 본을 찾을 수 없습니다!" },
			{ "NO_CLIPS", "작업할 애니메이션 클립이 없습니다!" },
			{ "NO_FACEMESH", "얼굴 메쉬를 찾을 수 없습니다" },
			{ "NO_PREFAB_MODE", "Prefab 편집 모드에서는 진행할 수 없습니다" },
			{ "NO_SHAPEKEY", "값이 설정된 쉐이프키가 없습니다" }
		};

		// 일본어 사전 데이터
		static readonly Dictionary<string, string> String_Japanese = new Dictionary<string, string>() {
			{ "String_Active", "有効化" },
			{ "String_Add", "追加" },
			{ "String_After", "変更" },
			{ "String_AnimationClip", "アニメーション·クリップ" },
			{ "String_AnimationOrigin", "アニメーションほっぺの原点" },
			{ "String_AnimationStrength", "アニメーション強盗" },
			{ "String_Apply", "適用" },
			{ "String_Avatar", "アバター" },
			{ "String_AvatarAuthor", "アバター製作者" },
			{ "String_AvatarOrigin", "アバターほっぺの原点" },
			{ "String_Before", "既存" },
			{ "String_BlendShape", "シェイプキー" },
			{ "String_Browse", "参照" },
			{ "String_Close", "閉じる" },
			{ "String_FXLayer", "FXレイヤー" },
			{ "String_GetPosition", "ほっぺデータのインポート" },
			{ "String_HeadMesh", "顔メッシュ" },
			{ "String_Hide", "非表示" },
			{ "String_Language", "言語" },
			{ "String_Material", "マテリアル" },
			{ "String_NewAvatar", "新規アバター" },
			{ "String_Okay", "確認" },
			{ "String_OldAvatar", "原本アバター" },
			{ "String_Parameter", "パラメータ" },
			{ "String_Path", "パス" },
			{ "String_Refresh", "更新" },
			{ "String_Reload", "リロード" },
			{ "String_Remove", "削除" },
			{ "String_Replace", "交換" },
			{ "String_Save", "保存" },
			{ "String_Show", "表示" },
			{ "String_Strength", "強度" },
			{ "String_Texture", "テクスチャ" },
			{ "String_Undo", "元に戻す" },
			{ "String_Update", "アップデート" },

			// AnimatedHairPhysBone
			{ "String_PhysBoneName", "PhysBone名" },

			// AnimatedPhysBone
			{ "String_AnimatedPhysBone", "アバターをアップロードする際、頬ボーンのPhysBoneコンポーネントのAnimatedプロパティが有効化されます" },

			// AnimatorView
			{ "String_FollowGameObject", "GameObjectを追従" },
			{ "String_LockRotation", "カメラ回転固定" },

			// AvatarPatcher
			{ "String_AvatarPatcher", "このツールは現在開発中です、できるだけ早く公開できるよう開発を進めております" },
			{ "COMPLETED_PATCH", "{0}アバターをパッチしました" },

			// AvatarRebuilder
			{ "String_AvatarRebuilder", "このツールはBlenderでパッチを適用したアバターモデルを差し替えるためのツールです\nUnity上で直接アバターをパッチしたい場合は、このツールではなく、以下のメニューからアバターモデルのパッチを実行してください\nTools → VRSuya → Installer → HDiffPatcher" },
			{ "NO_NEW_ANIMATOR", "新しいアバターにアニメーターが見つかりません" },
			{ "NO_NEW_AVATAR", "新しいアバターが指定されていません" },
			{ "NO_MATCHED_SKINNEDMESHRENDERERS", "対応するSkinnedMeshRendererが見つかりません" },
			{ "NO_OLD_ANIMATOR", "元のアバターにアニメーターが見つかりません" },
			{ "NO_OLD_AVATAR_SCENE", "アバターがシーン内に配置されていません" },
			{ "NO_OLD_AVATAR", "アバターが指定されていません" },
			{ "SAME_AVATAR", "原本と同じアバターです、復旧したいアバターと同じ種類のアバターを作って入れてください" },

			// AvatarScaler
			{ "String_AvatarHeight", "アバターの高さ (cm)" },

			// AvatarSettingUpdater
			{ "String_AvatarSettingUpdater", "最新のVRSuyaアイテムはModular Avatar対応アイテムと同様に、アバターのアップロード時に自動で設定されるようになりました、AvatarSettingUpdaterは今後使用されません" },
			{ "String_OpenBOOTH", "BOOTHを開く" },

			// ChangeStandingPose
			{ "String_ChangeStandingPose", "Actionレイヤー内のデフォルトVRChatスタンドポーズを、アバターのスタンドポーズに変更します" },

			// ConstraintConnector
			{ "String_LeftHand", "左手" },
			{ "String_RightHand", "右手" },

			// FixFacialAnimation
			{ "String_AddBlink", "まばたきシェイプキーを追加" },
			{ "String_AddLayerControl", "レイヤーコントロールを追加" },
			{ "String_GetAvatarData", "アバターデータを追加" },
			{ "String_LayerIndex", "レイヤーインデックス" },

			// ForceOnWriteDefaults
			{ "String_ForceOnWriteDefaults", "FXレイヤーのWrite DefaultsをONに設定します\nその影響により、一部のアバターギミックが正常に動作しなくなる場合があります、その場合は、このコンポーネントを削除することで問題を解決できますが、VRSuyaアイテムの表情アニメーションが正常に動作しなくなる可能性があります" },
			
			// HDiffPatcher
			{ "String_HDiffPatcher", "パッチはオリジナルのアバターモデルファイルにのみ適用できます\nフェイシャルパッチなどでモデルファイルを変更している場合は、BlenderのAvatarPatcherアドオンを使用して変更済みモデルにパッチを適用し、その後AvatarRebuilderでアバターモデルを差し替えてください" },
			{ "String_PatchData", "パッチデータ" },
			{ "String_ReplaceAfterPatch", "パッチ後にアバターを置き換える" },
			{ "ERROR_CONSOLE", "アバターのパッチ中にエラーが発生しました！UnityのConsoleウィンドウでエラーメッセージを確認してください" },
			{ "ERROR_FAILEDRUN", "HDiffPatchの起動に失敗しました" },
			{ "ERROR_FBX", "元ファイルのパスが無効です" },
			{ "ERROR_HDIFF", "HDiffパッチファイルのパスが無効です" },
			{ "ERROR_NOHDIFFPATCH", "HDiffPatchの実行ファイルが見つかりません" },
			{ "ERROR_NOPERMISSION", "HDiffPatchへの実行権限の付与に失敗しました" },
			{ "ERROR_OUTPUTPATH", "UnityプロジェクトのAssetsフォルダー内にのみエクスポートできます、パスを再指定してください" },
			{ "ERROR_PLATFORM", "HDiffPatchはWindows、macOS、Linuxのみサポートしています" },
			{ "ERROR_TIMEDOUT", "HDiffPatchプロセスがタイムアウトしました" },
			{ "NOT_MATCH", "選択された元ファイルがパッチと一致しないため、HDiffパッチを適用できませんでした、正しい元ファイルを選択してください" },

			// MenuSelector
			{ "String_MenuLanguage", "メニュー言語" },

			// PhysBoneConnector
			{ "String_PhysBoneType", "PhysBoneタイプ" },
			{ "String_LeftCheek", "左頬" },
			{ "String_RightCheek", "右頬" },
			{ "String_LeftToe", "左指" },
			{ "String_RightToe", "右指" },
			{ "String_LeftThumbToe", "左親指" },
			{ "String_RightThumbToe", "右親指" },
			{ "String_LeftIndexToe", "左人差し指" },
			{ "String_RightIndexToe", "右人差し指" },
			{ "String_LeftMiddleToe", "左中指" },
			{ "String_RightMiddleToe", "右中指" },
			{ "String_LeftRingToe", "左薬指" },
			{ "String_RightRingToe", "右薬指" },
			{ "String_LeftLittleToe", "左小指" },
			{ "String_RightLittleToe", "右小指" },

			// RemoveAnimatorLayer
			{ "String_LayerName", "レイヤー名" },

			// RemoveFXMask
			{ "String_RemoveFXMask", "FXレイヤーにマスクが割り当てられている場合、マスクを削除します" },

			// RemovePhysBone
			{ "String_RemovePhysBone", "アバターをアップロードする際、頬ボーンのPhysBoneコンポーネントは削除されます" },

			// TextureReplacer
			{ "String_Null", "項目をクリアすると、該当テクスチャがマテリアルから削除されます" },
			{ "NO_DATA", "該当オブジェクトでテクスチャを見つけることができません" },

			// 성공 코드
			{ "COMPLETED_GETPOSITION", "ほっぺ位置データを取得しました" },
			{ "COMPLETED_UPDATE", "アニメーション·クリップのオフセットを更新しました" },

			// 에러 코드
			{ "NO_ANIMATOR", "アバターにアニメーターが見つかりません" },
			{ "NO_ANIMSHAPEKEY", "FXレイヤーのアニメーションで顔関連のシェイプキーがありません" },
			{ "NO_CHEEKBONE", "アバターにほっぺの骨が見つかりません！" },
			{ "NO_CLIPS", "作業するアニメーション·クリップがありません！" },
			{ "NO_FACEMESH", "顔のメッシュが見つかりません" },
			{ "NO_PREFAB_MODE", "Prefab編集モードでは実行できません" },
			{ "NO_SHAPEKEY", "値が設定されたシェイプキーがありません" }
		};

		static readonly Dictionary<AvatarAuthor, string[]> AvatarAuthorNameList = new Dictionary<AvatarAuthor, string[]>() {
			{ AvatarAuthor.General, new string[] { "General", "일반", "一般" } },
			{ AvatarAuthor.ChocolateRice, new string[] { "Chocolate rice", "초콜렛 라이스", "チョコレートライス" } },
			{ AvatarAuthor.JINGO, new string[] { "JINGO", "진권", "ジンゴ" } },
			{ AvatarAuthor.Komado, new string[] { "Komado", "코마도", "こまど" } },
			{ AvatarAuthor.Plusone, new string[] { "Plusone", "플러스원", "ぷらすわん" } }
		};

		static readonly Dictionary<AvatarType, string[]> AvatarNameList = new Dictionary<AvatarType, string[]>() {
			{ AvatarType.General, new string[] { "General", "일반", "一般" } },
			{ AvatarType.None, new string[] { "None", "없음", "無い" } },
			{ AvatarType.Airi, new string[] { "Airi", "아이리", "愛莉" } },
			{ AvatarType.Aldina, new string[] { "Aldina", "알디나", "アルディナ" } },
			{ AvatarType.Angura, new string[] { "Angura", "앙그라", "アングラ" } },
			{ AvatarType.Anon, new string[] { "Anon", "아논", "あのん" } },
			{ AvatarType.Anri, new string[] { "Anri", "안리", "杏里" } },
			{ AvatarType.Ash, new string[] { "Ash", "애쉬", "アッシュ" } },
			{ AvatarType.Chiffon, new string[] { "Chiffon", "쉬폰", "シフォン" } },
			{ AvatarType.Chise, new string[] { "Chise", "치세", "チセ" } },
			{ AvatarType.Chocolat, new string[] { "Chocolat", "쇼콜라", "ショコラ" } },
			{ AvatarType.Cygnet, new string[] { "Cygnet", "시그넷", "シグネット" } },
			{ AvatarType.Eku, new string[] { "Eku", "에쿠", "エク" } },
			{ AvatarType.Emmelie, new string[] { "Emmelie", "에밀리", "Emmelie" } },
			{ AvatarType.EYO, new string[] { "EYO", "이요", "イヨ" } },
			{ AvatarType.Firina, new string[] { "Firina", "휘리나", "フィリナ" } },
			{ AvatarType.Flare, new string[] { "Flare", "플레어", "フレア" } },
			{ AvatarType.Fuzzy, new string[] { "Fuzzy", "퍼지", "ファジー" } },
			{ AvatarType.Glaze, new string[] { "Glaze", "글레이즈", "ぐれーず" } },
			{ AvatarType.Grus, new string[] { "Grus", "그루스", "Grus" } },
			{ AvatarType.Hakka, new string[] { "Hakka", "하카", "薄荷" } },
			{ AvatarType.IMERIS, new string[] { "IMERIS", "이메리스", "イメリス" } },
			{ AvatarType.Karin, new string[] { "Karin", "카린", "カリン" } },
			{ AvatarType.Kikyo, new string[] { "Kikyo", "키쿄", "桔梗" } },
			{ AvatarType.Kipfel, new string[] { "Kipfel", "키펠", "キプフェル" } },
			{ AvatarType.Kokoa, new string[] { "Kokoa", "코코아", "ここあ" } },
			{ AvatarType.Koyuki, new string[] { "Koyuki", "코유키", "狐雪" } },
			{ AvatarType.KUMALY, new string[] { "KUMALY", "쿠마리", "クマリ" } },
			{ AvatarType.Kuronatu, new string[] { "Kuronatu", "쿠로나츠", "くろなつ" } },
			{ AvatarType.Lapwing, new string[] { "Lapwing", "랩윙", "Lapwing" } },
			{ AvatarType.Lazuli, new string[] { "Lazuli", "라줄리", "ラズリ" } },
			{ AvatarType.Leefa, new string[] { "Leefa", "리파", "リーファ" } },
			{ AvatarType.Leeme, new string[] { "Leeme", "리메", "リーメ" } },
			{ AvatarType.Lime, new string[] { "Lime", "라임", "ライム" } },
			{ AvatarType.LUMINA, new string[] { "LUMINA", "루미나", "ルミナ" } },
			{ AvatarType.Lunalitt, new string[] { "Lunalitt", "루나릿트", "ルーナリット" } },
			{ AvatarType.Mafuyu, new string[] { "Mafuyu", "마후유", "真冬" } },
			{ AvatarType.Maki, new string[] { "Maki", "마키", "碼希" } },
			{ AvatarType.Mamehinata, new string[] { "Mamehinata", "마메히나타", "まめひなた" } },
			{ AvatarType.MANUKA, new string[] { "MANUKA", "마누카", "マヌカ" } },
			{ AvatarType.Mariel, new string[] { "Mariel", "마리엘", "まりえる" } },
			{ AvatarType.Marron, new string[] { "Marron", "마론", "マロン" } },
			{ AvatarType.Maya, new string[] { "Maya", "마야", "舞夜" } },
			{ AvatarType.MAYO, new string[] { "MAYO", "마요", "まよ" } },
			{ AvatarType.Merino, new string[] { "Merino", "메리노", "メリノ" } },
			{ AvatarType.Milfy, new string[] { "Milfy", "미르피", "ミルフィ" } },
			{ AvatarType.Milk, new string[] { "Milk(New)", "밀크(신)", "ミルク（新）" } },
			{ AvatarType.Milltina, new string[] { "Milltina", "밀티나", "ミルティナ" } },
			{ AvatarType.Minahoshi, new string[] { "Minahoshi", "미나호시", "みなほし" } },
			{ AvatarType.Minase, new string[] { "Minase", "미나세", "水瀬" } },
			{ AvatarType.Mint, new string[] { "Mint", "민트", "ミント" } },
			{ AvatarType.Mir, new string[] { "Mir", "미르", "ミール" } },
			{ AvatarType.Mishe, new string[] { "Mishe", "미셰", "ミーシェ" } },
			{ AvatarType.Moe, new string[] { "Moe", "모에", "萌" } },
			{ AvatarType.Nayu, new string[] { "Nayu", "나유", "ナユ" } },
			{ AvatarType.Nehail, new string[] { "Nehail", "네하일", "ネハイル" } },
			{ AvatarType.Nochica, new string[] { "Nochica", "노치카", "ノーチカ" } },
			{ AvatarType.Platinum, new string[] { "Platinum", "플레티늄", "プラチナ" } },
			{ AvatarType.Plum, new string[] { "Plum", "플럼", "プラム" } },
			{ AvatarType.Pochimaru, new string[] { "Pochimaru", "포치마루", "ぽちまる" } },
			{ AvatarType.Quiche, new string[] { "Quiche", "킷슈", "キッシュ" } },
			{ AvatarType.Rainy, new string[] { "Rainy", "레이니", "レイニィ" } },
			{ AvatarType.Ramune, new string[] { "Ramune", "라무네", "ラムネ" } },
			{ AvatarType.Ramune_Old, new string[] { "Ramune(Old)", "라무네(구)", "ラムネ（古）" } },
			{ AvatarType.RINDO, new string[] { "RINDO", "린도", "竜胆" } },
			{ AvatarType.Rokona, new string[] { "Rokona", "로코나", "ロコナ" } },
			{ AvatarType.Rue, new string[] { "Rue", "루에", "ルウ" } },
			{ AvatarType.Rurune, new string[] { "Rurune", "루루네", "ルルネ" } },
			{ AvatarType.Rusk, new string[] { "Rusk", "러스크", "ラスク" } },
			{ AvatarType.SELESTIA, new string[] { "SELESTIA", "셀레스티아", "セレスティア" } },
			{ AvatarType.Sephira, new string[] { "Sephira", "세피라", "セフィラ" } },
			{ AvatarType.Shinano, new string[] { "Shinano", "시나노", "しなの" } },
			{ AvatarType.Shinra, new string[] { "Shinra", "신라", "森羅" } },
			{ AvatarType.SHIRAHA, new string[] { "SHIRAHA", "시라하", "シラハ" } },
			{ AvatarType.Shiratsume, new string[] { "Shiratsume", "시라츠메", "しらつめ" } },
			{ AvatarType.Sio, new string[] { "Sio", "시오", "しお" } },
			{ AvatarType.Sue, new string[] { "Sue", "스우", "透羽" } },
			{ AvatarType.Sugar, new string[] { "Sugar", "슈가", "シュガ" } },
			{ AvatarType.Suzuhana, new string[] { "Suzuhana", "스즈하나", "すずはな" } },
			{ AvatarType.Tien, new string[] { "Tien", "티엔", "ティエン" } },
			{ AvatarType.TubeRose, new string[] { "TubeRose", "튜베로즈", "TubeRose" } },
			{ AvatarType.Ukon, new string[] { "Ukon", "우콘", "右近" } },
			{ AvatarType.Usasaki, new string[] { "Usasaki", "우사사키", "うささき" } },
			{ AvatarType.Uzuki, new string[] { "Uzuki", "우즈키", "卯月" } },
			{ AvatarType.VIVH, new string[] { "VIVH", "비브", "ビィブ" } },
			{ AvatarType.Wolferia, new string[] { "Wolferia", "울페리아", "ウルフェリア" } },
			{ AvatarType.Yoll, new string[] { "Yoll", "요루", "ヨル" } },
			{ AvatarType.YUGI_MIYO, new string[] { "YUGI MIYO", "유기 미요", "ユギ ミヨ" } },
			{ AvatarType.Yuuko, new string[] { "Yuuko", "유우코", "幽狐" } }
			// 검색용 신규 아바타 추가 위치
		};
	}
}
#endif