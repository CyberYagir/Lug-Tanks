using System;
using System.Collections;
using Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Web.Classes;

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


        public static Responce data = null;
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

        public void Update()
        {
            if (!PhotonNetwork.InRoom)
            {
                if (tankData != null)
                {
                    SetResolution.SetName("Lug-Tanks: " + data.playerData.name);
                }
            }
            else
            {
                SetResolution.SetName("Lug-Tanks: " + data.playerData.name + " | Room: " + PhotonNetwork.CurrentRoom.Name);
            }
        }

        public void LoginStart()
        {
            if (!PHPMenuService.Instance.CanSendData(out error, false))
            {
                StartCoroutine(Login(PHPMenuService.Instance.GetLogin(), PHPMenuService.Instance.GetPass()));
                PHPMenuService.Instance.DisableButtons(false);
            }
        }

        public void RegStart()
        {
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
                    data = JsonUtility.FromJson<Responce>(www.text);
                    PhotonLobbyService.Instance.InitPUN();
                    OnLogin.Invoke();
                }
            }
            else
                error = new Error() {error = "Connection lost", isError = true};
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
                    PHPMenuService.Instance.Change(true);
            }
            else
                error = new Error() {error = "Connection lost", isError = true};
        }

        public static void SaveStart()
        {
            Instance.StartCoroutine(Instance.Save());
        }

        IEnumerator Save()
        {
            WWWForm form = new WWWForm();
            form.AddField("fromUnity", "2003");
            form.AddField("save", "");
            form.AddField("response", JsonUtility.ToJson(data));
        
            AddHeaders(form);
        
            WWW www = new WWW(URL, form);
            yield return www;
        }

        private async void OnApplicationQuit()
        {
            WWWForm form = new WWWForm();
            form.AddField("fromUnity", "2003");
            form.AddField("statisticsAdd", "");
            form.AddField("userid", data.playerData.id);

            data.statistics = statistics.GetStats();
            form.AddField("response", JsonUtility.ToJson(data));
            AddHeaders(form);
        
        
            UnityWebRequest req = UnityWebRequest.Post(URL, form);

            req.SendWebRequest();
            while (!req.isDone)
            {
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
