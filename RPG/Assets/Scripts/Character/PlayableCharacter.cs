using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayableCharacter : Character
{
    [Header("Stat")]
    [SerializeField] private   int   level    = 1;    // ����
    [SerializeField] private   int   maxLevel = 99;   // �ִ� ����
    [SerializeField] protected float hp;              // ���� HP
    [SerializeField] protected float maxHp    = 100;  // �ִ� HP
    [SerializeField] protected float mp;              // ���� MP
    [SerializeField] protected float maxMp    = 50;   // �ִ� MP
    [SerializeField] protected int   atk;             // ���ݷ�
    [SerializeField] protected int   def;             // ����
    [SerializeField] protected int   spd;             // �̵��ӵ�
    [SerializeField] protected float cri;             // ġ��Ÿ Ȯ��
    protected Weapon weapon;    // ���� �������� Weapon
    protected Shoes  shoes;     // ���� �������� Shoes
    protected Shoes  shoes0;
    protected Halmet halmet;    // ���� �������� Helmet
    protected Armor  armor;     // ���� �������� Armor

    [Header("Equip")]
    [SerializeField] private Transform  hand;       // Weapon ���� ��ġ
    [SerializeField] private Transform  leftFoot;   // Shoes  ���� ��ġ (�޹�)
    [SerializeField] private Transform  rightFoot;  // Shoes  ���� ��ġ (������)
    [SerializeField] private Transform  body;       // Armor  ���� ��ġ
    [SerializeField] private Transform  head;       // Halmet ���� ��ġ

    [Header("Attack")]
    [SerializeField] private GameObject attackCollision; // ���ݽ� �浹������ ���� GameObject
    
    public int   Level
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
    public virtual float Hp
    {
        set
        {
            hp = Mathf.Clamp(value, 0, maxHp);
            hpSlider.value = hp / maxHp;

            if ( hp == 0 )
            {
                canMove = false;

                animator.Play("CharacterDeath");
            }
        }
        get
        {
            return hp;       
        }
    }
    public float MaxHp { protected set => maxHp = value; get => maxHp; }
    public virtual float Mp
    {
        set
        {
            mp = Mathf.Clamp(value, 0, maxMp);
            mpSlider.value = mp / maxMp;
        }
        get
        {
            return mp;
        }
    }
    public float MaxMp { protected set => maxMp = value; get => maxMp; }

    public int   Atk { get => atk + weapon.Atk; }
    public int   Def { get => def + halmet.Def + armor.Def; }
    public int   Spd { get => spd + shoes.Spd; }
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
    public Shoes  Shoes
    {
        set
        {
            if ( shoes )  { Destroy(shoes.gameObject); }
            if ( shoes0 ) { Destroy(shoes0.gameObject); }
            shoes = value;

            // �̵��ӵ�
            movement.AddSpeed = Spd;
        }
        get
        {
            return shoes;
        }
    }
    public Halmet Halmet
    {
        set
        {
            if ( halmet ) { Destroy(halmet.gameObject); }
            halmet = value;
        }
        get
        {
            return halmet;
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

    [Header("UI")]
    [SerializeField] private   GameObject hpSliderPrefab;   // ü�� Slider UI ������
    [SerializeField] private   GameObject mpSliderPrefab;   // ü�� Slider UI ������
    [SerializeField] private   GameObject levelTextPrefab;  // ���� Text UI ������
    [SerializeField] private   Text       DmgText;          // ������ ����Ʈ Text
    private Slider hpSlider;  // HP Slider UI
    private Slider mpSlider;  // MP Slider UI
    private Text   levelText; // ���� Text UI


    protected override void OnEnable()
    {
        base.OnEnable();

        // UI ����
        if ( hpSlider == null )
        {
            // HP Slider UI
            hpSlider = Instantiate(hpSliderPrefab, canvas.transform).GetComponent<Slider>();
        }
        if ( mpSlider == null )
        {
            // MP Slider UI
            mpSlider = Instantiate(mpSliderPrefab, canvas.transform).GetComponent<Slider>();
        }
        if ( levelText == null )
        {
            // ���� Text UI
            levelText = Instantiate(levelTextPrefab, canvas.transform).GetComponent<Text>();
        }

        // ĳ���� ���� ����
        Level = level;
        Hp    = MaxHp;
        Mp    = MaxMp;

        // ��� ����
        ChangeWeapon("Hand");
        ChangeShoes("Foot");
        ChangeHalmet("Head");
        ChangeArmor("Body");
    }


    /// <summary>
    /// ĳ���� ��� (Animation���� ȣ��)
    /// </summary>
    protected override void Die()
    {
        base.Die();

        hp = maxHp;
    }


    public virtual void Attack()
    {
        if (!canMove) { return; }

        // Attack �ִϸ��̼�
        animator.SetTrigger("Attack");
    }

    /// <summary>
    /// ������ �ǰݽ� ȣ��
    /// </summary>
    public virtual void TakeDamage(float damage, Transform attacker)
    {
        // ��� ���½� �ǰ� ����
        if ( hp == 0 ) { return; }

        damage = damage * 100 / (100 + Def);

        // HitEffect
        EffectPool.Instance.HitEffect(transform.position);
        DmgText.text = string.Format("{0:##,##0}", damage);
        DmgText.gameObject.SetActive(true);

        animator.Play("CharacterHit");

        Hp -= damage;
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
    /// Weapon ��ü �޼ҵ�
    /// </summary>
    public virtual void ChangeWeapon(string _name) => Weapon = Instantiate(ItemManager.Instance.GetWeapon(_name), hand);

    /// <summary>
    /// Shoes ��ü �޼ҵ�
    /// </summary>
    public virtual void ChangeShoes(string _name)
    {
        Shoes  = Instantiate(ItemManager.Instance.GetShoes(_name), leftFoot);
        shoes0 = Instantiate(ItemManager.Instance.GetShoes(_name), rightFoot);
    }

    /// <summary>
    /// Halmet ��ü �޼ҵ�
    /// </summary>
    public virtual void ChangeHalmet(string _name) => Halmet = Instantiate(ItemManager.Instance.GetHalmet(_name), head);

    /// <summary>
    /// Armor ��ü �޼ҵ�
    /// </summary>
    public virtual void ChangeArmor(string _name) => Armor = Instantiate(ItemManager.Instance.GetArmor(_name), body);
}
