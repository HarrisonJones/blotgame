using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup_Spawn : MonoBehaviour
{
    public float spawn_time;
    private float increasing_time;
    private float fixed_spawn_time;

    public GameObject spawn_point;

    public Image spawn_image;

    public List<GameObject> powerups = new List<GameObject>();

    private bool spawned;

	// Use this for initialization
	void Start ()
    {
        fixed_spawn_time = spawn_time;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //spawn_time -= Time.deltaTime;
        increasing_time += Time.deltaTime;
        if(increasing_time <= spawn_time)
        {
            spawn_image.fillAmount = ((increasing_time - 0) / (fixed_spawn_time - 0)) * (1 - 0) + 0;
        }
    }
}
