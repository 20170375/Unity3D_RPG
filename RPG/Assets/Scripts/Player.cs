using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField]
    private float  currentExp;  // ���� ����ġ

    private Weapon weapon;

    private void Awake()
    {
        movement3D = GetComponent<Movement3D>();

        // �⺻ ����
        gameObject.AddComponent<Weapon>();
        weapon = GetComponent<Weapon>();
    }

    private void Update()
    {
        
    }
}
