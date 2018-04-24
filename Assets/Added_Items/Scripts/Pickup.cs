using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum powerup_types { generic, SpeedBoost, ExtraLife, Invincible, Nuke, AreaWipe }

public class Pickup : MonoBehaviour
{
    [Header("The type of powerup this is")]
    public powerup_types powerup_type;

    [Header("Used for rotating the model")]
    public GameObject powerup_body;
    public float rotation_speed;

    [Header("Variables to be altered by powerup")]
    [Header("Only variable for particular powerup need to be set")]
    public float powerup_active_time;
    public float move_speed;

	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        float step = rotation_speed * Time.deltaTime;
        powerup_body.transform.Rotate(Vector3.forward, step);
    }

    void OnTriggerEnter (Collider col)
    {
        Debug.Log("Triggered");
        if(col.transform.gameObject.tag == "Player")
        {
            if(powerup_type == powerup_types.ExtraLife)
            {
                GameObject player = col.transform.gameObject;
                PlayerInput player_script = player.GetComponent<PlayerInput>();
                if(player_script.arelives && player_script.lives <= 2)
                {
                    player_script.PowerUp(powerup_type, powerup_active_time, move_speed);
                    Destroy(gameObject);
                }
            }
            else
            {
                GameObject player = col.transform.gameObject;
                PlayerInput player_script = player.GetComponent<PlayerInput>();
                player_script.PowerUp(powerup_type, powerup_active_time, move_speed);
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("Player not recognised");
        }
    }
}
