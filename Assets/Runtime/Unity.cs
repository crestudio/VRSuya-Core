#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/*
 * VRSuya Core
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 */

namespace VRSuya.Core {

	[ExecuteInEditMode]
	public class Unity {

		/// <summary>Unity Undo 그룹 기능을 초기화 합니다.</summary>
		/// <param name="TargetUndoName">원하는 Undo 그룹 이름</param>
		/// <returns>Undo 그룹 인덱스</returns>
		public static int InitializeUndoGroup(string TargetUndoName) {
			Undo.IncrementCurrentGroup();
			Undo.SetCurrentGroupName(TargetUndoName);
			return Undo.GetCurrentGroup();
		}

		/// <summary>HEX 문자열을 Color로 변환합니다.</summary>
		/// <param name="HEXColorCode">변환할 HEX 문자열 (예: #RRGGBB)</param>
		/// <returns>변환된 Color 객체</returns>
		public static Color HexToColor(string HEXColorCode) {
			if (HEXColorCode.StartsWith("#")) HEXColorCode = HEXColorCode.Substring(1);
			if (HEXColorCode.Length != 6) return Color.black;
			float R = int.Parse(HEXColorCode.Substring(0, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
			float G = int.Parse(HEXColorCode.Substring(2, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
			float B = int.Parse(HEXColorCode.Substring(4, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
			return new Color(R, G, B);
		}

		/// <summary>Color를 HEX 문자열로 변환합니다.</summary>
		/// <param name="TargetColor">변환할 Color</param>
		/// <param name="IncludeAlpha">알파 값을 포함할지 여부</param>
		/// <returns>변환된 HEX 문자열</returns>
		public static string ColorToHex(Color TargetColor, bool IncludeAlpha = false) {
			int R = Mathf.RoundToInt(TargetColor.r * 255);
			int G = Mathf.RoundToInt(TargetColor.g * 255);
			int B = Mathf.RoundToInt(TargetColor.b * 255);
			int A = Mathf.RoundToInt(TargetColor.a * 255);
			return IncludeAlpha ?
				$"#{R:X2}{G:X2}{B:X2}{A:X2}" :
				$"#{R:X2}{G:X2}{B:X2}";
		}

		/// <summary>RGB Color를 HSV 값으로 변환합니다</summary>
		/// <param name="TargetColor">변환할 RGB Color</param>
		/// <returns>HSV Vector3</returns>
		public static Vector3 ConvertRGBToHSV(Color TargetColor) {
			float H, S, V;
			Color.RGBToHSV(TargetColor, out H, out S, out V);
			return new Vector3(H, S, V);
		}

		/// <summary>요청한 유형의 컴포넌트가 존재하는지 확인하고 존재하지 않는다면 생성해서 반환합니다.</summary>
		/// <returns>요청한 유형 컴포넌트</returns>
		public static TargetComponent GetOrCreateComponent<TargetComponent>(GameObject TargetGameObject) where TargetComponent : Component {
			TargetComponent Component = TargetGameObject.GetComponent<TargetComponent>();
			if (!Component) Component = TargetGameObject.AddComponent<TargetComponent>();
			return Component;
		}
	}
}
#endif