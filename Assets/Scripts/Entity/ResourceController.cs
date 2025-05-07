using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = 0.5f; // ü�� ��ȭ ���� ���� �ð�(���̵� ���������� ���ɼ��ִ�.���� �ǰ� �� ����

    private BaseController baseController;
    private StatHandler statHandler;
    private AnimationHandler animationHandler;

    private float timeSinceLastChange = float.MaxValue;
    // ������ ��ȭ�κ��� ����� �ð��� ����
    // float.MaxValue; �� ���� ü�� ��ȭ�� �ѹ��� �������� �ǹ�

    public float CurrentHealth {  get; private set; } // ü���� ���к��ϰ� ������� �ʵ��� ������Ƽ�� ��ȣ
    public float Maxhealth => statHandler.Health; // ���ٷ� statHandler.Health�� �ǽð����� ��ȯ��

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
        if (timeSinceLastChange < healthChangeDelay) // �ǰ����� ������ �ִϸ��̼� �۵��� �ð��� �帣�� �Ѵ�
        {
            timeSinceLastChange += Time.deltaTime;

            if (timeSinceLastChange >= healthChangeDelay) // �帥 �ð��� 0.5�ʰ� �Ǵ� �������� ������ �ִϸ��̼��� ������.
            {
                animationHandler.InvincibilityEnd();
            }
        }
        // �ִϸ��̼� ��������� ��������� Ȯ�强�� ���� �ڵ��̱⵵ �ϴ�. ���� ������ ���ϸ��̼��� �����ִµ��� ������ �Ǵ� �ڵ嵵 �̰��� ������ �ִ�.
    }

    public bool ChangeHealth(float change)
    {
        if (change == 0 || timeSinceLastChange < healthChangeDelay) // ü�� ��ȭ�� 0�̸� �ƹ��ϵ� ����.
        {  return false; }

        timeSinceLastChange = 0f;
        CurrentHealth += change;
        CurrentHealth = CurrentHealth > Maxhealth ? Maxhealth : CurrentHealth; // ���� ������ A = b < c ? d : e; ��� b�� c���� ������ e��� d�� �ְڴ�.
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;                 // ü���� ������ �ǰų� �ִ�ü���� ���� �ʰ� �����ϴ� ȿ��

        OnChangeHealth?.Invoke(CurrentHealth, Maxhealth);

        if (change < 0)
        {
            animationHandler.Damage();// change�� ü�º�ȭ���̶� ���� ���ذ� ����. ü���� ���Ѵٴ°� damage�� �Ծ����״� ���⼭ damage �ִϸ��̼��� �����ش�.
        }

        if (CurrentHealth <= 0f)
        {
            Death();
        }

        return true;
    }

    private void Death()
    {
        // �Ϲ������� ���anim, ����, ���� ����ó��, ��Ʈ�� ��Ȱ��ȭ, n���� ������ ���� ����� ���ü��ִ�.
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
