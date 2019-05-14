using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
	public AudioSource winningSound;
	public Transform shotLocation;
	public GameObject playerExplosion;
	public GameObject bulletOne;
	PlayerMovement player;
	private float bossHealth;
	private int frameTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
		bossHealth = 200;
		player = GameObject.Find("Player").GetComponent<PlayerMovement>();		
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 100, 0) * Time.deltaTime);  
		
		if(frameTimer == 20) {
		
			Instantiate(bulletOne, shotLocation.position, shotLocation.rotation);
			frameTimer = 0;
		}
		frameTimer++;
    }
	
	void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "Bullet") 
		{
			bossHealth -= 10;
			if (bossHealth <= 0) {
				Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
				bossDeath();
			}
		} 
	}
	
	void bossDeath() {
		
		player.winning();
		Destroy(this.gameObject);
	}
}
