using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private bool isLeftHandUpEndAnim = false;
    private bool isRightHandUpEndAnim = false;

    public void LeftHandUpStartAnim()
    {
        isLeftHandUpEndAnim = false;
    }

    public void RightHandUpStartAnim()
    {
        isRightHandUpEndAnim = false;
    }

    public void LeftHandUpEndAnim()
    {
        isLeftHandUpEndAnim = true;
    }

    public void RightHandUpEndAnim()
    {
        isRightHandUpEndAnim = true;
    }

    public void PlayLeftHandAttackSFX(AudioClip clip)
    {
        if (isLeftHandUpEndAnim == false)
            SoundManager.PlaySFX(clip);
    }

    public void PlayRightHandAttackSFX(AudioClip clip)
    {
        if (isRightHandUpEndAnim == false)
            SoundManager.PlaySFX(clip);
    }
}
