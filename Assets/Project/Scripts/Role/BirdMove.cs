using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMove : MonoBehaviour
{
    float flo;
    RectTransform rectTrans;
    Animator ani;
    bool isStop;
    float _flo;
    void Start()
    {
        ani = GetComponent<Animator>();
        flo = Random.Range(10f, 60f);
        rectTrans = transform as RectTransform;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (Recovery.GameData.Instance.isStopGame)
        {
            ani.SetBool("A", false);
            isStop = true;
        }
        if (Recovery.GameData.Instance.isStopGame)
            return;
        if (isStop)
        {
            ani.SetBool("A", true);
            isStop = false;
        }
        flo -= Time.deltaTime;
        if (flo < 0 && flo != _flo)
        {
            rectTrans.anchoredPosition += Vector2.left;
            if (rectTrans.anchoredPosition.x <= -580)
            {
                rectTrans.anchoredPosition = new Vector2(580, 660);
                flo = Random.Range(10f, 60f);
                _flo = flo;
            }
        }
        else
        {
            rectTrans.anchoredPosition = new Vector2(580, 660);
        }
    }
}
