using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Controller : MonoBehaviour {

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
		//float moveVertical = Input.GetAxis ("P1_Vertical");
		//Vector2 movement = new Vector2 (0, moveVertical);
		//transform.Translate (movement * speed * Time.deltaTime);
		//rb2d.AddForce (movement * speed, ForceMode2D.Impulse);
	}

	// Before any physics calculations
	void FixedUpdate()
	{
		float moveVertical = Input.GetAxisRaw ("P1_Vertical");
		rb2d.velocity = new Vector2 (0, moveVertical) * speed * invert;
	}

}
