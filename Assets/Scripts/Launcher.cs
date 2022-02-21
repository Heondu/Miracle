using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace OnionBagel.PcGame.Miracle
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [SerializeField]
        private TMP_InputField nameInputField;
        [SerializeField]
        private Button playButton;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        #endregion

        #region Private Fields

        bool isConnecting;

        #endregion

        #region MonoBehaviour CallBacks

        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            IsNameInput();
        }

        #endregion

        #region Public Methods

        public void IsNameInput()
        {
            if(nameInputField.text != "")
                playButton.interactable = true;
            else
                playButton.interactable = false;
        }

        public void Play()
        {
            SceneManager.LoadScene("Lobby");
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);

            Debug.LogWarningFormat("Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        #endregion
    }
}