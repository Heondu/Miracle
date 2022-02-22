using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public void PlaySFX(AudioClip clip)
    {
        SoundManager.PlaySFX(clip);
    }
}
