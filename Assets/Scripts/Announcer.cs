using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Announcer : MonoBehaviour {

	List<PlayerInput> allPlayers;
	public AudioClip victorySound;
	public GameObject victoryUI;
	public Image splatImage;
	private bool gameHasEnded = false;

	void Start () {
		PlayerInput[] allPlayersStatic = GameObject.FindObjectsOfType<PlayerInput>();
		allPlayers = new List<PlayerInput>();

		for (int i = 0; i < allPlayersStatic.Length; ++i)
		{
			allPlayersStatic[i].ReadyUp(i);
			allPlayers.Add(allPlayersStatic[i]);
		}
		StartCoroutine(Countdown());
	}

	// Update is called once per frame
	void Update () {
		if (gameHasEnded)
			return;

		for (int i = 0; i < allPlayers.Count; ++i)
		{
			if (allPlayers[i] == null)
			{
				allPlayers.RemoveAt(i);
				break;
			}
		}
		if (allPlayers.Count == 1)
		{
			Victory(allPlayers[0]);
		}
		else if (allPlayers.Count == 0)
		{
			Victory2();
		}
	}

	void Victory(PlayerInput winner)
	{
		Camera.main.GetComponent<AudioSource>().Stop();
		Camera.main.GetComponent<AudioSource>().PlayOneShot(victorySound);
		splatImage.color = winner.paintingMaterial.GetColor("PaintBrushColour");
		winner.SendMessage("Die");
		victoryUI.SetActive(true);
		gameHasEnded = true;
	}

	void Victory2()
	{
		Camera.main.GetComponent<AudioSource>().Stop();
		Camera.main.GetComponent<AudioSource>().PlayOneShot(victorySound);

		victoryUI.SetActive(true);
		splatImage.color = Color.white;
		gameHasEnded = true;
	}

	IEnumerator Countdown()
	{
		SetPlayerText(new int[]{0});
		yield return new WaitForSeconds(1);
		SetPlayerText(new int[] { 1 });
		yield return new WaitForSeconds(1);
		SetPlayerText(new int[] { 2 });
		yield return new WaitForSeconds(1);
		SetPlayerText(new int[] { 3 });
		yield return new WaitForSeconds(1);
		SetPlayerText(new int[] {});
	}

	void SetPlayerText(int[] switchTheseOn)
	{
		for (int i = 0; i < transform.childCount; ++i)
		{
			if (switchTheseOn.Contains(i))
				transform.GetChild(i).gameObject.SetActive(true);
			else
				transform.GetChild(i).gameObject.SetActive(false);
		}
	}
}
