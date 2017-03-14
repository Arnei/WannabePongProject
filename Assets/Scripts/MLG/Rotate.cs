using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

	public float speed;

	private int invert;

	// Use this for initialization
	void Start () {
		int[] myarray = {-1, 1};
		int index = Random.Range(0, myarray.Length);
		invert = myarray[index];
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 0, speed * Time.deltaTime * invert);
	}
}
