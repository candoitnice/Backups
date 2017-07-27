using System.Collections;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
namespace Recovery
{
    public class Main : MonoBehaviour
    {
        public static Main Instance;

        public Recovery.Type.GameEditionType GEType = Recovery.Type.GameEditionType.Adult;
        public Recovery.Type.MachineMode machineMode = Recovery.Type.MachineMode.Down;

        Process process;
        public delegate void HandleOnSendDataEvent();
        public event HandleOnSendDataEvent OnSendDataEvent;
        public float sendTime = 0f;
        public float fps = 30;
        public float delaytime = 0f;

        Resolution[] resolutions;
        void Awake()
        {
            Instance = this;
            resolutions = Screen.resolutions;
            delaytime = 1f / fps;
        }
        public IEnumerator IEResolutions()
        {
            yield return new WaitForSeconds(1);
            foreach (var item in resolutions)
            {
                if (item.height != 1920 && item.width != 1080)
                {
                    Screen.SetResolution(540, 960, false);
                }
                else
                {
                    Screen.SetResolution(1080, 1920, true);
                    break;
                }
            }
        }
        // Use this for initialization
        void Start()
        {
            UIUpdateManaget.Instance.InitUI();
            Http_WWW.Instance.Init();
            Recovery.InfoHandle.Instance.InitEvent();
            EventManager.Instance.onGameUpdate += GameManager.instance.QueuePlay;
            EventManager.Instance.onGameInit += Init;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public void Init()
        {
            switch (Recovery.GameData.Instance.gameModeType)
            {
                case Recovery.Type.GameModeType.single:
                case Recovery.Type.GameModeType.automatic:
                    GameManager.instance.InitGameSingle();
                    break;
                case Recovery.Type.GameModeType.many:
                    if (Recovery.GameData.Instance.isHost)
                    {
                        Screen.SetResolution(540, 960, false);
                        process = Process.Start(Application.streamingAssetsPath + "/Release/launch.exe");
                    }
                    Thread.Sleep(1000);
                    GameManager.instance.InitGameMany();
                    break;
            }
            StartCoroutine(IEResolutions());
            EventManager.Instance.onGameInit -= Init;

        }



        // Update is called once per frame
        void Update()
        {

            sendTime += Time.deltaTime;

            if (sendTime >= delaytime)
            {
                sendTime = 0;
                if (OnSendDataEvent != null)
                    OnSendDataEvent();
            }

            EventManager.Instance._OnGameUpdate();
        }

        /// <summary>
        /// 脚本失效时运行
        /// </summary>
        void OnApplicationQuit()
        {
            if (Recovery.GameData.Instance.isHost)
            {
                if (process != null && !process.HasExited)
                {
                    process.CloseMainWindow();
                }
            }
            MySocket.Instance.close();
            Recovery.InfoHandle.Instance.CloseEvent();
            EventManager.Instance.onGameUpdate -= GameManager.instance.QueuePlay;
            EventManager.Instance._OnApplicationQuit();
        }
    }
}