using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerNickname : MonoBehaviourPun
{
    public TMP_Text nickname;

    void Start()
    {
        nickname.text = photonView.Owner.NickName;
    }
}
