﻿using UnityEngine;
using System.Collections;

public class player : MonoBehaviour
{
    public GameObject[] Prefabs;
    public Light Sun;
    public UnityEngine.UI.Text CurrentItemText;
    //j
    private GameObject currentPrefabObject;
    private DigitalRuby.PyroParticles.FireBaseScript currentPrefabScript;
    private int currentPrefabIndex;
    private float savedTime;
    Health health;
    public float playerHealth;
    public float playerHealthBar;
    private float sensitivityX = 1.1F;
    private float sensitivityY = 1.1F;
    private float minimumX = -360F;
    private float maximumX = 360F;
    private float minimumY = -60F;
    private float maximumY = 60F;
    private float rotationX = 0F;
    private float rotationY = 0F;
    private int num = 0;
    private Vector3 velocity;
    // Use this for initialization
    void Start()
    {
        savedTime = Time.time;
		health = gameObject.GetComponentInParent<Health> ();
		playerHealth = health.healthPoints;
		playerHealthBar = health.healthPoints;
        Input.gyro.enabled = true;
        Input.gyro.updateInterval = 0.02f;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.Find("Player_life");
		playerHealth = Mathf.Max (0, health.healthPoints);
        if (playerHealth>=0)
        {
            float x = ((playerHealthBar - playerHealth) * 0.01f);
            playerHealthBar = playerHealth;
            if (player.transform.localScale.x > x)
            {
                player.transform.localScale -= new Vector3(x, 0, 0);
            }
            else
            {
                player.transform.localScale = new Vector3(0, 0, 0);
            }
        }

        GameObject bar = GameObject.Find("Player_FireBallBar");
        if (bar.transform.localScale.x < 1)
        {
            bar.transform.localScale += new Vector3(0.1f, 0, 0);
        }

		if (GameManager.gm.gameIsOver)
			return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
           
            if(bar.transform.localScale.x>=1)
            {
                BeginEffect(0);
                bar.transform.localScale -= new Vector3(1,0,0);
                savedTime = Time.time;
             }
        }

		if (Input.GetKeyDown(KeyCode.Alpha2)) {

			if(bar.transform.localScale.x>=1)
			{
				BeginEffect(1);
				bar.transform.localScale -= new Vector3(1,0,0);
				savedTime = Time.time;
			}
		}
        //print(playerHealth);
    }
		    

    void FixedUpdate()
    {
        UpdateCameraLook();
    }
		
    private void BeginEffect(int i)
    {
        Vector3 pos;
        float yRot = transform.rotation.eulerAngles.y;
        Vector3 forwardY = Quaternion.Euler(0.0f, yRot, 0.0f) * Vector3.forward;
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 up = transform.up;
        Quaternion rotation = Quaternion.identity;
        currentPrefabObject = GameObject.Instantiate(Prefabs[i]);
		currentPrefabScript = currentPrefabObject.GetComponent<DigitalRuby.PyroParticles.FireConstantBaseScript>();
        if (currentPrefabScript == null)
        {
            // temporary effect, like a fireball
            currentPrefabScript = currentPrefabObject.GetComponent<DigitalRuby.PyroParticles.FireBaseScript>();
            if (currentPrefabScript.IsProjectile)
            {
                // set the start point near the player
                rotation = transform.rotation;
				pos = transform.position + forward;	
              //  pos = transform.position + forward + right + up;
            }
            else
            {
                // set the start point in front of the player a ways
                pos = transform.position + (forwardY * 10.0f);
            }
        }
        else
        {
            // set the start point in front of the player a ways, rotated the same way as the player
            pos = transform.position + (forwardY * 5.0f);
            rotation = transform.rotation;
            pos.y = 0.0f;
        }
        

		DigitalRuby.PyroParticles.FireProjectileScript projectileScript = currentPrefabObject.GetComponentInChildren<DigitalRuby.PyroParticles.FireProjectileScript>();
        if (projectileScript != null)
        {
            // make sure we don't collide with other friendly layers
            projectileScript.ProjectileCollisionLayers &= (~UnityEngine.LayerMask.NameToLayer("FriendlyLayer"));
        }


        currentPrefabObject.transform.position = pos;
        currentPrefabObject.transform.rotation = rotation;
    }



    private void UpdateCameraLook()
    {
//        velocity += (Input.acceleration*Time.fixedDeltaTime);
//        transform.Translate(velocity);
        transform.Rotate(-Input.gyro.rotationRate);

    }


}
