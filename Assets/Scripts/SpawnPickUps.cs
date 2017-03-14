using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPickUps : MonoBehaviour {

	public GameObject pickUp;
	public GameObject field;
	public float minTime;
	public float maxTime;
	public float waitBeforeStart;

	private float timeUntilNext;
	private Vector2 spawnPos;
	private float spawnBorderWidth;
	private float spawnBorderHeight;


	// Use this for initialization
	void Start () {
		timeUntilNext = setRandomTime ();
		spawnBorderWidth = 1.0f;
		spawnBorderHeight = 0.5f;
		StartCoroutine (WaitAtTheStart ());
	}

	IEnumerator WaitAtTheStart()
	{
		yield return new WaitForSeconds (waitBeforeStart);
	}
	
	// Update is called once per frame
	void Update () {
		timeUntilNext -= Time.deltaTime;

		if(timeUntilNext <= 0)
		{
			spawnPos = setSpawnPos ();
			Instantiate (pickUp, spawnPos, transform.rotation);
			timeUntilNext = setRandomTime ();
		}
	}

	// Returns a position within the playing field, which is further limited by two offset variables
	Vector2 setSpawnPos()
	{
		float halfWidth = field.GetComponent<Renderer> ().bounds.size.x / 2;
		float halfHeight = field.GetComponent<Renderer> ().bounds.size.y / 2;
		Vector3 center = field.GetComponent<Renderer> ().bounds.center;

		return new Vector2 (center.x + Random.Range (-halfWidth + spawnBorderWidth, halfWidth - spawnBorderWidth), 
							center.y + Random.Range (-halfHeight + spawnBorderHeight, halfHeight - spawnBorderHeight));
	}


	float setRandomTime()
	{
		return Random.Range (minTime, maxTime);
	}
}
