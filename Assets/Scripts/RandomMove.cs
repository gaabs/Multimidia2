using UnityEngine;
using System.Collections;

public class RandomMove : MonoBehaviour {

	public float speed = 0.1f;
	public float xMaxVariation,yMaxVariation,zMaxVariation;
	public bool superRandom = false;
	int i = 0;
	Vector3[] steps;
	Vector3 soFar;

	// Use this for initialization
	void Start () {
		steps = new Vector3[10];
		float x, y, z;
		for (int i = 0; i < 5; i++) {
			x = Random.value * xMaxVariation;
			if (Random.value > 0.5f)
				x *= -1;
			y = Random.value * xMaxVariation;
			z = Random.value * xMaxVariation;
			if (Random.value > 0.5f)
				z *= -1;
			steps [i] = new Vector3 (x, y, z);
			steps [i+5] = new Vector3 (-x, -y, -z);
		}
		soFar = steps [0];
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 temp = steps[i] * speed * Time.deltaTime;
		Vector3 dif = soFar - temp;

		if (steps[i].x > 0)
			temp.x = Mathf.Min (soFar.x, temp.x);
		else if (steps[i].x < 0)
			temp.x = Mathf.Max (soFar.x, temp.x);
		else
			soFar.x = 0;

		if (steps[i].y > 0)
			temp.y = Mathf.Min (soFar.y, temp.y);
		else if (steps[i].y < 0)
			temp.y = Mathf.Max (soFar.y, temp.y);
		else
			soFar.y = 0;

		if (steps[i].z > 0)
			temp.z = Mathf.Min (soFar.z, temp.z);
		else if (steps[i].z < 0)
			temp.z = Mathf.Max (soFar.z, temp.z);
		else
			soFar.z = 0;

		gameObject.transform.Translate (temp);

		soFar -= temp;

		if (Mathf.Abs(soFar.x) < 0.01 && Mathf.Abs(soFar.y) < 0.01 && Mathf.Abs(soFar.z) < 0.01){
			i++;
			if (i == 10) {
				i = 0;
				if (superRandom)
					Start ();
			}
			soFar = steps [i];
		}

	}
}
