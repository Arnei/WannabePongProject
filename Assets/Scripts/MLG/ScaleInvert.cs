using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleInvert : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int Xinvert = random_invert ();
		int Yinvert = random_invert ();
		transform.localScale = new Vector3(transform.localScale.x * Xinvert, transform.localScale.y * Yinvert, transform.localScale.z);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	int random_invert()
	{
		int[] myarray = {-1, 1};
		int index = Random.Range(0, myarray.Length);
		return myarray[index];
	}
}
