using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private ItemSO ItemSO;

    [SerializeField] protected string     weaponName;   // ���� �̸�
    [SerializeField] protected int        attackDamage; // ���ݷ�
    [SerializeField] protected float      attackRange;  // ���� ����
    [SerializeField] protected float      attackSpeed;  // ���� �ӵ�
    [SerializeField] protected float      critical;     // ġ��Ÿ Ȯ��

    public int ID => ItemSO.Items.Find(x => x.name == weaponName).id;

    public string     WeaponName  => weaponName;
    public int        Atk         => attackDamage;
    public float      AttackRange => attackRange;
    public float      AttackSpeed => attackSpeed;
    public float      Cri         => critical;
}
