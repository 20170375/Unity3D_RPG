using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    [Header("Inventory")]
    [SerializeField] private Inventory inventory;

    [Header("UI")]
    [SerializeField] private GameObject expSliderPrefab;  // ����ġ Slider UI ������
    private Slider expSlider;                             // ����ġ Slider UI
    private float  currentExp = 0;  // ���� ����ġ
    private float  maxExp = 300;    // �ִ� ����ġ

    public Inventory Inventory { get => inventory; }

    protected override void OnEnable()
    {
        base.OnEnable();

        // UI ����
        Canvas canvas = GetComponentInChildren<Canvas>();
        GameObject expSliderClone = Instantiate(expSliderPrefab, canvas.transform);
        expSlider = expSliderClone.GetComponent<Slider>();
        expSlider.value = currentExp / maxExp;
    }

    /// <summary>
    /// ĳ���� ��� (Animation���� ȣ��)
    /// </summary>
    protected override void Die()
    {
        GetComponent<PlayerController>().enabled = false;

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
