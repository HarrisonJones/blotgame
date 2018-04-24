using UnityEngine;
using System.Collections;

namespace Kalagaan
{
public class DisableVertexMotion : MonoBehaviour {

	public bool m_disable = false;
	[HideInInspector]
	public bool m_disableTrigger = false;

	void Update () {
	
		if( m_disable != m_disableTrigger )
		{

			VertExmotion[] lst = GetComponentsInChildren<VertExmotion>();
			for( int i=0; i<lst.Length; ++i )
			{
				lst[i].enabled = !m_disable;
				lst[i].DisableMotion();
			}

			m_disableTrigger = m_disable;
		}

	}


	void OnGUI()
	{
		if (GUILayout.Button (m_disable?"Enable\nVertExmotion":"Disable\nVertExmotion"))
			m_disable = !m_disable;
	}
}
}
