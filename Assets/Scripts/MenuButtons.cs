using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
	[Tooltip("Canvas items that will only appear when not selecting a level")]
	[SerializeField] private GameObject mainMenuObjects;

	[Tooltip("Canvas items that will only appear when selecting a level")]
	[SerializeField] private GameObject levelSelectObjects;
	public void loadNextScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	} 

	public void quitGame()
	{
		Application.Quit();
	}

	public void loadPreviousScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}

	public void showLevelSelect()
	{
		mainMenuObjects.SetActive(false);
		levelSelectObjects.SetActive(true);
	}

	public void hideLevelSelect()
	{
		levelSelectObjects.SetActive(false);
		mainMenuObjects.SetActive(true);
	}

	public void loadSceneByName(string name)
	{
		SceneManager.LoadScene(name);
	}
}
