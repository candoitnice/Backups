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
    public override void InitData(Vector3 pos, string Name = "player", string ID = "0", bool isMainPlay = false, string no = "111111111", string _name = "ZS", string sex = "30", string age = "M")
    {
        base.InitData(pos, Name, ID, isMainPlay, no, _name, sex, age);
    }

    private Vector3 vecpos;

    internal Transform target;

    public int frequency;

    /// <summary>
    /// 移动
    /// </summary>
    public override void Move()
    {
        pos=transform.position+new Vector3(0,0,252*frequency);
        if (Recovery.GameData.Instance.isPlayGame)
        {
            if (!Recovery.GameData.Instance.isStopGame)
            {
                Raycast();
                base.Move();
            }
            else
                anim.speed = 0f; if (isMainPlay)
            {
                if (transform.position.z > 252)
                {
                    frequency++;
                    for (int i = 0; i < Recovery.GameData.Instance.Dic_RoleCount.Count; i++)
                    {
                        Vector3 v3 = Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.transform.position;
                        Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.transform.position = new Vector3(v3.x, v3.y, v3.z - 252);
                        Camera.main.GetComponent<CameraFlow>().SetPos();
                    }
                    for (int i = 0; i < Recovery.GameData.Instance.allSlope.Count; i++)
                    {
                        Vector3 v3 = Recovery.GameData.Instance.allSlope[i].transform.position;
                        Recovery.GameData.Instance.allSlope[i].transform.position = new Vector3(v3.x, v3.y, v3.z - 252);
                    }
                }
            }
            transform.forward = Direction(new Vector3(transform.position.x, transform.position.y + 1 * 5f, transform.position.z),
                new Vector3(transform.position.x, transform.position.y + 1 * 5f, transform.position.z - 1 * 5f), -Vector3.up);

            vecpos = transform.position;
        }
        else
        {
            motorSpeed = 0;
        }

       
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
    public void Raycast()
    {
        Ray ray = new Ray(transform.position+new Vector3(0,2,0), -transform.up);
        RaycastHit hit;
        int i = 0;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            switch (hit.collider.tag)
            {
                case "slope":
                    i = 0;
                    break;
                case "slope15":
                    i = 1;
                    break;
                case "_slope15":
                    i = 2;
                    break;
                case "slope30":
                    i = 3;
                    break;
                case "_slope30":
                    i = 4;
                    break;
                case "slope45":
                    i = 5;
                    break;
                case "_slope45":
                    i = 6;
                    break;
            }
            if (i!= Recovery.GameData.Instance.slopeCount)
            {
                Recovery.GameData.Instance.slopeCount = i;
                print(Recovery.GameData.Instance.slopeCount);

                for (int j = 0; j< 5; j++)
                {
                    MySocket.Instance.listSendValue.Add("CTL:" + Recovery.GameData.Instance.Number +":"+ Recovery.GameData.Instance.slopeCount+";");
                }
            }
        }
    }
    /// <summary>
    ///重新游戏
    /// </summary>
    public void GameAgain()
    {
        transform.position = Vector3.zero;
        Camera.main.GetComponent<CameraFlow>().SetPos();
        Camera.main.GetComponent<CameraFlow>().GameAgain ();
        distance = 0;
        Recovery.GameData.Instance.distance = 0;
    }
}