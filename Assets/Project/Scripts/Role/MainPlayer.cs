using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 主要玩家
/// </summary>
public class MainPlayer : RoleBase
{
    public override void Start()
    {
        base.Start();
        target = transform.FindChild("target");

    }

    public override void Update()
    {
        base.Update();
        Move();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="Name"></param>
    /// <param name="ID"></param>
    /// <param name="isMainPlay"></param>
    /// <param name="no"></param>
    /// <param name="_name"></param>
    /// <param name="sex"></param>
    /// <param name="age"></param>
    public override void InitData(bool isConnect = true, Vector3 pos = default(Vector3), string Name = "player", string ID = "0", bool isMainPlay = false, string no = "111111111", string _name = "ZS", string sex = "30", string age = "M")
    {
        base.InitData(isConnect, pos, Name, ID, isMainPlay, no, _name, sex, age);
    }
    public override void SetInfo(float speed, float slope, float heartRate, float L_symmetry, float balloonCapacity)
    {
        base.SetInfo(speed, slope, heartRate, L_symmetry, balloonCapacity);
        this.balloonCapacity += balloonCapacity;
        distance += balloonCapacity;
    }


    public List<GameObject> cloneBalute = new List<GameObject>();
    internal Transform target;


    public override void Zoom()
    {
        base.Zoom();
        if (c >= 1)
        {
            GameObject cb = Recovery.GameManager.instance.EstablishProp("Prefabs/Ballute/" + _ballut.sprite.name);
            GameObject g = Recovery.GameManager.instance.EstablishProp(Source.Effect.blast, ballut.transform.position);
            Destroy(g, 3);
            cb.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            cb.transform.position = _ballut.transform.position;
            cb.transform.eulerAngles = _ballut.transform.eulerAngles;
            balloonCapacity = 0f;
            ballut.localScale = Vector3.zero;
            balluteCount++;

            cloneBalute.Add(cb);

            c = 0;
            cb.AddComponent<BalluteMove>();

            nowCount++;
            if (nowCount >= BalluteSpriteArray.Instance.greenSprite.Length)
                nowCount = 0;
            if (UsedCount < Recovery.GameData.Instance.allBall.Count)
            {
                if (Recovery.GameData.Instance.allBall[UsedCount].count == balluteCount)
                {
                    Debug.Log(Recovery.GameData.Instance.Number + " ==> " + balluteCount);

                    _ballut.sprite = Recovery.GameManager.instance.GetSprite(nowCount, Recovery.GameData.Instance.allBall[UsedCount].balluteType);
                    UsedCount++;
                }
                else
                {
                    _ballut.sprite = Recovery.GameManager.instance.GetSprite(nowCount);
                }
            }
            else
            {
                _ballut.sprite = Recovery.GameManager.instance.GetSprite(nowCount);
            }

        }
    }
    /// <summary>
    /// 移动
    /// </summary>
    public override void Move()
    {

        pos = transform.position;
        if (Recovery.GameData.Instance.isPlayGame)
        {
            if (!Recovery.GameData.Instance.isStopGame)
            {
                Zoom();
                base.Move();
            }
            else
                GetComponentInChildren<Animator>().speed = 0f;
        }

    }
    /// <summary>
    ///重新游戏
    /// </summary>
    public void GameAgain()
    {
        transform.position = new Vector3(0, -3.5f, 0);
        base.AgainGame();
        for (int i = 0; i < cloneBalute.Count; i++)
        {
            if (cloneBalute[i] != null)
                Destroy(cloneBalute[i]);
        }
        cloneBalute.Clear();
    }
}