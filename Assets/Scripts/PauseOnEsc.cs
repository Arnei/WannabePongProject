using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseOnEsc : MonoBehaviour {

	public GameObject pausePanel;
	public GameObject selectedObject;
	public EventSystem eventSystem;

	private bool paused;
	private Button continueButton;

	// Use this for initialization
	void Start () 
	{
		paused = false;
		Button[] buttons = pausePanel.GetComponentsInChildren<Button>();
		foreach (Button but in buttons)
		{
			if(but.name == "Continue Button")
			{
				continueButton = but;
			}
		}
		continueButton.onClick.AddListener (Resume);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown("escape"))
		{
			if(!paused)
			{
				Time.timeScale = 0.0f;
				pausePanel.SetActive(true);
				eventSystem.SetSelectedGameObject (selectedObject);
				paused = true;
			}
			else
			{
				Resume ();
			}
		}
	}

	void Resume()
	{
		Time.timeScale = 1.0f;
		pausePanel.SetActive(false);
		paused = false;
	}
}
