using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CharacterType { NPC, Player, Monster, }

public class Character : MonoBehaviour
{
    protected Movement movement;
    protected Animator animator;
    protected Canvas   canvas;

    [Header("Info")]
    [SerializeField] private   CharacterType type;      // Ÿ��
    [SerializeField] protected string characterName;    // ĳ���͸�
    [SerializeField] protected Text   nameText;         // ĳ���� �̸� Text

    protected bool canMove;   // �̵� ���� ����

    public CharacterType Type { get => type; }
    public string Name => characterName;

 
    private void Update()
    {
        if ( transform.position.y < 0 ) { Die(); }
    }

    protected virtual void OnEnable()
    {
        movement = GetComponent<Movement>();
        animator = GetComponent<Animator>();
        canvas   = GetComponentInChildren<Canvas>();

        // ĳ���� ���� ����
        nameText.text = characterName;

        // �̵� ����
        canMove = true;
    }


    /// <summary>
    /// ���� ���� ����
    /// </summary>
    public void MoveTo(Vector3 direction)
    {
        if ( !canMove )
        {
            movement.MoveTo(Vector3.zero);
            GetComponent<Animator>().SetBool("Walk", false);
            GetComponent<Animator>().ResetTrigger("Jump");
            return;
        }

        movement.MoveTo(direction);
        if ( direction != Vector3.zero )
        {
            GetComponent<Animator>().SetBool("Walk", true);
            GetComponent<Animator>().ResetTrigger("Jump");
        }
        else
        {
            GetComponent<Animator>().SetBool("Walk", false);
        }
    }

    /// <summary>
    /// ĳ���� ����
    /// </summary>
    public void JumpTo()
    {
        if ( movement.JumpTo() ) { GetComponent<Animator>().SetTrigger("Jump"); }
    }

    /// <summary>
    /// ĳ���� ��� (Animation���� ȣ��)
    /// </summary>
    protected virtual void Die() => gameObject.SetActive(false);

    /// <summary>
    /// ����/����� �̵� �Ұ� (Animation���� ȣ��)
    /// </summary>
    public void EnableMove()  => canMove = true;
    public void DisableMove() => canMove = false;
}
