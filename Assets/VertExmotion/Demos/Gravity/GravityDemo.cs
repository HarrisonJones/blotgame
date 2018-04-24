using UnityEngine;
using System.Collections;
using Kalagaan;

public class GravityDemo : MonoBehaviour {


	public float gravityFactor = .5f;
	public float gravityFactorMax = 1f;

	void OnGUI ()
	{
		GUILayout.Label ( "gravity" );
		GUILayout.BeginHorizontal ();
		GUILayout.Space ( 20 );
		gravityFactor = GUILayout.VerticalSlider ( gravityFactor, 0f, gravityFactorMax );
		Physics.gravity = -Vector3.up * gravityFactor;
		GUILayout.EndHorizontal ();
	}
}
