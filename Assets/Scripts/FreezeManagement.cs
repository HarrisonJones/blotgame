using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeManagement : MonoBehaviour
{
    public List<PlayerInput> players = new List<PlayerInput>();

    public void FreezeOn (PlayerInput player_input)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if(players[i] != player_input)
            {
                players[i].freezeOn = true;
                players[i].freeze_particle.Play();
                Camera.main.GetComponent<AudioSource>().PlayOneShot(players[i].freeze_audio);
            }
        }
    }

    public void FreezeOff ()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].freezeOn = false;
            players[i].freeze_particle.Stop();
        }
    }
}
