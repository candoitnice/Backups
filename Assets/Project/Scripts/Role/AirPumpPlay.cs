using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPumpPlay : MonoBehaviour {

    Animator ani;
    //bool isStop;
    void Awake()
    {
        ani = GetComponent<Animator>();
    }


	void Update () {
        Play();
    }

    void Play()
    {
        //if (Recovery.GameData.Instance.isStopGame)
        //{
        //    ani.SetBool("A", false);
        //    isStop = true;
        //}
        if (Recovery.GameData.Instance.isStopGame)
        {
            ani.speed = 0;
        }

        //if (isStop)
        //{
        //    ani.SetBool("A", true);
        //    isStop = false;
        //}
    }
}
