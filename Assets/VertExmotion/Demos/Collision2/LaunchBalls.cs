using UnityEngine;
using System.Collections;

public class LaunchBalls : MonoBehaviour {

	public Transform m_root;
	public GameObject m_projectilePrefab;
	public float m_power = 1f;
	public Kalagaan.DisableVertexMotion m_disableVM;
	public float m_lastLaunch = 0f;

	void Update () {
	
		if( Input.GetMouseButtonDown( 0 ) && Time.time - m_lastLaunch > .2f )
		{
			m_lastLaunch = Time.time;

			Vector3 mousePos = Input.mousePosition;
			mousePos.z = Camera.main.transform.position.z;
			Vector3 pos = Camera.main.ScreenToWorldPoint( mousePos );
			pos -= Camera.main.transform.forward * 4f;

			GameObject go = Instantiate( m_projectilePrefab, pos, Camera.main.transform.rotation ) as GameObject;
			go.transform.parent = m_root;
#if !UNITY_4_5
			go.GetComponent<Rigidbody> ().AddForce( Camera.main.transform.forward * m_power );
#else
			go.rigidbody.AddForce( Camera.main.transform.forward * m_power );
#endif

			if( m_disableVM.m_disable )
			{
				//disable all VM
				m_disableVM.m_disableTrigger = !m_disableVM.m_disable;
			}
		}

	}
}
