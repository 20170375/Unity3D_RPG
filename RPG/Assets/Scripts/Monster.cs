using UnityEngine;

public class Monster : Character
{
    [SerializeField]
    private float dropExp = 50.0f;  // ��� ����ġ

    private void Awake()
    {
        movement3D = GetComponent<Movement3D>();
    }
}
