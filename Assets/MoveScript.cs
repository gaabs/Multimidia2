using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Horizontal") > 0)
			gameObject.transform.Translate (Vector3.right);
	}
}
