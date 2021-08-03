using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject   slotPrefab;
    [SerializeField] private GameObject[] slots;

    private List<Item> allItems    = new List<Item>();
    private List<Item> curItems    = new List<Item>();
    private ItemType   currentType = ItemType.weapon;

    private string     filePath;

    private void Awake() => filePath = Application.dataPath + "/Resources/inventory.txt";

    private void OnEnable() => Load();

    /// <summary>
    /// ���Կ� �����۵��� �����Ѵ�.
    /// </summary>
    private void SlotUpdate()
    {
        curItems.Clear();
        curItems = allItems.FindAll(x => x.type == (int)currentType);

        for ( int i=0; i<slots.Length; ++i )
        {
            bool isExist = i < curItems.Count;
            slots[i].SetActive(isExist);
            slots[i].GetComponent<Slot>().Item = isExist ? curItems[i] : null;
        }
    }

    /// <summary>
    /// ���Ϸκ��� �κ��丮 ������ �о�´�.
    /// </summary>
    private void Load()
    {
        allItems.Clear();

        if ( !File.Exists(filePath) )
        {
            allItems.Add(new Item("ü�¹���",   100,  "�Һ�� ü���� 100 ȸ���Ѵ�.",  0, ItemType.consumable));
            allItems.Add(new Item("��������",   10,   "�Һ�� ������ 50 ȸ���Ѵ�.",   1, ItemType.consumable));
            allItems.Add(new Item("����ġ����", 2,    "�Һ�� ����ġ�� 10 ȹ���Ѵ�.", 2, ItemType.consumable));
            allItems.Add(new Item("�׾Ƹ�",     1000, "������ ����µ� �ʿ��� ���",  3, ItemType.material));
            Save();
        }
        //string code     = File.ReadAllText(filePath);
        //byte[] bytes    = System.Convert.FromBase64String(code);
        //string jsonData = System.Text.Encoding.UTF8.GetString(bytes);
        string jsonData = File.ReadAllText(filePath);
        allItems          = JsonUtility.FromJson<Serialization<Item>>(jsonData).target;

        SlotUpdate();
    }

    /// <summary>
    /// �κ��丮 ������ ���Ϸ� �����Ѵ�.
    /// </summary>
    private void Save()
    {
        string jsonData = JsonUtility.ToJson(new Serialization<Item>(allItems));
        //byte[] bytes    = System.Text.Encoding.UTF8.GetBytes(jsonData);
        //string code     = System.Convert.ToBase64String(bytes);
        //File.WriteAllText(filePath, code);
        File.WriteAllText(filePath, jsonData);
    }

    public void TabClick(int type)
    {
        if ( currentType == (ItemType)type ) { return; }

        currentType = (ItemType)type;

        SlotUpdate();
    }
}
