using UnityEngine;
using System.Collections;
using Kalagaan;

[RequireComponent(typeof(VertExmotionSensor))]
public class VertExmotionInteraction : MonoBehaviour {


    public enum eInteractionType
    {
        DIRECTION,
        PUSH,
        PULL
    }

	public VertExmotion m_target;
	public float m_radius = .1f;
    public eInteractionType m_interactionType;

	VertExmotionSensor m_sensor;

	// Use this for initialization
	void Start () {
	
		m_sensor = GetComponent<VertExmotionSensor> ();
		m_sensor.m_params.translation.amplitudeMultiplier = 0;

		if( m_target != null )
		{
			m_target.m_VertExmotionSensors.Add( m_sensor );
		}

	}

    // Update is called once per frame
    void Update() {

        switch (m_interactionType)
        {
            case eInteractionType.DIRECTION:
                m_sensor.m_params.translation.worldOffset = m_radius * transform.forward;
                break;

            case eInteractionType.PUSH:
                m_sensor.m_params.translation.worldOffset = m_radius * (m_target.transform.position - transform.position).normalized;
                break;

            case eInteractionType.PULL:
                m_sensor.m_params.translation.worldOffset = m_radius * (transform.position - m_target.transform.position).normalized;
                break;
        }
		m_sensor.m_envelopRadius = m_radius;

	}
}
