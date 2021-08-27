using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] slots;
    [SerializeField] private Text         goldText;

    private List<Item> curItems    = new List<Item>();
    private ItemType   currentType = ItemType.Equipment;
    private Player player;


    private void Update() => Show();


    /// <summary>
    /// target�� �����۵��� ���Կ� �����Ѵ�.
    /// </summary>
    public void Show()
    {
        player = GameManager.Instance.Player;

        List<Item> allItems = player.Inventory.Load();

        curItems.Clear();
        curItems = allItems.FindAll(x => x.type == currentType);

        for ( int i=0; i<slots.Length; ++i )
        {
            bool isExist = i < curItems.Count;
            slots[i].SetActive(isExist);
            slots[i].GetComponent<InventorySlot>().Item = isExist ? curItems[i] : null;
        }

        goldText.text = string.Format("{0:##,##0}", player.Inventory.Gold);
    }

    /// <summary>
    /// �� �޴� Ŭ���� �κ��丮 ����
    /// </summary>
    public void TabClick(int type) => currentType = (ItemType)type;
}
