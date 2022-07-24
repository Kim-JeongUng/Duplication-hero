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
    public bool canUseSkill = false;

    public LayerMask layerMask;  // 공격표시레이어
    public GameObject DangerMarker;  // 트레일 렌더러

    public GameObject LaserEffect;  // 라인렌더러
    float attackTime = 5f;
    float attackTimeCalc = 5f;

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
    protected override void Awake()
    {
        base.Awake();
        if (!isBossMonster)
        { //보스몹은 스킬 여러개라 각자 스크립트에서 처리
            StartCoroutine(UseEnemySkill());
            //StartCoroutine(LaserOff());  // 스킬 사용 후 LaserEffect를 off시킨다
        }
            
    }

    protected override void Death(Entity killer)  // 몬스터 Death 처리
    {
        Player player = killer as Player;
        if(player!=null)
            player.AddCoins(coinsToDrop);
        if (isBossMonster)
        {
            GameController.instance.OpenResultPannel();
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
    /*
    IEnumerator LaserOff()  // 레이저가 켜져있다면 5초 후에 꺼지도록 코루틴
    {
        while (true)
        {
            yield return null;
            if (LaserEffect.activeInHierarchy)
            {
                attackTimeCalc -= Time.deltaTime;
                if(attackTimeCalc <= 0)
                {
                    attackTimeCalc = attackTime;
                    LaserEffect.SetActive(false);
                }
            }
        }
    }*/
    public IEnumerator UseEnemySkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(skillcool);
            LaserEffect.SetActive(true);  // 위험표시 활성화

            if (hp <= 0)
                break;

            //yield return new WaitForSeconds(1f); // 스킬 쿨타임이 됐으면 1초뒤 dangermark를 활성화한다.
            //StartCoroutine(SetTarget());  // LaserEffect를 활성화 한 다음에 플레이어를 바라보도록 한다

            yield return new WaitForSeconds(2f);  // dangermark 표시되고 1.5초뒤에 스킬을 발사
            EnemySkill();

            Debug.Log("스킬사용");
        }
    }
    /*
    IEnumerator SetTarget()
    {
        LaserEffect.SetActive(true);
        yield return null;
        while (true)
        {
            yield return null;
            // 플레이어를 바랄보고 있지 않으면 break
            if (!LookAtTarget) break;

            // 플레이어의 위치를 바라보도록
            transform.LookAt(player.transform.position);
        }
    }*/


    public void EnemySkill()
    {
        bool isPassive = false;
        int num = GameManager.instance.gameData.SkillNameSet.IndexOf(getSkill.name);
        Debug.Log(num);
        num = num == -1 ? 0 : num;
        switch (GameManager.instance.gameData.SkillResource[num].name)  // 획득한 스킬의 이벤트 발생
        {
            case "Barrier":
                StartCoroutine(Invincible());
                isPassive = true;
                break;
            case "Healing":
                hp += 30;
                isPassive = true;
                break;
            default:
                break;
        }
        skill = isPassive ? Instantiate(GameManager.instance.gameData.SkillResource[num], this.transform.position, this.transform.rotation, this.transform) : Instantiate(GameManager.instance.gameData.SkillResource[num], this.transform.position, this.transform.rotation);

        foreach (MultipleObjectsMake c in skill.GetComponentsInChildren<MultipleObjectsMake>())
        {
            c.Attacker = this;
        }
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
            touchingPlayer = player;
            player.TakeDamage(new DamageReport(damage * touchDamageMultiplier, this));
            
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
    public IEnumerator Invincible(float time = 1f) //무적
    {
        isInvincible = true;
        yield return new WaitForSeconds(time);
        isInvincible = false;
    }
}
