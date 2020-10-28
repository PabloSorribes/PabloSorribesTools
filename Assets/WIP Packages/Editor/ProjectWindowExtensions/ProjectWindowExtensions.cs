//Source: http://answers.unity.com/answers/1475793/view.html
//UnityEditor.ProjectBrowser.cs: https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/ProjectBrowser.cs

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

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

			// --------------------------- //
			SetSearchStateToCurrentFolder();
		}

		private static void SetSearchStateToCurrentFolder()
		{
			// Trying to set the Search View State to 'current subfolder'
			// void SetSearchViewState(SearchViewState state)
			// 'SearchViewState' is an enum

			Type projectBrowserType = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
			if (projectBrowserType == null)
				return;

			MethodInfo setSearchViewStateMethodInfo = projectBrowserType.GetMethod("SetSearchViewState",
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

			if (setSearchViewStateMethodInfo == null)
			{
				Debug.LogError($"Couldn't find '{nameof(setSearchViewStateMethodInfo)}' through Reflection");
				return;
			}

			ParameterInfo searchViewStateParameter = setSearchViewStateMethodInfo.GetParameters()[0];
			var enumValues = searchViewStateParameter.ParameterType.GetEnumValues();

			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			builder.AppendLine($"SetSearchViewStateMethodInfo Parameter Values:");
			for (int i = 0; i < enumValues.Length; i++)
			{
				builder.AppendLine($"Enum Val {i}:\t{enumValues.GetValue(i)}");
			}
			Debug.Log($"{builder.ToString()}");

			EditorWindow window = EditorWindow.GetWindow(projectBrowserType);
			setSearchViewStateMethodInfo.Invoke(window, new object[] { enumValues.GetValue(4) });   // 4 = SearchViewState.SubFolders
		}
	}
}