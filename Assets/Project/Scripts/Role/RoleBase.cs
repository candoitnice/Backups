using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleBase : MonoBehaviour
{
    /// <summary>
    /// 排行
    /// </summary>
    public int Ranking=4;

    public Text RankingText;

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
    public float L_symmetry=50; 
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

    private GameObject car;

    private Animator anim;

    public virtual void Start()
    {
        car = transform.FindChild("GameObject").gameObject;
        anim=GetComponentInChildren<Animator>();
        anim.speed = 0.001f;
        L_symmetry = 50;
        RankingText = GetComponentInChildren<Text>();
        if (Recovery.GameData.Instance.gameModeType == Recovery.Type.GameModeType.many)
            GetComponentInChildren<Canvas>().gameObject.SetActive(true);
        else
            GetComponentInChildren<Canvas>().gameObject.SetActive(false);
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
    public virtual void InitData(Vector3 pos, string Name = "player", string ID = "0", bool isMainPlay = false,
        string no="111111111",string _name="ZS",string sex="30",string age="M")
    {
        transform.position = pos;
        name = Name;
        this.number = ID;
        this.isMainPlay = isMainPlay;
        if (number==Recovery.GameData.Instance.Number)
        {
            Recovery.GameData.Instance.mainPlayer = this as MainPlayer;
            this.isMainPlay = true;
        }
        this.no = no;
        this._name =_name;
        this.sex =sex;
        this.age=age;
    }
    /// <summary>
    /// 移动方法
    /// </summary>
    public virtual void Move()
    {
        Vector3 vecpos = transform.position + transform.forward * Time.deltaTime * moveSpeed * 0.05f;
        transform.position= vecpos;
        
            if (moveSpeed * 0.5f * 0.05f <= 1.2f)
            anim.speed = moveSpeed * 0.5f * 0.05f;
            else
            anim.speed = 1.2f;


        if (Recovery.GameData.Instance.gameModeType != Recovery.Type.GameModeType.many)
        {
            if ((L_symmetry - 50 != 0))
            {
                transform.Translate(transform.right * ((L_symmetry - 50) * 0.01f) * Time.deltaTime);
                SetPutACar(L_symmetry - 50);
            }
            if (transform.position.x > 1.6f)
            {
                transform.Translate(-transform.right * ((L_symmetry - 50) * 0.01f) * Time.deltaTime);
                SetPutACar(0);
            }
            if (transform.position.x < -1.6f)
            {
                transform.Translate(-transform.right * ((L_symmetry - 50) * 0.01f) * Time.deltaTime);
                SetPutACar(0);
            }
        }
        else
             RankingText.text = "第" + Ranking + "名";
    }

    /// <summary>
    /// 控制摆车
    /// </summary>
    public void SetPutACar(float Angle)
    {
        if (Angle < 15)
            car.transform.localEulerAngles = Vector3.Lerp(car.transform.localEulerAngles, new Vector3(0, 0, 90 - Angle), Time.deltaTime * 3);
        else
            car.transform.localEulerAngles = Vector3.Lerp(car.transform.localEulerAngles, new Vector3(0, 0, 90 - 15), Time.deltaTime * 3);
        if (Angle > -15)
            car.transform.localEulerAngles = Vector3.Lerp(car.transform.localEulerAngles, new Vector3(0, 0, 90 - Angle), Time.deltaTime * 3);
        else
            car.transform.localEulerAngles = Vector3.Lerp(car.transform.localEulerAngles, new Vector3(0, 0, 90 - -15), Time.deltaTime * 3);
    }


    /// <summary>
    ///定义一个位置发出射线得到碰撞数据
    /// </summary>
    public virtual RaycastHit Raycast(Vector3 trans, Vector3 direction)
    {
        Ray ray = new Ray(trans, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            return hit;
        }
        else
            return hit;
    }
    /// <summary>
    /// 得到两个位置射线碰到地面的方向
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="trans2"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public virtual Vector3 Direction(Vector3 trans, Vector3 trans2, Vector3 direction)
    {
        Vector3 vec = Raycast(trans, direction).point;
        Vector3 vec2 = Raycast(trans2, direction).point;
        transform.position = Vector3.Slerp(transform.position, vec, Time.deltaTime*10f);
        return vec - vec2;
    }
    //int count;
    //public void SetPos()
    //{
    //    count++;
    //    if (count >= 20)
    //    {
    //        //if (GameData.Instance.mainPlayer.frequency <= 0)
    //        //    transform.position = new Vector3(pos.x, pos.y, distance * 6 - (252 * GameData.Instance.mainPlayer.frequency));
    //        //else
    //        //{
    //        //    transform.position = pos-new Vector3(0,0,252* GameData.Instance.mainPlayer.frequency);
    //        //}
    //        count = 0;
    //    }
    //}
}
