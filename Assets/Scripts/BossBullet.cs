using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {

        transform.Translate(0f,0f,0.20f);
    }
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			Destroy(gameObject);
		}
		if (other.gameObject.CompareTag("Wall"))
		{
			Destroy(gameObject);
		}
	}
}
