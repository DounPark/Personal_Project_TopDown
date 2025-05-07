using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;
    // 이걸 작성해주는 이유는 이 스크립트를 붙이는 오브젝트에 rigidbody2d 컴포넌트가 붙어있고
    // 다른 스크립트의 영향을 받는 다른 오브젝트가 있을때 영향을 주지 않기 위해 작성
    // (protected는 인스펙터에 할당되진 않지만 자식 클래스에선 사용 가능)

    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Transform weaponPivot;
    // [SerializeField]를 붙여주는 이유는 private은 외부에서 접근하지 못하게 해주는 녀석인데
    // [SerializeField]를 붙여주면 인스펙터창에서 할당이 가능
    // Player obj에 하위객체로 SpriteRenderer와 weaponPivot이 있음에도 할당할 수 있게 만들어 주는 이유는
    // 유니티는 계층 구조를 자동으로 추적하지 않아서 이다
    // 자식 오브젝트의 컴포넌트를 찾으려면 GetComponentInChildren<SpriteRenderer>()를 사용해야 하지만, 성능 저하가 발생할 수 있습니다.
    // 이렇게 쓰면 좋은 이유는 에디터에서 컴포넌트 누락을 즉시 발견할 수 있고, 게임 실행 전에 참조 오류를 사전 방지합니다.
    // 그럼 GetComponentInChildren()은 언제 쓰냐면
    // 동적 생성 오브젝트: 런타임에 생성되는 프리팹.
    // 자주 변경되는 구조: 프로토타입 단계에서 임시로 사용.
    // 성능 영향이 없는 경우: 간단한 데모 프로젝트.

    protected Vector2 movementDirection = Vector2.zero;
    // protected movementDirection은 캐릭터의 현재 이동 방향을 저장하는 protected 변수
    // Vector2.zero는 (0, 0)으로 초기화 해주어 게임 시작 시에 캐릭터는 정지 상태로 만들어준다는 말
    public Vector2 MovementDirection { get { return movementDirection; } }
    // Property로 변수의 값을 안전하게 읽고 쓰기위한 C# 기능으로 get만 있으면 읽기 전용이다.
    // 이렇게 작성하는 이유는 외부 스크립트가 movementDirection의 값을 읽을수만 있고 수정은 불가능케 하게끔이고
    // public Vector2 movementDirection;로 쓰지 않은 이유는 별수를 public으로 노출하면 외부에서 마음대로 덮어쓸 수 있다.
    // 그러므로 읽기 전용으로 공개를 하면 안정성이 높아진다.

    protected Vector2 lookDirection = Vector2.zero;// 현재 바라보는 방향
    public Vector2 LookDirection { get { return lookDirection; } }

    private Vector2 knockback = Vector2.zero; //넉백 방향
    private float knockbackDuration = 0.0f; // 넉백 지속 시간

    protected AnimationHandler animationHandler;
    protected StatHandler statHandler;

    [SerializeField] private WeaponHandler WeaponPrefab;
    protected WeaponHandler weaponHandler;

    protected bool isAttacking;
    private float timeSinceLastAttack = float.MaxValue;

    


    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<AnimationHandler>(); // 위와 같음
        statHandler = GetComponent<StatHandler>();// 위와 같음 스크립트라도 컴포넌트로 추가되는 거기때문에 같은거임

        if (WeaponPrefab != null)
            weaponHandler = Instantiate(WeaponPrefab, weaponPivot);
        else
            weaponHandler = GetComponentInChildren<WeaponHandler>(true);
    }
    // virtual : 자식 클래스의 확장 허용, 부모 class의 Awake()를 필요에 따라 개조할 수 있게 한다.
    // Rigidbody2D 컴포넌트와 연결해주는 기능 - 게임 오브젝트 : 차, Rigidbody2D : 엔진, _rigidbody : 키
    
    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleAction();
        Rotate(lookDirection);
        HandleAttackDelay();

    }

    protected virtual void FixedUpdate()
    {
        Movement(movementDirection);
        if (knockbackDuration > 0.0f)
            knockbackDuration -= Time.fixedDeltaTime; // 넉백 시간 감소
    }

    protected virtual void HandleAction()
    {

    }

    protected void Movement(Vector2 direction)
    {
        direction = direction * statHandler.Speed; // 이동 속도

        if (knockbackDuration > 0.0f) // 넉백 지속 시간 중이면
        {
            direction *= 0.2f; // 원래 이동속도의 0.2배
            direction += knockback; // 넉백 방향으로 이동
        }

        _rigidbody.velocity = direction; // 위에서 계산한 값들로 실제 물리 이동 적용
        animationHandler.Move(direction); // AnimationHandler 스크립트의 Move함수를 불러오고 direction 매개변수를 넣어줌
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f; 
        // Mathf.Abs 는 절대값을 의미하고 위에서 Mathf.Rad2Deg가 Radius를 Degree로 바꿔주는 것으로 절대값 90도 보다 크면 왼쪽이라는 뜻이다.

        playerSpriteRenderer.flipX = isLeft; 
        // 90도 이상이 isLeft가 true, playerSpriteRenderer의 flipX가 true가 되면 인스펙터창의 flipX가 체크가 되고 좌우반전이 일어난다.

        if (weaponPivot != null)
        {
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
        // 무기 회전 처리, Transform.rotation은 Quaternion 타입만 받고 Euler() 메서드가 변환에 편리한 함수
        // x,y값이 0인 이유는 z축을 고정하고 축이동을 해야 2D화면에서 제대로 보이기 때문(손으로 축을만들고 돌려가며 이해함)
        // 전체 흐름
        // 위치를 감지했는데 좌표가 (3,4)이면 float rotZ = Mathf.Atan2 (4, 3) * Mathf.Rad2Deg; 이 되고 이 값은 계산 상 53.13도
        // 그럼 weaponPivot 이 null이 아니면 weaponPivot.rotation = Quaternion.Euler(0f, 0f, 53.13f); 이 되고 53.13도로 회전
        // 주의 : weaponPivot.localRotation을 쓰면 부모 기준으로 회전합니다. 보통은 rotation을 사용해 월드 기준 회전을 적용합니다.

        weaponHandler?.Rotate(isLeft);
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        knockback = -(other.position - transform.position).normalized * power;
        // -(other.position - transform.position) 쉽게 말해 (적 -> 플레이어) 방향벡터를 생성하고
        // -를 붙여주어 방향이 플레이어 -> 적으로 밀려나는 방향이 됨 반대도 같음. normalized는 무슨 값이 나오든 1의 값으로 만들어 줌
        // 적이 플레이어를 공격한다 치면 그 값을 예로 player.ApplyKnockback(tranform, 8f, 0.5f); 나오면 
        // Transform other 는 같이 방향을 결정해주는 상대 오브젝트 적이나 플레이어
        // power는 계산된 벡터값을 1로 만들어준 후 입력된 수치만큼 밀려남
        // 받은 duration을 knockbackDuration 에 넣어주고 Update()에서 처리해줌
    }

    private void HandleAttackDelay()
    {
        if (weaponHandler == null)
            return;

        if (timeSinceLastAttack <= weaponHandler.Delay)
            timeSinceLastAttack += Time.deltaTime;

        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            Attack();
        }
    }

    protected virtual void Attack()
    {
        if (weaponHandler == null)
        {
            return;
        }

        if (!weaponHandler.CanAttack())
        {
            return;
        }

        weaponHandler.Attack();
    }

    public virtual void Death()
    {
        _rigidbody.velocity = Vector3.zero;

        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        Destroy(gameObject, 1.5f);
    }
}
