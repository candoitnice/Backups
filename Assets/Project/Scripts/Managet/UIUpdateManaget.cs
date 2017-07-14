using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIUpdateManaget : MonoBehaviour
{
    public static UIUpdateManaget Instance;
    void Awake() { Instance = this; }
    

    public Button playGame;
    public TimeDown timeDome;
    public TimeDown gameOver;
    public GameObject Single, Many, Back;
    public TimeDown connectionFailed;
    public Image WaitingConnection;
    public Image ConnectionException;

    [Header("single")]
    public Image s_CountDown_Image;
    public Text s_CountDown_Text;
    public Text s_distance;
    public Text s_speed;
    public Text s_timespeed;
    public Text s_heartrate;
    [Header("many")]
    public Image m_CountDown_Image;
    public Text m_CountDown_Text;
    public List<Text> m_number=new List<Text>();
    public List<Text> m_distance = new List<Text>();
    public List<Text> m_speed = new List<Text>();
    public List<Text> m_timespeed = new List<Text>();
    public List<Text> m_heartrate = new List<Text>();
    public List<RectTransform>Ranking=new List<RectTransform>();
    public RectTransform rankingList;
    public List<Text> rl_number=new List<Text>();
    public List<Text> rl_name =new List<Text>();

    // Use this for initialization
    void Start ()
    {
    }

    public void InitUI()
    {
        Transform Back = MT.SceneObject.Find<Transform>(transform, "Back");
        this.Back = Back.gameObject;
        playGame = MT.SceneObject.Find<Button>(Back, "GamePlay");
        timeDome = MT.SceneObject.Find<TimeDown>(transform, "TimeDown");
        gameOver = MT.SceneObject.Find<TimeDown>(transform, "GameOver");
        connectionFailed= MT.SceneObject.Find<TimeDown>(transform, "连接失败");
        ConnectionException = MT.SceneObject.Find<Image>(transform, "训练机连接异常");


        WaitingConnection = MT.SceneObject.Find<Image>(Back, "WaitingConnection");

        //------------------------------------------------------single
        Transform S_single = MT.SceneObject.Find<Transform>(transform,"single");
        Single = S_single.gameObject;

        Transform S_back = MT.SceneObject.Find<Transform>(S_single, "back");

        Transform S_Info = MT.SceneObject.Find<Transform>(S_back, "Info");
        s_distance = MT.SceneObject.Find<Text>(S_Info, "距离");
        s_speed = MT.SceneObject.Find<Text>(S_Info, "速度");
        s_timespeed = MT.SceneObject.Find<Text>(S_Info, "时速");
        s_heartrate = MT.SceneObject.Find<Text>(S_Info, "心率");
        Transform S_time = MT.SceneObject.Find<Transform>(S_back, "time");
        Transform S_ProgressBar = MT.SceneObject.Find<Transform>(S_time, "ProgressBar");
        s_CountDown_Image = MT.SceneObject.Find<Image>(S_ProgressBar, "进度条");
        s_CountDown_Text = MT.SceneObject.Find<Text >(S_ProgressBar, "倒计时");

        //------------------------------------------------------many
        Transform M_many = MT.SceneObject.Find<Transform>(transform, "many");
        Many = M_many.gameObject;
        Transform M_back = MT.SceneObject.Find<Transform>(M_many, "back");
        Transform M_Info = MT.SceneObject.Find<Transform>(M_back, "Info");
        Transform M_1 = MT.SceneObject.Find<Transform>(M_Info, "1");
        Transform M_2 = MT.SceneObject.Find<Transform>(M_Info, "2");
        Transform M_3 = MT.SceneObject.Find<Transform>(M_Info, "3");
        Transform M_4 = MT.SceneObject.Find<Transform>(M_Info, "4");
        Transform M_5 = MT.SceneObject.Find<Transform>(M_Info, "5");



        foreach (var item in MT.SceneObject.Finds<Text>(M_1, "编号"))
        {
            m_number.Add(item);
        }
        foreach (var item in MT.SceneObject.Finds<Text>(M_2, "距离"))
        {
            m_distance.Add(item);
        }
        foreach (var item in MT.SceneObject.Finds<Text>(M_3, "速度"))
        {
            m_speed.Add(item);
        }
        foreach (var item in MT.SceneObject.Finds<Text>(M_4, "时速"))
        {
            m_timespeed.Add(item);
        }
        foreach (var item in MT.SceneObject.Finds<Text>(M_5, "心率"))
        {
            m_heartrate.Add(item);
        }

        Transform M_time = MT.SceneObject.Find<Transform>(M_back, "time");
        Transform M_ProgressBar = MT.SceneObject.Find<Transform>(M_time, "ProgressBar");
        m_CountDown_Image = MT.SceneObject.Find<Image>(M_ProgressBar, "进度条");
        m_CountDown_Text = MT.SceneObject.Find<Text >(M_ProgressBar, "倒计时");

        Transform M_Map =MT.SceneObject.Find<Transform>(M_many, "Map");
        foreach (var item in MT.SceneObject.Finds<Image>(M_Map))
        {
            Ranking.Add(item.rectTransform);
        }
        rankingList = MT.SceneObject.Find<RectTransform>(M_many,"排行榜");
        Transform M_R = MT.SceneObject.Find<Transform>(M_many, "排行榜");
        Transform M_number = MT.SceneObject.Find<Transform>(M_R, "number");
        Transform M_Name = MT.SceneObject.Find<Transform>(M_R, "Name");
        foreach (var item in MT.SceneObject.Finds<Text>(M_number, "编号"))
        {
            rl_number.Add(item);
        }
        foreach (var item in MT.SceneObject.Finds<Text>(M_Name, "名字"))
        {
            rl_name.Add(item);
        }


        playGame.gameObject.SetActive(false);
        timeDome.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
        connectionFailed.gameObject.SetActive(false);
        rankingList.gameObject.SetActive(false);
        ConnectionException.gameObject.SetActive(false);
    }

    
    /// <summary>
    ///进度条
    /// </summary>
    /// <param name="value"></param>
    public void Fill(float value)
    {
        s_CountDown_Image.fillAmount = value;
        m_CountDown_Image.fillAmount = value;
        float time = (Recovery.GameData.Instance.gameTime * 60 - Recovery.GameData.Instance.currentGameTime);
        s_CountDown_Text.text = (int)(time/60)+ ":"+(int)(time%60)+"/m";
        m_CountDown_Text.text = (int)(time / 60) + ":" + (int)(time % 60) + "/m";
    }
    /// <summary>
    ///初始化单机版UI数据
    /// </summary>
    public void S_Init_InfoData()
    {
        s_distance.text = "距离:" + Recovery.GameData.Instance.mainPlayer.distance.ToString("N2") + "m";
        s_speed.text = "速度:" + Recovery.GameData.Instance.mainPlayer.motorSpeed + "c/min";
        s_timespeed.text = "时速:" + Recovery.GameData.Instance.mainPlayer.speedmax.ToString("N2") + "km/m";
        s_heartrate.text = "心率:" + Recovery.GameData.Instance.mainPlayer.heartRate + "bpm";
    }

    /// <summary>
    ///初始化多人版UI数据
    /// </summary>
    public void M__Init_InfoData()
    {
        for (int i = 0; i < Recovery.GameData.Instance.Dic_RoleCount.Count; i++)
        {
            switch (Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Key)
            {
                case "0001":
                    M__Init_InfoData(m_number[0],"0001");
                    //M__Init_InfoData(m_no[0], "病历号:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.no);
                    M__Init_InfoData(m_distance[0], "距离:" +Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.distance.ToString("N2") + "m");
                    M__Init_InfoData(m_speed[0], "速度:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.motorSpeed + "c/min");
                    M__Init_InfoData(m_timespeed[0],"时速:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.speedmax.ToString("N2") + "km/m");
                    M__Init_InfoData(m_heartrate[0], "心率:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.heartRate + "bpm");
                    break;
                case "0002":
                    M__Init_InfoData(m_number[1], "0002");
                    //M__Init_InfoData(m_no[1], "病历号:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.no);
                    M__Init_InfoData(m_distance[1], "距离:" +Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.distance.ToString("N2") + "m");
                    M__Init_InfoData(m_speed[1], "速度:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.motorSpeed + "c/min");
                    M__Init_InfoData(m_timespeed[1], "时速:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.speedmax.ToString("N2") + "km/m");
                    M__Init_InfoData(m_heartrate[1], "心率:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.heartRate + "bpm");
                    break;
                case "0003":
                    M__Init_InfoData(m_number[2], "0003");
                    //M__Init_InfoData(m_no[2], "病历号:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.no);
                    M__Init_InfoData(m_distance[2], "距离:" +Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.distance.ToString("N2") + "m");
                    M__Init_InfoData(m_speed[2], "速度:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.motorSpeed + "c/min");
                    M__Init_InfoData(m_timespeed[2], "时速:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.speedmax.ToString("N2") + "km/m");
                    M__Init_InfoData(m_heartrate[2], "心率:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.heartRate + "bpm");
                    break;
                case "0004":
                    M__Init_InfoData(m_number[3], "0004");
                    //M__Init_InfoData(m_no[3], "病历号:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.no);
                    M__Init_InfoData(m_distance[3], "距离:" +Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.distance.ToString("N2") + "m");
                    M__Init_InfoData(m_speed[3], "速度:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.motorSpeed + "c/min");
                    M__Init_InfoData(m_timespeed[3], "时速:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.speedmax.ToString("N2") + "km/m");
                    M__Init_InfoData(m_heartrate[3], "心率:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.heartRate + "bpm");
                    break;
            }
        }
        SetRanking();
    }
    public void M__Init_InfoData(Text _text,string value)
    {
        _text.text=value;
    }

    /// <summary>
    ///更新主要颜色
    /// </summary>
    public void M_Init_Color()
    {
        switch (Recovery.GameData.Instance.Number)
        {
            case "0001":
                //m_no[0]
                m_number[0].color= Color.green;
                m_distance[0].color = Color.green;
                m_speed[0].color = Color.green;
                m_timespeed[0].color = Color.green;
                m_heartrate[0].color = Color.green;
                break;
            case "0002":
                m_number[1].color = Color.green;
                m_distance[1].color = Color.green;
                m_speed[1].color = Color.green;
                m_timespeed[1].color = Color.green;
                m_heartrate[1].color = Color.green;
                break;
            case "0003":
                m_number[2].color = Color.green;
                m_distance[2].color = Color.green;
                m_speed[2].color = Color.green;
                m_timespeed[2].color = Color.green;
                m_heartrate[2].color = Color.green;
                break;
            case "0004":
                m_number[3].color = Color.green;
                m_distance[3].color = Color.green;
                m_speed[3].color = Color.green;
                m_timespeed[3].color = Color.green;
                m_heartrate[3].color = Color.green;
                break;
        }
    }

    private float[] flo_Y = new float[4] { 89.6f, 29f, -31.6f, -92.2f};
    /// <summary>
    /// 更新小地图
    /// </summary>
    public void SetRanking()
    {
        List<object> play = new List<object>();
        foreach (var dic_1 in Recovery.GameData.Instance.Dic_RoleCount)
        {
            int count = 0;
            foreach (var dic_2 in Recovery.GameData.Instance.Dic_RoleCount)
            {
                if (dic_1.Value.transform.position.z>=dic_2.Value.transform.position.z)
                    continue;
                count++;
            }
            dic_1.Value.Ranking = count + 1;

            switch (dic_1.Key)
            {
                case "0001":
                    Ranking[0].localPosition = new Vector3(Ranking[0].localPosition.x, flo_Y[dic_1.Value.Ranking - 1]);
                    break;
                case "0002":
                    Ranking[1].localPosition = new Vector3(Ranking[1].localPosition.x, flo_Y[dic_1.Value.Ranking - 1]);
                    break;
                case "0003":
                    Ranking[2].localPosition = new Vector3(Ranking[2].localPosition.x, flo_Y[dic_1.Value.Ranking - 1]);
                    break;
                case "0004":
                    Ranking[3].localPosition = new Vector3(Ranking[3].localPosition.x, flo_Y[dic_1.Value.Ranking - 1]);
                    break;
            }
        }
    }

    /// <summary>
    ///更新排行榜
    /// </summary>
    public void SetRankingList()
    {
        rankingList.gameObject.SetActive(true);
        foreach (var dic_1 in Recovery.GameData.Instance.Dic_RoleCount)
        {
            switch (dic_1.Value.Ranking)
            {
                case 1:
                    rl_number[0].text = dic_1.Value.number;
                    rl_name[0].text = dic_1.Value.distance.ToString("N2");
                    break;
                case 2:
                    rl_number[1].text = dic_1.Value.number;
                    rl_name[1].text = dic_1.Value.distance.ToString("N2");
                    break;
                case 3:
                    rl_number[2].text = dic_1.Value.number;
                    rl_name[2].text = dic_1.Value.distance.ToString("N2");
                    break;
                case 4:
                    rl_number[3].text = dic_1.Value.number;
                    rl_name[3].text = dic_1.Value.distance.ToString("N2");
                    break;
            }
        }
            //rl_number
            //    rl_name
        }

    // Update is called once per frame
    void Update () {
		
	}
}
