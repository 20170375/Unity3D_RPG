using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    [SerializeField] protected int    attackDamage; // ���ݷ�
    [SerializeField] protected float  attackRange;  // ���� ����
    [SerializeField] protected float  attackSpeed;  // ���� �ӵ�
    [SerializeField] protected float  critical;     // ġ��Ÿ Ȯ��

    public int   Atk         => attackDamage;
    public float AttackRange => attackRange;
    public float AttackSpeed => attackSpeed;
    public float Cri         => critical;
}
