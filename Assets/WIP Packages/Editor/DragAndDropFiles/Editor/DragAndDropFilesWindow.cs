using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Paalo.Tools
{
	public class DragAndDropFilesWindow : EditorWindow
	{
		#region ToolName and SetupWindow
		private const int menuIndexPosition = PabloSorribesToolsConstants.defaultPaaloMenuIndexPosition;     //To make the menu be at the top of the GameObject-menu and the first option in the hierarchy.
		private const string baseMenuPath = PabloSorribesToolsConstants.defaultPaaloMenuPath;
		private const string rightClickMenuPath = "GameObject/" + baseMenuPath + toolName;
		private const string toolsMenuPath = "Window/" + baseMenuPath + toolName;
		private const string toolName = "Drag and Drop AudioClips Window";

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
		#endregion ToolName and SetupWindow

		#region Tool-specific Variables
		public AudioClip[] audioClips = null;
		/// <summary>
		/// Use this action to get an array of the objects that were dragged into the area.
		/// </summary>
		public static event System.Action<UnityEngine.Object[]> OnDragPerformed;
		#endregion

		private void OnEnable()
		{
			//OnDragPerformed += UpdateClipsOnDragPerform;
		}

		private void UpdateClipsOnDragPerform<T>(T[] draggedObjects) where T : Object
		{
			Debug.Log($"Dragged Obj Array: {draggedObjects.GetType().FullName}");

			foreach (var draggedObj in draggedObjects)
			{
				Debug.Log($"Dragged Obj: {draggedObj.GetType().FullName}");
			}

			audioClips = draggedObjects as AudioClip[];
			Debug.Log("Array length: " + audioClips.Length);
		}

		private void OnGUI()
		{
			EditorGUILayout.Space();
			DrawDragAndDropArea<AudioClip>(new DragAndDropAreaInfo("Audio Clips"), UpdateClipsOnDragPerform);
			EditorGUILayout.Space();
		}

		/// <summary>
		/// Draws a Drag and Drop Area and raises the <see cref="OnDragPerformed"/>-event, which you can use use to get an array of the objects that were dragged into the area.
		/// </summary>
		/// <typeparam name="T">The object type you want the <see cref="OnDragPerformed"/> method to handle.</typeparam>
		/// <param name="dragAreaInfo"></param>
		/// <returns></returns>
		public static void DrawDragAndDropArea<T>(DragAndDropAreaInfo dragAreaInfo, System.Action<T[]> OnPerformedDragCallback = null) where T : UnityEngine.Object
		{
			//Change color and create Drag Area
			Color originalGUIColor = GUI.color;
			GUI.color = dragAreaInfo.outlineColor;
			EditorGUILayout.BeginVertical(GUI.skin.box);
			GUI.color = dragAreaInfo.backgroundColor;
			var dragArea = GUILayoutUtility.GetRect(dragAreaInfo.dragAreaWidth, dragAreaInfo.dragAreaHeight, GUILayout.ExpandWidth(true));
			GUI.Box(dragArea, dragAreaInfo.DragAreaText);

			//See if the current Editor Event is a DragAndDrop event.
			var anEvent = Event.current;
			switch (anEvent.type)
			{
				case EventType.DragUpdated:
				case EventType.DragPerform:
					if (!dragArea.Contains(anEvent.mousePosition))
					{
						//Early Out in case the drop is made outside the drag area.
						break;
					}

					//Change mouse cursor icon to the "Copy" icon
					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

					//If mouse is released 
					if (anEvent.type == EventType.DragPerform)
					{
						DragAndDrop.AcceptDrag();
						var draggedTypeObjectsArray = GetDraggedObjects<T>();
						OnPerformedDragCallback?.Invoke(draggedTypeObjectsArray);
					}

					Event.current.Use();
					break;
			}

			EditorGUILayout.EndVertical();
			GUI.color = originalGUIColor;
		}

		private static T[] GetDraggedObjects<T>() where T : Object
		{
			List<T> draggedTypeObjects = new List<T>();

			foreach (var dragged in DragAndDrop.objectReferences)
			{
				T draggedAsset = null;

				//A "DefaultAsset" is a folder in the Unity Editor.
				if (dragged is DefaultAsset)
				{
					var assetPaths = AssetDatabase.FindAssets("t:AudioClip", DragAndDrop.paths);
					foreach (var assetPath in assetPaths)
					{
						draggedAsset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(assetPath));
						if (draggedAsset == null)
						{
							continue;
						}

						Debug.Log($"Default Asset Dragged: {draggedAsset.name}");
						draggedTypeObjects.Add(draggedAsset as T);
					}
					continue;
				}

				draggedAsset = dragged as T;
				if (draggedAsset == null)
				{
					continue;
				}

				//Debug.Log($"Asset of type '{draggedAsset.GetType().FullName}' dragged. Asset Name: '{draggedAsset.name}'");
				draggedTypeObjects.Add(draggedAsset as T);
			}
			return draggedTypeObjects.ToArray();
		}
	}

	/// <summary>
	/// Use this class to create some values for the DragAndDrop-area that you want to create.
	/// </summary>
	public class DragAndDropAreaInfo
	{
		public string DragAreaText
		{
			get => $"Drag {draggedObjectTypeName} or a folder containing some {draggedObjectTypeName} here!";
			//private set => DragAreaText = value;
		}

		public string draggedObjectTypeName = "AudioClips";
		public float dragAreaWidth = 0f;
		public float dragAreaHeight = 35f;

		public Color outlineColor = Color.black;
		public Color backgroundColor = Color.yellow;

		public DragAndDropAreaInfo(string draggedObjectTypeName)
		{
			this.draggedObjectTypeName = draggedObjectTypeName;
		}

		public DragAndDropAreaInfo(string draggedObjectTypeName, Color outlineColor, Color backgroundColor, float dragAreaWidth = 0f, float dragAreaHeight = 35f)
		{
			this.draggedObjectTypeName = draggedObjectTypeName;
			this.outlineColor = outlineColor;
			this.backgroundColor = backgroundColor;
			this.dragAreaWidth = dragAreaWidth;
			this.dragAreaHeight = dragAreaHeight;
		}
	}

}
