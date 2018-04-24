using UnityEngine;
using System.Collections;

public class MotionTest : MonoBehaviour {

	Vector3 m_initPos;
	Vector3 m_targetPos;
	float m_lastRefreshTime = 0;

	public float m_amplitude = 1f;
	public float m_speed = 1f;
	public float m_refreshTimer = 1f;

	public bool m_enableTranslation = true;
	public bool m_enableRotation = true;

	public float m_rotationSpeed = 1f;
	public bool m_rotationStartStop = false;

	// Use this for initialization
	void Start () {
		m_initPos = transform.localPosition;
		m_targetPos = m_initPos;
	}
	
	// Update is called once per frame
	void Update () {

		if( m_enableTranslation )
		{
			if (Time.time - m_lastRefreshTime > m_refreshTimer)
			{
				m_targetPos = Random.insideUnitSphere * m_amplitude + m_initPos;
				m_lastRefreshTime = Time.time;
			}

			transform.localPosition = Vector3.Lerp (transform.localPosition, m_targetPos, Time.deltaTime * m_speed);
		}


		if( m_enableRotation )
		{
			transform.RotateAround( transform.position, transform.up, Time.deltaTime * m_rotationSpeed * ( m_rotationStartStop ? Mathf.Floor(Mathf.Sin(Time.time)) : 1f ) );
		}
	}
}
