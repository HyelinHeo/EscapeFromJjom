using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResetSettingWindow : EditorWindow
{
	[MenuItem("Setting/Reset Setting")]
	static void Init()
	{
		PlayerPrefs.DeleteAll();
		Debug.Log("DeleteAll PlayerPrefs Values");
		//ResetSettingWindow window = (ResetSettingWindow)EditorWindow.GetWindow(typeof(ResetSettingWindow));
		//window.Show();
	}

	
}
