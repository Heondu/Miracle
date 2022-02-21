using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

namespace OnionBagel.PcGame.Miracle
{
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants

        const string playerNamePrefKey = "PlayerName";

        #endregion

        #region MonoBehaviour CallBacks
        
        void Start()
        {
            string defaultName = string.Empty;
            TMP_InputField inputField = GetComponent<TMP_InputField>();
            if (inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }

        #endregion

        #region Public Methods

        public void SetPlayerName(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                Debug.Log("Player Name is null or empty");

                return;
            }

            PhotonNetwork.NickName = value;

            PlayerPrefs.SetString(playerNamePrefKey, value);
        }

        #endregion
    }
}