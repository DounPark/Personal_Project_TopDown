using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;
    // �̰� �ۼ����ִ� ������ �� ��ũ��Ʈ�� ���̴� ������Ʈ�� rigidbody2d ������Ʈ�� �پ��ְ�
    // �ٸ� ��ũ��Ʈ�� ������ �޴� �ٸ� ������Ʈ�� ������ ������ ���� �ʱ� ���� �ۼ�
    // (protected�� �ν����Ϳ� �Ҵ���� ������ �ڽ� Ŭ�������� ��� ����)

    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Transform weaponPivot;
    // [SerializeField]�� �ٿ��ִ� ������ private�� �ܺο��� �������� ���ϰ� ���ִ� �༮�ε�
    // [SerializeField]�� �ٿ��ָ� �ν�����â���� �Ҵ��� ����
    // Player obj�� ������ü�� SpriteRenderer�� weaponPivot�� �������� �Ҵ��� �� �ְ� ����� �ִ� ������
    // ����Ƽ�� ���� ������ �ڵ����� �������� �ʾƼ� �̴�
    // �ڽ� ������Ʈ�� ������Ʈ�� ã������ GetComponentInChildren<SpriteRenderer>()�� ����ؾ� ������, ���� ���ϰ� �߻��� �� �ֽ��ϴ�.
    // �̷��� ���� ���� ������ �����Ϳ��� ������Ʈ ������ ��� �߰��� �� �ְ�, ���� ���� ���� ���� ������ ���� �����մϴ�.
    // �׷� GetComponentInChildren()�� ���� ���ĸ�
    // ���� ���� ������Ʈ: ��Ÿ�ӿ� �����Ǵ� ������.
    // ���� ����Ǵ� ����: ������Ÿ�� �ܰ迡�� �ӽ÷� ���.
    // ���� ������ ���� ���: ������ ���� ������Ʈ.

    protected Vector2 movementDirection = Vector2.zero;
    // protected movementDirection�� ĳ������ ���� �̵� ������ �����ϴ� protected ����
    // Vector2.zero�� (0, 0)���� �ʱ�ȭ ���־� ���� ���� �ÿ� ĳ���ʹ� ���� ���·� ������شٴ� ��
    public Vector2 MovementDirection { get { return movementDirection; } }
    // Property�� ������ ���� �����ϰ� �а� �������� C# ������� get�� ������ �б� �����̴�.
    // �̷��� �ۼ��ϴ� ������ �ܺ� ��ũ��Ʈ�� movementDirection�� ���� �������� �ְ� ������ �Ұ����� �ϰԲ��̰�
    // public Vector2 movementDirection;�� ���� ���� ������ ������ public���� �����ϸ� �ܺο��� ������� ��� �� �ִ�.
    // �׷��Ƿ� �б� �������� ������ �ϸ� �������� ��������.

    protected Vector2 lookDirection = Vector2.zero;// ���� �ٶ󺸴� ����
    public Vector2 LookDirection { get { return lookDirection; } }

    private Vector2 knockback = Vector2.zero; //�˹� ����
    private float knockbackDuration = 0.0f; // �˹� ���� �ð�

    protected AnimationHandler animationHandler;
    protected StatHandler statHandler;

    [SerializeField] private WeaponHandler WeaponPrefab;
    protected WeaponHandler weaponHandler;

    protected bool isAttacking;
    private float timeSinceLastAttack = float.MaxValue;

    


    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<AnimationHandler>(); // ���� ����
        statHandler = GetComponent<StatHandler>();// ���� ���� ��ũ��Ʈ�� ������Ʈ�� �߰��Ǵ� �ű⶧���� ��������

        if (WeaponPrefab != null)
            weaponHandler = Instantiate(WeaponPrefab, weaponPivot);
        else
            weaponHandler = GetComponentInChildren<WeaponHandler>(true);
    }
    // virtual : �ڽ� Ŭ������ Ȯ�� ���, �θ� class�� Awake()�� �ʿ信 ���� ������ �� �ְ� �Ѵ�.
    // Rigidbody2D ������Ʈ�� �������ִ� ��� - ���� ������Ʈ : ��, Rigidbody2D : ����, _rigidbody : Ű
    
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
            knockbackDuration -= Time.fixedDeltaTime; // �˹� �ð� ����
    }

    protected virtual void HandleAction()
    {

    }

    protected void Movement(Vector2 direction)
    {
        direction = direction * statHandler.Speed; // �̵� �ӵ�

        if (knockbackDuration > 0.0f) // �˹� ���� �ð� ���̸�
        {
            direction *= 0.2f; // ���� �̵��ӵ��� 0.2��
            direction += knockback; // �˹� �������� �̵�
        }

        _rigidbody.velocity = direction; // ������ ����� ����� ���� ���� �̵� ����
        animationHandler.Move(direction); // AnimationHandler ��ũ��Ʈ�� Move�Լ��� �ҷ����� direction �Ű������� �־���
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f; 
        // Mathf.Abs �� ���밪�� �ǹ��ϰ� ������ Mathf.Rad2Deg�� Radius�� Degree�� �ٲ��ִ� ������ ���밪 90�� ���� ũ�� �����̶�� ���̴�.

        playerSpriteRenderer.flipX = isLeft; 
        // 90�� �̻��� isLeft�� true, playerSpriteRenderer�� flipX�� true�� �Ǹ� �ν�����â�� flipX�� üũ�� �ǰ� �¿������ �Ͼ��.

        if (weaponPivot != null)
        {
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
        // ���� ȸ�� ó��, Transform.rotation�� Quaternion Ÿ�Ը� �ް� Euler() �޼��尡 ��ȯ�� ���� �Լ�
        // x,y���� 0�� ������ z���� �����ϰ� ���̵��� �ؾ� 2Dȭ�鿡�� ����� ���̱� ����(������ ��������� �������� ������)
        // ��ü �帧
        // ��ġ�� �����ߴµ� ��ǥ�� (3,4)�̸� float rotZ = Mathf.Atan2 (4, 3) * Mathf.Rad2Deg; �� �ǰ� �� ���� ��� �� 53.13��
        // �׷� weaponPivot �� null�� �ƴϸ� weaponPivot.rotation = Quaternion.Euler(0f, 0f, 53.13f); �� �ǰ� 53.13���� ȸ��
        // ���� : weaponPivot.localRotation�� ���� �θ� �������� ȸ���մϴ�. ������ rotation�� ����� ���� ���� ȸ���� �����մϴ�.

        weaponHandler?.Rotate(isLeft);
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        knockback = -(other.position - transform.position).normalized * power;
        // -(other.position - transform.position) ���� ���� (�� -> �÷��̾�) ���⺤�͸� �����ϰ�
        // -�� �ٿ��־� ������ �÷��̾� -> ������ �з����� ������ �� �ݴ뵵 ����. normalized�� ���� ���� ������ 1�� ������ ����� ��
        // ���� �÷��̾ �����Ѵ� ġ�� �� ���� ���� player.ApplyKnockback(tranform, 8f, 0.5f); ������ 
        // Transform other �� ���� ������ �������ִ� ��� ������Ʈ ���̳� �÷��̾�
        // power�� ���� ���Ͱ��� 1�� ������� �� �Էµ� ��ġ��ŭ �з���
        // ���� duration�� knockbackDuration �� �־��ְ� Update()���� ó������
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
