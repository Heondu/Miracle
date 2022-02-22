using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerChatView : MonoBehaviourPunCallbacks
{
    public GameObject ChatPrefab;
    public GameObject Chat_UI;
    public InputField ifSendMsg;
    public TMP_Text Chat;

    public int textspt = 12;

    bool isChat;
    float timer;
    float waitingTime = 3.0f;

    public void OnSendChatMsg(string text)
    {
        if(photonView.IsMine)
        {
            string msg = "";

            for (int i = 0; i < text.Length; i += textspt)
            {
                if (i + textspt >= text.Length)
                    msg += text.Substring(i, text.Length - i);
                else
                {
                    msg += text.Substring(i, textspt);
                    msg += "\n";
                }
            }
            photonView.RPC("SendChat", RpcTarget.Others, msg, photonView.ViewID);
            SendChat(msg, photonView.ViewID);
        }
    }

    void Awake()
    {
        ChatPrefab = GameObject.FindGameObjectWithTag("CHAT");
        ifSendMsg = ChatPrefab.GetComponent<InputField>();
        ifSendMsg.onEndEdit.AddListener(OnSendChatMsg);

        Chat_UI.SetActive(false);
    }

    private void Start()
    {
        timer = 0.0f;
        isChat = false;
    }

    void Update()
    {
        if (isChat)
        {
            timer += Time.deltaTime;

            if (timer > waitingTime)
            {
                Chat_UI.SetActive(false);

                isChat = false;
                timer = 0.0f;
            }
        }
    }

    [PunRPC]
    public void SendChat(string msg, int ID)
    {
        if (ID == photonView.ViewID)
        {
            isChat = true;
            timer = 0.0f;

            Chat_UI.SetActive(true);
            Chat.text = msg;
        }

    }
}
