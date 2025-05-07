using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatHandler : MonoBehaviour
{
    [Range(1, 100)][SerializeField] private int health = 10;
    // �ν����Ϳ��� 1~100���̸� ���� ����(�����̴� �� ����)
    public int Health{
        get => health; 
        set => health = Mathf.Clamp(value, 0, 100); // ���� ���� 0~100���� ���� (������ �ʰ� ����)
    }

    [Range(1f, 20f)][SerializeField] private float speed = 3f;
    public float Speed{
        get => speed;
        set => speed = Mathf.Clamp(value, 0, 20);
    }
    // ������Ƽ�� ���� �ݰ� �ý��� ���� ��
    // ���� health = 9999 �� ���ٰ� �ϸ� ������ ���� �����̱⶧���� ������ ����� ����
    // Health = 9999 �� ���� ��ü������ ������ �Ǳ⶧���� Clamp�� ���� 0�̶�� ���� ����
}
