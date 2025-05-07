using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("IsMove");
    private static readonly int IsDamage = Animator.StringToHash("IsDamage");
    // StringToHash의 역할은 문자열("IsMove", "IsDamage")을 정수형 해시값으로 변환, 애니메이터 파라미터 접근 시 문자열 비교보다 300% 빠른 성능 제공

    protected Animator animator; // 다른데서 손데지 못하고 확장성을 위해 protected

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        // 프리팹 계층 구조는 예측 불가하기에 어디있든 찾아주는 GetComponentInChildren를 사용하면 구조변경에 강건하고 안정성이 올라간다
        // (협업시 Animator 위치 신경쓰지않아도 되고 프리팹계층을 마음대로 변경이 가능하다)
    }

    public void Move(Vector2 obj)
    {
        animator.SetBool(IsMoving, obj.magnitude > 0.5f);
        // obj - 일반적으로 Input.GetAxisRaw 로 얻은 이동 입력 벡터값
        // obj.magnitude > 0.5f 는 벡터의 길이가 0.5보다 클때인데 이는 임계값으로 실제로 움직이고 있다 판단할 최소 이동량
    }

    public void Damage()
    {
        animator.SetBool(IsDamage, true);
    }

    public void InvincibilityEnd()
    {
        animator.SetBool(IsDamage, false); 
        // 대부분의 액션 게임에서 사용되는 게임 패턴으로 보통 무적시간을 의미하는데 확장성을 위해 넣은듯 데미지 애니메이션이 끝나면 돌아오는 거임
    }
}
