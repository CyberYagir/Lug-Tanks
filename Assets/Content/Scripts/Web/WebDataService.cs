﻿using System;
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

        public static Responce UserData { get; private set; } = null;
        public static PlayerTankData TankData => UserData?.Tank;


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
            if (UserData != null && UserData.PlayerData.ID < 0) return;
            if (!PHPMenuService.Instance.CanSendData(out error, false))
            {
                StartCoroutine(Login(PHPMenuService.Instance.GetLogin(), PHPMenuService.Instance.GetPass()));
                PHPMenuService.Instance.DisableButtons(false);
            }
        }

        public void RegStart()
        {
            if (UserData != null && UserData.PlayerData.ID < 0) return;
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
                    UserData = JsonUtility.FromJson<Responce>(www.text).Obfuscate();

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
            if (UserData != null && UserData.PlayerData.ID < 0)
            {
                PlayerPrefs.SetString(guestKey, JsonUtility.ToJson(UserData.UnObfuscate()));
                return;
            }
            
            Instance.StartCoroutine(Instance.Save());
        }

        IEnumerator Save()
        {
            WWWForm form = new WWWForm();
            form.AddField("fromUnity", "2003");
            form.AddField("save", "");
            form.AddField("response", JsonUtility.ToJson(UserData.UnObfuscate()));

            AddHeaders(form);

            UnityWebRequest www = UnityWebRequest.Post(URL, form);
            www.timeout = 1;
            yield return www;
        }

        private async void OnApplicationQuit()
        {
            if (UserData != null && UserData.PlayerData.ID < 0) return;
            
            if (UserData == null) return;
            
            WWWForm form = new WWWForm();
            form.AddField("fromUnity", "2003");
            form.AddField("statisticsAdd", "");
            form.AddField("userid", UserData.PlayerData.ID);

            UserData.SetStatistics(statistics.GetStats());
            form.AddField("response", JsonUtility.ToJson(UserData.UnObfuscate()));
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
                UserData = JsonUtility.FromJson<Responce>(PlayerPrefs.GetString(guestKey)).Obfuscate();
            }
            else
            {
                var userID = -Random.Range(10000, 99999);
                UserData = new Responce(
                    playerData: new PlayerData(userID, $"Guest-{Random.Range(100, 999)}"),
                    userStatistics: new UserStatistics(-1, userID, 0, 0, DateTime.Now.ToString("yyyy-MM-dd")),
                    statistics: null,
                    tank: new PlayerTankData(-1, userID, 0, 0, 0, 0)
                ).Obfuscate();

                SaveStart();
            }

            PhotonLobbyService.Instance.InitPUN();
            OnLogin.Invoke();
        }
    }
}
