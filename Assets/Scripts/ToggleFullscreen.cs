using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFullscreen : MonoBehaviour {

	private GraphicPanelInit init_script;

	// Use this for initialization
	void Start () {
		init_script = GetComponentInParent<GraphicPanelInit> ();	
	}
	
	public void toggleFullscreen()
	{
		Screen.fullScreen = !Screen.fullScreen;
		// Reinit resolution menu to adapt to fullscreen/windowed
		//init_script.initResolutionDropdown (init_script.resDD, Screen.fullScreen);
	}
}
