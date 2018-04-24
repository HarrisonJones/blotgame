using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTrigger : MonoBehaviour {
	public Color myColor;

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player")
		{
			collider.GetComponent<PlayerInput>().ChangeColor(myColor);

		}
	}
}
