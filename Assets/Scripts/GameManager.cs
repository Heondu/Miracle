using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace OnionBagel.PcGame.Miracle
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Public Variables region

        public GameObject playerPrefab;
        public EGameMode GameMode;

        public Text msgList;
        public Scrollbar vert;
        public InputField ifSendMsg;
        public Text playerCount;

        public int max_chat_log = 100;
        private int chat_log;

        #endregion

        #region Photon Callbacks

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(1);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            if(PhotonNetwork.IsMasterClient)//생각 해보기
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

                //LoadArena();
            }

            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);
            CheckPlayerCount();

            string msg = string.Format("\n<color=#00ff00>[{0}]님이 입장했습니다.</color>", other.NickName);

            photonView.RPC("ReceiveMsg", RpcTarget.Others, msg);

            ReceiveMsg(msg);
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
            }

            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);

            CheckPlayerCount();

            string msg = string.Format("\n<color=#ff0000>[{0}]님이 퇴장했습니다.</color>", other.NickName);

            photonView.RPC("ReceiveMsg", RpcTarget.Others, msg);

            ReceiveMsg(msg);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            SceneManager.LoadScene(0);

            Debug.LogWarningFormat("Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        #endregion

        #region Public Field

        public static GameManager Instance;

        #endregion

        #region Public Methods

        public void OnSendChatMsg()
        {
            string msg = string.Format("[{0}] {1}", PhotonNetwork.LocalPlayer.NickName, ifSendMsg.text);
            photonView.RPC("ReceiveMsg", RpcTarget.Others, msg);
            ReceiveMsg(msg);

            ifSendMsg.text = "";
            //ifSendMsg.ActivateInputField();
        }

        [PunRPC]
        void ReceiveMsg(string msg)
        {
            msgList.text += "\n" + msg;
            vert.value = 0;

            chat_log++;
            if (chat_log > max_chat_log)
                msgList.text = msgList.text.Substring(msgList.text.IndexOf("\n") + 1);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region Private Methods

        void Start()
        {
            Instance = this;
            chat_log = 0;

            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
            }
            else
            {
                if(Entity.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene().name);
                    GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                    CheckGameMode(player.GetComponent<Entity>());
                    UIManager.Instance.CheckGameMode(GameMode);
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }

            PhotonNetwork.IsMessageQueueRunning = true;
            Invoke("CheckPlayerCount", 0.5f);
        }

        void CheckPlayerCount()
        {
            int currPlayer = PhotonNetwork.PlayerList.Length;
            int maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;
            playerCount.text = String.Format("[{0} / {1}]", currPlayer, maxPlayer);
        }

        void CheckGameMode(Entity player)
        {
            GameMode[] gameModes = GetComponentsInChildren<GameMode>();
            foreach (GameMode gm in gameModes)
            {
                if (gm.gameMode == GameMode)
                {
                    gm.Init(player);
                }
                else
                {
                    gm.gameObject.SetActive(false);
                }

            }
        }
        
        #endregion
    }
}