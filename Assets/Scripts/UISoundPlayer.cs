using UnityEngine;
using UnityEngine.EventSystems;

public enum UISoundPlayType
{
    Always,
    PointerOverGameObject,
}

public class UISoundPlayer : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private UISoundPlayType playType = UISoundPlayType.Always;

    private void Update()
    {
        if (playType == UISoundPlayType.Always && Input.GetMouseButtonDown(0))
        {
            SoundManager.PlaySFX(clip);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (playType == UISoundPlayType.PointerOverGameObject)
        {
            SoundManager.PlaySFX(clip);
        }
    }
}
