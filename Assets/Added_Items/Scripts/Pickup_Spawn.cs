using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup_Spawn : MonoBehaviour
{
    public float spawn_time;
    private float increasing_time;
    private float fixed_spawn_time;
	
	private Vector3 randomPosition;
	
    //public GameObject spawn_point;

    //public Image spawn_image;

    public List<GameObject> powerups = new List<GameObject>();

    private bool spawned;

	// Use this for initialization
	void Start ()
    {
        fixed_spawn_time = spawn_time;
		randomPosition = new Vector3(Random.Range (-10.5f, 9.5f), 0, Random.Range (6.0f, -5.0f));
	}
	
	// Update is called once per frame
	void Update ()
    {
        //spawn_time -= Time.deltaTime;
        increasing_time += Time.deltaTime;
		
        if(increasing_time <= spawn_time + 0.1f)
        {
            //spawn_image.fillAmount = ((increasing_time - 0) / (fixed_spawn_time - 0)) * (1 - 0) + 0;
        }
        else
        {
            if (!spawned)
            {
                spawned = true;
                //spawn_image.enabled = false;
                int random_number = Random.Range(0, powerups.Count);
                GameObject powerup = Instantiate(powerups[random_number], randomPosition, this.transform.rotation) as GameObject;
                Pickup powerup_script = powerup.GetComponent<Pickup>();
                powerup_script.Spawned(this);
            }
        }
    }

    public void Powerup_Taken ()
    {
        //spawn_image.enabled = true;
        increasing_time = 0.0f;
		randomPosition = new Vector3(Random.Range (-10.5f, 9.5f), 0, Random.Range (6.0f, -5.0f));
        spawned = false;
    }
}