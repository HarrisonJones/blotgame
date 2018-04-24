using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeManagement : MonoBehaviour {
	
	public bool freeze;
	private int timer;
	private int	timerMax = 180;
	
	void FixedUpdate () {
		
		if (timerMax > 0)
		{
			freeze = true;
		}
		else
		{
			freeze = false;
			timerMax = 0;
		}
	}
}
