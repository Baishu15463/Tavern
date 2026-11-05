using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewOnItemChangeSO", menuName = "Tavern/Events/OnItemChangeSO")]
public class OnItemChangeSO : ScriptableObject
{
    public event Action<ItemData> OnEventRaised;
    public void RaiseEvent(ItemData item) => OnEventRaised?.Invoke(item);
}