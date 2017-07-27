using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleScript_______ : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
#region RoleBase
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleBase : MonoBehaviour
{

    public bool isConnect;
    /// <summary>
    /// 排行
    /// </summary>
    public int Ranking = 4;

    public Text RankingText;

    public Transform ballut;
    public SpriteRenderer _ballut;
    //气管
    public Transform tube;
    public float balloonCapacity;
    public float MaxBalloonCapacity;
    public Canvas canvas;

    /// <summary>
    /// 是否主要玩家
    /// </summary>
    public bool isMainPlay;
    /// <summary>
    /// 玩家ID
    /// </summary>
    public string number;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float moveSpeed
    {
        get { return motorSpeed; }
    }

    /// <summary>
    /// 运动距离
    /// </summary>
    public float distance;
    /// <summary>
    /// 时速
    /// </summary>
    public float speedmax;
    /// <summary>
    ///电机转速
    /// </summary>
    public float motorSpeed;
    /// <summary>
    ///坡度(0~6)
    /// </summary>
    public float slope;
    /// <summary>
    ///心率
    /// </summary>
    public float heartRate;
    /// <summary>
    ///对称性(左%, 右%)
    /// </summary>
    public float L_symmetry = 50;
    /// <summary>
    /// 病历号
    /// </summary>
    public string no;
    /// <summary>
    /// 姓名
    /// </summary>
    public string _name;
    /// <summary>
    /// 年龄
    /// </summary>
    public string age;
    /// <summary>
    /// 性别
    /// </summary>
    public string sex;

    public Vector3 pos;

    //private GameObject car;

    public Animator anim;

    public int nowCount;

    public bool isStopAnim = false;



    public static RoleBase instance;
    void Awake()
    {
        instance = this;
        canvas = gameObject.GetComponentInChildren<Canvas>();
        canvas.enabled = false;
        anim = GetComponentInChildren<Animator>();
        anim.speed = 0.001f;
    }



    public virtual void Start()
    {
        L_symmetry = 50;
        Transform[] sr = GetComponentsInChildren<Transform>();
        for (int i = 0; i < sr.Length; i++)
        {
            if (sr[i].name == "balloon")
                ballut = sr[i];
            if (sr[i].name == "_balloon")
                _ballut = sr[i].GetComponent<SpriteRenderer>();
            if (sr[i].name == "tube")
                tube = sr[i];
        }
        MaxBalloonCapacity = (0.08f * 2 * 3.1415f) * 5;
        balloonCapacity = MaxBalloonCapacity;

    }
    public virtual void Update()
    {
    }

    /// <summary>
    ///初始化玩家数据
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="Name"></param>
    /// <param name="ID"></param>
    /// <param name="isMainPlay"></param>
    public virtual void InitData(bool isConnect = true, Vector3 pos = default(Vector3), string Name = "player", string ID = "0", bool isMainPlay = false,
        string no = "111111111", string _name = "ZS", string sex = "30", string age = "M")
    {
        this.isConnect = isConnect;
        transform.position = pos;
        name = Name;
        this.number = ID;
        this.isMainPlay = isMainPlay;
        if (number == Recovery.GameData.Instance.Number)
        {
            Recovery.GameData.Instance.mainPlayer = this as MainPlayer;
            this.isMainPlay = true;
        }
        this.no = no;
        this._name = _name;
        this.sex = sex;
        this.age = age;
    }


    public virtual void SetInfo(float speed, float slope, float heartRate, float L_symmetry, float balloonCapacity)
    {
        motorSpeed = speed;
        this.slope = slope;
        this.heartRate = heartRate;
        this.L_symmetry = L_symmetry;
    }

    public int balluteCount;

    public float c;
    public float b;
    //GameObject cloneBalute;

    public int UsedCount;
    public virtual void Zoom()
    {
        c = balloonCapacity / MaxBalloonCapacity;
        ballut.localScale = Vector3.Lerp(ballut.localScale, new Vector3(c, c, c), Time.deltaTime * 3);

    }
    /// <summary>
    /// 移动方法
    /// </summary>
    public virtual void Move()
    {
        if (isStopAnim)
            return;
        if (isConnect)
        {
            if (moveSpeed * 0.5f * 0.05f <= 1.2f)
                anim.speed = moveSpeed * 0.5f * 0.05f;
            else
                anim.speed = 1.2f;
            if (Recovery.GameData.Instance.gameModeType != Recovery.Type.GameModeType.many)
            {
                SetPutACar(L_symmetry - 50);
            }
        }
    }

    /// <summary>
    /// 控制摆车
    /// </summary>
    public void SetPutACar(float Angle)
    {
        if (Angle < 15)
            tube.transform.localEulerAngles = Vector3.Lerp(tube.transform.localEulerAngles, new Vector3(0, 0, 90 - Angle), Time.deltaTime * 3);
        else
            tube.transform.localEulerAngles = Vector3.Lerp(tube.transform.localEulerAngles, new Vector3(0, 0, 90 - 15), Time.deltaTime * 3);
        if (Angle > -15)
            tube.transform.localEulerAngles = Vector3.Lerp(tube.transform.localEulerAngles, new Vector3(0, 0, 90 - Angle), Time.deltaTime * 3);
        else
            tube.transform.localEulerAngles = Vector3.Lerp(tube.transform.localEulerAngles, new Vector3(0, 0, 90 - -15), Time.deltaTime * 3);
    }



    public void Off_line()
    {
        //RankingText.text = "已离线";
        //RankingText.color = Color.red;
        Ranking = 4;
        isConnect = false;
        anim.speed = 0.001f;
        motorSpeed = 0;
        distance = 0;

        Recovery.GameData.Instance.offDic_RoleCount.Add(this);
        //if (Recovery.GameData.Instance.Dic_RoleCount.ContainsKey(number))
        //    Recovery.GameData.Instance.Dic_RoleCount.Remove(number);
        //number = null;
    }



    public virtual void AgainGame()
    {
        distance = 0;
        balluteCount = 0;
        ballut.localScale = Vector3.zero;
        balloonCapacity = 0;
        Recovery.GameData.Instance.distance = 0;
    }
}
*/
#endregion

#region MainPlayer
/*
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


public List< GameObject> cloneBalute=new List<GameObject>();
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
                Debug.Log(Recovery.GameData.Instance.Number+" ==> "+ balluteCount);

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
*/
#endregion

#region _Player
    /*
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
    float _balloonCapacity;
    public List <GameObject> cloneBalute=new List<GameObject>();
    public override void Zoom()
    {
        base.Zoom();
        if (balluteCount > _balloonCount)//balloonCapacity<_balloonCapacity
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
            if (UsedCount<Recovery.GameData.Instance.allBall.Count)
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
        //_balloonCapacity = balloonCapacity;
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
    */
#endregion