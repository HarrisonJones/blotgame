using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using InControl;

public class Overlord_Controller : MonoBehaviour
{

    public GameObject playerPrefab;

    public Transform[] spawnPositions;

    public Transform background;
    public int numberOfPlayers = 0;
    List<PlayerInput> playerList;

    Bounds goBounds;

    void Start()
    {
        StartCoroutine(SetupControls());
        goBounds = GetComponent<BoxCollider>().bounds;
        playerList = new List<PlayerInput>();
        Painter.ColoursInUse = new List<Color>();
    }

    void Update()
    {
        if (numberOfPlayers > 1)
        {
            bool anyFailures = false;
            List<Color> allColors = new List<Color>();

            for (int i = 0; i < playerList.Count; ++i)
            {
                if (allColors.Count == 0)
                    allColors.Add(playerList[i].currentColor);
                else if (allColors[0] != playerList[i].currentColor)
                    allColors.Add(playerList[i].currentColor);

                if (!goBounds.Contains(playerList[i].transform.position))
                {
                    anyFailures = true;
                }
            }

            if (allColors.Count == 1)
                anyFailures = true;

            if (!anyFailures)
                ReadyToPlay();
        }
    }

    void AddPlayer(PlayerInput.CharacterActions newcharacterActions)
    {
        GameObject newPlayer = GameObject.Instantiate(playerPrefab, spawnPositions[numberOfPlayers].position, Quaternion.Euler(0, 0, 0));
        playerList.Add(newPlayer.GetComponent<PlayerInput>());
        newPlayer.GetComponent<PlayerInput>().SetControls(newcharacterActions);

        SetPlayerText(numberOfPlayers, new int[] { 1, 4, 7, 11 });

        ++numberOfPlayers;
    }

    public void ReadyToPlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("AdamMain");
        StopAllCoroutines();

    }

    IEnumerator SetupControls()
    {
        List<BindingSource> usedSources = new List<BindingSource>();

        for (int i = 0; i < 4; ++i)
        {
            string displayText = "";

            PlayerInput.CharacterActions newCharacterActions = new PlayerInput.CharacterActions();

            newCharacterActions.ListenOptions.IncludeMouseScrollWheel = false;
            newCharacterActions.ListenOptions.AllowDuplicateBindingsPerSet = false;
            newCharacterActions.ListenOptions.IncludeMouseButtons = false;
            newCharacterActions.ListenOptions.IncludeUnknownControllers = true;
            newCharacterActions.ListenOptions.IncludeModifiersAsFirstClassKeys = true;
            newCharacterActions.ListenOptions.OnBindingFound = ((PlayerAction x, BindingSource newSource) => !usedSources.Contains(newSource));

            // A Button
            newCharacterActions.AButton.ListenForBinding();
            displayText = "Player " + (i + 1).ToString() + " press A Button";
            print(displayText);
            SetPlayerText(i, new int[] { 3, 5, 8, 12 });

            while (newCharacterActions.AButton.Bindings.Count <= 0)
            {

                yield return null;
            }

            usedSources.Add(newCharacterActions.AButton.Bindings[0]);

            // B Button
            newCharacterActions.BButton.ListenForBinding();
            displayText = "Player " + (i + 1).ToString() + " press B Button";
            print(displayText);

            SetPlayerText(i, new int[] { 4, 6, 8, 13 });
            while (newCharacterActions.BButton.Bindings.Count <= 0)
            {

                yield return null;
            }

            usedSources.Add(newCharacterActions.BButton.Bindings[0]);

            // Left
            newCharacterActions.Left.ListenForBinding();
            displayText = "Player " + (i + 1).ToString() + " press Left";
            print(displayText);

            SetPlayerText(i, new int[] { 4, 7, 8, 9, 14 });
            while (newCharacterActions.Left.Bindings.Count <= 0)
            {

                yield return null;
            }

            usedSources.Add(newCharacterActions.Left.Bindings[0]);

            // Right
            newCharacterActions.Right.ListenForBinding();
            displayText = "Player " + (i + 1).ToString() + " press Right";
            print(displayText);

            SetPlayerText(i, new int[] { 4, 7, 8, 10, 14 });
            while (newCharacterActions.Right.Bindings.Count <= 0)
            {

                yield return null;
            }

            usedSources.Add(newCharacterActions.Right.Bindings[0]);

            AddPlayer(newCharacterActions);
        }
    }

    void SetPlayerText(int playerIndex, int[] switchTheseOn)
    {
        Transform target = background.GetChild(playerIndex);

        for (int i = 0; i < target.childCount; ++i)
        {
            if (switchTheseOn.Contains(i))
                target.GetChild(i).gameObject.SetActive(true);
            else
                target.GetChild(i).gameObject.SetActive(false);
        }
    }
}