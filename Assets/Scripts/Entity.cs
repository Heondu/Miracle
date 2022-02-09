using UnityEngine;

public class Entity : MonoBehaviour
{
    private Status status;
    public Status Status => status;
    [SerializeField] private HPViewer hpViewer;

    private void Awake()
    {
        status = GetComponent<Status>();
        status.onModifierUpdate.AddListener(hpViewer.UpdateHP);
    }

    public void TakeDamage(float damage)
    {
        if (status.CurrentHP == 0)
            return;

        status.CurrentHP -= (int)damage;
    }
}
