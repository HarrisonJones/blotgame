using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Simple script used to set the background splat image colour with the colour of the blob, upon victory.
// By Peter Liang.
public class BlobbyColour : MonoBehaviour
{
	public Image image;
	Material blobMat;

	// Use this for initialization
	void Start()
	{
		blobMat = GetComponent<Renderer>().material;
	}

	// Update is called once per frame
	void Update()
	{
		blobMat.color = image.color;
	}
}
