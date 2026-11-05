using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewOnInventoryChangeSO", menuName = "Tavern/Events/OnInventoryChangeSO")]
public class OnInventoryChangeSO : ScriptableObject
{
    public event Action OnEventRaised;
    public void RaiseEvent() => OnEventRaised?.Invoke();
}