//Source: http://answers.unity.com/answers/1475793/view.html

using System;
using System.Reflection;
using UnityEditor;

namespace Paalo.WIP.EditorTools
{
	public static class ProjectWindowExtensions
	{
		private const string toolName = "Filter to all assets of same type as the selected asset";

		[MenuItem(CurrentPackageConstants.packageAssetsMenuPath + toolName, false, CurrentPackageConstants.packageMenuIndexPosition)]
		private static void FilterToAllAssetsOfSelectedObjectsType()
		{
			Type objType = Selection.activeObject.GetType();
			SetProjectWindowSearchString($"t: {objType.Name}");
		}

		private static void SetProjectWindowSearchString(string searchString)
		{
			Type projectBrowserType = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
			if (projectBrowserType == null)
				return;

			MethodInfo setSearchMethodInfo = projectBrowserType.GetMethod("SetSearch", 
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, 
				null, 
				new Type[] { typeof(string) }, 
				null);

			if (setSearchMethodInfo == null)
				return;

			EditorWindow window = EditorWindow.GetWindow(projectBrowserType);
			setSearchMethodInfo.Invoke(window, new object[] { searchString });
		}
	}
}