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

    [SerializeField] private int numberofProjectilesPerShot; // 1회 발사 갯수
    public int NumberofProjectilesPerShot { get { return numberofProjectilesPerShot; } }

    [SerializeField] private float multipleProjectileAngle; // 발사체 각도
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
            // 수치를 예로 들면 numberOfProjectilesPerShot값이 5고 projectileAngleSpace이 30이면
            // float minAngle 여기서 시작각도가 형성되고 for문으로 들어오면 각 계산하면 -60, -30, 0, 30, 60 의 각도로 발사
            // float randomSpread = Random.Range(-spread, spread);
            // angle += randomSpread; 여기서 약간의 랜덤 흔들림이 추가되면서 좀더 역동적인 느낌을 준다.
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
        // Quaternion.Euler는 유니티 내부에 이미 회전 계산을 알아서 처리해 주는 준비된 기능이다.
        // 각도를 삼각함수를 사용해서 직접계산하는것보다 최적화됨
        // 게임 오브젝트가 특정 각도만큼 방향을 틀 때 반드시 필요한 핵심도구로 자주 사용된다.
    }

    //public void ApplyWaveUpgrade(int waveLevel)
    //{
    //    Delay = Mathf.Max(0.1f, baseDelay - (waveLevel * attackSpeedIncreasePerLevel));
    //    Power = basePower + (waveLevel * damageIncreasePerLevel);
    //    numberofProjectilesPerShot = baseProjectileCount + (waveLevel * extraProjectilesPerLevel);

    //}
}
