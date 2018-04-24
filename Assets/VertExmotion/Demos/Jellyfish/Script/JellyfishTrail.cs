using UnityEngine;
using System.Collections;

public class JellyfishTrail : MonoBehaviour {

	Vector3 initPos;

	void Start () {
		initPos = transform.localPosition;
	}
	

	void Update () {
	
		transform.localPosition = Vector3.Lerp ( transform.localPosition, initPos + Random.onUnitSphere * 2f, Time.deltaTime * .2f );

	}
}
