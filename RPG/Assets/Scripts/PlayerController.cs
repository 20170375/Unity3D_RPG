using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player    player;
    private Transform cameraTransform;

    [SerializeField]
    private KeyCode   jumpKeyCode = KeyCode.Space;

    private void Awake()
    {
        player = GetComponent<Player>();
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        // ����Ű�� ���� �̵�
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // �̵� �Լ� ȣ�� (ī�޶� �����ִ� ������ �������� ����Ű�� ���� �̵�)
        player.MoveTo(cameraTransform.rotation * new Vector3(x, 0, z));
        // ȸ�� ���� (�׻� �ո� ������ ĳ������ ȸ���� ī�޶�� ���� ȸ�� ������ ����)
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

        // SpaceŰ�� ������ ����
        if ( Input.GetKeyDown(jumpKeyCode) )
        {
            player.JumpTo();
        }
    }
}
