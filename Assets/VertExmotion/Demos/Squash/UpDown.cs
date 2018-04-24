using UnityEngine;
using System.Collections;

public class UpDown : MonoBehaviour {

	public float m_speed = 1f;
	public float m_amplitude = 5f;
	//public float m_TimeScale = .1f;
	Vector3 m_startPos;


	void Start()
	{

		m_startPos = transform.position;
	}


	// Update is called once per frame
	void Update () {
	
		//m_TimeScale = Mathf.Clamp (m_TimeScale,0f,100f);
		//Time.timeScale = m_TimeScale;
		transform.position = Mathf.Sin (Time.time * m_speed) * m_amplitude * Vector3.up + m_startPos;


	}
}
