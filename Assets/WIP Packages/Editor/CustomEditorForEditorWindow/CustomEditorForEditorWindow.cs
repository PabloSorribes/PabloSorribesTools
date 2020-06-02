//Source: http://answers.unity.com/answers/1641578/view.html
//Also shows how to show lists and arrays

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ListTestEditor : EditorWindow
{
	[MenuItem(itemName: "TestEditorList", menuItem = "Window/TestList")]
	public static void Init() { GetWindow<ListTestEditor>("Haha", true); }

	Editor editor;

	[SerializeField] List<MyClass> ListTest = new List<MyClass>();

	void OnGUI()
	{
		if (!editor) { editor = Editor.CreateEditor(this); }
		if (editor) { editor.OnInspectorGUI(); }
	}

	void OnInspectorUpdate() { Repaint(); }
}

[System.Serializable]
public class MyClass
{
	public List<int> myList;
	public string myString;
	public int myInt;
}

[CustomEditor(typeof(ListTestEditor), true)]
public class ListTestEditorDrawer : Editor
{

	public override void OnInspectorGUI()
	{
		var list = serializedObject.FindProperty("ListTest");
		EditorGUILayout.PropertyField(list, new GUIContent("My List Test"), true);
	}
}