using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private Player player;
    private string filePath;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private KeyCode   jumpKeyCode = KeyCode.Space;


    private void Awake() => filePath = Application.dataPath + "/Resources/DB_playerData.txt";

    private void OnEnable()
    {
        player = GetComponent<Player>();

        GameManager.Instance.Player = player;
        GameManager.Instance.CloseAllPanel();
        LoadData();
    }

    private void OnDisable()
    {
        player.Hp = player.MaxHp;
        SaveData();
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

        // UI â �˾�
        if ( Input.GetKeyDown(KeyCode.I) )      { ToggleUI(GameManager.Instance.InventoryPanel); }
        if ( Input.GetKeyDown(KeyCode.E) )      { ToggleUI(GameManager.Instance.StatusPanel); }
        if ( Input.GetKeyDown(KeyCode.Escape) ) { GameManager.Instance.CloseAllPanel(); }

        // ���콺 ��Ŭ���� ���� ����
        if ( EventSystem.current.IsPointerOverGameObject() ) { return; }
        if ( Input.GetMouseButtonDown(0) ) { player.Attack(); }
        if ( Input.GetKeyDown(KeyCode.R) ) { player.Attack(); }
    }

    /// <summary>
    /// UI Ȱ��ȭ/��Ȱ��ȭ (Status/Inventory)
    /// </summary>
    private void ToggleUI(GameObject ui)
    {
        bool isOff = !ui.activeInHierarchy;
        ui.SetActive(isOff);
        if      ( isOff && (ui == GameManager.Instance.InventoryPanel) ) { ui.GetComponent<InventoryPanel>().Show(); }
        else if ( isOff && (ui == GameManager.Instance.StatusPanel) )    { ui.GetComponent<StatusPanel>().Show(); }
    }

    /// <summary>
    /// PlayerData�� ���� (Inventory���� ȣ��)
    /// </summary>
    public void SaveData()
    {
        string jsonData = JsonUtility.ToJson(new PlayerData(player));
        //byte[] bytes    = System.Text.Encoding.UTF8.GetBytes(jsonData);
        //string code     = System.Convert.ToBase64String(bytes);
        //File.WriteAllText(filePath, code);
        File.WriteAllText(filePath, jsonData);
    }

    /// <summary>
    /// ����� PlayerData�� �ҷ��´� (Inventory���� ȣ��)
    /// </summary>
    public void LoadData()
    {
        if ( !File.Exists(filePath) ) { SaveData(); }

        //string code     = File.ReadAllText(filePath);
        //byte[] bytes    = System.Convert.FromBase64String(code);
        //string jsonData = System.Text.Encoding.UTF8.GetString(bytes);
        string jsonData = File.ReadAllText(filePath);
        player.LoadData(JsonUtility.FromJson<PlayerData>(jsonData));
    }
}

[System.Serializable]
public class PlayerData
{
    public int   Level;
    public float Hp;
    public float Exp;
    public int   WeaponID;
    public int   ShoesID;
    public int   HalmetID;
    public int   ArmorID;

    public PlayerData(Player player)
    {
        Level    = player.Level;
        Hp       = player.Hp;
        Exp      = player.Exp;
        WeaponID = player.Weapon.ID;
        ShoesID  = player.Shoes.ID;
        HalmetID = player.Halmet.ID;
        ArmorID  = player.Armor.ID;
    }
}
