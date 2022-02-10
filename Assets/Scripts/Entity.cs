using UnityEngine;
using Photon.Pun;

public class Entity : MonoBehaviourPunCallbacks, IPunObservable
{
    private Status status;
    public Status Status => status;
    [SerializeField] private HPViewer hpViewer;

    #region

    public static GameObject LocalPlayerInstance;

    #endregion

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(status);
        }
        else
        {
            this.status=(Status)stream.ReceiveNext();
        }
    }


    #endregion

    private void Awake()
    {
        status = GetComponent<Status>();
        status.onModifierUpdate.AddListener(hpViewer.UpdateHP);

        if(photonView.IsMine)
        {
            Entity.LocalPlayerInstance = this.gameObject;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        #if UNITY_5_4_OR_NEWER
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        };
        #endif
    }

    #if !UNITY_5_4_OR_NEWER
    void OnLevelWasLoaded(int level)
    {
        this.CalledOnLevelWasLoaded(level);
    }
    #endif
    
    
    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
    }

    public void TakeDamage(float damage)
    {
        if (status.CurrentHP == 0)
        {
            OnionBagel.PcGame.Miracle.GameManager.Instance.LeaveRoom();//namespace해야함...
        }

        status.CurrentHP -= (int)damage;
    }
}