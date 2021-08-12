using System.Collections;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine("AutoDisable");
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("Attackable") )
        {
            Transform attacker = GetComponentInParent<Character>().transform;
            float damage = GetComponentInParent<Character>().Weapon.Damage;
            other.GetComponent<Character>().TakeDamage(damage, attacker);
        }
    }

    private IEnumerator AutoDisable()
    {
        // 0.1�� �Ŀ� ������Ʈ ��Ȱ��ȭ
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }
}
