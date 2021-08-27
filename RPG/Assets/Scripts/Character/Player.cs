using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : PlayableCharacter
{
    [Header("Inventory")]
    [SerializeField] private Inventory inventory;

    [Header("EXP")]
    [SerializeField] private GameObject expSliderPrefab;    // ����ġ Slider UI ������
    private Slider expSlider;   // ����ġ Slider UI

    public override float Hp
    {
        set
        {
            base.Hp = value;

            // Data ����
            SaveData();
        }
        get
        {
            return base.Hp;
        }
    }
    public override float Mp
    {
        set
        {
            base.Mp = value;

            // Data ����
            SaveData();
        }
        get
        {
            return base.Mp;
        }
    }

    public Inventory Inventory { get => inventory; }
    public float Exp    { private set; get; } = 0;
    public float MaxExp { private set; get; } = 300;


    protected override void OnEnable()
    {
        base.OnEnable();

        // UI ����
        expSlider = Instantiate(expSliderPrefab, canvas.transform).GetComponent<Slider>();
        expSlider.value = Exp / MaxExp;

        // Data ����
        SaveData();
    }


    /// <summary>
    /// ĳ���� ��� (Animation���� ȣ��)
    /// </summary>
    protected override void Die()
    {
        GetComponent<PlayerController>().enabled = false;

        base.Die();
    }

    /// <summary>
    /// ������ �ǰݽ� ȣ��
    /// </summary>
    public override void TakeDamage(float damage, Transform attacker)
    {
        base.TakeDamage(damage, attacker);

        // Data ����ȭ
        PlayerController controller = GetComponent<PlayerController>();
        if ( (controller != null) && controller.enabled ) { controller.SaveData(); }
    }

    /// <summary>
    /// Weapon ��ü �޼ҵ�
    /// </summary>
    public override void ChangeWeapon(string _name)
    {
        base.ChangeWeapon(_name);

        // Data ����
        SaveData();
    }

    /// <summary>
    /// Shoes ��ü �޼ҵ�
    /// </summary>
    public override void ChangeShoes(string _name)
    {
        base.ChangeShoes(_name);

        // Data ����
        SaveData();
    }

    /// <summary>
    /// Halmet ��ü �޼ҵ�
    /// </summary>
    public override void ChangeHalmet(string _name)
    {
        base.ChangeHalmet(_name);

        // Data ����
        SaveData();
    }

    /// <summary>
    /// Armor ��ü �޼ҵ�
    /// </summary>
    public override void ChangeArmor(string _name)
    {
        base.ChangeArmor(_name);

        // Data ����
        SaveData();
    }


    /// <summary>
    /// ������
    /// </summary>
    private void LevelUp()
    {
        Level++;
        Hp  = MaxHp;
        Exp = 0;

        // Data ����
        SaveData();
    }

    /// <summary>
    /// ����ġ ȹ��
    /// </summary>
    public void IncreaseExp(float exp)
    {
        Exp += exp;

        // �ִ� ����ġ�� ���޽� ������
        if ( Exp >= MaxExp ) { LevelUp(); }

        // UI ����
        if ( expSlider != null ) { expSlider.value = Exp / MaxExp; }

        // Data ����
        SaveData();
    }

    /// <summary>
    /// ��� ȹ��
    /// </summary>
    public void IncreaseGold(int gold)
    {
        Inventory.IncreaseGold(gold);
        GameManager.Instance.Notice(characterName + " get " + gold + " G");
    }

    /// <summary>
    /// �Ҹ�ǰ ���
    /// </summary>
    public void UseConsumable(Consumable consumable)
    {
        Hp += consumable.hpGain;
        //Mp += consumable.mpGain;
        IncreaseExp(consumable.expGain);
    }

    /// <summary>
    /// ������ �ҷ�����
    /// </summary>
    public void LoadData(PlayerData playerData)
    {
        Level = playerData.Level;
        Hp    = playerData.Hp;
        IncreaseExp(playerData.Exp);
        ChangeWeapon(ItemManager.Instance.GetWeapon(playerData.WeaponID).EquipName);
        ChangeShoes(ItemManager.Instance.GetShoes(playerData.ShoesID).EquipName);
        ChangeHalmet(ItemManager.Instance.GetHalmet(playerData.HalmetID).EquipName);
        ChangeArmor(ItemManager.Instance.GetArmor(playerData.ArmorID).EquipName);
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    private void SaveData()
    {
        PlayerController controller = GetComponent<PlayerController>();
        if ( (controller != null) && controller.enabled ) { controller.SaveData(); }
    }
}
