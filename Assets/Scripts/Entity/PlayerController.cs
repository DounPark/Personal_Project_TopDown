using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : BaseController
{
    private Camera _camera;
    private GameManager gameManager;
    // 변수명을 camera로 하면 부모 클래스인 BaseController의 camera를 덮어씌워버리는 것이기 때문에 변수명만 간단히 바꿔주는것이 좋다.
    public static PlayerController Instance { get; private set; }

    [Header("Equipment Settings")]
    [SerializeField] private KeyCode toggleKey = KeyCode.E; // 무기를 단축키를 눌러 보이고 안보이고를 결정해주기 위한 준비
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

        if (Input.GetKeyDown(toggleKey))// 토글키를 E로 위에서 설정해줌
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
        // horizontal = ←, →, A, D    / vertical ↑, ↓, W, D
        // GetAxis vs GetAxisRaw
        // -1에서 1까지 부드럽게 변화 (조이스틱/게임패드용)
        // 즉시 -1, 0, 1 중 하나를 반환(부드러운 가속은 없지만 키보드 입력에 적합)
        // new Vector2(horizontal, vertical)는 수평 + 수직 입력을 하나의 벡터로 합친다. 예) 오른쪽 1,0 + 위쪽 0,1 = 오른쪽 위 대각선 1,1
        // normalized의 핵심 역할은 대각선이다. 위의 예처럼 대각선의 직선길이는 루트2 즉 1.414...
        // 대각선이 더빠르게 되므로 벡터의 길이를 1로 고정시켜 모든 방향 동일 속도

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = _camera.transform.position.z; // 명시적으로 표시해줘야 좌표 계산해줘야 한다.
        Vector2 worldPos = _camera.ScreenToWorldPoint(mousePosition);
        lookDirection = (worldPos - (Vector2)transform.position);
        // Input.mousePosition는 유니티에서 마우스 좌표를 자동으로 받아온다.
        // _camera.WorldToScreenPoint(mousePosition); -> 스크린 좌표는 픽셀단위, 게임 세계는 유니티 단위(미터)를 사용
        // 지금 카메라 이동을 위해 CinemachineCamera를 사용하고 있기 때문에 WorldToScreenPoint가 아닌 반대로 ScreenToWorldPoint를 써줘야 한다.
        // 카메라를 통해 픽셀 좌표를 게임 월드 자표로 변환.( 2D 게임 카메라는 화면 <-> 월드 좌표 변환의 기준 (카메라가 줌아웃되면 마우스의 월드 좌표도 달라진다.))
        // 단순 거리계산이나 힘의 세기처럼 길이 정보가 필요할 때는 normalized를 생략해도 되지만 그게 아닌경우 준필수적으로 정규화를 해줘야한다.

        if (lookDirection.magnitude < 0.9f) // magnitude = 벡터의 길이 -> 벡터의 강도 또는 거리를 측정할 때
                                            // lookDirection.magnitude < 0.9f의 의도 - 조준 입력이 약할 때는 무시해라(살짝만 움직였을때, 0.9가 임계값
        {
            lookDirection = Vector2.zero; // Vector2.zero 는 조준 안함 0,0
        }
        else
        {
            lookDirection = lookDirection.normalized; // 정규화
        }

        isAttacking = Input.GetMouseButtonDown(0);

    }

    public override void Death()
    {
        base.Death();
        gameManager.GameOver();
    }

}
