using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatHandler : MonoBehaviour
{
    [Range(1, 100)][SerializeField] private int health = 10;
    // 인스펙터에서 1~100사이만 선택 가능(슬라이더 바 생성)
    public int Health{
        get => health; 
        set => health = Mathf.Clamp(value, 0, 100); // 실제 값은 0~100으로 고정 (음수나 초과 방지)
    }

    [Range(1f, 20f)][SerializeField] private float speed = 3f;
    public float Speed{
        get => speed;
        set => speed = Mathf.Clamp(value, 0, 20);
    }
    // 프로퍼티는 은행 금고 시스템 같은 것
    // 예로 health = 9999 로 쓴다고 하면 변수에 직접 접근이기때문에 문제가 생길수 있음
    // Health = 9999 로 쓰면 자체적으로 검증이 되기때문에 Clamp에 의해 0이라는 값이 나옴
}
