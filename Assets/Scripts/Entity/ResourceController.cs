using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = 0.5f; // 체력 변화 적용 지연 시간(난이도 조절용으로 사용될수있다.예로 피격 후 무적

    private BaseController baseController;
    private StatHandler statHandler;
    private AnimationHandler animationHandler;

    private float timeSinceLastChange = float.MaxValue;
    // 마지막 변화로부터 경과한 시간을 추적
    // float.MaxValue; 는 아직 체력 변화가 한번도 없었음을 의미

    public float CurrentHealth {  get; private set; } // 체력이 무분별하게 변경되지 않도록 프로퍼티로 보호
    public float Maxhealth => statHandler.Health; // 람다로 statHandler.Health를 실시간으로 반환함

    private Action<float, float> OnChangeHealth;

    private void Awake()
    {
        baseController = GetComponent<BaseController>();
        statHandler = GetComponent<StatHandler>();
        animationHandler = GetComponent<AnimationHandler>();
    }

    private void Start()
    {
        CurrentHealth = statHandler.Health;
    }

    private void Update()
    {
        if (timeSinceLastChange < healthChangeDelay) // 피격직후 데미지 애니메이션 작동후 시간이 흐르게 한다
        {
            timeSinceLastChange += Time.deltaTime;

            if (timeSinceLastChange >= healthChangeDelay) // 흐른 시간이 0.5초가 되는 시점에서 데미지 애니메이션이 꺼진다.
            {
                animationHandler.InvincibilityEnd();
            }
        }
        // 애니메이션 연출용으로 사용하지만 확장성을 위한 코드이기도 하다. 추후 데미지 에니메이션이 켜져있는동안 무적이 되는 코드도 이곳에 넣을수 있다.
    }

    public bool ChangeHealth(float change)
    {
        if (change == 0 || timeSinceLastChange < healthChangeDelay) // 체력 변화가 0이면 아무일도 없다.
        {  return false; }

        timeSinceLastChange = 0f;
        CurrentHealth += change;
        CurrentHealth = CurrentHealth > Maxhealth ? Maxhealth : CurrentHealth; // 삼항 연산자 A = b < c ? d : e; 라면 b가 c보다 작으면 e대신 d를 넣겠다.
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;                 // 체력이 음수가 되거나 최대체력을 넘지 않게 방지하는 효과

        OnChangeHealth?.Invoke(CurrentHealth, Maxhealth);

        if (change < 0)
        {
            animationHandler.Damage();// change는 체력변화량이라 보면 이해가 쉽다. 체력이 변한다는건 damage를 입었을테니 여기서 damage 애니메이션을 보여준다.
        }

        if (CurrentHealth <= 0f)
        {
            Death();
        }

        return true;
    }

    private void Death()
    {
        // 일반적으로 사망anim, 사운드, 게임 오버처리, 컨트롤 비활성화, n초후 리스폰 등의 기능이 들어올수있다.
        baseController.Death();
    }

    public void AddHealthChangeEvent(Action<float, float> action)
    {
        OnChangeHealth += action;
    }

    public void RemoveHealthChangeEvent(Action<float, float> action)
    {
        OnChangeHealth -= action;
    }
}
