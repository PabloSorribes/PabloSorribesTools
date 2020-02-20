using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Paalo.Tools
{
	public class CopyFolderToNewPath : EditorWindow
	{
		private const int menuIndexPosition = PabloSorribesToolsConstants.defaultPaaloMenuIndexPosition;     //To make the menu be at the top of the GameObject-menu and the first option in the hierarchy.
		private const string baseMenuPath = PabloSorribesToolsConstants.defaultPaaloMenuPath;
		private const string rightClickMenuPath = "GameObject/" + baseMenuPath + toolName;
		private const string toolsMenuPath = "Window/" + baseMenuPath + toolName;
		private const string toolName = "Copy Folder To New Path";

		public string startPath = "Assets";
		public string newDestinationSubFolderName = "CopiedFolder";
		string sourcePath = "";
		string destinationPath = "";

		[MenuItem(rightClickMenuPath, false, menuIndexPosition)]
		public static void RightClickMenu()
		{
			SetupWindow();
		}

		[MenuItem(toolsMenuPath, false, menuIndexPosition)]
		public static void ToolsMenu()
		{
			SetupWindow();
		}

		public static void SetupWindow()
		{
			var window = GetWindow<CopyFolderToNewPath>(true, toolName, true);
			window.minSize = new Vector2(300, 200);
			window.maxSize = new Vector2(window.minSize.x + 100, window.minSize.y + 100);
		}

		private void OnGUI()
		{
			GUISection_ButtonsAndMain();
			EditorGUILayout.Space();
		}

		private void GUISection_ButtonsAndMain()
		{
			EditorGUILayout.BeginVertical(GUI.skin.box);

			EditorGUILayout.Space();
			startPath = EditorGUILayout.TextField("Starting Path: ", startPath);
			newDestinationSubFolderName = EditorGUILayout.TextField("New Destination Subfolder Name: ", newDestinationSubFolderName);
			EditorGUILayout.Space();

			if (GUILayout.Button($"Get Folders!"))
			{
				sourcePath = EditorUtility.OpenFolderPanel("Source Path", startPath, "");
				Debug.Log("Source: " + sourcePath);
			}

			if (GUILayout.Button($"Set Destination!"))
			{
				destinationPath = $"{EditorUtility.OpenFolderPanel("New destination", startPath, "")}/{newDestinationSubFolderName}";
				Debug.Log("Destination: " + destinationPath);
			}

			if (GUILayout.Button($"Copy to destination!"))
			{
				FileUtil.CopyFileOrDirectory(sourcePath, destinationPath);
				AssetDatabase.Refresh();
				Debug.Log("Perform Copy to: " + destinationPath);
			}

			EditorGUILayout.EndVertical();
		}
	}
}
