using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> monsterList;
    [SerializeField] private int              spawnCount   = 5;
    [SerializeField] private float            spawnTime    = 5.0f;
    [SerializeField] private float            deliveryTime = 2.0f;
    [SerializeField] private Transform        spawnArea;

    public List<GameObject> MonsterList { get => monsterList; }


    private void Start() => StartCoroutine("Spawn");


    private IEnumerator Spawn()
    {
        while ( true )
        {
            // ���� ���� �����ϸ� ����x
            if ( monsterList.FindAll(x => x.activeSelf).Count < spawnCount )
            {
                // monsterList���� ��Ȱ��ȭ�� player�� �ϳ� Ȱ��ȭ
                GameObject monster = monsterList.Find(x => !x.activeSelf);
                monster.transform.position = spawnArea.position;
                monster.SetActive(true);
                StartCoroutine(Delivery(monster.GetComponent<Monster>()));
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }

    private IEnumerator Delivery(Character target)
    {
        // ���� ��ġ ����
        Vector3 randPos = Random.insideUnitSphere.normalized;
        randPos.y = target.transform.position.y;
        Vector3 goalPos = target.transform.position + randPos;
        goalPos.y = target.transform.position.y;

        // �ش� ��ġ �������� deliveryTime���� �̵�
        Vector3 direction = (goalPos - target.transform.position).normalized;
        target.MoveTo(direction);
        target.transform.LookAt(goalPos);

        yield return new WaitForSeconds(deliveryTime);

        target.MoveTo(Vector3.zero);

        // MonsterState ����
        target.GetComponent<Monster>().ChangeState(MonsterState.SearchTarget);
        target.GetComponent<Monster>().StartCoroutine("Idle");
    }
}
