using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeColorWhenSelected : MonoBehaviour {

	private Text buttonText;

	// Use this for initialization
	void Start () {
		buttonText = GetComponentInChildren<Text> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnSelect(BaseEventData eventData)
	{
		print ("SELECTED");
		buttonText.color = InvertColor (buttonText.color);
	}

	private Color InvertColor (Color color){
		return new Color (1.0f-color.r, 1.0f-color.g, 1.0f-color.b);
	}
}
