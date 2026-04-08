using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEngine;

using static VRSuya.Core.Avatar;

/*
 * VRSuya Core
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 */

namespace VRSuya.Core {

	public class Translator {

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

		// 영어 사전 데이터
		static readonly Dictionary<string, string> String_English = new Dictionary<string, string>() {
			{ "String_Advanced", "Advanced" },
			{ "String_AnimationClip", "Animation Clip" },
			{ "String_AnimationOrigin", "Animation Origin" },
			{ "String_AnimationStrength", "Animation Strength" },
			{ "String_Avatar", "Avatar Type" },
			{ "String_AvatarAuthor", "Avatar Author" },
			{ "String_AvatarOrigin", "Avatar Origin" },
			{ "String_AvatarType", "Avatar Type" },
			{ "String_ChangeAnchorOverride", "Change AnchorOverride" },
			{ "String_ChangeBounds", "Change Bounds" },
			{ "String_Debug", "Debug" },
			{ "String_GetAvatarData", "Get Avatar Data" },
			{ "String_GetPosition", "Get cheek bone position" },
			{ "String_ImportSkinnedMeshRenderer", "Import SkinnedMeshRenderer List" },
			{ "String_KeepAnimatorController", "Keep Animator Controller" },
			{ "String_KeepLinkAnimatorLayer", "Keep link Animator Layer with Original" },
			{ "String_Language", "Language" },
			{ "String_NewAvatar", "New Avatar" },
			{ "String_ObjectAnchorOverride", "AnchorOverride GameObject" },
			{ "String_OriginalAvatar", "Original Avatar" },
			{ "String_Reload", "Reload" },
			{ "String_ReorderGameObject", "Reorder Armature" },
			{ "String_ReplaceAvatar", "Replace Avatar" },
			{ "String_RestoreTransform", "Restore Transforms" },
			{ "String_RestPose", "Reset to Rest Pose" },
			{ "String_RootBone", "Avatar Root Bone" },
			{ "String_SetupProduct", "VRSuya products to setup" },
			{ "String_SkinnedMeshRendererList", "Replacement SkinnedMeshRenderer List" },
			{ "String_TargetAnimationBlendShape", "Animated Face Blendshapes" },
			{ "String_TargetAnimations", "Target Animation Clips" },
			{ "String_TargetAvatar", "Target Avatar" },
			{ "String_TargetBlendShape", "Facial Blendshapes with Value" },
			{ "String_TargetFXLayer", "FX Layer" },
			{ "String_TargetMesh", "Face Mesh" },
			{ "String_TwoSidedShadow", "Set to Two-sided Shadow" },
			{ "String_Undo", "Undo" },
			{ "String_UpdateAnimation", "Update Animation" },
			{ "String_UpdateAnimations", "Update Animation Clips" },
			{ "String_UpdateAvatarData", "Update Avatar" },
			{ "String_Okay", "OK" },

			// 아이템명
			{ "String_ProductAFK", "AFK Package" },
			{ "String_ProductMogumogu", "Mogumogu Project" },
			{ "String_ProductWotagei", "Wotagei" },
			{ "String_ProductFeet", "HopeskyD Asiasi Project" },
			{ "String_ProductNyoronyoro", "Nyoronyoro Locomotion" },
			{ "String_ProductModelWalking", "Model Walking" },
			{ "String_ProductHandmotion", "Handmotion" },
			{ "String_ProductSuyasuya", "Suyasuya" },
			{ "String_ProductSoundPad", "SoundPad" },

			// Animated PhysBone
			{ "String_AnimatedPhysBone", "When uploading the avatar, the Animated property of PhysBone components on cheek bones will be enabled." },

			// AvatarPatcher
			{ "COMPLETED_PATCH", "The {0} avatar has been patched" },

			// AvatarSettingUpdater
			{ "String_AvatarSettingUpdater", "The latest VRSuya items are now configured automatically during avatar upload, similar to Modular Avatar-compatible items. AvatarSettingUpdater is no longer used." },
			{ "String_OpenBOOTH", "Open BOOTH" },

			// Hair PhysBone
			{ "String_PhysBoneName", "PhysBone Name" },

			// Remove Animator Layer
			{ "String_LayerName", "Layer Name" },

			// PhysBone Connector
			{ "String_PhysBoneType", "PhysBone Type" },
			{ "String_LeftCheek", "Left Cheek" },
			{ "String_RightCheek", "Right Cheek" },
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

			// Remove PhysBone
			{ "String_RemovePhysBone", "When uploading the avatar, PhysBone components on cheek bones will be removed." },

			// 상태 메시지
			{ "String_General", "Please select [General] for avatars that do not exist in the list" },
			{ "String_KeepAnimatorController_Info", "Don't replace Locomotion and Action Layer, use it if you want edit it yourself" },
			{ "String_Warning", "Other components than SkinnedMeshRenderer will not be copied" },

			// 성공 코드
			{ "COMPLETED", "{0} blendshapes have been added to the animation clip" },
			{ "COMPLETED_GETPOSITION", "Imported cheek bone origin position" },
			{ "COMPLETED_UPDATE", "Updated the offset of the animation clip" },
			{ "UPDATED_RENDERER", "Completed import SkinnedMeshRenderer List" },

			// 에러 코드
			{ "NO_ANIMATOR", "Not found Animator Component in the Avatar!" },
			{ "NO_ANIMSHAPEKEY", "There are no face-related shape keys in the FX layer animation" },
			{ "NO_AVATAR", "No Avatar is selected" },
			{ "NO_CHEEKBONE", "Not found any cheek bone in the Avatar!" },
			{ "NO_CLIPS", "There is no animation clip to update!" },
			{ "NO_FACEMESH", "Face mesh not found" },
			{ "NO_MORE_MENU", "Need {0} more space to add VRC Menu" },
			{ "NO_MORE_PARAMETER", "Need {0} more space to add VRC Parameter" },
			{ "NO_NEW_ANIMATOR", "Not found Animator Component in the New Avatar" },
			{ "NO_OLD_ANIMATOR", "Not found Animator Component in the Original Avatar" },
			{ "NO_ROOTBONE", "Not found Hips bone in the New Avatar" },
			{ "NO_SHAPEKEY", "No shapekeys with values set" },
			{ "NO_SOURCE_FILE", "Not found VRC Assets(likes Animator Controller, Menu, Parameter) in the Avatar" },
			{ "NO_VRCAVATARDESCRIPTOR", "Not found VRC Avatar Descriptor Component in the Avatar" },
			{ "NO_VRCSDK_MENU", "Not found VRC Avatar Menu" },
			{ "NO_VRCSDK_PARAMETER", "Not found VRC Avatar Parameter" },
			{ "NO_VRSUYA_FILE", "VRSuya package is not installed on Unity project" },
			{ "SAME_OBJECT", "Same as the original avatar! Select a new GameObject of the same avatar" }
		};

		// 한국어 사전 데이터
		static readonly Dictionary<string, string> String_Korean = new Dictionary<string, string>() {
			{ "String_Advanced", "고급" },
			{ "String_AnimationClip", "애니메이션 클립" },
			{ "String_AnimationOrigin", "애니메이션 본 원점" },
			{ "String_AnimationStrength", "애니메이션 강도" },
			{ "String_Avatar", "아바타 종류" },
			{ "String_AvatarAuthor", "아바타 제작자" },
			{ "String_AvatarOrigin", "아바타 볼 원점" },
			{ "String_AvatarType", "아바타 타입" },
			{ "String_ChangeAnchorOverride", "AnchorOverride 설정" },
			{ "String_ChangeBounds", "Bounds 설정" },
			{ "String_Debug", "디버그" },
			{ "String_GetAvatarData", "아바타 데이터 가져오기" },
			{ "String_GetPosition", "볼 데이터 가져오기" },
			{ "String_ImportSkinnedMeshRenderer", "스킨드 메쉬 렌더러 목록 가져오기" },
			{ "String_KeepAnimatorController", "애니메이터 유지" },
			{ "String_KeepLinkAnimatorLayer", "애니메이터 레이어를 원본과 연결" },
			{ "String_Language", "언어" },
			{ "String_NewAvatar", "신규 아바타" },
			{ "String_ObjectAnchorOverride", "AnchorOverride 오브젝트" },
			{ "String_OriginalAvatar", "원본 아바타" },
			{ "String_Reload", "다시 불러오기" },
			{ "String_ReorderGameObject", "아마추어 순서 복원" },
			{ "String_ReplaceAvatar", "아바타 교체" },
			{ "String_RestoreTransform", "아바타 트랜스폼 복원" },
			{ "String_RestPose", "기본 포즈로 복원" },
			{ "String_RootBone", "아바타 루트 본" },
			{ "String_SetupProduct", "세팅하려는 VRSuya 제품" },
			{ "String_SkinnedMeshRendererList", "복원될 스킨드 메쉬 렌더러 목록" },
			{ "String_TargetAnimationBlendShape", "애니메이션 얼굴 쉐이프키" },
			{ "String_TargetAnimations", "대상 애니메이션 클립" },
			{ "String_TargetAvatar", "대상 아바타" },
			{ "String_TargetBlendShape", "적용된 얼굴 쉐이프키" },
			{ "String_TargetFXLayer", "FX 레이어" },
			{ "String_TargetMesh", "얼굴 메쉬" },
			{ "String_TwoSidedShadow", "Two-sided 그림자 설정" },
			{ "String_Undo", "실행 취소" },
			{ "String_UpdateAnimation", "애니메이션 업데이트" },
			{ "String_UpdateAnimations", "애니메이션 업데이트" },
			{ "String_UpdateAvatarData", "아바타 업데이트" },
			{ "String_Okay", "확인" },

			// 아이템명
			{ "String_ProductAFK", "AFK 3종 세트" },
			{ "String_ProductMogumogu", "모구모구 프로젝트" },
			{ "String_ProductWotagei", "오타게" },
			{ "String_ProductFeet", "HopeskyD 아시아시 프로젝트" },
			{ "String_ProductNyoronyoro", "뇨로뇨로 로코모션" },
			{ "String_ProductModelWalking", "모델 워킹" },
			{ "String_ProductHandmotion", "핸드모션" },
			{ "String_ProductSuyasuya", "스야스야" },
			{ "String_ProductSoundPad", "사운드패드" },

			// Animated PhysBone
			{ "String_AnimatedPhysBone", "아바타 업로드 할 때, 볼 본의 PhysBone 컴포넌트의 Animated 속성을 활성화 합니다" },

			// AvatarPatcher
			{ "COMPLETED_PATCH", "{0} 아바타를 패치하였습니다" },

			// AvatarSettingUpdater
			{ "String_AvatarSettingUpdater", "최신 VRSuya 아이템은 모듈러 아바타 대응 아이템처럼 이제 아바타 업로드시에 자동으로 설정합니다, AvatarSettingUpdater는 더 이상 사용하지 않습니다" },
			{ "String_OpenBOOTH", "BOOTH 열기" },

			// Hair PhysBone
			{ "String_PhysBoneName", "PhysBone 이름" },

			// Remove Animator Layer
			{ "String_LayerName", "레이어 이름" },

			// PhysBone Connector
			{ "String_PhysBoneType", "PhysBone 종류" },
			{ "String_LeftCheek", "왼쪽 볼" },
			{ "String_RightCheek", "오른쪽 볼" },
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

			// Remove PhysBone
			{ "String_RemovePhysBone", "아바타 업로드 할 때, 볼 본의 PhysBone 컴포넌트들을 제거합니다" },

			// 상태 메시지
			{ "String_General", "목록에 존재하지 않는 아바타는 [일반]을 선택해 주세요" },
			{ "String_RemoveAFKLayer", "아바타 업로드할 때 FX 레이어에서 기존 AFK 레이어를 제거합니다" },
			{ "String_Warning", "스킨드 메쉬 렌더러 외의 속성은 가져오지 않습니다!" },

			// 성공 코드
			{ "COMPLETED", "{0}개의 블렌드쉐이프 애니메이션 키가 추가되었습니다" },
			{ "COMPLETED_GETPOSITION", "볼 위치 데이터를 가져왔습니다" },
			{ "COMPLETED_UPDATE", "애니메이션 클립의 오프셋을 업데이트 하였습니다" },
			{ "UPDATED_RENDERER", "복원될 스킨드 메쉬 렌더러 목록을 가져왔습니다" },

			// 에러 코드
			{ "NO_ANIMATOR", "아바타에서 애니메이터를 찾을 수 없습니다!" },
			{ "NO_ANIMSHAPEKEY", "FX 레이어의 애니메이션에서 얼굴 관련 쉐이프키가 없습니다" },
			{ "NO_AVATAR", "아바타가 지정되지 않았습니다" },
			{ "NO_CHEEKBONE", "아바타에서 볼 본을 찾을 수 없습니다!" },
			{ "NO_CLIPS", "작업할 애니메이션 클립이 없습니다!" },
			{ "NO_FACEMESH", "얼굴 메쉬를 찾을 수 없습니다" },
			{ "NO_MORE_MENU", "VRC 메뉴를 추가할 공간이 {0}개 부족합니다" },
			{ "NO_MORE_PARAMETER", "VRC 파라메터를 추가할 공간이 {0}개 부족합니다" },
			{ "NO_NEW_ANIMATOR", "새 아바타에서 애니메이터를 찾을 수 없습니다" },
			{ "NO_OLD_ANIMATOR", "원본 아바타에서 애니메이터를 찾을 수 없습니다" },
			{ "NO_ROOTBONE", "아바타에서 루트 본을 찾을 수 없습니다" },
			{ "NO_SHAPEKEY", "값이 설정된 쉐이프키가 없습니다" },
			{ "NO_SOURCE_FILE", "아바타에서 VRC용 에셋(애니메이터, 메뉴, 파라메터)을 찾을 수 없습니다" },
			{ "NO_VRCAVATARDESCRIPTOR", "아바타에서 VRC 아바타 디스크립터를 찾을 수 없습니다" },
			{ "NO_VRCSDK_MENU", "VRC 메뉴가 존재하지 않습니다" },
			{ "NO_VRCSDK_PARAMETER", "VRC 파라메터가 존재하지 않습니다" },
			{ "NO_VRSUYA_FILE", "Unity 프로젝트에 VRSuya 패키지가 설치되어 있지 않습니다" },
			{ "SAME_OBJECT", "원본과 같은 아바타입니다, 복구하려는 아바타와 같은 종류의 아바타를 만들어 넣어주세요" }
		};

		// 일본어 사전 데이터
		static readonly Dictionary<string, string> String_Japanese = new Dictionary<string, string>() {
			{ "String_Advanced", "詳細" },
			{ "String_AnimationClip", "アニメーション·クリップ" },
			{ "String_AnimationOrigin", "アニメーションほっぺの原点" },
			{ "String_AnimationStrength", "アニメーション強盗" },
			{ "String_Avatar", "アバタータイプ" },
			{ "String_AvatarAuthor", "アバター製作者" },
			{ "String_AvatarOrigin", "アバターほっぺの原点" },
			{ "String_AvatarType", "アバタータイプ" },
			{ "String_ChangeAnchorOverride", "AnchorOverride設定" },
			{ "String_ChangeBounds", "Bounds設定" },
			{ "String_Debug", "デバッグ" },
			{ "String_GetAvatarData", "アバターデータの取得" },
			{ "String_GetPosition", "ほっぺデータのインポート" },
			{ "String_ImportSkinnedMeshRenderer", "SkinnedMeshRendererリストを取得" },
			{ "String_KeepAnimatorController", "Animator 維持" },
			{ "String_KeepLinkAnimatorLayer", "アニメーターレイヤーを原本と接続" },
			{ "String_Language", "言語" },
			{ "String_NewAvatar", "新規アバター" },
			{ "String_ObjectAnchorOverride", "AnchorOverrideオブジェクト" },
			{ "String_OriginalAvatar", "原本アバター" },
			{ "String_Reload", "リロード" },
			{ "String_ReorderGameObject", "アーマチュア順序復元" },
			{ "String_ReplaceAvatar", "アバター交換" },
			{ "String_RestoreTransform", "アバターTransform復元" },
			{ "String_RestPose", "基本ポーズに復元" },
			{ "String_RootBone", "アバタールートボーン" },
			{ "String_SetupProduct", "セットしようとするVRSuya製品" },
			{ "String_SkinnedMeshRendererList", "復元されるSkinnedMeshRenderer一覧" },
			{ "String_TargetAnimationBlendShape", "アニメーション顔シェイプキー" },
			{ "String_TargetAnimations", "対象アニメーションクリップ" },
			{ "String_TargetAvatar", "対象アバター" },
			{ "String_TargetBlendShape", "適用された顔のシェイプキー" },
			{ "String_TargetFXLayer", "FXレイヤー" },
			{ "String_TargetMesh", "顔メッシュ" },
			{ "String_TwoSidedShadow", "Two-sided影設定" },
			{ "String_Undo", "元に戻す" },
			{ "String_UpdateAnimation", "アニメーション·アップデート" },
			{ "String_UpdateAnimations", "アニメーション·アップデート" },
			{ "String_UpdateAvatarData", "アバターアップデート" },
			{ "String_Okay", "確認" },

			// 아이템명
			{ "String_ProductAFK", "AFK 3種セット" },
			{ "String_ProductMogumogu", "もぐもぐ プロジェクト" },
			{ "String_ProductWotagei", "ヲタ芸" },
			{ "String_ProductFeet", "HopeskyD 足足プロジェクト" },
			{ "String_ProductNyoronyoro", "にょろにょろ ロコモーション" },
			{ "String_ProductModelWalking", "ランウェイ・モデルウォーク" },
			{ "String_ProductHandmotion", "ハンドモーション" },
			{ "String_ProductSuyasuya", "すやすや" },
			{ "String_ProductSoundPad", "サウンドパッド" },

			// Animated PhysBone
			{ "String_AnimatedPhysBone", "アバターをアップロードする際、頬ボーンのPhysBoneコンポーネントのAnimatedプロパティが有効化されます。" },

			// AvatarPatcher
			{ "COMPLETED_PATCH", "{0}アバターをパッチしました。" },

			// AvatarSettingUpdater
			{ "String_AvatarSettingUpdater", "最新のVRSuyaアイテムはModular Avatar対応アイテムと同様に、アバターのアップロード時に自動で設定されるようになりました。AvatarSettingUpdaterは今後使用されません。" },
			{ "String_OpenBOOTH", "BOOTHを開く" },

			// Hair PhysBone
			{ "String_PhysBoneName", "PhysBone名" },

			// Remove Animator Layer
			{ "String_LayerName", "レイヤー名" },

			// PhysBone Connector
			{ "String_PhysBoneType", "PhysBoneタイプ" },
			{ "String_LeftCheek", "左頬" },
			{ "String_RightCheek", "右頬" },
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

			// Remove PhysBone
			{ "String_RemovePhysBone", "アバターをアップロードする際、頬ボーンのPhysBoneコンポーネントは削除されます。" },

			// 상태 메시지
			{ "String_General", "リストに存在しないアバターは「一般」を選択してください" },
			{ "String_RemoveAFKLayer", "アバターをアップロードする際、FXレイヤー内の既存のAFKレイヤーは削除されます。" },
			{ "String_Warning", "SkinnedMeshRenderer以外のプロパティは取得しません！" },

			// 성공 코드
			{ "COMPLETED", "{0}個のブレンドシェイプアニメーションキーが追加されました" },
			{ "COMPLETED_GETPOSITION", "ほっぺ位置データを取得しました" },
			{ "COMPLETED_UPDATE", "アニメーション·クリップのオフセットを更新しました" },
			{ "UPDATED_RENDERER", "復元されるSkinnedMeshRendererのリストを取得しました。" },

			// 에러 코드
			{ "NO_ANIMATOR", "アバターにアニメーターが見つかりません" },
			{ "NO_ANIMSHAPEKEY", "FXレイヤーのアニメーションで顔関連のシェイプキーがありません" },
			{ "NO_AVATAR", "アバターが指定されていません" },
			{ "NO_CHEEKBONE", "アバターにほっぺの骨が見つかりません！" },
			{ "NO_CLIPS", "作業するアニメーション·クリップがありません！" },
			{ "NO_FACEMESH", "顔のメッシュが見つかりません" },
			{ "NO_MORE_MENU", "VRCメニューを追加するスペース{0}スロットが不足しています" },
			{ "NO_MORE_PARAMETER", "VRCパラメータを追加するスペース{0}スロットが不足しています" },
			{ "NO_NEW_ANIMATOR", "新しいアバターにアニメーターが見つかりません" },
			{ "NO_OLD_ANIMATOR", "元のアバターにアニメーターが見つかりません" },
			{ "NO_ROOTBONE", "アバターにルートボーンが見つかりません" },
			{ "NO_SHAPEKEY", "値が設定されたシェイプキーがありません" },
			{ "NO_SOURCE_FILE", "アバターにVRC用アセット(アニメーター、メニュー、パラメータ)が見つかりません" },
			{ "NO_VRCAVATARDESCRIPTOR", "アバターにVRCアバターディスクリプターが見つかりません" },
			{ "NO_VRCSDK_MENU", "VRCメニューが存在しません" },
			{ "NO_VRCSDK_PARAMETER", "VRCパラメータが存在しません" },
			{ "NO_VRSUYA_FILE", "UnityプロジェクトにVRSuyaパッケージがインストールされていません" },
			{ "SAME_OBJECT", "原本と同じアバターです、復旧したいアバターと同じ種類のアバターを作って入れてください" }
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