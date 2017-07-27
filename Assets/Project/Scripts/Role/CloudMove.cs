using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour {
    RectTransform[] RectTransArr;
    Vector2 vec2Start;
    Vector2 vec2End;
    Vector2 Speed;

	// Use this for initialization
	void Start () {
        vec2Start = new Vector2(-900,900);
        vec2End = new Vector2(900,400);
        Speed = new Vector2(3, -1);
        RectTransArr = new RectTransform[transform.childCount];
        for (int i = 0; i < RectTransArr.Length; i++)
        {
            RectTransArr[i] = transform.GetChild(i) as RectTransform;
        }
	}
	
	// Update is called once per frame
	void Update () {
        Move();
    }

    void Move()
    {
        if (Recovery.GameData.Instance.isStopGame)
            return;
        for (int i = 0; i < RectTransArr.Length; i++)
        {
            RectTransArr[i].anchoredPosition += Speed * 0.1f;
            RectTransArr[i].localScale = Vector3.Lerp(RectTransArr[i].localScale, Vector3.zero, Time.deltaTime*0.001f);
        }
        for (int i = 0; i < RectTransArr.Length; i++)
        {
            if (RectTransArr[i].anchoredPosition.x > vec2End.x && RectTransArr[i].anchoredPosition.y < vec2End.y)
            {
                RectTransArr[i].anchoredPosition = vec2Start;
                RectTransArr[i].localScale = Vector3.one;
            }
        }
    }



}
