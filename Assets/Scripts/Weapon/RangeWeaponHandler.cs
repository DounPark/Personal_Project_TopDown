using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RangeWeaponHandler : WeaponHandler
{
    [Header("Ranged Attack Data")]
    [SerializeField] private Transform projectileSpawnPosition;

    [SerializeField] private int bulletIndex;
    public int BulletIndex { get { return bulletIndex; } }

    [SerializeField] private float bulletSize = 1f;
    public float BulletSize { get { return bulletSize; } }

    [SerializeField] private float duration;
    public float Duration { get { return duration; } }

    [SerializeField] private float spread;
    public float Spread { get { return spread; } }

    [SerializeField] private int numberofProjectilesPerShot; // 1ȸ �߻� ����
    public int NumberofProjectilesPerShot { get { return numberofProjectilesPerShot; } }

    [SerializeField] private float multipleProjectileAngle; // �߻�ü ����
    public float MultipleProjectilesAngle { get { return multipleProjectileAngle; } }

    [SerializeField] private Color projectileColor;
    public Color ProjectileColor { get { return projectileColor; } }

    private ProjectileManager projectileManager;

    //[Header("Wave Upgrade Settings")]
    //[SerializeField] private float damageIncreasePerLevel = 0.5f;
    //[SerializeField] private float attackSpeedIncreasePerLevel = 0.1f;
    //[SerializeField] private int extraProjectilesPerLevel = 1;
    //[SerializeField] private float spreadReductionPerLevel = 0.05f;

    //private float baseDelay;
    //private float basePower;
    //private int baseProjectileCount;
    //private float baseSpread;

    protected override void Start()
    {
        base.Start();
        projectileManager = ProjectileManager.Instance;

        //baseDelay = Delay;
        //basePower = Power;
        //baseProjectileCount = numberofProjectilesPerShot;
        //baseSpread = Spread;
    }


    public override void Attack()
    {
        if (!CanAttack()) return;

        base.Attack();

        float projectileAngleSpace = multipleProjectileAngle;
        int numberOfProjectilePerShot = numberofProjectilesPerShot;

        float minAngle = -(numberOfProjectilePerShot / 2f) * projectileAngleSpace;

        for (int i = 0; i < numberOfProjectilePerShot; i++)
        {
            float angle = minAngle + projectileAngleSpace * i;
            float randomSpread = Random.Range(-spread, spread);
            angle += randomSpread;
            CreateProjectile(Controller.LookDirection, angle);
            // ��ġ�� ���� ��� numberOfProjectilesPerShot���� 5�� projectileAngleSpace�� 30�̸�
            // float minAngle ���⼭ ���۰����� �����ǰ� for������ ������ �� ����ϸ� -60, -30, 0, 30, 60 �� ������ �߻�
            // float randomSpread = Random.Range(-spread, spread);
            // angle += randomSpread; ���⼭ �ణ�� ���� ��鸲�� �߰��Ǹ鼭 ���� �������� ������ �ش�.
        }
    }

    private void CreateProjectile(Vector2 _lookDirection, float angle)
    {
        projectileManager.ShootBullet(
            this,
            projectileSpawnPosition.position,
            RotateVector2(_lookDirection, angle)
            );
    }

    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;
        // Quaternion.Euler�� ����Ƽ ���ο� �̹� ȸ�� ����� �˾Ƽ� ó���� �ִ� �غ�� ����̴�.
        // ������ �ﰢ�Լ��� ����ؼ� ��������ϴ°ͺ��� ����ȭ��
        // ���� ������Ʈ�� Ư�� ������ŭ ������ Ʋ �� �ݵ�� �ʿ��� �ٽɵ����� ���� ���ȴ�.
    }

    //public void ApplyWaveUpgrade(int waveLevel)
    //{
    //    Delay = Mathf.Max(0.1f, baseDelay - (waveLevel * attackSpeedIncreasePerLevel));
    //    Power = basePower + (waveLevel * damageIncreasePerLevel);
    //    numberofProjectilesPerShot = baseProjectileCount + (waveLevel * extraProjectilesPerLevel);

    //}
}
