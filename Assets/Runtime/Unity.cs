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
	}
}
#endif