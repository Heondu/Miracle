using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace OnionBagel.PcGame.Miracle
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 20;
        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        #endregion

        #region Public Fields

        public TMP_InputField txtRoomName;

        public GameObject room;
        public Transform gridTr;//필요없는거 같으면 지우기

        #endregion

        #region Private Fields

        string gameVersion = "1";

        bool isConnecting;

        #endregion

        #region MonoBehaviour CallBacks

        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

        void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        #endregion

        #region Public Methods

        public void OnCreateRoomClick()
        {
            PhotonNetwork.CreateRoom(txtRoomName.text, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });//잘 조정하면 방인원 설정 ㄱㄴ
        }

        public void OnJoinRandomRoomClick()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public void Connect()//커넥팅 창 표시 함수.
        {
            isConnecting = true;

            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
        }

        #endregion

        #region

        void OnClickRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName, null);
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("Launcher: OnConnectedToMaster() was called by PUN");

            PhotonNetwork.JoinLobby();

            /*
            if(isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            */
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Joined Lobby");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("ROOM"))//태그 지우고 자식들 전부 삭제로 할지 생각.
            {
                Destroy(obj);
            }
            foreach(RoomInfo roomInfo in roomList)
            {
                GameObject _room = Instantiate(room, gridTr);
                Room roomData = _room.GetComponent<Room>();
                roomData.roomName = roomInfo.Name;
                roomData.maxPlayer = roomInfo.MaxPlayers;
                roomData.playerCount = roomInfo.PlayerCount;
                roomData.UpdateInfo();
                roomData.GetComponent<Button>().onClick.AddListener
                (
                    delegate
                    {
                        OnClickRoom(roomData.roomName);
                    }

                );
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);

            Debug.LogWarningFormat("Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Launcher: OnJoinRandomFailed() was called by PUN. No random room abailable, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = maxPlayersPerRoom});
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");


            //포톤네트워크의 데이터 통신을 잠시 정지 시킴.
            //플레이어 오브젝트가 생성되면 다시 연결.
            PhotonNetwork.IsMessageQueueRunning = false;

            PhotonNetwork.LoadLevel("Room for 1");
        }

        #endregion
    }
}