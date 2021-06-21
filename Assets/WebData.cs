using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData
{
    public int id = -1;
    public string name;
    public int weapon, corpus, exp, level;

    public PlayerData()
    {

    }
}
[System.Serializable]
public class Error {
    public string error;
    public bool isError;
}


public class WebData : MonoBehaviour
{
    public static WebData webData;
    public string URL;
    public static PlayerData playerData = null;
    public Error error;
    private void Start()
    {
        if (webData == null)
        {
            webData = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        StopAllCoroutines();
    }

    public void Update()
    {
        if (!PhotonNetwork.InRoom)
        {
            if (playerData != null)
            {
                SetResolution.SetName("Tanks Of Donbass: " + playerData.name);
            }
        }
        else
        {
            SetResolution.SetName("Tanks Of Donbass: " + playerData.name + " | Room: " + PhotonNetwork.CurrentRoom.Name);
        }
    }
    public void LoginStart()
    {
        var l = PHPMenuManager.manager.login;
        var p = PHPMenuManager.manager.pass;
        if (l.text.Length <= l.characterLimit)
        {
            if (l.text.Length >= 4)
            {
                if (p.text.Length >= 4)
                {
                    StartCoroutine(Login(l.text, PHPMenuManager.manager.pass.text));
                    PHPMenuManager.manager.loginb.GetComponent<Button>().interactable = false;
                    PHPMenuManager.manager.toRegb.GetComponent<Button>().interactable = false;
                }
                else
                {
                    error = new Error() { error = "Password is so short..", isError = true };
                }
            }
            else
            {
                error = new Error() { error = "Login is so short..", isError = true };
            }
        }
        else
        {
            error = new Error() { error = "Login is so big..", isError = true };
        }
    }
    public void RegStart()
    {
        var l = PHPMenuManager.manager.login;
        var p = PHPMenuManager.manager.pass;
        var p1 = PHPMenuManager.manager.passr;
        if (l.text.Length <= l.characterLimit)
        {
            if (l.text.Length >= 4)
            {
                if (p.text.Length >= 4)
                {
                    if (p1.text == p.text)
                    {
                        StartCoroutine(Register(PHPMenuManager.manager.login.text, PHPMenuManager.manager.pass.text));
                        PHPMenuManager.manager.regb.GetComponent<Button>().interactable = false;
                        PHPMenuManager.manager.toLoginb.GetComponent<Button>().interactable = false;
                    }
                    else
                    {
                        error = new Error() { error = "Passwords not equal", isError = true };
                    }
                }
                else
                {
                    error = new Error() { error = "Password is so short..", isError = true };
                }
            }
            else
            {
                error = new Error() { error = "Login is so short..", isError = true };
            }
        }
        else
        {
            error = new Error() { error = "Login is so big..", isError = true };
        }
    }

    IEnumerator Login(string name, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("fromUnity", "2003");
        form.AddField("login", name.ToLower());
        form.AddField("password", password.ToLower());
        WWW www = new WWW(URL, form);
        yield return www;

        PHPMenuManager.manager.loginb.GetComponent<Button>().interactable = true;
        PHPMenuManager.manager.toRegb.GetComponent<Button>().interactable = true;
        if (String.IsNullOrEmpty(www.error))
        {
//            print(www.text);
            error = JsonUtility.FromJson<Error>(www.text);
            if (!error.isError)
            {
                playerData = JsonUtility.FromJson<PlayerData>(www.text);
                PhotonLobby.lobby.InitPUN();
            }
        }
        else
            error = new Error() { error = "Connection lost", isError = true };
    }

    IEnumerator Register(string name, string password)
    {

        WWWForm form = new WWWForm();
        form.AddField("fromUnity", "2003");
        form.AddField("register", "");
        form.AddField("login", name.ToLower());
        form.AddField("password", password.ToLower());
        WWW www = new WWW(URL, form);
        yield return www;

        PHPMenuManager.manager.regb.GetComponent<Button>().interactable = true;
        PHPMenuManager.manager.toLoginb.GetComponent<Button>().interactable = true;
        if (String.IsNullOrEmpty(www.error))
        {
            error.isError = false;
            //playerData = JsonUtility.FromJson<PlayerData>(www.text);
            error = JsonUtility.FromJson<Error>(www.text);
            if (!error.isError)
                PHPMenuManager.manager.Change(true);
        }
        else
            error = new Error() { error = "Connection lost", isError = true };
    }


    public static void SaveStart()
    {
        webData.StartCoroutine(webData.Save());
    }
    IEnumerator Save()
    {
        print("save");
        WWWForm form = new WWWForm();
        form.AddField("save", "");
        form.AddField("fromUnity", "2003");
        form.AddField("exp", playerData.exp);
        form.AddField("corpus", playerData.corpus);
        form.AddField("weapon", playerData.weapon);
        form.AddField("id", playerData.id);
        WWW www = new WWW(URL, form);
        yield return www;
    }

    public void Exit()
    {
        SaveStart();
        //isLogged = false;
    }
}
