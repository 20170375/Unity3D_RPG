using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Character : MonoBehaviour
{
    private Movement movement3D;

    [Header("Info")]
    [SerializeField] private   int   level    = 1;    // ����
    [SerializeField] private   int   maxLevel = 99;   // �ִ� ����
    [SerializeField] protected float hp;              // ���� ü��
    [SerializeField] protected float maxHp = 100;     // �ִ� ü��

    [Header("Attack")]
    protected Weapon         weapon;    // ����

    [SerializeField] private GameObject attackCollision; // ���ݽ� �浹������ ���� GameObject

    [Header("UI")]
    [SerializeField] private GameObject hpSliderPrefab;   // ü�� Slider UI ������
    [SerializeField] private GameObject levelTextPrefab;  // ���� Text UI ������

    private Slider           hpSlider;  // ü�� Slider UI
    private TextMeshProUGUI  levelText; // ���� Text UI

    protected int Level
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

    public float Hp
    {
        set
        {
            hp = Mathf.Clamp(value, 0, MaxHp);
            hpSlider.value = Hp / MaxHp;
        }
        get
        {
            return hp;       
        }
    }

    public float MaxHp { protected set => maxHp = value; get => maxHp; }

    public Weapon Weapon
    {
        set
        {
            weapon = value;

            // ���� ������ ���� �浹���� ����
            attackCollision.transform.localPosition = new Vector3(0, 0, Weapon.AttackRange);
            attackCollision.transform.localScale = new Vector3(attackCollision.transform.localScale.x, Weapon.AttackRange, attackCollision.transform.localScale.z);
        }
        get
        {
            return weapon;
        }
    }

    protected virtual void OnEnable()
    {
        movement3D    = GetComponent<Movement>();
        Canvas canvas = GetComponentInChildren<Canvas>();

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
        Weapon = new Hand();
    }

    public void MoveTo(Vector3 direction)
    {
        if ( Hp == 0 )
        {
            movement3D.MoveTo(Vector3.zero);
            return;
        }

        movement3D.MoveTo(direction);
    }

    public void JumpTo()
    {
        movement3D.JumpTo();
    }

    public virtual void Attack()
    {
        OnAttackCollision();

        Debug.Log(gameObject.name + " Attack");
    }

    private void OnAttackCollision()
    {
        // �浹������ ���� GameObject Ȱ��ȭ
        attackCollision.SetActive(true);
    }

    public virtual void TakeDamage(float damage, Transform attacker)
    {
        Debug.Log(gameObject.name + " TakeDamage (" + damage + ") by " + attacker.name);

        Hp -= damage;

        if ( Hp == 0 ) { Die(); }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + "���");

        gameObject.SetActive(false);
    }
}
