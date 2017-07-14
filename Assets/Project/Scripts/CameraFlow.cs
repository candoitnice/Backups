using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]
public class CameraFlow : MonoBehaviour {

    public Transform target;

    public Vector3 offset = Vector3.zero;
    public Vector3 lookOffset = Vector3.zero;


	// Use this for initialization
	void Start ()
    {
        pos=transform.position; angles = transform.localEulerAngles;
        //transform.position = target.transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            transform.position = Vector3.Slerp(transform.position, target.position + transform.forward, 0.618f * 50);
            Vector3 fwd = target.forward;
            transform.forward = Vector3.Lerp(transform.forward, fwd,Time.deltaTime);
            //transform.position = Vector3.Slerp(transform.position, target.position + offset, 0.1f);
        }
    }

    public void SetPos()
    {
        transform.position = target.transform.position;
    }
    Vector3 pos , angles;
    /// <summary>
       ///重新游戏
       /// </summary>
    public void GameAgain()
    {
        transform.position = pos;
        transform.localEulerAngles = angles;
        this.enabled = false;
    }
}
