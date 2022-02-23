using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class SandboxMode : GameMode
{
    public override void Init(Entity player)
    {
        base.Init(player);
        player.onDeath.AddListener(OnPlayerDeath);
    }

    private void OnPlayerDeath(Entity player)
    {
        player.Setup();
    }
}
