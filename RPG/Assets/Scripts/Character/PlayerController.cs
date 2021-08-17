using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Player player;

    [SerializeField] private Transform  cameraTransform;
    [SerializeField] private GameObject statusPanel;
    [SerializeField] private GameObject inventoryPanel;

    [SerializeField] private KeyCode jumpKeyCode = KeyCode.Space;

    private void OnEnable()
    {
        player = GetComponent<Player>();

        statusPanel.SetActive(false);
        inventoryPanel.SetActive(false);
    }

    private void Update() => KeyboardControl();

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

        // UI â �˾�
        if ( Input.GetKeyDown(KeyCode.I) )      { ToggleUI(inventoryPanel); }
        if ( Input.GetKeyDown(KeyCode.E) )      { ToggleUI(statusPanel); }
        if ( Input.GetKeyDown(KeyCode.Escape) ) { QuitAllUI(); }

        // UI Mode ������ ���� x
        if ( inventoryPanel.activeInHierarchy || statusPanel.activeInHierarchy ) { return; }

        // ���콺 ��Ŭ���� ���� ����
        if ( Input.GetMouseButtonDown(0) ) { player.Attack(); }
        if ( Input.GetKeyDown(KeyCode.R) ) { player.Attack(); }
        if ( Input.GetKeyDown(KeyCode.Alpha1) ) { player.ChangeWeapon("Hand"); }
        if ( Input.GetKeyDown(KeyCode.Alpha2) ) { player.ChangeWeapon("Sword"); }
    }

    private void ToggleUI(GameObject ui)
    {
        ui.SetActive(!ui.activeInHierarchy);

        bool uiMode = inventoryPanel.activeInHierarchy | statusPanel.activeInHierarchy;

        Camera.main.GetComponent<CameraController>().UIMode(uiMode);
    }

    private void QuitAllUI()
    {
        statusPanel.SetActive(false);
        inventoryPanel.SetActive(false);

        Camera.main.GetComponent<CameraController>().UIMode(false);
    }
}
