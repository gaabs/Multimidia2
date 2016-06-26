﻿using UnityEngine;
using System.Collections;

public class skelleton : MonoBehaviour {
	public float maxHealth;
    public float curHealth;
    private float curHealthBar;
    public float count;
    public int controle;
    public float atk;
    private GameObject healthbar;
    Animator animator;
    //player player;
	GameObject player;

	public GameObject projectile;
	public float power = 10.0f;

	public float speed = 20.0f;
	public float minDist = 1f;
	public Transform target;

	// Use this for initialization
	void Start () {
        count = 0;
        controle = 0;
        animator = GetComponent<Animator>();
		maxHealth = 100f;
		curHealth = 100f;
        curHealthBar = 100f;
        healthbar = GameObject.Find("life");
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
       
       
        if (curHealth != curHealthBar) {
            float x = ((curHealthBar - curHealth) * 0.01f);
            curHealthBar = curHealth;
            if(healthbar.transform.localScale.x > x)
            {
                healthbar.transform.localScale -= new Vector3(x,0,0);
            }
            else
            {
                healthbar.transform.localScale = new Vector3(0, 0, 0);
            }
        }
        AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
        count = Mathf.Round(asi.normalizedTime * 100f)/100f;
        if (asi.IsName("Attack") && controle==1&&count>=0.8) {
            attack(atk);
            controle--;
        }

        else if (asi.IsName("iddle") && controle == 0) {
            controle++;
        }

        else if ((asi.IsName("death") && !animator.IsInTransition(0) && count>=1))
        {
            Destroy(this.gameObject);
        }

        else if (asi.IsName("walkFront"))
        {
			// face the target
			transform.LookAt(target);

			//get the distance between the chaser and the target
			float distance = Vector3.Distance(transform.position,target.position);

			//so long as the chaser is farther away than the minimum distance, move towards it at rate speed.
			if(distance > minDist)	
				transform.position += transform.forward * speed * Time.deltaTime;
			else 
				animator.Play("iddle");
        }

    }

    public void walkFront()
    {
       Vector3 a = transform.position;
        if (transform.position.x == 0)
        {
            if (transform.position.z>0)
            {
                Vector3 b = new Vector3(a.x, a.y, a.z - 0.3f);
                transform.position = b;
            }
            else
            {
                Vector3 b = new Vector3(a.x, a.y, a.z + 0.3f);
                transform.position = b;
            }
        }
        else
        {
            if (transform.position.x >0)
            {
                Vector3 b = new Vector3(a.x - 0.3f, a.y, a.z );
                transform.position = b;
            }
            else
            {
                Vector3 b = new Vector3(a.x + 0.3f, a.y, a.z);
                transform.position = b;
            }
        }



        
       
    }

    public void attack(float atk) {

		// Instantiante projectile at the camera + 1 meter forward with camera rotation
		GameObject newProjectile = Instantiate(projectile, transform.position + transform.forward, transform.rotation) as GameObject;

		// if the projectile does not have a rigidbody component, add one
		if (!newProjectile.GetComponent<Rigidbody>()) 
		{
			newProjectile.AddComponent<Rigidbody>();
		}
		// Apply force to the newProjectile's Rigidbody component if it has one
		newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * power, ForceMode.VelocityChange);

        //player.damage(atk);

    }

    public void decreasingHealth() {
         if (curHealth > 0)
        {
           curHealth -= 20f;

			if (curHealth < 0)
				curHealth = 0;
        }
        if (curHealth <= 0)
        {
            animator.Play("death", 0);

        }
        else
        {
            animator.Play("hit", 0);
        }

    }

    void OnCollisionEnter(Collision col) {

        Destroy(col.gameObject);
        decreasingHealth();
    }


    void OnGUI() {
       

        /*GameObject player = GameObject.Find("Player_life");
        GUIStyle a = new GUIStyle();
        a.fontSize = 10;
        a.normal.textColor = Color.white;
        GUILayout.Label("playerHealth: "+playerHealth,a);
        GUILayout.Label("playerHealthBar: " + player.transform.localScale, a);

        */
    }

}
