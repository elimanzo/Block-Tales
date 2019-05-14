using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
	private GameObject player;
	private NavMeshAgent nav;
	
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
		player = GameObject.Find("Player");
		nav.speed = 8;
    }
    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(player.transform.position);

    }
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Bullet"))
		{
			Destroy (gameObject);
		}
	}

}
