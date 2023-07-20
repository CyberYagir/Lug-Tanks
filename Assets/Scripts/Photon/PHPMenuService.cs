using Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Web;

namespace Photon
{
    public class PHPMenuService : MonoBehaviour
    {
        public static PHPMenuService Instance;
        
        
        [SerializeField] private bool is_login;
        [SerializeField] private GameObject loginb, regb;
        [SerializeField] private GameObject toLoginb, toRegb;
        [SerializeField] private TMP_InputField login;
        [SerializeField] private TMP_InputField pass, passr;
        [SerializeField] private TMP_Text error;

   

        public void Init(Base.Controller.Tank tank)
        {
            foreach (var item in tank.weapons)
            {
                item.enabled = false;
            } 
            Instance = this;
        }
    
        public void Login()
        {
            WebDataService.Instance.LoginStart();
        }

        public void Register()
        {
            WebDataService.Instance.RegStart();
        }
    
        private void Update()
        {
            if (WebDataService.tankData != null)
            {
                Instance.gameObject.SetActive(false);
            }
        
            if (WebDataService.Instance.ErrorData.isError)
            {
                error.transform.parent.gameObject.SetActive(true);
                error.text = LocalizationService.GetWorld(WebDataService.Instance.ErrorData.error);
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



        public bool CanSendData(out Error errorOut, bool checkRepeatPassword = false)
        {
            errorOut = new Error() { isError = false};
            if (login.text.Length > login.characterLimit)
            {
                errorOut = new Error() {error = "error_web01", isError = true};
                return errorOut.isError;
            }

            if (login.text.Length < 4)
            {
                errorOut = new Error() {error = "error_web02", isError = true};
                return errorOut.isError;
            }

            if (pass.text.Length < 4)
            {
                errorOut = new Error() {error = "error_web03", isError = true};
                return errorOut.isError;
            }

            if (pass.text != passr.text && checkRepeatPassword)
            {
                errorOut = new Error() {error = "error_web04", isError = true};
                return errorOut.isError;
            }

            return errorOut.isError;
        }

        public void DisableButtons(bool interactable = false)
        {
            regb.GetComponent<Button>().interactable = interactable;
            loginb.GetComponent<Button>().interactable = interactable;
        
            toRegb.GetComponent<Button>().interactable = interactable;
            toLoginb.GetComponent<Button>().interactable = interactable;
        }

        public string GetLogin() => login.text;
        public string GetPass() => pass.text;
    }
}
