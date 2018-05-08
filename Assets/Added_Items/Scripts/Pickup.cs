using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum powerup_types { generic, SpeedBoost, ExtraLife, Invincible, EnemyFreeze, Nuke, AreaWipe }

public class Pickup : MonoBehaviour
{
    [Header("The type of powerup this is")]
    public powerup_types powerup_type;

    //[Header("Used for rotating the model")]
    //public GameObject powerup_body;
    //public float rotation_speed;

    [Header("Variables to be altered by powerup")]
    [Header("Only variable for particular powerup need to be set")]

    [Header("General")]
    public float powerup_active_time;

    [Header("SpeedBoost")]
    public float Sprint_Modifier;

    private Pickup_Spawn spawn_point;

    //[Header("Area Wipe")]
    //public GameObject arena;
    //public GameObject og_arena;
    //public List<Color> colorstorer = new List<Color>();


	// Use this for initialization
	void Start ()
    {
        //if(powerup_type == powerup_types.AreaWipe)
        //{
        //    og_arena = GameObject.FindGameObjectWithTag("ArenaFloor");
        //}
	}

    // Update is called once per frame
    void Update()
    {
        //float step = rotation_speed * Time.deltaTime;
        //powerup_body.transform.Rotate(Vector3.forward, step);
    }

    public void Spawned (Pickup_Spawn spawnpoint)
    {
        spawn_point = spawnpoint;
    }

    void OnTriggerEnter (Collider col)
    {
        Debug.Log("Triggered");
        if(col.transform.gameObject.tag == "Player")
        {
            GameObject player = col.transform.gameObject;
            PlayerInput player_script = player.GetComponent<PlayerInput>();
            if(!player_script.occupied)
            {
                player_script.occupied = true;
                player_script.pt = powerup_type;
                player_script.timer = powerup_active_time;
                player_script.powerup_icon.sprite = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("Player not recognised");
        }
    }
}
