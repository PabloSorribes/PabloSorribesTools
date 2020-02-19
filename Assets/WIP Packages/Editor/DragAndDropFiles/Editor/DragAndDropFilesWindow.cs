using UnityEditor;
using UnityEngine;

namespace Paalo.Tools
{


	public class DragAndDropFilesWindow : EditorWindow
	{
		private const int menuIndexPosition = PabloSorribesToolsConstants.defaultPaaloMenuIndexPosition;     //To make the menu be at the top of the GameObject-menu and the first option in the hierarchy.
		private const string baseMenuPath = PabloSorribesToolsConstants.defaultPaaloMenuPath;
		private const string rightClickMenuPath = "GameObject/" + baseMenuPath + toolName;
		private const string toolsMenuPath = "Window/" + baseMenuPath + toolName;
		private const string toolName = "Drag and Drop AudioClips Window";

		public AudioClip[] audioClips = null;

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
			var window = GetWindow<DragAndDropFilesWindow>(true, toolName, true);
			window.minSize = new Vector2(300, 200);
			window.maxSize = new Vector2(window.minSize.x + 100, window.minSize.y + 100);
		}

		private void OnGUI()
		{
			EditorGUILayout.Space();
			GUISection_DragAndDropArea();
			EditorGUILayout.Space();
		}

		private void GUISection_DragAndDropArea()
		{
			Color originalGUIColor = GUI.color;

			GUI.color = Color.black;
			EditorGUILayout.BeginVertical(GUI.skin.box);

			//GUI.color = DTGUIHelper.BrightTextColor;
			GUI.color = Color.yellow;

			var dragArea = GUILayoutUtility.GetRect(0f, 35f, GUILayout.ExpandWidth(true));
			//GUI.Box(dragArea, MasterAudio.DragAudioTip + " to create Variations!");
			GUI.Box(dragArea, "Drag Audio clips or a folder containing some Audio Clips here!");

			GUI.color = Color.white;

			var anEvent = Event.current;
			switch (anEvent.type)
			{
				case EventType.DragUpdated:
				case EventType.DragPerform:
					if (!dragArea.Contains(anEvent.mousePosition))
					{
						break;
					}

					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

					if (anEvent.type == EventType.DragPerform)
					{
						DragAndDrop.AcceptDrag();

						foreach (var dragged in DragAndDrop.objectReferences)
						{
							if (dragged is DefaultAsset)
							{
								var assetPaths = AssetDatabase.FindAssets("t:AudioClip", DragAndDrop.paths);
								foreach (var assetPath in assetPaths)
								{
									var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(assetPath));
									if (clip == null)
									{
										continue;
									}

									//Add clip to list of clips to set on AudioSources

									Debug.Log(clip.name);

									//CreateVariation(_group, _ma, clip);
								}

								continue;
							}

							var aClip = dragged as AudioClip;
							if (aClip == null)
							{
								continue;
							}

							Debug.Log(aClip.name);


							//CreateVariation(_group, _ma, aClip);
						}
					}
					Event.current.Use();
					break;
			}
			EditorGUILayout.EndVertical();
			GUI.color = originalGUIColor;
		}
	}
}
