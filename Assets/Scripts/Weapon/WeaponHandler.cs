using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Attack Info")] // �̷��� �Ķ���Ͱ� ���� ��� �������� �����ϰ� ã�� ���ϰ� �ϱ� ����, �׸��� �о��� ȿ���� ����. �Ķ���Ͱ� ���� ��� �ʼ������� ���
    [SerializeField] private float delay = 1f; // ���� ��Ÿ��
    public float Delay { get => delay; set => delay = value; }

    [SerializeField] private float weaponSize = 1f; // ���� ũ�� ����
    public float WeaponSize { get => weaponSize; set => weaponSize = value; }

    [SerializeField] private float power = 1f; // ������ ���
    public float Power { get => power; set => power = value; }

    [SerializeField] private float speed = 1f; // ����ü/���ݼӵ�
    public float Speed { get => speed; set => speed = value; }

    [SerializeField] private float attackRange = 10f; // �����Ÿ�
    public float AttackRange { get => attackRange; set => attackRange = value; }

    public LayerMask target;

    [Header("Knock Back Info")]
    [SerializeField] private bool isOnKnockback = false; // �˹� Ȱ��ȭ ����
    public bool IsOnKnockback { get => isOnKnockback; set => isOnKnockback = value; }

    [SerializeField] private float knockbackPower = 0.1f; // �˹�Ǵ� ��
    public float KnockbackPower { get => knockbackPower; set => knockbackPower = value; }

    [SerializeField] private float knockbackTime = 0.5f; // �˹� ���� �ð�
    public float KnockbackTime { get => knockbackTime; set => knockbackTime = value; }

    private static readonly int IsAttack = Animator.StringToHash("IsAttack"); // �ִϸ������� �Ķ���Ϳ� ����, ��ũ��Ʈ���� ��� ����

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

        animator.speed = 1.0f / delay; // ���ݼӵ��� �ִϸ��̼��� ����ȭ
        transform.localScale = Vector3.one * weaponSize; // ���� ũ�� �ð��� ����

    }

    protected virtual void Start() // ���� Ŭ������ �ڽ�Ŭ���������� override�����ϴ�. �⺻ ������ �����ϵ�, �ڽĿ��� Ȯ���� ������ �޼��� ���
    {

    }


    public virtual void Attack() // � Ŭ���������� ���� �����ϰ� �ڽ� Ŭ������ ������ �����ϴ�
    {
        AttackAnimation();
    }

    public void AttackAnimation() // public void �Լ��� �ܺ�, �ڽ� ��� ���� ���������� ������ ������ �� �Ұ��� -> ���� ���� ���� �ʿ�� ��� -> ������
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

        // �������� ���ӿ�����Ʈ ���� ����ȭ
        weaponRenderer.enabled = isWeaponVisible;
        gameObject.SetActive(isWeaponVisible); // ����: �̷��� �ϸ� Update() ���� ��Ȱ��ȭ��

    }

    public bool CanAttack()
    {
        // ��� ���� ��ġ Ȯ��
        bool canAttack = isWeaponVisible &&
                        weaponRenderer != null &&
                        weaponRenderer.enabled &&
                        gameObject.activeSelf;

        return canAttack;
    }
}
