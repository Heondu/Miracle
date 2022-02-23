using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class SandboxMode : GameMode
{
    Entity localPlayer;

    public GameObject Block1;
    public GameObject Block2;
    public GameObject Block3;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            PhotonNetwork.Instantiate(Block1.name, localPlayer.Root.position, Quaternion.identity);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            PhotonNetwork.Instantiate(Block2.name, localPlayer.Root.position, Quaternion.identity);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            PhotonNetwork.Instantiate(Block3.name, localPlayer.Root.position, Quaternion.identity);

    }

    public override void Init(Entity player)
    {
        base.Init(player);
        player.onDeath.AddListener(OnPlayerDeath);
        if (player.GetComponent<PhotonView>().IsMine)
            localPlayer = player;
    }

    private void OnPlayerDeath(Entity player)
    {
        player.Setup();
    }
}
