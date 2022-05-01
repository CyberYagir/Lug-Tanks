using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PHPMenuManager : MonoBehaviour
{
    public static PHPMenuManager manager;
    public bool is_login;
    public GameObject loginb, regb;
    public GameObject toLoginb, toRegb;
    public TMP_InputField login;
    public TMP_InputField pass, passr;
    public TMP_Text error;

    public void Login()
    {
        WebData.webData.LoginStart();
    }

    public void Register()
    {
        WebData.webData.RegStart();
    }

    void Start()
    {
        foreach (var item in FindObjectOfType<Tank>().weapons)
        {
            item.enabled = false;
        } 
        manager = this;
    }
    private void Update()
    {

        if (WebData.playerData != null)
        {
            manager.gameObject.SetActive(false);
        }
        if (WebData.webData.error.isError)
        {
            error.transform.parent.gameObject.SetActive(true);
            error.text = WebData.webData.error.error;
        }
        else
        {
            error.transform.parent.gameObject.SetActive(false);
        }
    }
    public void Change(bool n)
    {
        is_login = n;

        toRegb.SetActive(is_login);
        toLoginb.SetActive(!is_login);

        passr.gameObject.SetActive(!is_login);

        loginb.SetActive(is_login);
        regb.SetActive(!is_login);


        LayoutRebuilder.ForceRebuildLayoutImmediate(login.transform.parent.parent.GetComponent<RectTransform>());
    }
}
