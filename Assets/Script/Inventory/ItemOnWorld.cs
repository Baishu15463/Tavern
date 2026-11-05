using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public ItemData thisItem;
    public OnItemChangeSO onItemGained;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (thisItem != null && onItemGained != null)
            {
                onItemGained.RaiseEvent(thisItem);
            }
            Destroy(gameObject);
        }
    }
}