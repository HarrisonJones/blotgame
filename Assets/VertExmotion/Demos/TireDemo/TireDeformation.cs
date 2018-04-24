using UnityEngine;
using System.Collections;
using Kalagaan;
using UnityEngine.UI;

public class TireDeformation : MonoBehaviour {

    public VertExmotionSensor m_sensor;
    public Vector3 m_offset;
    public CapsuleCollider m_wheelCollider;

    [System.Serializable]
    public class DeformationData
    {
        public float m_sensorRadius = 1.2f;
        public float m_colliderRadius = .85f;
        public float m_sensorDeformation = .3f;
        public float m_sensorInflate = .08f;
    }

    public DeformationData m_tireOk = new DeformationData();
    public DeformationData m_tireFlat = new DeformationData();

    public float m_tireInflated = .5f;

    // Use this for initialization
    void Start () {

        m_sensor.transform.parent = null;
    }
	
	// Update is called once per frame
	void Update () {

        m_wheelCollider.radius = Mathf.Lerp( m_tireFlat.m_colliderRadius, m_tireOk.m_colliderRadius, m_tireInflated);
        m_sensor.m_params.inflate = Mathf.Lerp(m_tireFlat.m_sensorInflate, m_tireOk.m_sensorInflate, m_tireInflated);

        RaycastHit[] hitInfo = Physics.RaycastAll(transform.position, Vector3.down, 10f);
        
        for(int i=0; i<hitInfo.Length; ++i)
        {
            if (hitInfo[i].collider != m_wheelCollider)
            {
                m_sensor.transform.position = hitInfo[i].point + m_offset;
                m_sensor.m_params.translation.worldOffset.y = Mathf.Lerp(m_tireFlat.m_sensorDeformation, m_tireOk.m_sensorDeformation, m_tireInflated);

                float f = (1f - (Vector3.Distance(m_wheelCollider.transform.position, hitInfo[i].point) - Mathf.Lerp(m_tireFlat.m_colliderRadius, m_tireOk.m_colliderRadius, m_tireInflated)) / m_offset.magnitude);
                m_sensor.m_envelopRadius = Mathf.Lerp(m_tireFlat.m_sensorRadius, m_tireOk.m_sensorRadius, m_tireInflated) * f;
                
            }


        }

	}

    public void Inflate( Slider sld )
    {
        m_tireInflated = sld.value;

    }

}
