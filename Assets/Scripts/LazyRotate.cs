using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazyRotate : MonoBehaviour {

	void Update () {
		transform.Rotate(0, Time.deltaTime * 200, 0, Space.World);
	}
}
