using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerList;
    [SerializeField] private int              spawnCount = 1;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Transform        spawnArea;

    private void Awake()
    {
        StartCoroutine("Spawn");
    }

    private IEnumerator Spawn()
    {
        while ( true )
        {
            // ���� ���� �����ϸ� ����x
            if ( FindObjectsOfType<Player>().Length < spawnCount )
            {
                // playerList���� ��Ȱ��ȭ�� player�� �ϳ� Ȱ��ȭ
                GameObject player = playerList.Find(x => !x.activeSelf);
                player.transform.position = spawnArea.position;
                player.SetActive(true);
                cameraController.Setup(player);
            }

            yield return null;
        }
    }
}
