using Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Web;

namespace Photon
{
    [System.Serializable]
    public class AutorizationWindow
    {
        [SerializeField] private bool isLoginState;
        [SerializeField] private Button loginButton;
        [SerializeField] private Button regButton;
        [SerializeField] private Button toLoginButton;
        [SerializeField] private Button toRegButton;
        [SerializeField] private Button toGuestButton;
        [SerializeField] private TMP_InputField loginInput;
        [SerializeField] private TMP_InputField passInput;
        [SerializeField] private TMP_InputField passRepInput;


        public TMP_InputField PassRepInput => passRepInput;
        public TMP_InputField PassInput => passInput;
        public TMP_InputField LoginInput => loginInput;
        public Button ToRegButton => toRegButton;
        public Button ToLoginButton => toLoginButton;
        public Button RegButton => regButton;
        public Button LoginButton => loginButton;
        public Button ToGuestButton => toGuestButton;


        public void Change(bool n)
        {
            isLoginState = n;

            toRegButton.gameObject.SetActive(isLoginState);
            toLoginButton.gameObject.SetActive(!isLoginState);

            passRepInput.gameObject.SetActive(!isLoginState);

            loginButton.gameObject.SetActive(isLoginState);
            regButton.gameObject.SetActive(!isLoginState);


            LayoutRebuilder.ForceRebuildLayoutImmediate(loginInput.transform.parent.parent.GetComponent<RectTransform>());
        }
        public void DisableButtons(bool interactable = false)
        {
           RegButton.interactable = interactable;
           LoginButton.interactable = interactable;

           ToRegButton.interactable = interactable;
           ToLoginButton.interactable = interactable;
           ToGuestButton.interactable = interactable;
        }
    }

    
    public class PHPMenuService : MonoBehaviour
    {
        public static PHPMenuService Instance;

        [SerializeField] private Canvas guestWindow, autorizeWindow;
        [SerializeField] private TMP_Text error;
        [SerializeField] private AutorizationWindow autorization;

        
        public AutorizationWindow Autorization => autorization;


        public void Init(Base.Controller.Tank tank)
        {
            foreach (var item in tank.weapons)
            {
                item.enabled = false;
            } 
            Instance = this;

            guestWindow.enabled = true;
            autorizeWindow.enabled = false;
        }

        private void Update()
        {
            if (WebDataService.Instance.ErrorData.isError)
            {
                error.transform.parent.gameObject.SetActive(true);
                var localizedError = LocalizationService.GetWorld(WebDataService.Instance.ErrorData.error);
                if (localizedError == "")
                {
                    error.text = WebDataService.Instance.ErrorData.error;
                }
                else
                {
                    error.text = localizedError;
                }
            }
            else
            {
                error.transform.parent.gameObject.SetActive(false);
            }
        }



        public void PlayAsGuest()
        {
            WebDataService.Instance.CreateGuestAccount();
        }
        public void Login() => WebDataService.Instance.LoginStart();
        public void Register()=> WebDataService.Instance.RegStart();
        public bool CanSendData(out Error errorOut, bool checkRepeatPassword = false)
        {
            errorOut = new Error() { isError = false};
            if (Autorization.LoginInput.text.Length > Autorization.LoginInput.characterLimit)
            {
                errorOut = new Error() {error = "error_web01", isError = true};
                return errorOut.isError;
            }

            if (Autorization.LoginInput.text.Length < 4)
            {
                errorOut = new Error() {error = "error_web02", isError = true};
                return errorOut.isError;
            }

            if (Autorization.PassInput.text.Length < 4)
            {
                errorOut = new Error() {error = "error_web03", isError = true};
                return errorOut.isError;
            }

            if (Autorization.PassInput.text != Autorization.PassRepInput.text && checkRepeatPassword)
            {
                errorOut = new Error() {error = "error_web04", isError = true};
                return errorOut.isError;
            }

            return errorOut.isError;
        }
        public void Change(bool n) => autorization.Change(n);
        public void DisableButtons(bool interactable = false) => autorization.DisableButtons(interactable);
        public string GetLogin() => Autorization.LoginInput.text;
        public string GetPass() => Autorization.PassInput.text;
    }
}
