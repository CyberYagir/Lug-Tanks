using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class WebData : MonoBehaviour
{
    public static WebData webData;

    public static PlayerTankData tankData
    {
        get => data?.tank;
        set => data.tank = value;
    }

    public static Responce data = null;
    [SerializeField] private string URL;
    [SerializeField] private Statistics statistics;
    public Error error;

    private void Start()
    {
        if (webData == null)
        {
            webData = this;
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
        if (!PHPMenuManager.manager.CanSendData(out error, false))
        {
            StartCoroutine(Login(PHPMenuManager.manager.GetLogin(), PHPMenuManager.manager.GetPass()));
            PHPMenuManager.manager.DisableButtons(false);
        }
    }

    public void RegStart()
    {
        if (!PHPMenuManager.manager.CanSendData(out error, true))
        {
            StartCoroutine(Register(PHPMenuManager.manager.GetLogin(), PHPMenuManager.manager.GetPass()));
            PHPMenuManager.manager.DisableButtons();
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


        PHPMenuManager.manager.DisableButtons(true);
        if (String.IsNullOrEmpty(www.error))
        {
            print(www.text);
            error = JsonUtility.FromJson<Error>(www.text);
            if (!error.isError)
            {
                data = JsonUtility.FromJson<Responce>(www.text);
                PhotonLobby.lobby.InitPUN();
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

        PHPMenuManager.manager.DisableButtons(true);

        if (String.IsNullOrEmpty(www.error))
        {
            error.isError = false;
            error = JsonUtility.FromJson<Error>(www.text);
            if (!error.isError)
                PHPMenuManager.manager.Change(true);
        }
        else
            error = new Error() {error = "Connection lost", isError = true};
    }

    public static void SaveStart()
    {
        webData.StartCoroutine(webData.Save());
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
