using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalluteSpriteArray : MonoBehaviour
{
    public static BalluteSpriteArray Instance;

    void Awake()
    {
        Instance = this;
    }

    public Sprite[] redSprite;
    public Sprite[] blueSprite;
    public Sprite[] violetSprite;
    public Sprite[] greenSprite;

}
