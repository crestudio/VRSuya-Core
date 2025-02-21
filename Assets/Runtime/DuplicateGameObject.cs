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

		/// <summary>요청한 타입의 첫번째 윈도우 창 오브젝트를 반환합니다.</summary>
		/// <param name="EditorWindowType">Unity Editor 윈도우 타입</param>
		/// <returns>해당 타입의 첫번째 윈도우</returns>
		public EditorWindow FindFirstWindow(Type EditorWindowType) {
			if (EditorWindowType == null)
				throw new ArgumentNullException(nameof(EditorWindowType));
			if (!typeof(EditorWindow).IsAssignableFrom(EditorWindowType))
				throw new ArgumentException("The given type (" + EditorWindowType.Name + ") does not inherit from " + nameof(EditorWindow) + ".");
			Object[] TypeOpenWindows = Resources.FindObjectsOfTypeAll(EditorWindowType);
			if (TypeOpenWindows.Length <= 0) return null;
			EditorWindow Window = (EditorWindow)TypeOpenWindows[0];
			return Window;
		}

		/// <summary>요청한 오브젝트의 복제된 GameObject를 반환합니다.</summary>
		/// <param name="GameObjectInstance">완전 복제를 원하는 GameObject</param>
		/// <returns>복제된 GameObject</returns>
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