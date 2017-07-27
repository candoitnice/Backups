using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 次要玩家
/// </summary>
public class _Player : RoleBase
{
    public override void Start()
    {
        base.Start();
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
        this.balloonCapacity = balloonCapacity;
    }

    float _balloonCount;
    float _balloonCapacity = 5;
    public List<GameObject> cloneBalute = new List<GameObject>();
    public override void Zoom()
    {
        base.Zoom();
        if (balluteCount > _balloonCount && balloonCapacity < _balloonCapacity)//balloonCapacity<_balloonCapacity
        {
            GameObject cb = Recovery.GameManager.instance.EstablishProp("Prefabs/Ballute/" + _ballut.sprite.name);
            GameObject g = Recovery.GameManager.instance.EstablishProp(Source.Effect.blast, ballut.transform.position);
            Destroy(g, 3);
            cb.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            cb.transform.position = _ballut.transform.position;
            cb.transform.eulerAngles = _ballut.transform.eulerAngles;
            ballut.localScale = Vector3.zero;
            c = 0;
            balloonCapacity = 0f;
            cb.AddComponent<BalluteMove>();
            cloneBalute.Add(cb);

            if (nowCount >= BalluteSpriteArray.Instance.greenSprite.Length)//--
                nowCount = 0;
            if (UsedCount < Recovery.GameData.Instance.allBall.Count)
            {
                if (Recovery.GameData.Instance.allBall[UsedCount].count == balluteCount)
                {
                    Debug.Log(Recovery.GameData.Instance.Number + " + _Player ==> " + balluteCount);
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

        _balloonCount = balluteCount;
        _balloonCapacity = balloonCapacity;
    }

    /// <summary>
    /// 移动
    /// </summary>
    public override void Move()
    {
        if (Recovery.GameData.Instance.isPlayGame)
        {
            Zoom();
            base.Move();
        }
    }

    public override void AgainGame()
    {
        base.AgainGame();
        for (int i = 0; i < cloneBalute.Count; i++)
        {
            if (cloneBalute[i] != null)
                Destroy(cloneBalute[i]);
        }
        cloneBalute.Clear();
    }
}
