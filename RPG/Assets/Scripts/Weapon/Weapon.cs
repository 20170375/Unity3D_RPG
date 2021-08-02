using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { None=0, Sword, }

public class Weapon
{
    public WeaponType WeaponType  { protected set; get; }   // ���� Ÿ��
    public float      Damage      { protected set; get; }   // ���ݷ�
    public float      AttackRange { protected set; get; }   // ���� ����
    public float      AttackSpeed { protected set; get; }   // ���� �ӵ�

}
