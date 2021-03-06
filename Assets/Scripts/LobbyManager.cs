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
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;
        [SerializeField]
        private GameObject onJoinPanel;
        [SerializeField]
        private GameObject createRoomPanel;
        [SerializeField]
        private TMP_InputField cRInput;
        [SerializeField]
        private Button cRButton;
        [SerializeField]
        private Button rDButton;
        [SerializeField]
        private GameObject passwordPanel;

        #endregion

        #region Public Fields

        public TMP_InputField txtRoomName;
        public TMP_InputField txtPswd;
        public TMP_InputField txtPassword;
        public TMP_Dropdown dropdown;
        public TMP_Dropdown gameModeDropdown;
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
            onJoinPanel.SetActive(true);

            progressLabel.SetActive(false);
            passwordPanel.SetActive(false);
            createRoomPanel.SetActive(false);

            cRButton.interactable = false;
            rDButton.interactable = false;
    }

    #endregion

    #region Public Methods

        public void IsNameInput()
        {
            if (cRInput.text != "")
                cRButton.interactable = true;
            else
                cRButton.interactable = false;
        }

        public void OnCreateRoomClick()
        {
            progressLabel.SetActive(false);
            passwordPanel.SetActive(false);
            createRoomPanel.SetActive(true);
        }

        public void OnCreateClick()
        {
            string roomName = txtRoomName.text;
            string room;

            RoomOptions ros = new RoomOptions();

            ros.MaxPlayers = (byte)(Mathf.Pow(2, dropdown.value + 1));
            ros.IsVisible = true;

            string passwordFlag = "0";
            if (txtPswd.text != "")
            {
                roomName += "*" + txtPswd.text;
                passwordFlag = "1";
            }

            if (mapDropdown.options[mapDropdown.value].text == "Random")
            {
                int i = Random.Range(1, mapDropdown.options.Count);
                room = mapDropdown.options[i].text;
            }
            else
                room = mapDropdown.options[mapDropdown.value].text;

            ros.CustomRoomProperties = new Hashtable()
                {
                    { "pwd", passwordFlag },
                    { "gameMode", gameModeDropdown.options[gameModeDropdown.value].text },
                    { "room", room }
                };
            ros.CustomRoomPropertiesForLobby = new string[] { "pwd", "gameMode", "room" };

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ROOM"))
            {
                if (roomName == obj.GetComponent<Room>().roomName)
                {
                    progressLabel.SetActive(false);
                    passwordPanel.SetActive(false);
                    createRoomPanel.SetActive(true);
                    
                    return;
                }
            }

            PhotonNetwork.CreateRoom(roomName, ros);

            Debug.Log(ros.CustomRoomProperties.Values);
        }

        public void OnCreateFailedClick()
        {
            progressLabel.SetActive(false);
            passwordPanel.SetActive(false);
            createRoomPanel.SetActive(true);
        }

        public void OnCancelClick()
        {
            progressLabel.SetActive(false);
            passwordPanel.SetActive(false);
            createRoomPanel.SetActive(false);
        }

        public void OnPswdClick()
        {
            Debug.Log("1 " + (pwdRoom.Split('*'))[1] + "2" + txtPassword.text);
            if ((pwdRoom.Split('*'))[1] != txtPassword.text)
            {
                progressLabel.SetActive(false);
                passwordPanel.SetActive(false);
                createRoomPanel.SetActive(false);
            }
            else
                PhotonNetwork.JoinRoom(pwdRoom, null);
        }

        public void OnJoinRandomRoomClick()
        {
            Hashtable ros = new Hashtable() { { "pwd", "0" } };
            PhotonNetwork.JoinRandomRoom(ros, 0);
        }

        public void Connect()//커넥팅 창 표시 함수.
        {
            isConnecting = true;

            progressLabel.SetActive(true);
            //passwordPanel.SetActive(false);
            //crFaileddPanel.SetActive(false);
            //createRoomPanel.SetActive(false);
            //controlPanel.SetActive(false);
        }

        #endregion

        #region

        void OnClickRoom(string roomName)//여기서 새로운 팝업창 띄워서 비밀번호 검사.
        {
            if (-1!=roomName.IndexOf("*"))
            {
                //progressLabel.SetActive(false);
                passwordPanel.SetActive(true);
                //crFaileddPanel.SetActive(false);
                //createRoomPanel.SetActive(false);
                //controlPanel.SetActive(true);
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

            onJoinPanel.SetActive(false);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach(RoomInfo roomInfo in roomList)
            {
                if (roomInfo.PlayerCount <= 0)
                {
                    if(GameObject.FindGameObjectsWithTag("ROOM").Length <= 1)
                        rDButton.interactable = false;
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
                    rDButton.interactable = true;

                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ROOM"))
                    {
                        if (roomInfo.Name == obj.GetComponent<Room>().roomName)
                            return;
                    }
                    GameObject _room = Instantiate(room, gridTr);
                    Room roomData = _room.GetComponent<Room>();

                    Hashtable cp = roomInfo.CustomProperties;

                    Debug.Log(cp["gameMode"].ToString());

                    roomData.roomName = roomInfo.Name;
                    roomData.roomMode = cp["gameMode"].ToString();
                    roomData.roomMap = cp["room"].ToString();
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
            SceneManager.LoadScene(0);

            Debug.LogWarningFormat("Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnLeftLobby()
        {
            SceneManager.LoadScene(0);
            Debug.Log("LeftLobby");
        }

        public void LeftLobby()
        {
            PhotonNetwork.Disconnect();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("JoinFailed");
            /*
            bool name = true;
            string roomName = "Room of truth";
            int i = 0;

            Debug.Log("Launcher: OnJoinRandomFailed() was called by PUN. No random room abailable, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            RoomOptions ros = new RoomOptions();

            ros.MaxPlayers = maxPlayersPerRoom;
            ros.IsVisible = true;
            ros.CustomRoomProperties = new Hashtable() { { "pwd", "0" }, { "room", mapDropdown.options[Random.Range(1, mapDropdown.options.Count)].text } };

            while (name)
            {

                name = false;

                roomName = "Room of truth " + i;

                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ROOM"))
                {
                    if (roomName == obj.GetComponent<Room>().roomName)
                    {
                        i++;

                        name = true;
                    }
                }
            }

            PhotonNetwork.CreateRoom(roomName, ros);//동일한 이름이 생성이 안되면 다시 시도.
            */
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

            Debug.Log("Finish MakeRoom");
        }

        #endregion
    }
}