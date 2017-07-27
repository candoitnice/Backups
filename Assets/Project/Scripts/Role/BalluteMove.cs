using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalluteMove : MonoBehaviour
{

    void Start()
    {
        Destroy(gameObject,60);
    }
    public void Update()
    {
        M();
    }

    float f=1;
    public void M()
    {
        if (Recovery.GameData.Instance.isStopGame)
            return;
        if (f > 0)
        {
            f -= Time.deltaTime * 0.3f;
            transform.position += transform.up * Time.deltaTime * f;
        }
        if (transform.position.z < 9)
        {
            transform.position += Vector3.up * Time.deltaTime * (1 - f);
            transform.position += new Vector3(0, 0, 1 * Time.deltaTime);
        }
        else
        {
            GameObject g = Recovery.GameManager.instance.EstablishProp(Source.Effect.Ballute_Blast,transform.position);
            Destroy(g, 3);
            Destroy(gameObject);
        }
    }
}
