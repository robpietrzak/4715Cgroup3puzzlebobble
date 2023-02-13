using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleDeleter : MonoBehaviour
{
    public GameObject BlueTwo;
    bool fgone = false;
    bool sgone = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{
       /* if(fgone == true) //&& sgone == true)
        {
            Destroy(gameObject);
        }
    }*/

    private void OnTriggerEnter2D(Collider2D first)
    {
        //pickupEffect = GetComponent<ParticleSystem>();
        if (first.tag == "BlueBubble" && this.tag == "BlueBubble" && BlueTwo.tag == "BlueBubble")
        {
            Destroy(first.gameObject);
            Destroy(this.gameObject);
            Destroy(BlueTwo.gameObject);
            fgone = true;
        }
    }

   /* private void OnSecondTriggerEnter2D(Collider2D second)
    {
        //pickupEffect = GetComponent<ParticleSystem>();
        if (second.tag == "BlueBubble" && fgone == true)
        {
            Destroy(second.gameObject);
            Destroy(this.gameObject);
            //fgone = true;
        }
    }*/

}
