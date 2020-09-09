using UnityEditor;
using UnityEngine;

namespace Paalo.WIP.EditorTools
{
	public class DrawMenusAndPopupsExample : EditorWindow
	{
		#region ToolName and SetupWindow
		private const string toolName = "Draw Menus and Popups Example";

		[MenuItem(CurrentPackageConstants.packageWindowMenuPath + toolName, false, CurrentPackageConstants.packageMenuIndexPosition)]
		public static void ToolsMenu()
		{
			SetupWindow();
		}

		public static void SetupWindow()
		{
			var window = GetWindow<DrawMenusAndPopupsExample>(true, toolName, true);
			window.minSize = new Vector2(300, 200);
			window.maxSize = new Vector2(window.minSize.x + 100, window.minSize.y + 100);
		}
		#endregion ToolName and SetupWindow

		string[] optionsStringArray = { "first", "second", "third", "whatevs" };

		private void OnGUI()
		{
			EditorGUILayout.BeginVertical(GUI.skin.box);
			GUI_DrawPopup(optionsStringArray);
			EditorGUILayout.EndVertical();

			EditorGUILayout.Space();

			EditorGUILayout.BeginVertical(GUI.skin.box);
			GUI_DrawMenu(optionsStringArray);
			EditorGUILayout.EndVertical();
		}

		int selectedPopupIndex = 0;

		// Using a normal EditorGUILayout.Popup() by getting the index, etc etc
		private void GUI_DrawPopup(string[] optionsStringArray)
		{
			selectedPopupIndex = EditorGUILayout.Popup("Selected Popup Index: ", selectedPopupIndex, optionsStringArray);
			string selectedPopupStringValue = optionsStringArray[selectedPopupIndex];

			//Allow for free text bus adding
			GUIContent textFieldContent = new GUIContent("Selected Popup String Value: ", "The option you selected in the menu above.");
			selectedPopupStringValue = EditorGUILayout.TextField(textFieldContent, selectedPopupStringValue);
		}

		string selectedMenuOption = "";

		//Creating a menu "on demand" when clicking the button. 
		//Better in some cases, and probably less GUI intensive, since it's just generated when you click the button.
		private void GUI_DrawMenu(string[] optionsStringArray)
		{
			if (GUILayout.Button("Select a Menu Option"))
			{
				//Create a dropdown based on strings (without using an enum);
				GenericMenu menu = new GenericMenu();
				foreach (var item in optionsStringArray)
					menu.AddItem(new GUIContent(item), false, () => { selectedMenuOption = item; });

				menu.ShowAsContext();
			}

			//Allow for free text bus adding
			GUIContent textFieldContent = new GUIContent("Selected Menu Option: ", "The option you selected in the menu above.");
			selectedMenuOption = EditorGUILayout.TextField(textFieldContent, selectedMenuOption);
		}
	}
}