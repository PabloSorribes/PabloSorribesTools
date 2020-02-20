using UnityEditor;
using UnityEngine;

namespace Paalo.Tools
{
	public class FindDirectoryOfScript : EditorWindow
	{
		#region ToolName and SetupWindow
		private const int menuIndexPosition = CurrentPackageConstants.paaloMenuIndexPosition;     //To make the menu be at the top of the GameObject-menu and the first option in the hierarchy.
		private const string baseMenuPath = CurrentPackageConstants.paaloMenuPath;
		private const string rightClickMenuPath = "GameObject/" + baseMenuPath + toolName;
		private const string toolsMenuPath = "Window/" + baseMenuPath + toolName;
		private const string toolName = "Find Directory Of Script";

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
			var window = GetWindow<FindDirectoryOfScript>(true, toolName, true);
			window.minSize = new Vector2(340, 200);
			window.maxSize = new Vector2(340, 1024);
		}
		#endregion ToolName and SetupWindow

		public Object scriptSource;
		public string searchName;

		public enum SearchFilterOptions
		{
			Type,
			Label
		}
		public SearchFilterOptions filterType;

		public SearchByTypeFilter typeSearchFilter;
		public SearchByLabelFilter labelSearchFilter;

		private void OnGUI()
		{
			EditorGUILayout.BeginVertical(GUI.skin.box);
			EditorGUILayout.BeginHorizontal(GUI.skin.box);
			scriptSource = EditorGUILayout.ObjectField("Script: ", scriptSource, typeof(Object), false);
			EditorGUILayout.EndHorizontal();

			if (GUILayout.Button("Find object's directory: Object Reference Approach"))
			{
				var directoryOfChosenObject = FindDirectoryOfObject(scriptSource);
				Debug.Log(directoryOfChosenObject);
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.Space();

			EditorGUILayout.BeginVertical(GUI.skin.box);
			EditorGUILayout.BeginHorizontal(GUI.skin.box);
			searchName = EditorGUILayout.TextField("Search string: ", searchName);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Separator();

			filterType = (SearchFilterOptions)EditorGUILayout.EnumPopup("Filter the search by: ", filterType);

			if (filterType == SearchFilterOptions.Type)
			{
				typeSearchFilter = (SearchByTypeFilter)EditorGUILayout.EnumPopup("Type to search for: ", typeSearchFilter);
			}
			else
			{
				labelSearchFilter = (SearchByLabelFilter)EditorGUILayout.EnumPopup("Label to search for: ", labelSearchFilter);
			}
			EditorGUILayout.Separator();

			string buttonText = $"Find object's directory: Search Filter Approach - {filterType.ToString()}";
			if (GUILayout.Button(buttonText))
			{
				switch (filterType)
				{
					case SearchFilterOptions.Type:
						SearchForObjectInAssets(searchName, typeSearchFilter);
						break;
					case SearchFilterOptions.Label:
						SearchForObjectInAssets(searchName, labelSearchFilter);
						break;
					default:
						break;
				}
			}

			EditorGUILayout.EndVertical();
		}

		public static string FindDirectoryOfObject(Object obj)
		{
			var objPath = AssetDatabase.GetAssetPath(obj);
			var dirPath = System.IO.Path.GetDirectoryName(objPath);
			dirPath = dirPath.Replace('\\', '/');
			return dirPath;
		}

		public enum SearchByTypeFilter
		{
			AnimationClip,
			AudioClip,
			AudioMixer,
			ComputeShader,
			Font,
			GUISkin,
			Material,
			Mesh,
			Model,
			PhysicMaterial,
			Prefab,
			Scene,
			Script,
			Shader,
			Sprite,
			Texture,
			VideoClip
		}

		public enum SearchByLabelFilter
		{
			Architecture,
			Audio,
			Character,
			Drip,
			Effect,
			ExcludeGfxTests,
			Glass,
			Ground,
			Particles,
			Prop,
			Rain,
			Splash,
			Streak,
			Water,
			Window
		}

		public static string SearchForObjectInAssets(string objectName, SearchByTypeFilter searchFilter)
		{
			//Assumes that there will only exist one file called like this
			string filter = $"t:{searchFilter.ToString()} ";
			string searchString = $"{filter} {objectName}";
			return GetObjectPathByFilter(searchString);
		}

		public static string SearchForObjectInAssets(string objectName, SearchByLabelFilter searchFilter)
		{
			//Assumes that there will only exist one file called like this
			string filter = $"l:{searchFilter.ToString()} ";
			string searchString = $"{filter} {objectName}";
			return GetObjectPathByFilter(searchString);
		}

		private static string GetObjectPathByFilter(string searchString)
		{
			string[] foundAssets = AssetDatabase.FindAssets(searchString);
			if (foundAssets.Length < 1)
			{
				Debug.LogWarning($"The searched asset '{searchString}' was not found or doesn't exist!");
				return "";
			}
			string objPath = AssetDatabase.GUIDToAssetPath(foundAssets[0]);

			string dirPath = System.IO.Path.GetDirectoryName(objPath);
			dirPath = dirPath.Replace('\\', '/');
			Debug.Log($"Object Directory Path: {dirPath}");
			return dirPath;
		}
	}
}