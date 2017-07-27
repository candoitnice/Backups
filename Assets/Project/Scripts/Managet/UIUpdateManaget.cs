using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class UIUpdateManaget : MonoBehaviour
{
    public static UIUpdateManaget Instance;
    void Awake() { Instance = this; }


    public Button playGame;
    public TimeDown timeDome;
    public Image gameOver;
    public GameObject Single, Many, Back;
    public Image connectionFailed;
    public Image WaitingConnection;
    public Image ConnectionException;
    public TimeDown GameExecutingNotice;
    public TimeDown Configuration;

    [Header("single")]
    public Image s_CountDown_Image;
    public Text s_Pattern;
    public Text s_CountDown_Text;
    public Text s_name;
    public Text s_distance;
    public Text s_speed;
    public Text s_timespeed;
    public Text s_heartrate;
    [Header("many")]
    public Image m_CountDown_Image;
    public Text m_CountDown_Text;
    public List<Text> m_One = new List<Text>();
    public List<Text> m_Two = new List<Text>();
    public List<Text> m_Three = new List<Text>();
    public List<Text> m_Four = new List<Text>();
    public Text M_1;
    public Text M_2;
    public Text M_3;
    public Text M_4;
    public List<RectTransform> Ranking = new List<RectTransform>();
    public Image rankingList;
    public List<Text> rl_number = new List<Text>();
    public List<Text> rl_name = new List<Text>();

    int sortId;

    // Use this for initialization
    void Start()
    {
    }

    public void InitUI()
    {
        Transform Back = MT.SceneObject.Find<Transform>(transform, "Back");
        this.Back = Back.gameObject;
        playGame = MT.SceneObject.Find<Button>(Back, "GamePlay");
        timeDome = MT.SceneObject.Find<TimeDown>(transform, "TimeDown");
        gameOver = MT.SceneObject.Find<Image>(transform, "GameOver");
        connectionFailed = MT.SceneObject.Find<Image>(transform, "连接失败");
        ConnectionException = MT.SceneObject.Find<Image>(transform, "训练机连接异常");
        GameExecutingNotice = MT.SceneObject.Find<TimeDown>(transform, "GameExecutingNotice");
        Configuration = MT.SceneObject.Find<TimeDown>(transform, "Configuration");

        WaitingConnection = MT.SceneObject.Find<Image>(Back, "WaitingConnection");

        //------------------------------------------------------single
        Transform S_single = MT.SceneObject.Find<Transform>(transform, "single");
        Single = S_single.gameObject;

        s_Pattern = MT.SceneObject.Find<Text>(S_single, "模式");

        Transform S_back = MT.SceneObject.Find<Transform>(S_single, "back");

        Transform S_Info = MT.SceneObject.Find<Transform>(S_back, "Info");
        s_name = MT.SceneObject.Find<Text>(S_Info, "名字");
        s_distance = MT.SceneObject.Find<Text>(S_Info, "距离");
        s_speed = MT.SceneObject.Find<Text>(S_Info, "速度");
        s_timespeed = MT.SceneObject.Find<Text>(S_Info, "时速");
        s_heartrate = MT.SceneObject.Find<Text>(S_Info, "心率");
        Transform S_time = MT.SceneObject.Find<Transform>(S_back, "time");
        Transform S_ProgressBar = MT.SceneObject.Find<Transform>(S_time, "ProgressBar");
        s_CountDown_Image = MT.SceneObject.Find<Image>(S_ProgressBar, "进度条");
        s_CountDown_Text = MT.SceneObject.Find<Text>(S_ProgressBar, "倒计时");

        //------------------------------------------------------many
        Transform M_many = MT.SceneObject.Find<Transform>(transform, "many");
        Many = M_many.gameObject;
        Transform M_back = MT.SceneObject.Find<Transform>(M_many, "back");
        Transform M_Info = MT.SceneObject.Find<Transform>(M_back, "Info");
        M_1 = MT.SceneObject.Find<Text>(M_Info, "1");
        M_2 = MT.SceneObject.Find<Text>(M_Info, "2");
        M_3 = MT.SceneObject.Find<Text>(M_Info, "3");
        M_4 = MT.SceneObject.Find<Text>(M_Info, "4");





        foreach (var item in MT.SceneObject.Finds<Text>(M_1.transform))
        {
            m_One.Add(item);
        }
        foreach (var item in MT.SceneObject.Finds<Text>(M_2.transform))
        {
            m_Two.Add(item);
        }
        foreach (var item in MT.SceneObject.Finds<Text>(M_3.transform))
        {
            m_Three.Add(item);
        }
        foreach (var item in MT.SceneObject.Finds<Text>(M_4.transform))
        {
            m_Four.Add(item);
        }

        Transform M_time = MT.SceneObject.Find<Transform>(M_back, "time");
        Transform M_ProgressBar = MT.SceneObject.Find<Transform>(M_time, "ProgressBar");
        m_CountDown_Image = MT.SceneObject.Find<Image>(M_ProgressBar, "进度条");
        m_CountDown_Text = MT.SceneObject.Find<Text>(M_ProgressBar, "倒计时");

        Transform M_Map = MT.SceneObject.Find<Transform>(M_many, "Map");
        foreach (var item in MT.SceneObject.Finds<Image>(M_Map))
        {
            Ranking.Add(item.rectTransform);
        }
        rankingList = MT.SceneObject.Find<Image>(M_many, "排行榜");
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
        GameExecutingNotice.gameObject.SetActive(false);
        Configuration.gameObject.SetActive(false);
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
        s_CountDown_Text.text = GetTime((int)(time / 60)) + ":" + GetTime((int)(time % 60));
        m_CountDown_Text.text = GetTime((int)(time / 60)) + ":" + GetTime((int)(time % 60));
    }
    private string GetTime(int count)
    {
        if (count >= 10)
            return count.ToString();
        else
            return "0" + count;
    }
    /// <summary>
    ///初始化单机版UI数据
    /// </summary>
    public void S_Init_InfoData()
    {
        if (Recovery.Main.Instance.GEType == Recovery.Type.GameEditionType.Adult)
        {
            if (Recovery.GameData.Instance.gameModeType == Recovery.Type.GameModeType.single)
                s_Pattern.text = "主动";
            if (Recovery.GameData.Instance.gameModeType == Recovery.Type.GameModeType.automatic)
                s_Pattern.text = "被动";
        }
        s_name.text = "姓名:" + Recovery.GameData.Instance._name;
        s_distance.text = "总数：" + Recovery.GameData.Instance.mainPlayer.balluteCount/*distance.ToString("N2")*/ + "个";
        s_speed.text = "速度：" + Recovery.GameData.Instance.mainPlayer.motorSpeed + "c/min";
        s_timespeed.text = "时速：" + Recovery.GameData.Instance.mainPlayer.speedmax.ToString("N2") + "km/h";
        s_heartrate.text = "心率：" + Recovery.GameData.Instance.mainPlayer.heartRate + "bpm";
        
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
                    M_1.text = "";
                    M__Init_InfoData(m_One[0], "0001");
                    //M__Init_InfoData(m_no[0], "病历号:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.no);
                    M__Init_InfoData(m_One[1], "总数:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.balluteCount + "个");
                    M__Init_InfoData(m_One[2], "速度:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.motorSpeed + "c/min");
                    M__Init_InfoData(m_One[3], "时速:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.speedmax.ToString("N2") + "km/h");
                    M__Init_InfoData(m_One[4], "心率:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.heartRate + "bpm");
                    break;
                case "0002":
                    M_2.text = "";
                    M__Init_InfoData(m_Two[0], "0002");
                    //M__Init_InfoData(m_no[1], "病历号:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.no);
                    M__Init_InfoData(m_Two[1], "总数:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.balluteCount + "个");
                    M__Init_InfoData(m_Two[2], "速度:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.motorSpeed + "c/min");
                    M__Init_InfoData(m_Two[3], "时速:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.speedmax.ToString("N2") + "km/h");
                    M__Init_InfoData(m_Two[4], "心率:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.heartRate + "bpm");
                    break;
                case "0003":
                    M_3.text = "";
                    M__Init_InfoData(m_Three[0], "0003");
                    //M__Init_InfoData(m_no[2], "病历号:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.no);
                    M__Init_InfoData(m_Three[1], "总数:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.balluteCount + "个");
                    M__Init_InfoData(m_Three[2], "速度:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.motorSpeed + "c/min");
                    M__Init_InfoData(m_Three[3], "时速:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.speedmax.ToString("N2") + "km/h");
                    M__Init_InfoData(m_Three[4], "心率:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.heartRate + "bpm");
                    break;
                case "0004":
                    M_4.text = "";
                    M__Init_InfoData(m_Four[0], "0004");
                    //M__Init_InfoData(m_no[3], "病历号:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.no);
                    M__Init_InfoData(m_Four[1], "总数:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.balluteCount + "个");
                    M__Init_InfoData(m_Four[2], "速度:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.motorSpeed + "c/min");
                    M__Init_InfoData(m_Four[3], "时速:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.speedmax.ToString("N2") + "km/h");
                    M__Init_InfoData(m_Four[4], "心率:" + Recovery.GameData.Instance.Dic_RoleCount.ElementAt(i).Value.heartRate + "bpm");
                    break;
            }
        }
        SetRanking();
    }
    public void M__Init_InfoData(Text _text, string value)
    {
        _text.text = value;
    }

    /// <summary>
    ///更新主要颜色
    /// </summary>
    public void M_Init_Color()
    {
        switch (Recovery.GameData.Instance.Number)
        {
            case "0001":
                foreach (var item in m_One)
                {
                    item.color = Color.green;
                }
                break;
            case "0002":
                foreach (var item in m_Two)
                {
                    item.color = Color.green;
                }
                break;
            case "0003":
                foreach (var item in m_Three)
                {
                    item.color = Color.green;
                }
                break;
            case "0004":
                foreach (var item in m_Four)
                {
                    item.color = Color.green;
                }
                break;
        }
    }


    public void Disconnect(string number)
    {
        print(number);
        switch (number)
        {
            case "0001":
                M_1.text = "玩家退出";
                foreach (var item in m_One)
                {
                    item.text = "";
                }
                Ranking[0].localPosition = new Vector3(Ranking[0].localPosition.x, flo_Y[3]);
                Recovery.GameData.Instance.Dic_RoleCount[number].canvas.enabled = true;
                Recovery.GameData.Instance.Dic_RoleCount[number].anim.speed = 0.001f;
                Recovery.GameData.Instance.Dic_RoleCount[number].isStopAnim = true;
                break;
            case "0002":
                M_2.text = "玩家退出";
                foreach (var item in m_Two)
                {
                    item.text = "";
                }
                Ranking[1].localPosition = new Vector3(Ranking[1].localPosition.x, flo_Y[3]);
                Debug.Log(number);
                Recovery.GameData.Instance.Dic_RoleCount[number].canvas.enabled = true;
                Recovery.GameData.Instance.Dic_RoleCount[number].anim.speed = 0.001f;
                Recovery.GameData.Instance.Dic_RoleCount[number].isStopAnim = true;
                
                break;
            case "0003":
                M_3.text = "玩家退出";
                foreach (var item in m_Three)
                {
                    item.text = "";
                }
                Ranking[2].localPosition = new Vector3(Ranking[2].localPosition.x, flo_Y[3]);
                Recovery.GameData.Instance.Dic_RoleCount[number].canvas.enabled = true;
                Recovery.GameData.Instance.Dic_RoleCount[number].anim.speed = 0.001f;
                Recovery.GameData.Instance.Dic_RoleCount[number].isStopAnim = true;
                break;
            case "0004":
                M_4.text = "玩家退出";
                foreach (var item in m_Four)
                {
                    item.text = "";
                }
                Ranking[3].localPosition = new Vector3(Ranking[3].localPosition.x, flo_Y[3]);
                Recovery.GameData.Instance.Dic_RoleCount[number].canvas.enabled = true;
                Recovery.GameData.Instance.Dic_RoleCount[number].anim.speed = 0.001f;
                Recovery.GameData.Instance.Dic_RoleCount[number].isStopAnim = true;
                break;
        }
    }

    private float[] flo_Y = new float[4] { 89.6f, 29f, -31.6f, -92.2f };
    /// <summary>
    /// 更新小地图
    /// </summary>
    public void SetRanking()
    {
        foreach (var dic_1 in Recovery.GameData.Instance.Dic_RoleCount)
        {
            int count = 0;
            foreach (var dic_2 in Recovery.GameData.Instance.Dic_RoleCount)
            {
                if (dic_1.Value.balluteCount >= dic_2.Value.balluteCount)
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
            int count = 0;
            foreach (var dic_2 in Recovery.GameData.Instance.Dic_RoleCount)
            {
                if (dic_1.Value.distance >= dic_2.Value.distance)
                    continue;
                count++;
            }
            switch (count)
            {
                case 0:
                    rl_number[0].text = dic_1.Value.number;
                    //rl_name[0].text = dic_1.Value.distance.ToString(/*"N2"*/);
                    rl_name[0].text = dic_1.Value.balluteCount.ToString();
                    break;
                case 1:
                    rl_number[1].text = dic_1.Value.number;
                    //rl_name[1].text = dic_1.Value.distance.ToString(/*"N2"*/);
                    rl_name[1].text = dic_1.Value.balluteCount.ToString();

                    break;
                case 2:
                    rl_number[2].text = dic_1.Value.number;
                    //rl_name[2].text = dic_1.Value.distance.ToString(/*"N2"*/);
                    rl_name[2].text = dic_1.Value.balluteCount.ToString();
                    break;
                case 3:
                    rl_number[3].text = dic_1.Value.number;
                    //rl_name[3].text = dic_1.Value.distance.ToString(/*"N2"*/);
                    rl_name[3].text = dic_1.Value.balluteCount.ToString();
                    break;
            }
        }
    }



    public void SortNumber(int num, string str)
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
