using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Skills
{
    public GameObject bomb;
    public GameObject effect;
    public GameObject spark;

    // Bomber스킬 스크립트
    void Start()
    {
        StartCoroutine(explosion());
    }

    IEnumerator explosion()
    {
        spark.SetActive(true);  // 폭탄 심지 스파크 활성화
        // 스파크 사운드 추가하면 좋을듯

        yield return new WaitForSeconds(2f);  // 2초 대기 후 폭발 실행

        spark.SetActive(false);  // 스파크 비활성화
        effect.SetActive(true);  // 폭발 이펙트 활성화
        // 폭발 사운드 추가

        bomb.SetActive(false);  // 폭탄 오브젝트 비활성화

        StartCoroutine(destroyBomb());  // 폭탄 프리팹 제거
    }

    IEnumerator destroyBomb()
    {
        yield return new WaitForSeconds(3f);  // 3초뒤에 프리팹 제거
        Destroy(this.gameObject);
    }

}
