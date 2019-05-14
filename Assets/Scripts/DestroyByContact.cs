using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
    public GameObject playerExplosion;


    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Bullet")
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

    }
}