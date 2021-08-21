using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ItemType { Equipment=0, Consumable, Material }

[System.Serializable]
public class Item
{
    public string   name;   // �̸�
    public int      count;  // ����
    public string   desc;   // ����
    public int      id;     // ID
    public ItemType type;   // Ÿ��

    public Item(string _name, int _count, string _desc, int _id, ItemType _type)
    {
        name  = _name;
        count = _count;
        desc  = _desc;
        id    = _id;
        type  = _type;
    }

    public Item(Item target)
        : this(target.name, target.count, target.desc, target.id, target.type) { }
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Object/ItemSO")]
public class ItemSO : ScriptableObject
{
    [SerializeField] private List<Item> items;

    public List<Item> Items { get => items; }
}
