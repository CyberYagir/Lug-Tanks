using System;
using System.Collections;
using Content.Scripts.Anticheat;
using Content.Scripts.Web.Classes;
using CrazyGames;
using Photon;
using Photon.Pun;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Web.Classes;
using Random = UnityEngine.Random;

namespace Web
{
    public class WebDataService : MonoBehaviour
    {
        public static WebDataService Instance;

        public static PlayerTankData tankData
        {
            get => data?.tank;
            set => data.tank = value;
        }


        public static Responce data { get; set; } = null;
        [SerializeField] private string URL;
        [SerializeField] private Statistics statistics;
        [SerializeField] Error error;

        public Error ErrorData => error;
        public UnityEvent OnLogin;


        public void Init()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                SaveStart();
            }

            StopAllCoroutines();
        }

        // public void Update()
        // {
        //     // if (!PhotonNetwork.InRoom)
        //     // {
        //     //     if (tankData != null)
        //     //     {
        //     //         SetResolution.SetName("Lug-Tanks: " + data.playerData.name);
        //     //     }
        //     // }
        //     // else
        //     // {
        //     //     SetResolution.SetName("Lug-Tanks: " + data.playerData.name + " | Room: " + PhotonNetwork.CurrentRoom.Name);
        //     // }
        // }

        public void LoginStart()
        {
            if (data != null && data.playerData.id.ObfUn() < 0) return;
            if (!PHPMenuService.Instance.CanSendData(out error, false))
            {
                StartCoroutine(Login(PHPMenuService.Instance.GetLogin(), PHPMenuService.Instance.GetPass()));
                PHPMenuService.Instance.DisableButtons(false);
            }
        }

        public void RegStart()
        {
            if (data != null && data.playerData.id.ObfUn() < 0) return;
            if (!PHPMenuService.Instance.CanSendData(out error, true))
            {
                StartCoroutine(Register(PHPMenuService.Instance.GetLogin(), PHPMenuService.Instance.GetPass()));
                PHPMenuService.Instance.DisableButtons();
            }
        }

        public WWWForm GetForm(string name, string password)
        {
            var form = new WWWForm();
            form.AddField("fromUnity", "2003");
            form.AddField("login", name.ToLower());
            form.AddField("password", password.ToLower());

            return form;
        }

        IEnumerator Login(string name, string password)
        {
            WWWForm form = GetForm(name, password);
            AddHeaders(form);


            WWW www = new WWW(URL, form);
            yield return www;


            PHPMenuService.Instance.DisableButtons(true);
            if (String.IsNullOrEmpty(www.error))
            {
                print(www.text);
                error = JsonUtility.FromJson<Error>(www.text);
                if (!ErrorData.isError)
                {
                    data = JsonUtility.FromJson<Responce>(www.text).Obfuscate();

                    PhotonLobbyService.Instance.InitPUN();
                    OnLogin.Invoke();
                }
            }
            else
                error = new Error() {error = "error_web05", isError = true};
        }

        private static void AddHeaders(WWWForm form)
        {
            form.headers.Add("Access-Control-Allow-Credentials", "true");
            form.headers.Add("Access-Control-Expose-Headers", "Content-Length, Content-Encoding");
            form.headers.Add("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Application-Name, X-Request-Sent-Time, Content-Type");
            form.headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            form.headers.Add("Access-Control-Allow-Origin", "*");
        }

        IEnumerator Register(string name, string password)
        {
            WWWForm form = GetForm(name, password);
            form.AddField("register", "");

            AddHeaders(form);

            WWW www = new WWW(URL, form);
            yield return www;

            PHPMenuService.Instance.DisableButtons(true);

            if (String.IsNullOrEmpty(www.error))
            {
                ErrorData.isError = false;
                error = JsonUtility.FromJson<Error>(www.text);
                if (!ErrorData.isError)
                {
                    PHPMenuService.Instance.Change(true);
                }
            }
            else
                error = new Error() {error = "error_web05", isError = true};
        }

        public static void SaveStart()
        {
            if (data != null && data.playerData.id.ObfUn() < 0)
            {
                PlayerPrefs.SetString(guestKey, JsonUtility.ToJson(data.UnObfuscate()));
                return;
            }
            
            Instance.StartCoroutine(Instance.Save());
        }

        IEnumerator Save()
        {
            WWWForm form = new WWWForm();
            form.AddField("fromUnity", "2003");
            form.AddField("save", "");
            form.AddField("response", JsonUtility.ToJson(data.UnObfuscate()));

            AddHeaders(form);

            UnityWebRequest www = UnityWebRequest.Post(URL, form);
            www.timeout = 1;
            yield return www;
        }

        private async void OnApplicationQuit()
        {
            if (data != null && data.playerData.id.ObfUn() < 0) return;
            
            if (data == null) return;
            
            WWWForm form = new WWWForm();
            form.AddField("fromUnity", "2003");
            form.AddField("statisticsAdd", "");
            form.AddField("userid", data.playerData.id.ObfUn());

            data.statistics = statistics.GetStats();
            form.AddField("response", JsonUtility.ToJson(data.UnObfuscate()));
            AddHeaders(form);


            UnityWebRequest req = UnityWebRequest.Post(URL, form);

            req.SendWebRequest();
            while (!req.isDone)
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        public const string guestKey = "GuestData";
        public void CreateGuestAccount()
        {
            if (PlayerPrefs.HasKey(guestKey))
            {
                data = JsonUtility.FromJson<Responce>(PlayerPrefs.GetString(guestKey)).Obfuscate();
            }
            else
            {
                var userID = -Random.Range(10000, 99999);
                data = new Responce()
                {
                    playerData = new PlayerData()
                    {
                        id = userID,
                        name = $"Guest-{Random.Range(100, 999)}"
                    },
                    userStatistics = new UserStatistics()
                    {
                        id = -1,
                        deaths = 0,
                        kills = 0,
                        registerDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        userid = userID
                    },
                    statistics = null,
                    tank = new PlayerTankData()
                    {
                        corpus = 0,
                        weapon = 0,
                        exp = 0,
                        id = 0,
                        level = 0,
                        userid = userID
                    }
                }.Obfuscate();

                SaveStart();
            }

            PhotonLobbyService.Instance.InitPUN();
            OnLogin.Invoke();
        }
    }
}
