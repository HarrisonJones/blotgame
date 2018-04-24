using UnityEngine;
using System.Collections;
using Kalagaan;

public class LockOnSensorMotion : MonoBehaviour {

	public VertExmotionSensor m_sensor;

	Vector3 m_initLocalPos;

	void Start () {
		m_initLocalPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void LateUpdate () { 
	
		transform.position = m_sensor.TransformPosition (transform.parent.position + transform.parent.rotation * m_initLocalPos, 1f);

	}
}
