using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfOnCollisionWithBall : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.name == "Ball")
			Destroy (gameObject);
	}
}
