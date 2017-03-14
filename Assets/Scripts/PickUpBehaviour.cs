using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBehaviour : MonoBehaviour {

	private GameObject player1;
	private GameObject player2;
	private GameObject field;
	private GameObject mlgSpawner;
	private MLGSpawner mlgScript;
	private GameObject ball;

	private int behaviour;
	private string effect;
	private bool alive;
	private float lifetime;
	private string playerName;

	// Use this for initialization
	void Start () {
		player1 = GameObject.Find ("Player 1");
		player2 = GameObject.Find ("Player 2");
		field = GameObject.Find ("Field");
		ball = GameObject.Find ("Ball");
		// Should be exactly one
		mlgSpawner = GameObject.Find("MLGSpawnerParent").transform.FindChild("MLGSpawner").gameObject;
		if(mlgSpawner == null)
		{
			print ("OH NO");
		}
		mlgScript = (MLGSpawner) mlgSpawner.GetComponent<MLGSpawner>();
	
		alive = false;
		lifetime = 0;
		playerName = "No name";

		/** Behaviour Table
		 * 1 = Double Size
		 * 2 = MLG
		 * 3 = Half Size
		 * 4 = Safety Net
		 * 5 = New Direction
		 * 6 = Invert Controls
		 * TBI 7 = Connecting Top and Bottom Walls
		 */
		behaviour = Random.Range (1, 7); 	//Integer version is max-exclusive!
	}
	
	// Update is called once per frame
	void Update () {
		// If the effect is in effect, count down to the end
		if(alive)
		{
			lifetime -= Time.deltaTime;
			if(lifetime <= 0)
			{
				if(playerName == "Player 1")
				{
					behaviourDecision (player1, false);
				}
				else
				{
					behaviourDecision (player2, false);
				}
				CleanUp ();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.name == "Ball")
		{
			// Case Player 1
			if(col.GetComponent<SpriteRenderer>().sprite.name == "BallRed")
			{
				// Effectively remove PickUp from Game
				gameObject.GetComponent<SpriteRenderer>().sprite = null;
				gameObject.GetComponent<Collider2D> ().enabled = false;

				playerName = "Player 1";
				behaviourDecision (player1, true);
			}
			// Case Player 2
			else if(col.GetComponent<SpriteRenderer>().sprite.name == "BallBlue")
			{
				// Effectively remove PickUp from Game
				gameObject.GetComponent<SpriteRenderer>().sprite = null;
				gameObject.GetComponent<Collider2D> ().enabled = false;

				playerName = "Player 2";
				behaviourDecision (player2, true);
			}
		}
	}

	void behaviourDecision(GameObject player, bool start)
	{
		if(start)
		{
			switch(behaviour)
			{
			case 1:
				doubleSizeStart (player);
				break;
			case 2:
				mlgStart (player);
				break;
			case 3:
				halfSizeStart (player);
				break;
			case 4:
				safetynetStart (player);
				break;
			case 5:
				newDirectionStart ();
				break;
			case 6:
				invertPlayerStart (player);
				break;
			default:
				break;
			}
		}
		else
		{
			switch(behaviour)
			{
			case 1:
				doubleSizeEnd (player);
				break;
			case 2:
				mlgEnd ();
				break;
			case 3:
				halfSizeEnd (player);
				break;
			case 4:
				// No End method needed
				break;
			case 5:
				// NO End method needed
				break;
			case 6:
				invertPlayerEnd (player);
				break;
			default:
				break;
			}
		}

	}

	void doubleSizeStart(GameObject player)
	{
		player.transform.localScale = player.transform.localScale * 2.0f;
		startCountdown (3.0f);
	}

	void doubleSizeEnd(GameObject player)
	{
		player.transform.localScale = player.transform.localScale / 2.0f;
	}

	void mlgStart(GameObject player)
	{
		// Calc start position. 0,0 is center of the screen
		float positionX;
		float positionY = 0.0f;
		float halfWidth = field.GetComponent<Renderer> ().bounds.size.x / 2;
		if(player.name == "Player 1")
		{
			positionX = -(halfWidth / 2);
		}
		else{
			positionX = halfWidth / 2;
		}

		// Start MLG
		mlgSpawner.transform.position = new Vector3 (positionX, positionY, 0.0f);
		mlgScript.startMLG (); //mlgSpawner.SetActive (true);

		startCountdown (3.0f);
	}

	void mlgEnd()
	{
		mlgScript.stopMLG (); //mlgSpawner.SetActive (false);
	}

	void halfSizeStart(GameObject player)
	{
		player.transform.localScale = player.transform.localScale / 2.0f;
		startCountdown (3.0f);
	}

	void halfSizeEnd(GameObject player)
	{
		player.transform.localScale = player.transform.localScale * 2.0f;
	}

	void safetynetStart(GameObject player)
	{
		// Set position
		Vector3 pos = new Vector3 (0.0f, 0.0f);
		float offset = 0.7f;
		if (player.transform.position.x >= 0)
			pos.x = player.transform.position.x + offset;
		else
			pos.x = player.transform.position.x - offset;

		// Check if there is already a Safetynet in the way
		bool safetynetExists = false;
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll (pos, 0.1f);
		for (int i = 0; i < hitColliders.Length;i++)
		{
			if (hitColliders [i].tag == "Safetynet")
				safetynetExists = true;
		}

		if(!safetynetExists)
			Instantiate (Resources.Load ("Prefabs/Safetynet"), pos, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));

		startCountdown (0.0f);	// Instantly destory PickUp object, because lifetime of Safetynet is independent
	}

	void newDirectionStart()
	{
		Vector2 cur_vel = ball.GetComponent<Rigidbody2D> ().velocity;

		Vector2 desiredDirection = new Vector2(0,0); 
		bool up;
		if (Random.Range (0.0f, 1.0f) < 0.5f)
			up = false;
		else
			up = true;

		// Decide upon new direcion
		if (cur_vel.x < 0 && up)
			desiredDirection = new Vector2(-0.5f, Random.Range(0.4f, 0.8f)); // set this to the direction you want.
		else if (cur_vel.x < 0 && !up)
			desiredDirection = new Vector2(-0.5f, Random.Range(-0.4f, -0.8f));
		else if (cur_vel.x > 0 && up)
			desiredDirection = new Vector2(0.5f, Random.Range(0.4f, 0.8f));
		else if (cur_vel.x > 0 && !up)
			desiredDirection = new Vector2(0.5f, Random.Range(-0.4f, -0.8f)); 

		// Keep magnitude of ball the same
		Vector2 newVelocity = desiredDirection.normalized * ball.GetComponent<Rigidbody2D>().velocity.magnitude; 
		ball.GetComponent<Rigidbody2D> ().velocity = newVelocity;

		startCountdown (0.0f);
	}

	// Invert Player Controller, keep track of that in Player Script
	// Keep track of multiple calls to extend the invert period
	void invertPlayerStart(GameObject player)
	{
		if (player.name == "Player 1")
		{
			Player1Controller p1cScript = player.GetComponent<Player1Controller> ();
			p1cScript.invert = -1;
			p1cScript.invertExtend += 1;
		}
		else if (player.name == "Player 2")
		{
			Player2Controller p2cScript = player.GetComponent<Player2Controller> ();
			p2cScript.invert = -1;
			p2cScript.invertExtend += 1;
		}

		startCountdown (3.0f);
	}

	void invertPlayerEnd(GameObject player)
	{
		if (player.name == "Player 1")
		{
			Player1Controller p1cScript = player.GetComponent<Player1Controller> ();
			p1cScript.invertExtend -= 1;
			if (p1cScript.invertExtend <= 0)
				p1cScript.invert = 1;
		}
		else if (player.name == "Player 2")
		{
			Player2Controller p2cScript = player.GetComponent<Player2Controller> ();
			p2cScript.invertExtend -= 1;
			if (p2cScript.invertExtend <= 0)
				p2cScript.invert = 1;
		}
	}
		
		
	// Set time till destruction
	void startCountdown(float ttl)
	{
		lifetime = ttl;
		alive = true;
	}

	void CleanUp()
	{
		Destroy (this.gameObject);
	}
}
