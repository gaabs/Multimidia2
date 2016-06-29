using UnityEngine;
using System.Collections;
using Image = UnityEngine.UI.Image;

public class MiniBossBehavior : MonoBehaviour {
	Health health;
	public float maxHealth;
    public float curHealth;
    private float curHealthBar;
    public float count;
    public int controle;
    public float atk;
    private GameObject healthbar1;
    private GameObject healthbar2;
    Animator animator;
    //player player;
	//GameObject player;

	public GameObject projectile;
	public float power = 10.0f;

	public float speed = 20.0f;
	public float minDist = 1f;
	public Transform target;

	float maxScale; 


	// Use this for initialization
	void Start () {
        count = 0;
        controle = 0;
        animator = GetComponent<Animator>();
		//health = new Health ();
		//health.healthPoints = curHealth;
		curHealthBar = curHealth;
		healthbar1 = (gameObject.transform.FindChild ("MB_HealthBar").FindChild("MB_life")).gameObject;
        healthbar2 = (gameObject.transform.FindChild("MB_HealthBar").FindChild("MB_life2")).gameObject;

        if (target == null) {
			target = GameObject.FindWithTag ("Player").transform;
		}

		//player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.gm.gameIsOver) {
			animator.Play("idle");
			animator.Stop ();
			return;
		}
       
        if (curHealth != curHealthBar) {
          if (healthbar2.transform.localScale.x > 0)
            {
                Vector3 temp = healthbar2.transform.localScale;
                temp.x = (curHealth / 100);
                if (curHealth/100 >= 1) {
                    temp.x -= 1;
                }
                else
                {
                    temp.x = 0;
                }
                healthbar2.transform.localScale = temp;
                
            }
            else
            {
                Vector3 temp = healthbar1.transform.localScale;
                temp.x = (curHealth / 100);
                healthbar1.transform.localScale = temp;
            }
        }
        AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
        count = Mathf.Round(asi.normalizedTime * 100f) / 100f;
        transform.LookAt(target);

        if (asi.IsName("attack") && controle == 1 && count >= 0.8)
        {
            attack(atk);
            controle--;
            //get the distance between the chaser and the target
            float distance = Vector3.Distance(transform.position, target.position);

            //so long as the chaser is farther away than the minimum distance, move towards it at rate speed.
            if (distance > minDist) animator.Play("walk");

        }

        else if (asi.IsName("idle") && controle == 0)
        {
            controle++;
        }

        else if ((asi.IsName("death") && !animator.IsInTransition(0) && count >= 1 && controle < 2))
        {
            GameManager.gm.targetHit(1, 0);
            controle = 2;
            Destroy(this.gameObject);
        }

        else if (asi.IsName("walk") && target)
        {
            // face the target
            transform.LookAt(target);

            //get the distance between the chaser and the target
            float distance = Vector3.Distance(transform.position, target.position);

            //so long as the chaser is farther away than the minimum distance, move towards it at rate speed.
            if (distance > minDist)
                transform.position += transform.forward * speed * Time.deltaTime;
            else
                animator.Play("idle");
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
		newProjectile.AddComponent<Damage> ();
		Damage damage = newProjectile.GetComponent<Damage> ();
		damage.damageAmount = atk;
        
		// if the projectile does not have a rigidbody component, add one
		if (!newProjectile.GetComponent<Rigidbody>()) 
		{
			newProjectile.AddComponent<Rigidbody>();
		}
		// Apply force to the newProjectile's Rigidbody component if it has one
		newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * power, ForceMode.VelocityChange);

        //player.damage(atk);

    }

	public void decreasingHealth(float damage) {
         if (curHealth > 0)
        {
			curHealth -= damage;

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
			//animator.Play("walkFront", 0);
        }

    }

    void OnCollisionEnter(Collision col) {
		print ("MiniBoss health: " + curHealth);
		/*
		if (col.gameObject is Damager) {
			float damage = (Damager)col.gameObject.damage;
			Destroy (col.gameObject);
			decreasingHealth (damage);
		}
		*/

		Damage damager = col.gameObject.GetComponent<Damage> ();
		if (damager) {
			float damage = damager.damageAmount;
			Destroy (col.gameObject);
			decreasingHealth (damage);
		}
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
