using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Character : MonoBehaviour
{
    private Movement3D movement3D;

    [Header("Info")]
    [SerializeField]
    private int          level;           // ����
    [SerializeField]
    protected float      hp;              // ü��

    [Header("Attack")]
    protected Weapon     weapon;          // ����
    [SerializeField]
    private   GameObject attackCollision; // ���ݽ� �浹������ ���� GameObject

    [Header("UI")]
    [SerializeField]
    private GameObject       hpSliderPrefab;   // ü�� Slider UI ������
    private Slider           hpSlider;         // ü�� Slider UI
    [SerializeField]
    private GameObject       levelTextPrefab;  // ���� Text UI ������
    private TextMeshProUGUI  levelText;        // ���� Text UI

    protected int Level
    {
        set
        {
            level = Mathf.Max(1, value);
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
            hp = Mathf.Max(0, value);
            hpSlider.value = Hp / MaxHp;
        }
        get
        {
            return hp;       
        }
    }

    public float MaxHp { protected set; get; }

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

    protected virtual void Awake()
    {
        movement3D = GetComponent<Movement3D>();

        // UI ���� - ü�� Slider UI
        GameObject hpSliderClone = Instantiate(hpSliderPrefab);
        Canvas canvas = FindObjectOfType<Canvas>();
        hpSliderClone.transform.SetParent(canvas.transform);
        hpSliderClone.transform.localScale = Vector3.one;
        hpSliderClone.GetComponent<SliderAutoPosition>().Setup(gameObject.transform);
        hpSlider = hpSliderClone.GetComponent<Slider>();

        // UI ���� - ���� Text UI
        GameObject levelTextClone = Instantiate(levelTextPrefab);
        levelTextClone.transform.SetParent(canvas.transform);
        levelTextClone.transform.localScale = Vector3.one;
        levelTextClone.GetComponent<SliderAutoPosition>().Setup(gameObject.transform);
        levelText = levelTextClone.GetComponent<TextMeshProUGUI>();

        // ĳ���� ���� ����
        level = 1;
        Hp    = 100.0f;
        MaxHp = Hp;

        // ���� ����
        Weapon = new Hand();
    }

    public void MoveTo(Vector3 direction)
    {
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

        if ( Hp == 0 )
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + "���");

        gameObject.tag = "Untagged";

        StartCoroutine("Death");
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(1.0f);

        Destroy(gameObject);
    }
}
