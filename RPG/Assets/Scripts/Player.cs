using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    [SerializeField]
    private GameObject expSliderPrefab;  // ����ġ Slider UI ������
    private Slider     expSlider;        // ����ġ Slider UI

    private float  currentExp = 0;  // ���� ����ġ
    private float  maxExp = 300;    // �ִ� ����ġ

    protected override void Awake()
    {
        base.Awake();

        // UI ����
        GameObject expSliderClone = Instantiate(expSliderPrefab);
        Canvas canvas = FindObjectOfType<Canvas>();
        expSliderClone.transform.SetParent(canvas.transform);
        expSliderClone.transform.localScale = Vector3.one;
        expSliderClone.GetComponent<SliderAutoPosition>().Setup(gameObject.transform);
        expSlider = expSliderClone.GetComponent<Slider>();
        expSlider.value = currentExp / maxExp;
    }

    public override void Attack()
    {
        base.Attack();

        // Attack �ִϸ��̼�
        GetComponent<Animator>().SetTrigger("Attack");
    }

    public override void TakeDamage(float damage, Transform attacker)
    {
        base.TakeDamage(damage, attacker);
    }

    protected override void Die()
    {
        base.Die();

        Destroy(gameObject.GetComponent<PlayerController>());
    }

    public void IncreaseExp(float exp)
    {
        currentExp += exp;

        // �ִ� ����ġ�� ���޽� ������
        if ( currentExp >= maxExp ) { LevelUp(); }

        // UI ����
        expSlider.value = currentExp / maxExp;
    }

    private void LevelUp()
    {
        Level++;
        Hp = MaxHp;
        currentExp = 0;
    }
}
