using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Attack Info")] // 이렇게 파라미터가 많은 경우 묶음으로 관리하고 찾기 편하게 하기 위해, 그리고 분업시 효율이 좋다. 파라미터가 많은 경우 필수적으로 사용
    [SerializeField] private float delay = 1f; // 공격 쿨타임
    public float Delay { get => delay; set => delay = value; }

    [SerializeField] private float weaponSize = 1f; // 무기 크기 배율
    public float WeaponSize { get => weaponSize; set => weaponSize = value; }

    [SerializeField] private float power = 1f; // 데미지 배수
    public float Power { get => power; set => power = value; }

    [SerializeField] private float speed = 1f; // 투사체/공격속도
    public float Speed { get => speed; set => speed = value; }

    [SerializeField] private float attackRange = 10f; // 사정거리
    public float AttackRange { get => attackRange; set => attackRange = value; }

    public LayerMask target;

    [Header("Knock Back Info")]
    [SerializeField] private bool isOnKnockback = false; // 넉백 활성화 여부
    public bool IsOnKnockback { get => isOnKnockback; set => isOnKnockback = value; }

    [SerializeField] private float knockbackPower = 0.1f; // 넉백되는 힘
    public float KnockbackPower { get => knockbackPower; set => knockbackPower = value; }

    [SerializeField] private float knockbackTime = 0.5f; // 넉백 지속 시간
    public float KnockbackTime { get => knockbackTime; set => knockbackTime = value; }

    private static readonly int IsAttack = Animator.StringToHash("IsAttack"); // 애니메이터의 파라미터에 접근, 스크립트에서 사용 가능

    public BaseController Controller { get; private set; }
    

    private Animator animator;
    private SpriteRenderer weaponRenderer;

    private bool isWeaponVisible = true;

    //[Header("Wave Scaling")]
    //[SerializeField] private bool scaleWithWave = true;
    //[SerializeField] private float waveScaleFactor = 0.1f;

    protected virtual void Awake()
    {
        Controller = GetComponentInParent<BaseController>();
        animator = GetComponentInChildren<Animator>();
        weaponRenderer = GetComponentInChildren<SpriteRenderer>();

        animator.speed = 1.0f / delay; // 공격속도와 애니메이션의 동기화
        transform.localScale = Vector3.one * weaponSize; // 무기 크기 시각적 조정

    }

    protected virtual void Start() // 같은 클래스나 자식클래스에서만 override가능하다. 기본 구현을 제공하되, 자식에서 확장이 가능한 메서드 방식
    {

    }


    public virtual void Attack() // 어떤 클래스에서든 접근 가능하고 자식 클래스는 재정의 가능하다
    {
        AttackAnimation();
    }

    public void AttackAnimation() // public void 함수는 외부, 자식 모두 접근 가능하지만 누구도 재정의 가 불가능 -> 원본 동작 보장 필요시 사용 -> 강제성
    {
        animator.SetTrigger(IsAttack);
    }

    public virtual void Rotate(bool isLeft)
    {
        weaponRenderer.flipY = isLeft;
    }

    public void ToggleWeapon()
    {
        isWeaponVisible = !isWeaponVisible;

        // 렌더러와 게임오브젝트 상태 동기화
        weaponRenderer.enabled = isWeaponVisible;
        gameObject.SetActive(isWeaponVisible); // 주의: 이렇게 하면 Update() 등이 비활성화됨

    }

    public bool CanAttack()
    {
        // 모든 상태 일치 확인
        bool canAttack = isWeaponVisible &&
                        weaponRenderer != null &&
                        weaponRenderer.enabled &&
                        gameObject.activeSelf;

        return canAttack;
    }
}
