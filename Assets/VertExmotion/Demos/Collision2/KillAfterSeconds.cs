using UnityEngine;
using System.Collections;

public class KillAfterSeconds : MonoBehaviour {

	public float m_lifeTime = 1f;
	float m_startTime = 0f;


	void Start () {
		m_startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Time.time - m_startTime > m_lifeTime)
			Destroy (gameObject);

	}
}
