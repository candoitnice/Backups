using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Linq;

public class Http_WWW : MonoBehaviour
{
    public static Http_WWW Instance;
    void Awake()
    {
        Instance = this;
    }


    public void Init()
    {
        string url = Application.dataPath;
        url = url.Substring(0, url.LastIndexOf('/'));
        url = url + "/SetGame/游戏设置.json";
        IE_WWW("file:///" + url);
    }


    /// <summary>
    /// 返回读到的所有数据
    /// </summary>
    /// <param name="path">地址</param>
    /// <returns></returns>
    public void IE_HttpWWW(string path)
    {
        StartCoroutine(IE_httpwww(path));
    }

    public HttpInfo varConfig;
    //"http://127.0.0.1:8080/api/game/start?id=bick"后台地址;
    private IEnumerator IE_httpwww(string path)
    {
        WWW www = new WWW(path);
        yield return www;
        if (www.error == null)
        {
            Debug.Log(www.text.Trim());
            string Httpdata = www.text.Trim();

            varConfig = JsonConvert.DeserializeObject<HttpInfo>(Httpdata);

            Recovery.GameData.Instance.no = varConfig.no;
            Recovery.GameData.Instance._name = varConfig.name;
            Recovery.GameData.Instance.age = varConfig.age;
            Recovery.GameData.Instance.sex = varConfig.sex;
            Recovery.GameData.Instance.duration = varConfig.config[0].value;
            Recovery.GameData.Instance.mode = varConfig.config[1].value;
            Recovery.GameData.Instance.level = varConfig.config[2].value;
            Recovery.GameData.Instance.gameTime = int.Parse(varConfig.config[0].value);

            switch (Recovery.GameData.Instance.mode)
            {
                case "0":
                    Recovery.GameData.Instance.gameModeType = Recovery.Type.GameModeType.single;
                    break;
                case "1":
                    Recovery.GameData.Instance.gameModeType = Recovery.Type.GameModeType.automatic;
                    break;
                case "2":
                    Recovery.GameData.Instance.gameModeType = Recovery.Type.GameModeType.many;
                    break;
            }
        }
        else
        {
            Debug.Log("后台数据读取失败");
            UIUpdateManaget.Instance.Configuration.gameObject.SetActive(true);
            UIUpdateManaget.Instance.Configuration.OnPlay();
        }
        EventManager.Instance._OnGameInit();
        www.Dispose();
        www = null;
        Resources.UnloadUnusedAssets();


    }
    /// <summary>
    /// 返回读到的所有数据
    /// </summary>
    /// <param name="path"></param>
    public void IE_WWW(string path)
    {
        StartCoroutine(IE_www(path));
    }
    private IEnumerator IE_www(string path)
    {
        WWW www = new WWW(path);
        yield return www;
        if (www.error == null)
        {
            print(www.text);
            SetGame setG = JsonConvert.DeserializeObject<SetGame>(www.text.Trim());
            Recovery.GameData.Instance.bikeIp = setG.训练机通讯.IP;
            Recovery.GameData.Instance.bikePort = setG.训练机通讯.端口;
            Recovery.GameData.Instance.serverIp = setG.联机通讯服务器.IP;
            Recovery.GameData.Instance.serverPort = setG.联机通讯服务器.端口;
            Recovery.GameData.Instance.gamename = setG.GameName;
            Recovery.GameData.Instance.getUrl = setG.平台.Get;
            Recovery.GameData.Instance.setUrl = setG.平台.Set;
            Recovery.GameData.Instance.gameid = Recovery.GameData.Instance.getUrl.Split('=')[1];
            if (setG.是否主机)
                Recovery.GameData.Instance.isHost = true;
            else
                Recovery.GameData.Instance.isHost = false;

        }
        IE_HttpWWW(Recovery.GameData.Instance.getUrl);
        www.Dispose();
        www = null;
        Resources.UnloadUnusedAssets();
    }

    //上传
    public void UploadInfo(string data)
    {
        StartCoroutine(IE_UploadInfo(data));
    }
    private IEnumerator IE_UploadInfo(string data)
    {
        Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
        Debug.Log(dic.Count);
        WWWForm wwwf = new WWWForm();
        wwwf.AddField(dic.ElementAt(0).Key, dic.ElementAt(0).Value);
        wwwf.AddField(dic.ElementAt(1).Key, dic.ElementAt(1).Value);
        wwwf.AddField(dic.ElementAt(2).Key, dic.ElementAt(2).Value);
        wwwf.AddField(dic.ElementAt(3).Key, dic.ElementAt(3).Value);
        wwwf.AddField(dic.ElementAt(4).Key, dic.ElementAt(4).Value);
        wwwf.AddField(dic.ElementAt(5).Key, dic.ElementAt(5).Value);
        wwwf.AddField(dic.ElementAt(6).Key, dic.ElementAt(6).Value);
        wwwf.AddField(dic.ElementAt(7).Key, dic.ElementAt(7).Value);
        wwwf.AddField(dic.ElementAt(8).Key, dic.ElementAt(8).Value);
        wwwf.AddField(dic.ElementAt(9).Key, dic.ElementAt(9).Value);
        wwwf.AddField(dic.ElementAt(10).Key, dic.ElementAt(10).Value);
        wwwf.AddField(dic.ElementAt(11).Key, dic.ElementAt(11).Value);
        wwwf.AddField(dic.ElementAt(12).Key, dic.ElementAt(12).Value);
        wwwf.AddField(dic.ElementAt(13).Key, dic.ElementAt(13).Value);
        wwwf.AddField(dic.ElementAt(14).Key, dic.ElementAt(14).Value);
        wwwf.AddField(dic.ElementAt(15).Key, dic.ElementAt(15).Value);
        wwwf.AddField(dic.ElementAt(16).Key, dic.ElementAt(16).Value);
        wwwf.AddField(dic.ElementAt(17).Key, dic.ElementAt(17).Value);
        wwwf.AddField(dic.ElementAt(18).Key, dic.ElementAt(18).Value);
        wwwf.AddField(dic.ElementAt(19).Key, dic.ElementAt(19).Value);
        wwwf.AddField(dic.ElementAt(20).Key, dic.ElementAt(20).Value);
        wwwf.AddField(dic.ElementAt(21).Key, dic.ElementAt(21).Value);
        wwwf.AddField(dic.ElementAt(22).Key, dic.ElementAt(22).Value);

        Debug.Log(data);
        WWW www = new WWW(Recovery.GameData.Instance.setUrl, wwwf);
        yield return www;
        if (www.error == null)
        {
            Debug.Log(www.text);
            EventManager.Instance._OnGameOver();
        }
        www.Dispose();
        www = null;
        Resources.UnloadUnusedAssets();
    }
}
