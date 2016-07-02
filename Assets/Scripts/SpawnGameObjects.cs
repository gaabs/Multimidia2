using UnityEngine;
using System.Collections;

public class SpawnGameObjects : MonoBehaviour {

	public GameObject spawnPrefab;
    public int wave = 1;
	public float minSecondsBetweenSpawning = 3.0f;
	public float maxSecondsBetweenSpawning = 3.0f;
    public int numSpawn = 5;
    public int count = 0;
	public Transform chaseTarget;
	
	private float savedTime;
	private float secondsBetweenSpawning;

	// Use this for initialization
	void Start () {
		savedTime = Time.time;
		secondsBetweenSpawning = Random.Range (minSecondsBetweenSpawning, maxSecondsBetweenSpawning);
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.gm.gameIsOver)
			return;
		
		if (Time.time - savedTime >= secondsBetweenSpawning && count<numSpawn) // is it time to spawn again?
		{
			MakeThingToSpawn();
			savedTime = Time.time; // store for next spawn
			secondsBetweenSpawning = Random.Range (minSecondsBetweenSpawning, maxSecondsBetweenSpawning);
            count++;
        }

        if (count == numSpawn && GameManager.gm.wave != wave)
        {
            wave++;
            count = 0;
            numSpawn += 2;
/*            if (minSecondsBetweenSpawning > 1)
            {
                minSecondsBetweenSpawning -= 1;
            }
            if (maxSecondsBetweenSpawning > 2) { 
                 maxSecondsBetweenSpawning -= 1;
            }*/
        }
        
	}

	void MakeThingToSpawn()
	{
		// create a new gameObject
		GameObject clone = Instantiate(spawnPrefab, transform.position, transform.rotation) as GameObject;        
	}
}
