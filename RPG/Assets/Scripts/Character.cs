using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected Movement3D movement3D;

    [SerializeField]
    private int     level;  // ����
    [SerializeField]
    private float   hp;     // ü��

    public float Hp
    {
        set => hp = Mathf.Max(0, value);
        get => hp;
    }

    private void Awake()
    {
        movement3D = GetComponent<Movement3D>();
    }

    public void MoveTo(Vector3 direction)
    {
        movement3D.MoveTo(direction);
    }

    public void JumpTo()
    {
        movement3D.JumpTo();
    }
}
