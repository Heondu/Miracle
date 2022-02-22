using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip bgmClip;

    private void Start()
    {
        SoundManager.PlayBGM(bgmClip);
    }
}
