using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("IsMove");
    private static readonly int IsDamage = Animator.StringToHash("IsDamage");
    // StringToHash�� ������ ���ڿ�("IsMove", "IsDamage")�� ������ �ؽð����� ��ȯ, �ִϸ����� �Ķ���� ���� �� ���ڿ� �񱳺��� 300% ���� ���� ����

    protected Animator animator; // �ٸ����� �յ��� ���ϰ� Ȯ�强�� ���� protected

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        // ������ ���� ������ ���� �Ұ��ϱ⿡ ����ֵ� ã���ִ� GetComponentInChildren�� ����ϸ� �������濡 �����ϰ� �������� �ö󰣴�
        // (������ Animator ��ġ �Ű澲���ʾƵ� �ǰ� �����հ����� ������� ������ �����ϴ�)
    }

    public void Move(Vector2 obj)
    {
        animator.SetBool(IsMoving, obj.magnitude > 0.5f);
        // obj - �Ϲ������� Input.GetAxisRaw �� ���� �̵� �Է� ���Ͱ�
        // obj.magnitude > 0.5f �� ������ ���̰� 0.5���� Ŭ���ε� �̴� �Ӱ谪���� ������ �����̰� �ִ� �Ǵ��� �ּ� �̵���
    }

    public void Damage()
    {
        animator.SetBool(IsDamage, true);
    }

    public void InvincibilityEnd()
    {
        animator.SetBool(IsDamage, false); 
        // ��κ��� �׼� ���ӿ��� ���Ǵ� ���� �������� ���� �����ð��� �ǹ��ϴµ� Ȯ�强�� ���� ������ ������ �ִϸ��̼��� ������ ���ƿ��� ����
    }
}
