using UnityEngine;
using System.Collections;

public class ChangeTime : MonoBehaviour {

	public float m_timeScale = 1f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		m_timeScale = Mathf.Clamp (m_timeScale,0f,100f);
		Time.timeScale = m_timeScale;
	}
}
