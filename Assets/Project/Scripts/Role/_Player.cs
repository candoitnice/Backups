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
    public override void InitData(Vector3 pos, string Name = "player", string ID = "0", bool isMainPlay = false, string no = "111111111", string _name = "ZS", string sex = "30", string age = "M")
    {
        base.InitData(pos, Name, ID, isMainPlay, no, _name, sex, age);
    }
    /// <summary>
    /// 移动
    /// </summary>
    public override void Move()
    {
        if (Recovery.GameData.Instance.isPlayGame)
        {
            if (Mathf.Abs(distance-Recovery.GameData.Instance.mainPlayer.distance)*6<350)
                base.Move();
        }
        transform.forward = Direction(new Vector3(transform.position.x, transform.position.y + 1 * 5f, transform.position.z),
            new Vector3(transform.position.x, transform.position.y + 1 * 5f, transform.position.z - 1 * 5f), -Vector3.up);
    }
    /// <summary>
    /// 方向
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="trans2"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public override Vector3 Direction(Vector3 trans, Vector3 trans2, Vector3 direction)
    {
        return base.Direction(trans, trans2, direction);
    }

 
}
