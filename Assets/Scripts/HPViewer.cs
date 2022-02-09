using UnityEngine;
using UnityEngine.UI;

public class HPViewer : MonoBehaviour
{
    [Header("Image")]
    [SerializeField] private Image fillImage;
    [SerializeField] private Image backgroundImage;
    [Header("Gradient")]
    [SerializeField] private Gradient fillGradient;
    [SerializeField] private Gradient backgroundGradient;

    public void Start()
    {
        fillImage.color = fillGradient.Evaluate(1f);
        backgroundImage.color = backgroundGradient.Evaluate(1f);
    }

    public void UpdateHP(Status status, StatusType statusType, float currentValue, float pervValue)
    {
        if (statusType == StatusType.CurrentHP)
        {
            float value = currentValue / status.MaxHP;
            fillImage.fillAmount = value;
            fillImage.color = fillGradient.Evaluate(value);
            backgroundImage.color = backgroundGradient.Evaluate(value);
        }
    }
}
