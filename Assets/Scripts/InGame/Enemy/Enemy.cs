using UnityEngine;
using ThirteenPixels.Soda;
using System.Collections;

public class Enemy : Entity
{
    [SerializeField] protected GlobalTransform player;
    [SerializeField] protected GlobalEnemyHandler enemyHandler;
    [SerializeField] protected GameEvent onPlayerDeath;
    [SerializeField] protected float movingTime;  // 적 이동 시간
    [SerializeField] protected float waitingTime; // 적 다운타임
    [SerializeField] protected float randomTime;  // 랜덤값이 이동 시간에 추가됩니다.
    [SerializeField] protected float touchDamageMultiplier = 1; // 접촉 시 피해
    [SerializeField] protected int coinsToDrop = 100; // 킬을 위해 얼마나 많은 동전이 떨어질 것인지
    protected Player touchingPlayer; // 터치 공격 쿨다운 후 플레이어가 여전히 효과 범위에 있는지 확인하는 데 필요

    public GameObject getSkill;  // 몬스터가 보유한 스킬구슬
    public float skillcool;  // 스킬 쿨타임
    private GameObject skill; // 스킬이펙트 오브젝트

    private Rigidbody rb;  // 아이템구슬
    public bool isBossMonster = false;
    public bool canUseSkill = true;

    protected void OnEnable()
    {
        onPlayerDeath.onRaise.AddResponse(ResetTouchingPlayer);
    }
    protected void OnDisable()
    {
        onPlayerDeath.onRaise.RemoveResponse(ResetTouchingPlayer);
    }

    private void ResetTouchingPlayer()
    {
        touchingPlayer = null;
    }

    protected override void Death(Entity killer)  // 몬스터 Death 처리
    {
        Player player = killer as Player;
        if(player!=null)
            player.AddCoins(coinsToDrop);
        if (isBossMonster)
        {
            GameController.instance.OpenResultPannel();
            Time.timeScale = 0;
        }
        enemyHandler.componentCache.RemoveEnemy(this);
        Destroy(gameObject);
    }

    public void DropItem(GameObject getSkill)  // 아이템 구슬 생성
    {
        var itemGo = MultipleObjectPooling.instance.GetPooledObject(getSkill.name);
        itemGo.transform.position = this.gameObject.transform.position;  // 스킬구슬 생성 위치 설정

        rb = itemGo.GetComponent<Rigidbody>();
        rb.AddForce(transform.up * 5f, ForceMode.Impulse);
    }

    public void EnemySkill()
    {
        if (canUseSkill == true)
        {
            // 스킬 사용
            atkSkill();
            Debug.Log("몬스터 스킬 사용");

            //StartCoroutine(CoolTime(skillcool));
            StartCoroutine("CoolTime");

            canUseSkill = false;  // 스킬을 사용하면 쿨타임으로 스킬을 사용할 수 없는 상태
        }
    }
    IEnumerator CoolTime()
    {
        var cool = skillcool;
        print("몬스터 스킬 쿨타임 실행");
        
        while (cool > 0)
        {
            cool -= Time.deltaTime;
            // fillAmount
            yield return null;
        }

        canUseSkill = true;  // 스킬 쿨타임이 끝나면 스킬을 사용할 수 있는 상태

        print("몬스터 스킬 쿨타입 종료");
        yield break;
    }
    public void atkSkill()
    {
        int num = GameManager.instance.gameData.SkillNameSet.IndexOf(getSkill.name);
        Debug.Log(num);
        num = num == -1 ? 0 : num;
        skill = Instantiate(GameManager.instance.gameData.SkillResource[num], this.transform.position, this.transform.rotation);

        Invoke("DestroySkill", 1f);  // 사용한 스킬이펙트 삭제
    }
    public void DestroySkill()  // 사용한 스킬이펙트 삭제
    {
        Destroy(skill);
    }

    protected void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            if (!player.isInvincible)
            {
                touchingPlayer = player;
                player.TakeDamage(new DamageReport(damage * touchDamageMultiplier, this));
            }
        }

        if (other.CompareTag("Weapon"))
        {
            TakeDamage(new DamageReport(GameManager.instance.player.Damage * touchDamageMultiplier, GameManager.instance.player));
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.tag == Tags.playerTag)
            touchingPlayer = null;
    }
}
