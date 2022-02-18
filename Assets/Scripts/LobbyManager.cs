using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
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
        private byte maxPlayersPerRoom = 8;
        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;
        [SerializeField]
        private GameObject createRoomPanel;
        [SerializeField]
        private GameObject passwordPanel;

        #endregion

        #region Public Fields

        public TMP_InputField txtRoomName;
        public TMP_InputField txtPswd;
        public TMP_InputField txtPassword;
        public TMP_Dropdown dropdown;
        public TMP_Dropdown mapDropdown;

        public GameObject room;
        public Transform gridTr;//필요없는거 같으면 지우기

        public string pwdRoom;

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
            passwordPanel.SetActive(false);
            createRoomPanel.SetActive(false);
            controlPanel.SetActive(true);
        }

        #endregion

        #region Public Methods

        public void OnCreateRoomClick()
        {
            progressLabel.SetActive(false);
            passwordPanel.SetActive(false);
            createRoomPanel.SetActive(true);
            controlPanel.SetActive(true);
        }

        public void OnCreateClick()
        {
            string roomName = txtRoomName.text;

            RoomOptions ros = new RoomOptions();

            ros.MaxPlayers = (byte)(Mathf.Pow(2, dropdown.value + 1));
            ros.IsVisible = true;

            if (txtPswd.text != null)
            {
                roomName += "*" + txtPswd.text;
                ros.CustomRoomProperties = new Hashtable() { { "pwd", "1"}, { "room", mapDropdown.options[mapDropdown.value].text} };
            }
            else
            {
                ros.CustomRoomProperties = new Hashtable() { { "pwd", "0" }, { "room", mapDropdown.options[mapDropdown.value].text } };
            }
            PhotonNetwork.CreateRoom(roomName, ros);
            Debug.Log(ros.CustomRoomProperties.Values);
        }

        public void OnCancelClick()
        {
            progressLabel.SetActive(false);
            passwordPanel.SetActive(false);
            createRoomPanel.SetActive(false);
            controlPanel.SetActive(true);
        }

        public void OnPswdClick()
        {
            Debug.Log("1 " + (pwdRoom.Split('*'))[1] + "2" + txtPassword.text);
            if ((pwdRoom.Split('*'))[1] != txtPassword.text)
            {
                progressLabel.SetActive(false);
                passwordPanel.SetActive(false);
                createRoomPanel.SetActive(false);
                controlPanel.SetActive(true);
            }
            else
                PhotonNetwork.JoinRoom(pwdRoom, null);
        }

        public void OnJoinRandomRoomClick()
        {
            Hashtable ros = new Hashtable() { { "pwd", "0" } };
            PhotonNetwork.JoinRandomRoom(ros, 0);//랜덤 설정 막아놓던지 프로퍼티 설정하던지.
        }

        public void Connect()//커넥팅 창 표시 함수.
        {
            isConnecting = true;

            progressLabel.SetActive(true);
            passwordPanel.SetActive(false);
            createRoomPanel.SetActive(false);
            controlPanel.SetActive(false);
        }

        #endregion

        #region

        void OnClickRoom(string roomName)//여기서 새로운 팝업창 띄워서 비밀번호 검사.
        {
            if (-1!=roomName.IndexOf("*"))
            {
                progressLabel.SetActive(false);
                passwordPanel.SetActive(true);
                createRoomPanel.SetActive(false);
                controlPanel.SetActive(true);
                pwdRoom = roomName;
            }
            else
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
            foreach(RoomInfo roomInfo in roomList)
            {
                if (roomInfo.PlayerCount <= 0)
                {
                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ROOM"))
                    {
                        if (roomInfo.Name == obj.GetComponent<Room>().roomName)
                        {
                            Destroy(obj);
                            Debug.Log(obj.name);
                        }
                    }
                }
                else
                {
                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ROOM"))
                    {
                        if (roomInfo.Name == obj.GetComponent<Room>().roomName)
                            return;
                    }
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
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            passwordPanel.SetActive(false);
            createRoomPanel.SetActive(false);
            controlPanel.SetActive(true);

            Debug.LogWarningFormat("Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Launcher: OnJoinRandomFailed() was called by PUN. No random room abailable, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            RoomOptions ros = new RoomOptions();

            ros.MaxPlayers = maxPlayersPerRoom;
            ros.IsVisible = true;
            ros.CustomRoomProperties = new Hashtable() { { "pwd", "0" }, { "room", mapDropdown.options[Random.Range(0, mapDropdown.options.Count)].text } };

            PhotonNetwork.CreateRoom(null, ros);
        }

        public override void OnJoinedRoom()
        {
            Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
            Debug.Log("Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            //포톤네트워크의 데이터 통신을 잠시 정지 시킴.
            //플레이어 오브젝트가 생성되면 다시 연결.
            PhotonNetwork.IsMessageQueueRunning = false;

            if (!PhotonNetwork.IsMasterClient)//오류 나면 생각
                return;
            Debug.Log(cp["room"].ToString());
            PhotonNetwork.LoadLevel(cp["room"].ToString());
            Debug.Log("sk");
        }

        #endregion
    }
}