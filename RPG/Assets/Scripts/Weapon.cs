using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Hand=0, Sword, }

public class Weapon : MonoBehaviour
{
    [SerializeField] protected string     weaponName;   // ���� �̸�
    [SerializeField] protected WeaponType weaponType;   // ���� Ÿ��
    [SerializeField] protected float      damage;       // ���ݷ�
    [SerializeField] protected float      attackRange;  // ���� ����
    [SerializeField] protected float      attackSpeed;  // ���� �ӵ�

    public string     WeaponName  { get => weaponName; }
    public WeaponType WeaponType  { get => weaponType; }
    public float      Damage      { get => damage; }
    public float      AttackRange { get => attackRange; }
    public float      AttackSpeed { get => attackSpeed; }

}
