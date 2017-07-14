using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeDown : MonoBehaviour
{

    public UnityEvent PlayEvent;

    public Text timeText;

    public float _timeCount = 5;
    private float timeCount = 5;

    public Texture[] downTexture;


    private bool isDownCount =false;
    // Use this for initialization
    void Start()
    {
    }
   public void OnPlay()
    {
        isDownCount = true;
        if (downTexture.Length > 0)
            timeCount = downTexture.Length;
        else
            timeCount = _timeCount + 1;
    }


    // Update is called once per frame
    void Update()
    {

        if (isDownCount)
        {
            timeCount -= Time.deltaTime;
            if (downTexture.Length > 0)
            {
                if ((int)timeCount >= 0)
                {
                    timeText.text = "";
                    //Debug.Log((int)_timeCount);
                    GetComponent<RawImage>().texture = downTexture[(int)timeCount];
                }
                else
                {
                    PlayEvent.Invoke();
                    isDownCount = false;
                }
            }
            else
            {
                if ((int)timeCount > 0)
                    timeText.text = ((int)timeCount).ToString();
                else
                {
                    PlayEvent.Invoke();
                    isDownCount = false;
                }
            }
        }
    }
}
