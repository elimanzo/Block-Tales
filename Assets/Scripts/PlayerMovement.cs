using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;            // The speed that the player will move at.
    
    Vector3 movement;                   // The vector to store the direction of the player's movement.
    Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 100f;          // The length of the ray from the camera into the scene.

	public Text healthText;
	public Text expText;
	public Text lvlText;
	public Text npcText;
	
	private int revolver;
	private int exp = 0;
	private int health;
	private int maxhealth;
	private int level =1;

	public Transform shotLocation;
	public GameObject bulletOne;
 
	public AudioSource fireSound;
	public AudioSource heartSound;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;
    public float fireRate;
	private float nextFire;
   
    private GameObject triggeringNPC;
	private bool npcTriggering;
	private bool bossTriggering;
	private bool invincible = false;
	private bool win = false;
	public Flowchart flowchart;
	
	private bool gameOver;
	public GameObject playerExplosion;
	// cheap fix for not having multiple player explosion
	private int instantiateOnce;
    void Start()
    {
		AudioSource[] audio = GetComponents<AudioSource>();
		maxhealth = 10;
		health = maxhealth;
		revolver = 200;
		SetHealthText ();
		SetlvlText();
		SetExpText ();
		gameOver = false;
		instantiateOnce = 0;
		
		heartSound = audio[0];
		fireSound = audio[1];
    }
	
    void Awake ()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask ("Floor");

        playerRigidbody = GetComponent <Rigidbody> ();
        // Audiosource  //
        //source = GetComponent<AudioSource>();
    }

	void Update() 
	{
		if(!gameOver && !win) {
			npcText.text = "";	
		}
		if(gameOver) {
			npcText.text = "You Died! Press R to start over!";
			playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
			if (Input.GetKeyDown(KeyCode.R))
            {
				gameOver = false;
				instantiateOnce = 0;
				foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) {
					Destroy(o);
				}
                SceneManager.LoadScene(1);
				
            }
		}
		
		if(!gameOver && win) {
			npcText.text = "You Beat the Monster! Press R to go back to Main Menu!";
			playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
			if (Input.GetKeyDown(KeyCode.R))
            {
				gameOver = false;
				win = false;
				instantiateOnce = 0;
				foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) {
					Destroy(o);
				}
                SceneManager.LoadScene(0);
            }
		}
		
		if(npcTriggering) 
		{
			npcText.text = "Press E to Talk!";
			if(Input.GetKey("e")) 
			{
				npcTriggering = false;
				flowchart.ExecuteBlock("NPC Bob");
				npcText.text = "";
			}
			
		} else if (bossTriggering)
		{
			npcText.text = "Press E to go to the Boss Room!";
			
			if(Input.GetKey("e")) 
			{
				Camera.main.fieldOfView = 90f;
				bossTriggering = false;
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
				npcText.text = "";
			}
		}
		
		if(!gameOver) 
		{
			if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
			{
				//source.PlayOneShot(shootsound, 1f);
				nextFire = Time.time + fireRate;
				Instantiate(bulletOne, shotLocation.position, shotLocation.rotation);
				fireSound.Play();
				//revolver = 0;
			}
		}
		/*Level UP and Health
		*/
		if(exp == 10 && level == 1){
			level++;
			SetlvlText();
			
			maxhealth = 20;
			health = maxhealth;
			SetHealthText();
			
			speed = 8f;
		}
			if(exp == 25 && level == 2){
			level++;
			SetlvlText();
			
			maxhealth = 30;
			health = maxhealth;
			SetHealthText();
			
			speed = 10f;
		}
		
			if(exp == 50 && level == 3){
			level++;
			SetlvlText();
			
			maxhealth = 40;
			health = maxhealth;
			SetHealthText();
			
			speed = 12f;
		}
			if(exp == 100 && level ==4){
			level++;
			SetlvlText();
			
			maxhealth = 50;
			health = maxhealth;
			SetHealthText();
			
			speed = 14f;
		}
		
		checkGameOver();
    }
	
    void FixedUpdate ()
    {
        // Store the input axes.
        float h = Input.GetAxisRaw ("Horizontal");
        float v = Input.GetAxisRaw ("Vertical");

        // Move the player around the scene.
        Move (h, v);

        // Turn the player to face the mouse cursor.
        Turning ();

        /* Shoot */
        /*if(Input.GetMouseButtonDown(0) && revolver > 10	){
            source.PlayOneShot(shootsound, 1f);
			Instantiate(bulletOne, shotLocation.position,shotLocation.rotation);
			revolver = 0;
		}
		revolver++;*/
       
       // revolver++;
    }

    void Move (float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set (h, 0f, v);
        
        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        playerRigidbody.MovePosition (transform.position + movement);
    }

    void Turning ()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation (playerToMouse);

            // Set the player's rotation to this new rotation.
            playerRigidbody.MoveRotation (newRotation);
        }
    }
	
	void SetHealthText ()
	{
		healthText.text = "Health: " + health.ToString () + "/" + maxhealth.ToString() ;
		}
		
	void SetExpText ()
	{
			expText.text = "Exp: " + exp.ToString ();
		}
		
	void SetlvlText ()
		{
			lvlText.text = "Lv: " + level.ToString ();
	}
	
	void checkGameOver() {
		if(health <= 0) {
			gameOver = true;
			if (instantiateOnce == 0) 
			{
				Instantiate(playerExplosion, transform.position, transform.rotation);
			}
			instantiateOnce++;
		}
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "Bob") 
		{
			npcTriggering = true;
			triggeringNPC = other.gameObject;
		} 
		else if (other.tag == "Entrance") {
			bossTriggering = true;
			triggeringNPC = other.gameObject;
		}
		else if (other.tag == "Heart") {
			Destroy(other.gameObject);
			if (health+5 > maxhealth) 
			{
				health += (maxhealth - health);
			} else 
			{
				health += 5;
			}
			
			heartSound.Play();
		}
		if (!invincible)
		{
			if(other.tag == "Enemy"){
				invincible = true;
				health-=5;
				StartCoroutine(DoBlinks(0.12f, 0.1f));
				Invoke("resetInvulnerability", 2f);
			} 
			else if (other.tag == "Boss" || other.tag == "BossBullet")
			{
				invincible = true;
				health-=20;
				StartCoroutine(DoBlinks(0.12f, 0.1f));
				Invoke("resetInvulnerability", 2f);

			}
			SetHealthText();
		}
	}
	
	IEnumerator DoBlinks(float duration, float blinkTime) {
		Renderer renderer = GetComponentInChildren<Renderer>();
		while (duration > 0) {
			duration -= Time.deltaTime;

			//toggle renderer
			renderer.enabled = !renderer.enabled;

			//wait for a bit
			yield return new WaitForSeconds(blinkTime);
		}
		//make sure renderer is enabled when we exit
		renderer.enabled = true;

	}
	void resetInvulnerability()
	{	
		invincible = false;
	}
	
	void OnTriggerExit(Collider other)
    {
        if (other.tag == "Bob")
        {
            npcTriggering = false;
            triggeringNPC = null;
        } 
		else if (other.tag == "Entrance")
        {
            bossTriggering = false;
            triggeringNPC = null;
        }
    }
	/* level up player */
	public void OneExp(){
		exp++;
		SetExpText();
	}
	
	public void winning() {
		win = true;
	}
}