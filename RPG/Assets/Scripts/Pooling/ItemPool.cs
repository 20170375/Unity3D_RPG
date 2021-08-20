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
    public void DropItem(DroppedItem item, Vector3 position)
    {
        GameObject target = items.Find(x => (!x.activeSelf) && (x.GetComponent<DroppedItem>().ID == item.ID));
        if ( target != null )
        {
            target.transform.position = position;
            target.SetActive(true);
        }
    }
}
