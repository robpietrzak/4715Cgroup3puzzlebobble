using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    float speed = 5;
    public GameObject creditsTextObject;
    public GameObject blackScreen;
    // Start is called before the first frame update
   void Start()
    {
        Update();
   //     creditsTextObject.SetActive(false);
   //     blackScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
       // {
            transform.position = transform.position + new Vector3(0f, -700f, 0f);
            creditsTextObject.SetActive(true);
            blackScreen.SetActive(true);
            transform.Translate(Vector2.up * speed);
       // }
    }
}