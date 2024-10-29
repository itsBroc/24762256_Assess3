using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    // Start is called before the first frame update
    public Text timerTxt;
    private float startTime;
    private bool isTiming;

    void Start()
    {
        startTime = Time.time;
        isTiming = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTiming)
        {
            float timeElapsed = Time.time - startTime;
            int mins = Mathf.FloorToInt(timeElapsed / 60);
            int secs = Mathf.FloorToInt(timeElapsed % 60);
            int ms = Mathf.FloorToInt((timeElapsed * 100) % 100);
            timerTxt.text = string.Format("{0:00}:{1:00}:{2:00}", mins, secs, ms);
        }
    }
}
