using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    [SerializeField]
    private GameObject expSliderPrefab;  // ����ġ Slider UI ������
    private Slider     expSlider;        // ����ġ Slider UI

    private float  currentExp;  // ���� ����ġ
    private float  maxExp;      // �ִ� ����ġ

    protected override void Awake()
    {
        base.Awake();

        // �ʱ� ����ġ ����
        currentExp = 0;
        maxExp = 300;

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
    }

    public override void TakeDamage(float damage, Transform attacker)
    {
        base.TakeDamage(damage, attacker);
    }

    protected override void Die()
    {
        base.Die();
    }

    public void IncreaseExp(float exp)
    {
        currentExp += exp;

        // �ִ� ����ġ�� ���޽� ������
        if ( currentExp >= maxExp )
        {
            LevelUp();
        }

        // UI ����
        expSlider.value = currentExp / maxExp;

        Debug.Log("EXP: " + currentExp);
    }

    private void LevelUp()
    {
        Level++;

        currentExp -= maxExp;
    }
}
