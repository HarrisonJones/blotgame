using UnityEngine;
using System.Collections;

public class SetTimeScale : MonoBehaviour {

	public float m_timeScale = 1f;
	Kalagaan.VertExmotion m_vtm;
	Animator m_animator;

	void Start()
	{
		m_vtm = GetComponentInChildren<Kalagaan.VertExmotion> ();
		m_animator = GetComponent<Animator> ();

	}

	void Update () {
	

		if( m_vtm != null )
			m_vtm.SetTimeScale( m_timeScale );

		if( m_animator != null )
			m_animator.speed = m_timeScale;


	}
}
