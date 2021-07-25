using System.Collections;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject       PlayerPrefab;
    [SerializeField]
    private int              spawnCount = 1;
    [SerializeField]
    private CameraController cameraController;

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
                GameObject player = Instantiate(PlayerPrefab, transform);
                cameraController.Setup(player);
            }

            yield return null;
        }
    }
}
