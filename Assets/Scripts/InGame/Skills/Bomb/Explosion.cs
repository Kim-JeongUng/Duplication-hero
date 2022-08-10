using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Skills
{
    public GameObject bomb;
    public GameObject effect;
    public GameObject spark;

    // Bomber��ų ��ũ��Ʈ
    void Start()
    {
        StartCoroutine(explosion());
    }

    IEnumerator explosion()
    {
        spark.SetActive(true);  // ��ź ���� ����ũ Ȱ��ȭ
        // ����ũ ���� �߰��ϸ� ������

        yield return new WaitForSeconds(2f);  // 2�� ��� �� ���� ����

        spark.SetActive(false);  // ����ũ ��Ȱ��ȭ
        effect.SetActive(true);  // ���� ����Ʈ Ȱ��ȭ
        // ���� ���� �߰�

        bomb.SetActive(false);  // ��ź ������Ʈ ��Ȱ��ȭ

        StartCoroutine(destroyBomb());  // ��ź ������ ����
    }

    IEnumerator destroyBomb()
    {
        yield return new WaitForSeconds(3f);  // 3�ʵڿ� ������ ����
        Destroy(this.gameObject);
    }

}
