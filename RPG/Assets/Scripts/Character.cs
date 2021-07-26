using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Character : MonoBehaviour
{
    private Movement3D movement3D;

    [Header("Info")]
    [SerializeField]
    private int          level = 1;       // ����
    [SerializeField]
    private int          maxLevel = 99;   // �ִ� ����
    [SerializeField]
    protected float      hp = 100;        // ü��

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
        MaxHp = hp;
        Hp    = hp;
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

        // ���� ����
        StartCoroutine("OnHitColor");
    }

    private IEnumerator OnHitColor()
    {
        // ���� ���� ������ ������ �� 0.1�� �Ŀ� ���� �������� ����
        Color originColor = gameObject.GetComponentInChildren<MeshRenderer>().material.color;

        foreach ( MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>() )
        {
            renderer.material.color = Color.red;
        }

        yield return new WaitForSeconds(0.1f);

        foreach ( MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>() )
        {
            renderer.material.color = originColor;
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
        // �浹 ����
        foreach ( Collider collider in GetComponentsInChildren<Collider>() ) { collider.enabled = false; }

        // �̵� ����
        GetComponent<Movement3D>().enabled = false;

        yield return new WaitForSeconds(1.0f);

        Destroy(gameObject);
    }
}
