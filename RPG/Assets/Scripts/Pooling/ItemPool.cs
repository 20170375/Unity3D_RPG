using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    public static ItemPool Instance { private set; get; }
    private void Awake() => Instance = this;

    [SerializeField] private List<GameObject> items;

    /// <summary>
    /// �ش� ��ġ�� Item�� ����Ѵ�.
    /// </summary>
    public void DropItem(DroppedItem droppedItem, Vector3 position)
    {
        foreach ( GameObject item in items)
        {
            if ( !item.activeSelf && (item.GetComponent<DroppedItem>().ID == droppedItem.ID) )
            {
                item.transform.position = position;
                item.SetActive(true);
                return;
            }
        }
    }
}
