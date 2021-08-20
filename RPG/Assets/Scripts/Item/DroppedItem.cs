using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [SerializeField] private string itemName;   // �̸�
    [SerializeField] private int    count;      // ����
    [SerializeField] private string desc;       // ����
    [SerializeField] private int    itemID;     // ID
    [SerializeField] private int    itemType;   // Ÿ��

    public int ID => itemID;

    private void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("Player") )
        {
            print("get DroppedItem: " + itemName);

            other.GetComponent<Player>().Inventory.AddItem(new Item(itemName, count, desc, itemID, itemType));
            gameObject.SetActive(false);
        }
    }
}
