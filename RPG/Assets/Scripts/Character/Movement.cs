using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float orgMoveSpeed = 5.0f;   // �̵� �ӵ�
    [SerializeField] private float gravity   = -9.8f;     // �߷� ���
    [SerializeField] private float jumpForce = 2.0f;      // ���� ��
    private Vector3 moveDirection;  // �̵� ����
    private CharacterController characterController;

    public int   AddSpeed { set; private get; }
    public float MoveSpeed => orgMoveSpeed * (AddSpeed + 100) / 100;


    private void Awake() => characterController = GetComponent<CharacterController>();

    private void Update()
    {
        // �߷� ����. �÷��̾ ���� ��� ���� �ʴٸ�
        // y�� �̵����⿡ gravity*Time.deltaTime�� �����ش�.
        if ( characterController.isGrounded == false ) { moveDirection.y += gravity * Time.deltaTime; }

        // �̵� ����. CharacterController�� Move() �Լ��� �̿��� �̵�
        if ( characterController != null ) { characterController.Move(moveDirection * MoveSpeed * Time.deltaTime); }
    }


    public void MoveTo(Vector3 direction) => moveDirection = new Vector3(direction.x, moveDirection.y, direction.z);

    public bool JumpTo()
    {
        // ĳ���Ͱ� ���� ��� ������ ����
        if ( characterController.isGrounded == true )
        {
            moveDirection.y = jumpForce;
            return true;
        }

        return false;
    }
}
