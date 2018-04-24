using UnityEngine;
using System.Collections;
using Kalagaan;

public class SimpleMeshDemo : MonoBehaviour {

	public float m_moveAmplitude = 2f;
	public float m_moveSpeed = 1f;

	bool m_goRight = false;
	bool m_move = false;
	float speed = 0;
	public VertExmotionSensor m_sensor;


	void Awake () {
		
	}
	
	void Update () {

		speed = Mathf.Lerp ( speed, m_moveSpeed, Time.deltaTime * 2f );

		if( m_move )
		{
			if( m_goRight )
			{
				transform.position += Vector3.right * speed * Time.deltaTime;
				if( transform.position.x > m_moveAmplitude )
				{
					m_goRight = false;
					m_move = false;
				}
			}
			else
			{
				transform.position += -Vector3.right * speed * Time.deltaTime;
				if( transform.position.x < -m_moveAmplitude )
				{
					m_goRight = true;
					m_move = false;
				}
			}
		}

	}

	void OnGUI()
	{
		GUI.color = Color.black;

		GUILayout.Label ("speed");
		m_moveSpeed = GUILayout.HorizontalSlider (m_moveSpeed, 5f, 30f, GUILayout.Width(100f) );

		GUILayout.Label ("amplitude");
		m_sensor.m_params.translation.innerMaxDistance = GUILayout.HorizontalSlider (m_sensor.m_params.translation.innerMaxDistance, 0f, 1f, GUILayout.Width(100f) );
		m_sensor.m_params.translation.outerMaxDistance = m_sensor.m_params.translation.innerMaxDistance;

		GUILayout.Label ("damping");
		m_sensor.m_params.damping = GUILayout.HorizontalSlider (m_sensor.m_params.damping, 0f, 2f, GUILayout.Width(100f) );

		GUILayout.Label ("bouncing");
		m_sensor.m_params.bouncing = GUILayout.HorizontalSlider (m_sensor.m_params.bouncing, 0f, 2f, GUILayout.Width(100f) );

		GUILayout.Label ("inflate");
		m_sensor.m_params.inflate = GUILayout.HorizontalSlider (m_sensor.m_params.inflate, -.7f, 1f, GUILayout.Width(100f) );


		if( GUILayout.Button("move") )
		{
			m_move = true;
		}
	}
}
