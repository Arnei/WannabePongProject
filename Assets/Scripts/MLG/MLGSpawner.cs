using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLGSpawner : MonoBehaviour {

	public GameObject field;
	public int amountOfObjects;

	private float spawnBorderWidth;
	private float spawnBorderHeight;

	private string[] mlgNames;
	private Queue<GameObject> mlgObjects;
	private AudioSource audioSource;

	// Using Awake instead of Start as that gets called before OnEnable
	void Awake () {
		//field = GameObject.Find ("Field");
		spawnBorderWidth = 1.0f;
		spawnBorderHeight = 0.5f;
		mlgNames = new string[]{"MLG_frog", "MLG_doritos", "MLG_weed", "MLG_mlg"};
		mlgObjects = new Queue<GameObject> ();
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Called when the object is enabled
	void OnEnable()
	{
		// NOW DEAD
	}

	void OnDisable()
	{

	}

	public void startMLG()
	{
		int remainingAmountOfMLG = amountOfObjects;

		for(; remainingAmountOfMLG > 0; remainingAmountOfMLG--)
		{
			string name = randomMlgName ();
			Vector2 spawnPos = setSpawnPos ();
			//print (spawnPos);
			mlgObjects.Enqueue((GameObject)Instantiate(Resources.Load("Prefabs/"+name), spawnPos, transform.rotation));
			audioSource.Play ();
		}
	}

	public void stopMLG()
	{
		for(int i=0; i < amountOfObjects; i++)
		{
			Destroy (mlgObjects.Dequeue());
		}
	}

	// Spawn in a square around the MLGSpawner, with
	Vector2 setSpawnPos()
	{
		float half;
		float fieldWidth = field.GetComponent<Renderer> ().bounds.size.x;
		float fieldHeight = field.GetComponent<Renderer> ().bounds.size.y;

		if(fieldWidth > fieldHeight)
		{
			half = fieldHeight / 2;
		}
		else
		{
			half = fieldWidth / 2;
		}

		Vector3 center = transform.position;

		return new Vector2 (center.x + Random.Range (-half, half), 
							center.y + Random.Range (-half, half));
	}

	string randomMlgName()
	{
		print (mlgNames.Length);
		int index = Random.Range(0, mlgNames.Length);
		return mlgNames[index];
	}
}
