using System;
using ModestTree;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class ModalDialog : ITickable,IDisposable
    {
        private static ModalDialogView View { get; set; }

        public void SetView(ModalDialogView view)
        {
            View = view;
            View.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            View.Button1.onClick.RemoveAllListeners();
            View.Button2.onClick.RemoveAllListeners();
            View.Button3.onClick.RemoveAllListeners();
        }

        public void Tick()
        {
            if (View.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Return))
            {
                View.Button1.onClick.Invoke();
            }
        }

        public static void Show(ModalDialogDetails details)
        {
            Assert.That(View.gameObject.activeSelf == false, "You cannot call ModalDialog.Show until you dont close current one");
            View.transform.SetAsLastSibling();
            View.gameObject.SetActive(true);

            (View.transform as RectTransform).localPosition = Vector3.zero;

            View.Button1.gameObject.SetActive(false);
            View.Button2.gameObject.SetActive(false);
            View.Button3.gameObject.SetActive(false);

            View.DialogMessageText.text = details.DialogMessage;

            View.Button1.onClick.RemoveAllListeners();
            View.Button1.onClick.AddListener(CloseDialog);
            if (details.Button1.Handler != null)
                View.Button1.onClick.AddListener(() => details.Button1.Handler());
            View.Button1Text.text = details.Button1.Title;
            View.Button1.gameObject.SetActive(true);

            if (details.Button2 != null)
            {
                View.Button2.onClick.RemoveAllListeners();
                View.Button2.onClick.AddListener(CloseDialog);
                if (details.Button2.Handler != null)
                    View.Button2.onClick.AddListener(() => details.Button2.Handler());
                View.Button2Text.text = details.Button2.Title;
                View.Button2.gameObject.SetActive(true);
            }

            if (details.Button3 != null)
            {
                View.Button3.onClick.RemoveAllListeners();
                View.Button3.onClick.AddListener(CloseDialog);
                if (details.Button3.Handler != null)
                    View.Button3.onClick.AddListener(() => details.Button3.Handler());
                View.Button3Text.text = details.Button3.Title;
                View.Button3.gameObject.SetActive(true);
            }
        }

        public static void Show(string message)
        {
            var details = new ModalDialogDetails
            {
                DialogMessage = message,
                Button1 = new ModalDialogButtonDetails {Title = "OK"}
            };
            Show(details);
        }

        private static void CloseDialog()
        {
            View.gameObject.SetActive(false);
        }
    }

    public class ModalDialogDetails
    {
        public string DialogMessage { get; set; }
        public ModalDialogButtonDetails Button1 { get; set; }
        public ModalDialogButtonDetails Button2 { get; set; }
        public ModalDialogButtonDetails Button3 { get; set; }
    }

    public class ModalDialogButtonDetails
    {
        public string Title { get; set; }
        public Action Handler { get; set; }
    }
}