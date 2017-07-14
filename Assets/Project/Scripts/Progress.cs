using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Progress : MonoBehaviour {

    public Image progress;
    public float time=1;
    public UnityEvent PlayEvent;
    // Use this for initialization
    void Start ()
    {
        progress.fillAmount = 0;
        StartCoroutine(DaojiTime());
    }

    public IEnumerator DaojiTime()
    {
        while (true)
        {
            if (progress.fillAmount < 1)
            {
                progress.fillAmount += Time.deltaTime / time;
                yield return new WaitForFixedUpdate();
            }
            else
            {
                PlayEvent.Invoke();
                break;
            }
        }
    }
}
