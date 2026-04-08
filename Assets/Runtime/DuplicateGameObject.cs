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

	[ExecuteInEditMode]
	public class DuplicateGameObject {

		public EditorWindow FindFirstWindow(Type EditorWindowType) {
			if (EditorWindowType == null)
				throw new ArgumentNullException(nameof(EditorWindowType));
			if (!typeof(EditorWindow).IsAssignableFrom(EditorWindowType))
				throw new ArgumentException($"The given type ({EditorWindowType.Name}) does not inherit from {nameof(EditorWindow)}");
			Object[] TypeOpenWindows = Resources.FindObjectsOfTypeAll(EditorWindowType);
			if (TypeOpenWindows.Length <= 0) return null;
			EditorWindow Window = (EditorWindow)TypeOpenWindows[0];
			return Window;
		}

		public GameObject DuplicateGameObjectInstance(GameObject GameObjectInstance) {
			Selection.objects = new Object[] { GameObjectInstance };
			Selection.activeGameObject = GameObjectInstance;
			Type HierarchyViewType = Type.GetType("UnityEditor.SceneHierarchyWindow, UnityEditor");
			EditorWindow HierarchyView = FindFirstWindow(HierarchyViewType);
			HierarchyView.SendEvent(EditorGUIUtility.CommandEvent("Duplicate"));
			GameObject CloneGameObject = Selection.activeGameObject;
			return CloneGameObject;
		}
	}
}
#endif