#if UNITY_EDITOR
using System;

using UnityEditor;
using UnityEngine;

using Object = UnityEngine.Object;

/*
 * VRSuya Core
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 * Forked from ModLunar ( https://forum.unity.com/threads/solved-duplicate-prefab-issue.778553/ )
 */

namespace VRSuya.Core {

	public static class DuplicateUtility {

		public static EditorWindow GetEditorWindow(Type TargetEditorWindowType) {
			if (TargetEditorWindowType == null) return null;
			if (!typeof(EditorWindow).IsAssignableFrom(TargetEditorWindowType)) return null;
			Object[] TargetEditorWindows = Resources.FindObjectsOfTypeAll(TargetEditorWindowType);
			if (TargetEditorWindows.Length <= 0) return null;
			EditorWindow TargetEditorWindow = (EditorWindow)TargetEditorWindows[0];
			return TargetEditorWindow;
		}

		public static GameObject DuplicateGameObject(GameObject TargetGameObject) {
			Selection.objects = new Object[] { TargetGameObject };
			Selection.activeGameObject = TargetGameObject;
			Type HierarchyWindowType = Type.GetType("UnityEditor.SceneHierarchyWindow, UnityEditor");
			EditorWindow TargetHierarchyWindow = GetEditorWindow(HierarchyWindowType);
			TargetHierarchyWindow.SendEvent(EditorGUIUtility.CommandEvent("Duplicate"));
			GameObject DuplicatedGameObject = Selection.activeGameObject;
			return DuplicatedGameObject;
		}
	}
}
#endif