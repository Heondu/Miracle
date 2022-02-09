using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Modifier
{
    public StatusType Type;
    public float Value;
}

public enum StatusType
{
    MaxHP,
    CurrentHP,
    AttackDamage
}

public class Status : MonoBehaviour
{
    [SerializeField]
    private Modifier[] modifiers;

    [Header("Event")]
    [HideInInspector] public UnityEvent<Status, StatusType, float, float> onModifierUpdate;

    public float MaxHP => GetModifier(StatusType.MaxHP).Value;
    public float CurrentHP
    {
        get => GetModifier(StatusType.CurrentHP).Value;
        set => SetValue(StatusType.CurrentHP, Mathf.Clamp(value, 0f, MaxHP));
    }

    public float GetValue(StatusType type) => GetModifier(type).Value;

    public void SetValue(StatusType type, float value)
    {
        Modifier modifier = GetModifier(type);
        float prevValue = modifier.Value;
        modifier.Value = value;

        onModifierUpdate.Invoke(this, type, value, prevValue);
    }

    public Modifier GetModifier(StatusType type) => modifiers.First(x => x.Type == type);
}
