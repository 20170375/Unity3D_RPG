using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState { SearchTarget=0, Chase, TryAttack, }

public class Monster : PlayableCharacter
{
    [Header("Attack")]
    [SerializeField] private float detectRange = 10.0f;

    [Header("Drop Item")]
    [SerializeField] private float dropExp  = 50.0f; // ��� ����ġ
    [SerializeField] private int   dropGold = 5;     // ��� ���
    [SerializeField] private List<DroppedItem> droppedItems;
    [Range(0, 1)] [SerializeField] private List<float> dropPos;

    [Header("Start Equip")]
    [SerializeField] private string startWeapon;    // ���۽� Weapon
    [SerializeField] private string startShoes;     // ���۽� Shoes
    [SerializeField] private string startHalmet;    // ���۽� Halmet
    [SerializeField] private string startArmor;     // ���۽� Armor

    private GameObject   attackTarget = null;  // ���� ���
    private MonsterState monsterState;         // Monster FSM


    private void Update()
    {
        if ( attackTarget != null )
        {
            // Player�� �ٶ󺸵��� ����
            Vector3 position = new Vector3(attackTarget.transform.position.x, transform.position.y, attackTarget.transform.position.z);
            transform.LookAt(position);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // ��� ����
        ChangeWeapon(startWeapon);
        ChangeShoes(startShoes);
        ChangeHalmet(startHalmet);
        ChangeArmor(startArmor);
    }


    public override void TakeDamage(float damage, Transform attacker)
    {
        base.TakeDamage(damage, attacker);

        if ( Hp == 0 )
        {
            attacker.GetComponent<Player>()?.IncreaseExp(dropExp);
            attacker.GetComponent<Player>()?.IncreaseGold(dropGold);

            StopAllCoroutines();
            MoveTo(Vector3.zero);
            attackTarget = null;

            // ������ ���
            float pos = Random.Range(0f, 1f);
            for ( int i=0; i<droppedItems.Count; ++i )
            {
                if ( Random.Range(0f, 1f) <= dropPos[i] )
                {
                    ItemPool.Instance.DropItem(droppedItems[i], transform.position);
                }
            }
        }
    }

    public void ChangeState(MonsterState newState)
    {
        // ������ ������̴� ���� ����
        StopCoroutine(monsterState.ToString());
        // ���� ����
        monsterState = newState;
        // ���ο� ���� ���
        StartCoroutine(monsterState.ToString());
    }

    private IEnumerator Idle()
    {
        while ( true )
        {
            if ( monsterState == MonsterState.SearchTarget )
            {
                Vector3 randPos = transform.position + Random.insideUnitSphere;
                randPos.y = transform.position.y;
                Vector3 direction = (randPos - transform.position).normalized;
                MoveTo(direction);
                transform.LookAt(randPos);

                yield return new WaitForSeconds(1.0f);

                MoveTo(Vector3.zero);

                yield return new WaitForSeconds(1.0f);
            }

            yield return null;
        }
    }

    private IEnumerator SearchTarget()
    {
        while ( true )
        {
            // ���� ������ �ִ� ���� ���(Player) Ž��
            attackTarget = FindClosestAttackTarget();

            if ( attackTarget != null )
            {
                // Player���� �Ÿ��� ������ ����, �Ÿ��� �ִٸ� ����
                float distance = Vector3.Distance(attackTarget.transform.position, transform.position);
                if ( distance <= Weapon.AttackRange*2 ){ ChangeState(MonsterState.TryAttack); }
                else { ChangeState(MonsterState.Chase); }
            }
            
            yield return null;
        }
    }

    private IEnumerator Chase()
    {
        while ( attackTarget != null )
        {
            // �Ÿ��� �ʹ� �־����ų�, ����� ������ �������� ��� ����
            float distance = Vector3.Distance(attackTarget.transform.position, transform.position);
            if ( (distance > detectRange) || (distance <= Weapon.AttackRange*1.5) ) { break; }

            // Target �������� �̵�
            Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
            MoveTo(direction);

            yield return null;
        }

        MoveTo(Vector3.zero);
        ChangeState(MonsterState.SearchTarget);
    }

    private IEnumerator TryAttack()
    {
        Attack();

        yield return new WaitForSeconds(10.0f / Weapon.AttackSpeed);

        ChangeState(MonsterState.SearchTarget);
    }

    private GameObject FindClosestAttackTarget()
    {
        // ���� ������ �ִ� Player�� ã�� ���� ���� �Ÿ��� �ִ��� ũ�� ����
        float closestDistSqr = Mathf.Infinity;

        // ���� �ʿ� �����ϴ� ��� Player �˻�
        Player[] players = FindObjectsOfType<Player>();
        for ( int i=0; i< players.Length; ++i )
        {
            float distance = Vector3.Distance(players[i].transform.position, transform.position);
            // ������� �˻��� ������ �Ÿ��� ������
            if ( (distance <= detectRange) && (distance <= closestDistSqr) )
            {
                closestDistSqr = distance;
                attackTarget   = players[i].gameObject;
            }
        }

        if ( closestDistSqr != Mathf.Infinity ) { return attackTarget; }
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, 0.1f);
        Gizmos.DrawSphere(transform.position, detectRange);
    }
}
