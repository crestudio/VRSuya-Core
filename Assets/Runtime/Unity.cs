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

		public static int InitializeUndoGroup(string TargetUndoName) {
			Undo.IncrementCurrentGroup();
			Undo.SetCurrentGroupName(TargetUndoName);
			return Undo.GetCurrentGroup();
		}

		public static Color HexToColor(string HEXColorCode) {
			if (HEXColorCode.StartsWith("#")) HEXColorCode = HEXColorCode.Substring(1);
			if (HEXColorCode.Length != 6) return Color.black;
			float R = int.Parse(HEXColorCode.Substring(0, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
			float G = int.Parse(HEXColorCode.Substring(2, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
			float B = int.Parse(HEXColorCode.Substring(4, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
			return new Color(R, G, B);
		}

		public static string ColorToHex(Color TargetColor, bool IncludeAlpha = false) {
			int R = Mathf.RoundToInt(TargetColor.r * 255);
			int G = Mathf.RoundToInt(TargetColor.g * 255);
			int B = Mathf.RoundToInt(TargetColor.b * 255);
			int A = Mathf.RoundToInt(TargetColor.a * 255);
			return IncludeAlpha ?
				$"#{R:X2}{G:X2}{B:X2}{A:X2}" :
				$"#{R:X2}{G:X2}{B:X2}";
		}

		public static Vector3 ConvertRGBToHSV(Color TargetColor) {
			float H, S, V;
			Color.RGBToHSV(TargetColor, out H, out S, out V);
			return new Vector3(H, S, V);
		}

		public static TargetComponent GetOrCreateComponent<TargetComponent>(GameObject TargetGameObject) where TargetComponent : Component {
			TargetComponent Component = TargetGameObject.GetComponent<TargetComponent>();
			if (!Component) Component = TargetGameObject.AddComponent<TargetComponent>();
			return Component;
		}
	}
}
#endif