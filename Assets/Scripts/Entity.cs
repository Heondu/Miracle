using UnityEngine;
using ExitGames.Client.Photon;
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

    private static short statusSz = sizeof(float)*1;
    public static readonly byte[] memStatus = new byte[statusSz];
    private static short SerializeStatus(StreamBuffer outstream, object customobject)
    {
        Status st = (Status)customobject;
        lock(memStatus)
        {
            byte[] bytes = memStatus;
            int index = 0;
            Protocol.Serialize(st.CurrentHP, bytes, ref index);

            outstream.Write(bytes, 0, statusSz);
        }
        

        return statusSz;
    }

    private static object DeserializeStatus(StreamBuffer inStream, short length)
    {
        /*
        Status st = null;
        lock(memStatus)
        {
            inStream.Read(memStatus, 0, statusSz);
            int index = 0;
            int hp;
            Protocol.Deserialize(out hp, memStatus, ref index);
            st.CurrentHP = hp;
        }

        return st;
        */

        float CurrentHP;
        lock (memStatus)
        {
            inStream.Read(memStatus, 0, statusSz);
            int index = 0;
            Protocol.Deserialize(out CurrentHP, memStatus, ref index);
        }

        return CurrentHP;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(status);//직렬화 해놓기
            Debug.Log("Send" + this.status.CurrentHP);
        }
        else
        {
            //this.status=(Status)stream.ReceiveNext();//참고
            this.status.CurrentHP = (float)stream.ReceiveNext();//임시
            Debug.Log(this.status.CurrentHP);
        }
    }


    #endregion

    private void Awake()
    {
        status = GetComponent<Status>();
        status.onModifierUpdate.AddListener(hpViewer.UpdateHP);

        PhotonPeer.RegisterType(typeof(Status), 1, SerializeStatus, DeserializeStatus);

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