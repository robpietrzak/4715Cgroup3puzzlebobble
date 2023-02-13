using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BubbleController : MonoBehaviour
{
    float timesecs = 31;
    int timeint;
    public TextMeshProUGUI timerText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeint = (int)timesecs;

        timerText.text = timeint.ToString();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
    }

    void timer()
    {

        if (timesecs > 0)
        {
            timesecs -= Time.deltaTime;
        }

    }

}
