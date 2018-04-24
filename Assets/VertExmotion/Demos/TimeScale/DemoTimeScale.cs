using UnityEngine;
using System.Collections;

public class DemoTimeScale : MonoBehaviour {

	public SetTimeScale m_z1;
	public SetTimeScale m_z2;

	// Update is called once per frame
	void OnGUI () {
	
		GUILayout.BeginArea (new Rect (0, Screen.height*.1f, Screen.width, Screen.height*.9f));

		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		GUILayout.Label ("Time scale demo");
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Zombi 1");
		m_z1.m_timeScale = GUILayout.HorizontalSlider (m_z1.m_timeScale, 0f, 2f, GUILayout.Width(Screen.width/4f));

		GUILayout.FlexibleSpace ();

		GUILayout.Label ("Zombi 2");
		m_z2.m_timeScale = GUILayout.HorizontalSlider (m_z2.m_timeScale, 0f, 2f, GUILayout.Width(Screen.width/4f));
		GUILayout.EndHorizontal ();
		GUILayout.EndArea ();

	}
}
