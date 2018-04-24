using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {

    public float m_force = 1f;
	
	
	void Update () {
	
       if( Input.GetKeyDown( KeyCode.Space ) )
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * m_force);
        }


        if (Input.touchCount == 1 )
        {
            if( Input.touches[0].phase == TouchPhase.Began )
                GetComponent<Rigidbody>().AddForce(Vector3.up * m_force);
        }
    }
}
