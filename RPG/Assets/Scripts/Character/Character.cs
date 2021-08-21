using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CharacterType { NPC, Player, Monster, }

public class Character : MonoBehaviour
{
    private Movement movement;
    private Animator animator;
    private Canvas   canvas;

    [Header("Info")]
    [SerializeField] private   CharacterType type;    // Ÿ��
    [SerializeField] private   int   level    = 1;    // ����
    [SerializeField] private   int   maxLevel = 99;   // �ִ� ����
    [SerializeField] protected float hp;              // ���� ü��
    [SerializeField] protected float maxHp = 100;     // �ִ� ü��

    [Header("Stat")]
    [SerializeField] protected int   atk;   // ���ݷ�
    [SerializeField] protected int   def;   // ����
    [SerializeField] protected int   spd;   // �̵��ӵ�
    [SerializeField] protected float cri;   // ġ��Ÿ Ȯ��
    protected Weapon weapon;    // ���� �������� Weapon
    //protected Helmet helmet;    // ���� �������� Helmet
    //protected Armor  armor;     // ���� �������� Armor
    //protected Shoes  shoes;     // ���� �������� Shoes

    [Header("Attack")]
    [SerializeField] private Transform  hand;            // ���� ���� ��ġ
    [SerializeField] private GameObject attackCollision; // ���ݽ� �浹������ ���� GameObject
    private bool canMove;   // �̵� ���� ����

    [Header("UI")]
    [SerializeField] private GameObject hpSliderPrefab;   // ü�� Slider UI ������
    [SerializeField] private GameObject levelTextPrefab;  // ���� Text UI ������
    [SerializeField] private Text DmgText;          // ������ ����Ʈ Text
    private Slider           hpSlider;  // ü�� Slider UI
    private TextMeshProUGUI  levelText; // ���� Text UI

    public CharacterType Type { get => type; }

    protected int   Level
    {
        set
        {
            level = Mathf.Clamp(value, 0, maxLevel);
            levelText.text = "Lv." + level.ToString();
        }
        get
        {
            return level;
        }
    }
    public    float Hp
    {
        set
        {
            if ( value < hp )
            {
                GameObject hitEffect = EffectPool.Instance.HitEffect();
                hitEffect.transform.position = transform.position;
                hitEffect.SetActive(true);
                DmgText.text = string.Format("{0:##,##0}", hp - value);
                DmgText.gameObject.SetActive(true);
            }

            hp = Mathf.Clamp(value, 0, MaxHp);
            hpSlider.value = Hp / MaxHp;

            if  ( Hp == 0 )
            {
                canMove = false;

                animator.Play("CharacterDeath");
            }
            else
            {
                animator.Play("CharacterHit");
            }
        }
        get
        {
            return hp;       
        }
    }
    public    float MaxHp { protected set => maxHp = value; get => maxHp; }

    public int   Atk { get => atk + weapon.Atk; }
    //public int   Def { get => def + helmet.Def + armor.Def; }
    public int   Def { get => def; }
    //public int   Spd { get => spd + shoes.Spd; }
    public int   Spd { get => spd; }
    public float Cri { get => cri + weapon.Cri; }

    public Weapon Weapon
    {
        set
        {
            if ( weapon ) { Destroy(weapon.gameObject); }
            weapon = value;

            // ���� ������ ���� �浹���� ����
            attackCollision.transform.localPosition = new Vector3(0, 0, Weapon.AttackRange);
            attackCollision.transform.localScale    = new Vector3(attackCollision.transform.localScale.x, Weapon.AttackRange, attackCollision.transform.localScale.z);
        }
        get
        {
            return weapon;
        }
    }
    /*
    public Helmet Helmet
    {
        set
        {
            if ( helmet ) { Destroy(helmet.gameObject); }
            helmet = value;
        }
        get
        {
            return helmet;
        }
    }
    public Armor  Armor
    {
        set
        {
            if ( armor ) { Destroy(armor.gameObject); }
            armor = value;
        }
        get
        {
            return armor;
        }
    }
    public Shoes  Shoes
    {
        set
        {
            if ( shoes ) { Destroy(shoes.gameObject); }
            shoes = value;

            // �̵��ӵ�
            movement.AddSpeed = Spd;
        }
        get
        {
            return shoes;
        }
    }*/


    private void Update()
    {
        if ( transform.position.y < 0 ) { Die(); }
    }

    protected virtual void OnEnable()
    {
        movement = GetComponent<Movement>();
        animator = GetComponent<Animator>();
        canvas   = GetComponentInChildren<Canvas>();

        // UI ���� - ü�� Slider UI
        GameObject hpSliderClone = Instantiate(hpSliderPrefab, canvas.transform);
        hpSlider = hpSliderClone.GetComponent<Slider>();

        // UI ���� - ���� Text UI
        GameObject levelTextClone = Instantiate(levelTextPrefab, canvas.transform);
        levelText = levelTextClone.GetComponent<TextMeshProUGUI>();

        // ĳ���� ���� ����
        Hp    = MaxHp;
        Level = level;

        // ���� ����
        ChangeWeapon("Hand");

        // �̵� ����
        canMove = true;
    }


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

    public void JumpTo()
    {
        if ( movement.JumpTo() ) { GetComponent<Animator>().SetTrigger("Jump"); }
    }

    public virtual void Attack()
    {
        if ( !canMove ) { return; }

        // Attack �ִϸ��̼�
        animator.SetTrigger("Attack");
    }

    /// <summary>
    /// ���ݽ� �浹���� Ȱ��ȭ �޼ҵ� (Aniamtion���� ȣ��)
    /// </summary>
    private void OnAttackCollision()
    {
        // �浹������ ���� GameObject Ȱ��ȭ
        attackCollision.SetActive(true);
    }

    /// <summary>
    /// ������ �ǰݽ� ȣ��
    /// </summary>
    public virtual void TakeDamage(float damage, Transform attacker) => Hp -= damage * 100 / (100 + def);

    /// <summary>
    /// ĳ���� ��� (Animation���� ȣ��)
    /// </summary>
    protected virtual void Die() => gameObject.SetActive(false);

    /// <summary>
    /// ���� ��ü �޼ҵ�
    /// </summary>
    public void ChangeWeapon(string _name) => Weapon = Instantiate(ItemManager.Instance.GetWeapon(_name), hand);

    /// <summary>
    /// ����/����� �̵� �Ұ� (Animation���� ȣ��)
    /// </summary>
    public void EnableMove()  => canMove = true;

    public void DisableMove() => canMove = false;
}
