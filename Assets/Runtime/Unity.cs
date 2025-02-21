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
	}
}
#endif