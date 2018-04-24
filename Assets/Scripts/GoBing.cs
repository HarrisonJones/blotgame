using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBing : MonoBehaviour {
	public AudioClip blop;

	public void MakeSound()
	{
		Camera.main.GetComponent<AudioSource>().PlayOneShot(blop);
	}
}
