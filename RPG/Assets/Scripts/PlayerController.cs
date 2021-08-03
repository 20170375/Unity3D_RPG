using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Player     player;
    private Transform  cameraTransform;
    private GameObject statusPanel;
    private GameObject inventoryPanel;

    [SerializeField] private KeyCode jumpKeyCode = KeyCode.Space;


    private void Awake()
    {
        player          = GetComponent<Player>();
        cameraTransform = Camera.main.transform;
        statusPanel     = GameObject.FindGameObjectWithTag("Status");
        inventoryPanel  = GameObject.FindGameObjectWithTag("Inventory");

        statusPanel.SetActive(false);
        inventoryPanel.SetActive(false);
    }

    private void Update() => KeyboardControl();

    private void KeyboardControl()
    {
        // 방향키를 눌러 이동
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 이동 함수 호출 (카메라가 보고있는 방향을 기준으로 방향키에 따라 이동)
        player.MoveTo(cameraTransform.rotation * new Vector3(x, 0, z));
        // 회전 설정 (항상 앞만 보도록 캐릭터의 회전은 카메라와 같은 회전 값으로 설정)
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

        // Space키를 누르면 점프
        if ( Input.GetKeyDown(jumpKeyCode) ) { player.JumpTo(); }

        // UI 창 팝업
        if ( Input.GetKeyDown(KeyCode.I) )      { ToggleUI(inventoryPanel); }
        if ( Input.GetKeyDown(KeyCode.E) )      { ToggleUI(statusPanel); }
        if ( Input.GetKeyDown(KeyCode.Escape) ) { QuitAllUI(); }

        // UI Mode 수행중 동작 x
        if ( inventoryPanel.activeInHierarchy || statusPanel.activeInHierarchy ) { return; }

        // 마우스 좌클릭시 무기 공격
        if ( Input.GetMouseButtonDown(0) ) { player.Attack(); }
        if ( Input.GetKeyDown(KeyCode.R) ) { player.Attack(); }
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
