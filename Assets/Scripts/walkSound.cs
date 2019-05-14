using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkSound : MonoBehaviour
{
    public AudioSource walkingSound;

    // Start is called before the first frame update
    void Start()
    {
        walkingSound = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
       
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if(h != 0 || v != 0)
        {
            if (walkingSound.isPlaying)
            {
               // print("sound is playing");
            }
            else
            {
                walkingSound.Play();
            }
        }
        else if(h == 0 && v == 0)
        {
            walkingSound.Pause();
        }
    }
}
