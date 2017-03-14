using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour {

	public float speed;
	public float velocityInc;
	public Text scoreP1Text;
	public Text scoreP2Text;
	public Text countdownText;
	public Text P1GameOverText;
	public Text P2GameOverText;
	public EdgeCollider2D colliderP1;
	public EdgeCollider2D colliderP2;
	public Sprite spriteDefault;
	public Sprite spriteP1;
	public Sprite spriteP2;
	public AudioClip audioHitSomething;
	public AudioClip audioGameStart;
	public AudioClip audioBallExplodes;
	public AudioClip audioGameWon;
	public ParticleSystem psOnDeath;
	public ParticleSystem psTrail;

	private Rigidbody2D rb2d;
	private Vector2 startpos;
	private float startSpeed;
	private int scoreP1Int;
	private int scoreP2Int;
	private float countdownTime;
	private string whoScoredLast;
	private bool gameStarted;
	private SpriteRenderer spriteRenderer;
	private AudioSource audioSource;
	private float volLowRange;
	private float volHighRange;
	private Vector3 prev_loc;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		startpos = transform.position;
		startSpeed = speed;
		scoreP1Int = 0;
		scoreP2Int = 0;
		scoreP1Text.text = scoreP1Int.ToString();
		scoreP2Text.text = scoreP2Int.ToString();
		P1GameOverText.text = "";
		P2GameOverText.text = "";
		countdownTime = 3.0f;
		countdownText.text = "";
		whoScoredLast = "No one";
		gameStarted = false;
		spriteRenderer = GetComponent<SpriteRenderer>(); // we are accessing the SpriteRenderer that is attached to the Gameobject
		if (spriteRenderer.sprite == null) // if the sprite on spriteRenderer is null then
			spriteRenderer.sprite = spriteDefault; // set the sprite to sprite1
		audioSource = GetComponent<AudioSource>();
		volLowRange = 0.5f;
		volHighRange = 1.0f;
		prev_loc = transform.position;
	}

	// Update is called once per frame
	void Update () 
	{
		// Calculate the correct rotation for the trail
		Vector3 cur_vel = (transform.position - prev_loc); // / Time.deltaTime;
		if(cur_vel.y > 0)
			psTrail.transform.eulerAngles = new Vector3(Vector3.Angle(cur_vel, new Vector3(1, 0, 0)), -90, 0);
		else if (cur_vel.y < 0)
			psTrail.transform.eulerAngles = new Vector3(-Vector3.Angle(cur_vel, new Vector3(1, 0, 0)), -90, 0);
		else
			psTrail.transform.eulerAngles = new Vector3(0, 0, 0);
		

		//Manage Countdown
		if (countdownTime > -1.0)
		{
			countdown ();
		} 
		else if (countdownTime < -1.0)
		{
			countdownText.enabled = false;
		}

		//Start Game
		if (!gameStarted && countdownTime < 0)
		{
			gameStarted = true;
			audioSource.PlayOneShot (audioGameStart);
			startBall ();
		}

		prev_loc = transform.position;
	}

	void countdown()
	{
		countdownTime -= Time.deltaTime;
		countdownText.text = "" + Mathf.Round(countdownTime);
		if(countdownTime < 0)
		{
			countdownText.text = "GO!";
		}
	}

	// Generate proper random start direction
	void startBall()
	{
		Vector2 movement = new Vector2(0, 0);
		if (whoScoredLast == "P1") 
		{
			movement = Vector2.left;
		} 
		else if (whoScoredLast == "P2")
		{
			movement = Vector2.right;
		} 
		else
		{
			if(Random.Range(1.0f, -1.0f) < 0)
			{
				movement.x = -1.0f;
			}
			else
			{
				movement.x = 1.0f;
			}
		}
		movement.y = Random.Range (0.1f, 0.5f);
		movement = movement.normalized;
		rb2d.velocity = movement * speed;
	}

	// Before any physics calculations
	void FixedUpdate()
	{

	}



	void OnCollisionEnter2D(Collision2D col) 
	{
		// Play sound on collision
		float vol = Random.Range(volLowRange, volHighRange);
		audioSource.PlayOneShot (audioHitSomething, vol);


		// Note: 'col' holds the collision information. If the
		// Ball collided with a racket, then:
		//   col.gameObject is the racket
		//   col.transform.position is the racket's position
		//   col.collider is the racket's collider

		// Hit the left Racket?
		if (col.gameObject.tag == "Player")
		{
			// Calculate hit Factor
			float y = hitFactor(transform.position,
				col.transform.position,
				col.collider.bounds.size.y);

			Vector2 dir;
			if (col.gameObject.name == "Player 1") {
				// Calculate direction, make length=1 via .normalized
				dir = new Vector2 (1, y).normalized;
				spriteRenderer.sprite = spriteP1;
				ParticleSystem.MainModule psTrailMain = psTrail.main;		//Need to store mainModule in variable before usage
				psTrailMain.startColor = new ParticleSystem.MinMaxGradient(new Color32 (255, 0, 0, 255));
			} else {
				// Calculate direction, make length=1 via .normalized
				dir = new Vector2(-1, y).normalized;
				spriteRenderer.sprite = spriteP2;
				ParticleSystem.MainModule psTrailMain = psTrail.main;		//Need to store mainModule in variable before usage
				psTrailMain.startColor = new ParticleSystem.MinMaxGradient(new Color32 (0, 0, 255, 255));
			}

			// Set Velocity with dir * speed
			speed = speed + velocityInc;
			GetComponent<Rigidbody2D>().velocity = (dir * speed);
		}
	}

	float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight) 
	{
		return (ballPos.y - racketPos.y) / racketHeight;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col == colliderP1 || col == colliderP2)
		{
			goalScored (col);
		}
	}

	// If the ball collides with one of the goal colliders
	void goalScored(Collider2D col)
	{
		// Play sound on score
		float vol = Random.Range(volLowRange, volHighRange);
		audioSource.PlayOneShot (audioBallExplodes, vol);

		// Inc Score
		if (col == colliderP1)
		{
			scoreP2Int = scoreP2Int + 1;
			scoreP2Text.text = scoreP2Int.ToString ();
			whoScoredLast = "P2";
			// Spawn Particles
			psOnDeath.transform.position = transform.position;
			psOnDeath.transform.eulerAngles = new Vector3(0, 90, 0);
			psOnDeath.Play ();
		} else if(col == colliderP2) 
		{
			scoreP1Int = scoreP1Int + 1;
			scoreP1Text.text = scoreP1Int.ToString ();
			whoScoredLast = "P1";
			// Spawn Particles
			psOnDeath.transform.position = transform.position;
			psOnDeath.transform.eulerAngles = new Vector3(0, -90, 0);
			psOnDeath.Play ();
		}

		// Reset
		rb2d.velocity = new Vector2(0, 0);
		transform.position = startpos;
		speed = startSpeed;
		spriteRenderer.sprite = spriteDefault;
		ParticleSystem.MainModule psTrailMain = psTrail.main;		//Need to store mainModule in variable before usage
		psTrailMain.startColor = new ParticleSystem.MinMaxGradient(new Color32 (255, 127, 39, 255));


		// Check if game is over
		if(scoreP1Int >= 5 || scoreP2Int >= 5)
		{
			if (scoreP1Int >= 5) gameWon ("P1"); else gameWon("P2");
		}
		else
		{
			// Start again
			startBall ();
		}
	}

	// Display End of Game Text
	void gameWon(string whoWon)
	{
		audioSource.PlayOneShot(audioGameWon);

		if(whoWon == "P1")
		{
			P1GameOverText.text = "WIN!";
			P2GameOverText.text = "LOSE";
		}
		else
		{
			P1GameOverText.text = "LOSE";
			P2GameOverText.text = "WIN!";
		}
	}
}
