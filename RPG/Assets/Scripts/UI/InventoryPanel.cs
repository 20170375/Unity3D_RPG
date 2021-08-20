using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] slots;

    private List<Item> curItems    = new List<Item>();
    private ItemType   currentType = ItemType.weapon;
    private Player     target;

    /// <summary>
    /// ���Կ� �����۵��� �����Ѵ�.
    /// </summary>
    public void SlotUpdate(Player _target)
    {
        target = _target;

        List<Item> allItems = target.Inventory.Load();

        curItems.Clear();
        curItems = allItems.FindAll(x => x.type == (int)currentType);

        for (int i = 0; i < slots.Length; ++i)
        {
            bool isExist = i < curItems.Count;
            slots[i].SetActive(isExist);
            slots[i].GetComponent<Slot>().Item = isExist ? curItems[i] : null;
        }
    }

    /// <summary>
    /// �� �޴� Ŭ���� �κ��丮 ����
    /// </summary>
    public void TabClick(int type)
    {
        if ( currentType != (ItemType)type )
        {
            currentType = (ItemType)type;

            SlotUpdate(target);
        }
    }
}
