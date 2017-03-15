using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GraphicPanelInit : MonoBehaviour {

	public Dropdown resDD;
	public Dropdown fullscreenDD;

	// Use this for initialization
	void Start () {
		// Init resolution dropdown
		initResolutionDropdown (resDD, Screen.fullScreen);

	}

	/**
	// Get Dropdown Child Element
	Dropdown getDropDown(string theName)
	{
		Dropdown[] dropdowns = GetComponentsInChildren<Dropdown> ();
		foreach(Dropdown comp in dropdowns )
		{
			if (comp.name == theName)
				return comp;
		}
		throw new UnityException ("Could not find dropdown");
	}
	*/

	public void initResolutionDropdown(Dropdown resDD, bool fullscreen)
	{
		// Init Dropdown List
		resDD.ClearOptions ();

		// Get all resolutions 
		Resolution[] resolutions = Screen.resolutions;
		List<string> resList = new List<string> ();

		foreach (Resolution res in resolutions) {
			resList.Add (res.width + "x" + res.height);
		}
		resList = resList.Distinct().ToList ();

		// Add all resolutions
		resDD.AddOptions (resList);

		// Add listener to change resolution
		resDD.onValueChanged.AddListener(delegate { Screen.SetResolution(resolutions[resDD.value].width,
			resolutions[resDD.value].height, fullscreen); });
	}



	public void toggleLeFullscreen(int menuItemIndex)
	{
		if(menuItemIndex == 0)
		{
			Screen.fullScreen = false;
			initResolutionDropdown (resDD, false);
			print ("Windowed");
		}
		else if (menuItemIndex == 1)
		{
			Screen.fullScreen = true;
			initResolutionDropdown (resDD, true);
			print ("Fullscreen");
		}
	}


}
