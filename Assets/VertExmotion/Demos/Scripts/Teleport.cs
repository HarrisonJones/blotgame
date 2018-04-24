using UnityEngine;
using System.Collections;
using Kalagaan;

public class Teleport : MonoBehaviour {



    public Vector3 m_dir = Vector3.forward;
    public bool m_teleport = false;
    public bool m_resetMotion = false;

    VertExmotion m_vtm;

    void Start()
    {
        m_vtm = GetComponent<VertExmotion>();
    }


	void Update () {
	
        if(m_vtm!=null && m_teleport)
        {
            transform.position += m_dir;
            m_teleport = false;
                        
            if(m_resetMotion)
                m_vtm.ResetMotion();//reset motion
            else
                m_vtm.IgnoreFrame();//ignore teleport frame motion

        }

	}
}
