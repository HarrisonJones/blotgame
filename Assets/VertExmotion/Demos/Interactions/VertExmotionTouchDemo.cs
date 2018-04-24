using UnityEngine;
using System.Collections;

public class VertExmotionTouchDemo : MonoBehaviour {

    public float m_radius = 1;
    VertExmotionInteraction m_vmi;
    MeshRenderer m_mr;

    void Start()
    {
        m_vmi = GetComponent<VertExmotionInteraction>();
        m_mr = GetComponent<MeshRenderer>();
        m_mr.enabled = false;
    }
    
	void Update () {

        if( Input.GetMouseButton(0) )
        {
            RaycastHit hitInfo;
            if( Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) )
            {
                m_vmi.m_radius += Time.deltaTime;
                m_vmi.transform.position = hitInfo.point;
                //m_mr.enabled = true;
            }
            else
            {
                m_vmi.m_radius -= Time.deltaTime;
                //m_mr.enabled = false;
            }


        }
        else
        {
            m_vmi.m_radius -= Time.deltaTime;
            //m_mr.enabled = false;
        }

        m_vmi.m_radius = Mathf.Clamp(m_vmi.m_radius, 0, m_radius);


    }
}
