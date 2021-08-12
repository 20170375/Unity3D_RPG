using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    [SerializeField] private GameObject expSliderPrefab;  // ����ġ Slider UI ������
    private Slider     expSlider;                         // ����ġ Slider UI

    private float  currentExp = 0;  // ���� ����ġ
    private float  maxExp = 300;    // �ִ� ����ġ

    protected override void OnEnable()
    {
        base.OnEnable();

        // UI ����
        Canvas canvas = GetComponentInChildren<Canvas>();
        GameObject expSliderClone = Instantiate(expSliderPrefab, canvas.transform);
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
        // ��� �ִϸ��̼� ��� �߰��ؾߵɵ�

        base.Die();
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
