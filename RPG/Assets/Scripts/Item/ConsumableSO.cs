using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Consumable
{
    public int   id;        // ID
    public float hpGain;    // ȹ�� HP
    public float mpGain;    // ȹ�� MP
    public float expGain;   // ȹ�� EXP

    public Consumable(int _id, float _hpGain, float _mpGain, float _expGain)
    {
        id      = _id;
        hpGain  = _hpGain;
        mpGain  = _mpGain;
        expGain = _expGain;
    }

    public Consumable(Consumable target)
        : this(target.id, target.hpGain, target.mpGain, target.expGain) { }
}

[CreateAssetMenu(fileName = "ConsumableSO", menuName = "Scriptable Object/ConsumableSO")]
public class ConsumableSO : ScriptableObject
{
    [SerializeField] private List<Consumable> consumables;

    public List<Consumable> Consumables { get => consumables; }
}
