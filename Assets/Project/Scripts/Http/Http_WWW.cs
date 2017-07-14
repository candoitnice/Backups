using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

public class Http_WWW : MonoBehaviour {
    public static Http_WWW Instance;
    void Awake()
    {
        Instance = this;
    }


    public void Init()
    {
        IE_HttpWWW("http://127.0.0.1:8080/game/start?id=bike");
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
    //"http://127.0.0.1:8080/game/start?id=bike"后台地址;
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
        www.Dispose();
        www = null;
        Resources.UnloadUnusedAssets();


        string asfh = Application.streamingAssetsPath;
        asfh = asfh + "/SetGameData.json";
        IE_WWW("file:///" + asfh);
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
            SetGame setG=JsonConvert.DeserializeObject<SetGame>(www.text.Trim());
          Recovery.GameData.Instance.bikeIp = setG.自行车通讯.IP;
          Recovery.GameData.Instance.bikePort = setG.自行车通讯.端口;
          Recovery.GameData.Instance.serverIp = setG.服务器通讯.IP;
            Recovery.GameData.Instance.serverPort = setG.服务器通讯.端口;

            EventManager.Instance._OnGameInit();
           
        }
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
        WWWForm wwwf = new WWWForm();
        wwwf.AddField("", data);

        Debug.Log(data);
        WWW www = new WWW("http://127.0.0.1:8080/game/end", wwwf);
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
