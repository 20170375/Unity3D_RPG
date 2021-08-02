using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Player    player;
    private Transform cameraTransform;

    [SerializeField] private KeyCode    jumpKeyCode = KeyCode.Space;

    [Header("UI")]
    [SerializeField] private GameObject inventoryPanelPrefab;
    private GameObject inventoryPanel;

    private void Awake()
    {
        Cursor.visible   = false;                     // ���콺 Ŀ���� ������ �ʰ�
        Cursor.lockState = CursorLockMode.Locked;     // ���콺 Ŀ�� ��ġ ����

        player          = GetComponent<Player>();
        cameraTransform = Camera.main.transform;
        inventoryPanel  = Instantiate(inventoryPanelPrefab, GameObject.FindGameObjectWithTag("MainCanvas").transform);
        inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        KeyboardControl();
    }

    private void KeyboardControl()
    {
        // ����Ű�� ���� �̵�
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // �̵� �Լ� ȣ�� (ī�޶� �����ִ� ������ �������� ����Ű�� ���� �̵�)
        player.MoveTo(cameraTransform.rotation * new Vector3(x, 0, z));
        // ȸ�� ���� (�׻� �ո� ������ ĳ������ ȸ���� ī�޶�� ���� ȸ�� ������ ����)
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

        // SpaceŰ�� ������ ����
        if ( Input.GetKeyDown(jumpKeyCode) ) { player.JumpTo(); }

        // �κ��丮 â �˾�
        if ( Input.GetKeyDown(KeyCode.I) ) { ToggleInventory(); }

        // �κ��丮 â �˾� �߿��� ���� x
        if ( inventoryPanel.activeInHierarchy ) { return; }

        // ���콺 ��Ŭ���� ���� ����
        if ( Input.GetMouseButtonDown(0) ) { player.Attack(); }
        if ( Input.GetKeyDown(KeyCode.R) ) { player.Attack(); }
    }

    private void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);

        Camera.main.GetComponent<CameraController>().Setup(inventoryPanel.activeInHierarchy ? null : player.gameObject);
    }
}
