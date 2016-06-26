using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{

	// target impact on game
	public int scoreAmount = 0;
	public float timeAmount = 0.0f;
	private Health health;

	// explosion when hit?
	public GameObject explosionPrefab;

	void Start () 
	{
		health = this.GetComponent<Health>();
	}


	/*
	// when collided with another gameObject
	void OnCollisionEnter (Collision newCollision)
	{
		print (gameObject.name + " entrou colisao com " + newCollision.gameObject.name);

		// exit if there is a game manager and the game is over
		if (GameManager.gm) {
			if (GameManager.gm.gameIsOver)
				return;
		}

		// only do stuff if hit by a projectile
		if (newCollision.gameObject.tag == "Projectile") {
			if (explosionPrefab) {
				// Instantiate an explosion effect at the gameObjects position and rotation
				Instantiate (explosionPrefab, transform.position, transform.rotation);
			}

			// if game manager exists, make adjustments based on target properties
			if (GameManager.gm) {
				GameManager.gm.targetHit (scoreAmount, timeAmount);
			}
				
			// destroy the projectile
			Destroy (newCollision.gameObject);
				
			// destroy self
			Destroy (gameObject);
		}

		print ("Health: " + health.healthPoints);
	}
	*/

}
