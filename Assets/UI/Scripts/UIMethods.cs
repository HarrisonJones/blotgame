using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Simple, reusable scene/application methods that can be called on with Unity's UI buttons.
// By Peter Liang.
public class UIMethods : MonoBehaviour
{
	// Closes the game application. Will not work in editor mode.
	public void Quit()
	{
		Application.Quit();
	}

	// Loads the specified scene based on scene index (see scene index in Build Settings). Can be problematic if scenes are added/reordered constantly.
	public void LoadScene(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex);
	}

	// Loads the specified scene based on scene name. Name spelling is important.
	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}
