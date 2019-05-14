using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOne : MonoBehaviour
{
	private int die;
	PlayerMovement exp;
    void Start()
    {
        die =0;
		exp = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }


    void Update()
    {

        transform.Translate(0f,0f,0.20f);

		if(die == 100){
			Destroy(gameObject);
		}
		die++;
    }
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Boss"))
		{
			exp.OneExp();
		}
	}
}
