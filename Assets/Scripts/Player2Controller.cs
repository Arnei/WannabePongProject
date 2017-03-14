using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour {

	public float speed;
	public int invert;
	public int invertExtend;

	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {

	}

	// Before any physics calculations
	void FixedUpdate()
	{
		float moveVertical = Input.GetAxisRaw ("P2_Vertical");
		rb2d.velocity = new Vector2 (0, moveVertical) * speed * invert;
	}
}
