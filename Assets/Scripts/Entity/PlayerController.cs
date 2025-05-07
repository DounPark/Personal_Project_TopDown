using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : BaseController
{
    private Camera _camera;
    private GameManager gameManager;
    // �������� camera�� �ϸ� �θ� Ŭ������ BaseController�� camera�� ����������� ���̱� ������ ������ ������ �ٲ��ִ°��� ����.
    public static PlayerController Instance { get; private set; }

    [Header("Equipment Settings")]
    [SerializeField] private KeyCode toggleKey = KeyCode.E; // ���⸦ ����Ű�� ���� ���̰� �Ⱥ��̰� �������ֱ� ���� �غ�
    // [SerializeField] private AudioClip equipSound;

    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        base.Awake();
    }

    protected override void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        _camera = Camera.main;
    }


    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        _camera = Camera.main;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _camera = Camera.main;
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(toggleKey))// ���Ű�� E�� ������ ��������
        {
            ToggleWeapon();
        }
    }

    private void ToggleWeapon()
    {
        if (weaponHandler != null)
            weaponHandler.ToggleWeapon();
    }

    protected override void HandleAction()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector2(horizontal, vertical).normalized;
        // horizontal = ��, ��, A, D    / vertical ��, ��, W, D
        // GetAxis vs GetAxisRaw
        // -1���� 1���� �ε巴�� ��ȭ (���̽�ƽ/�����е��)
        // ��� -1, 0, 1 �� �ϳ��� ��ȯ(�ε巯�� ������ ������ Ű���� �Է¿� ����)
        // new Vector2(horizontal, vertical)�� ���� + ���� �Է��� �ϳ��� ���ͷ� ��ģ��. ��) ������ 1,0 + ���� 0,1 = ������ �� �밢�� 1,1
        // normalized�� �ٽ� ������ �밢���̴�. ���� ��ó�� �밢���� �������̴� ��Ʈ2 �� 1.414...
        // �밢���� �������� �ǹǷ� ������ ���̸� 1�� �������� ��� ���� ���� �ӵ�

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = _camera.transform.position.z; // ��������� ǥ������� ��ǥ �������� �Ѵ�.
        Vector2 worldPos = _camera.ScreenToWorldPoint(mousePosition);
        lookDirection = (worldPos - (Vector2)transform.position);
        // Input.mousePosition�� ����Ƽ���� ���콺 ��ǥ�� �ڵ����� �޾ƿ´�.
        // _camera.WorldToScreenPoint(mousePosition); -> ��ũ�� ��ǥ�� �ȼ�����, ���� ����� ����Ƽ ����(����)�� ���
        // ���� ī�޶� �̵��� ���� CinemachineCamera�� ����ϰ� �ֱ� ������ WorldToScreenPoint�� �ƴ� �ݴ�� ScreenToWorldPoint�� ����� �Ѵ�.
        // ī�޶� ���� �ȼ� ��ǥ�� ���� ���� ��ǥ�� ��ȯ.( 2D ���� ī�޶�� ȭ�� <-> ���� ��ǥ ��ȯ�� ���� (ī�޶� �ܾƿ��Ǹ� ���콺�� ���� ��ǥ�� �޶�����.))
        // �ܼ� �Ÿ�����̳� ���� ����ó�� ���� ������ �ʿ��� ���� normalized�� �����ص� ������ �װ� �ƴѰ�� ���ʼ������� ����ȭ�� ������Ѵ�.

        if (lookDirection.magnitude < 0.9f) // magnitude = ������ ���� -> ������ ���� �Ǵ� �Ÿ��� ������ ��
                                            // lookDirection.magnitude < 0.9f�� �ǵ� - ���� �Է��� ���� ���� �����ض�(��¦�� ����������, 0.9�� �Ӱ谪
        {
            lookDirection = Vector2.zero; // Vector2.zero �� ���� ���� 0,0
        }
        else
        {
            lookDirection = lookDirection.normalized; // ����ȭ
        }

        isAttacking = Input.GetMouseButtonDown(0);

    }

    public override void Death()
    {
        base.Death();
        gameManager.GameOver();
    }

}
