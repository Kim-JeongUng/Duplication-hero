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

    //public LayerMask layerMask;  // 공격표시레이어
    //public GameObject DangerMarker;  // 트레일 렌더러

    public GameObject LaserEffect;  // 라인렌더러
    public bool UseDangerMark;  // 위험표시를 사용할 것인지
    public bool isdanger = false;  // 현재위험표시 실행중인지 체크

    public bool isUseSkillState = false;  // 현재 스킬을 쓰고 있는 상태인지 체크

    //float attackTime = 5f;
    //float attackTimeCalc = 5f;

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
        enemyHandler.componentCache.RemoveEnemy(this);
        //DeadAnim 실행
        Destroy(gameObject,0.2f);
    }

    public void DropItem(GameObject getSkill)  // 스킬 구슬 생성
    {
        var itemGo = MultipleObjectPooling.instance.GetPooledObject(getSkill.name);
        itemGo.transform.position = this.transform.position;  // 스킬구슬 생성 위치 설정

        rb = itemGo.GetComponent<Rigidbody>();
        rb.AddForce(transform.up * 5f, ForceMode.Impulse);
    }
    public void DropItem(UserItemData Randitem)  // 아이템 구슬 생성
    {
        GameObject Bubble = Resources.Load<GameObject>(string.Format("Bubble/{0}", "preset"));
        var itemGo = Instantiate(Bubble);
        itemGo.name = Randitem.ItemName;
        itemGo.transform.GetChild(0).name = Randitem.ItemName;
        itemGo.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite= Resources.Load<Sprite>(string.Format("Icons/{0}/{1}", Randitem.type,Randitem.ItemName));
        itemGo.GetComponent<presetItemdata>().itemData = Randitem;

        itemGo.transform.position = this.transform.position;  // 스킬구슬 생성 위치 설정

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
            if (LaserEffect != null)
                if (UseDangerMark == true)
                {
                    LaserEffect.SetActive(true);  // 위험표시 활성화
                    isdanger = true;
                }


            if (hp <= 0)
                break;

            //yield return new WaitForSeconds(1f); // 스킬 쿨타임이 됐으면 1초뒤 dangermark를 활성화한다.
            //StartCoroutine(SetTarget());  // LaserEffect를 활성화 한 다음에 플레이어를 바라보도록 한다

            yield return new WaitForSeconds(1f);  // dangermark 표시되고 1.5초뒤에 스킬을 발사
            EnemySkill();

            isdanger = false;

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
            case "Bomb":
                StartCoroutine(Bomb()); // 폭탄 오브젝트 던짐
                break;
            default:
                break;
        }
        // 스킬 이펙트 생성
        // 폭탄이 아닐경우에만 스킬이펙트 생성 (폭탄은 폭탄오브젝트를 던지고 오브젝트에서 이펙트 생성)

        skill = isPassive ? Instantiate(GameManager.instance.gameData.SkillResource[num], this.transform.position, this.transform.rotation, this.transform) : Instantiate(GameManager.instance.gameData.SkillResource[num], this.transform.position, this.transform.rotation);

        if (GameManager.instance.gameData.SkillResource[num].name == "Bomb")
        {
            rb = skill.GetComponent<Rigidbody>();
            rb.AddForce(this.transform.forward * 15f, ForceMode.Impulse);
        }

        isUseSkillState = true;  // 스킬 사용상태 true
        StartCoroutine(isUseSkillstate());  // 스킬 사용상태  - 일반몬스터의 공격 애니메이션 작동 관리 위함
/*
        foreach (MultipleObjectsMake c in skill.GetComponentsInChildren<MultipleObjectsMake>())
        {
            c.Attacker = this;
        }
        foreach (ObjectMove c in skill.GetComponentsInChildren<ObjectMove>())
        {
            c.Attacker = this;
        }*/
        foreach (Skills s in skill.GetComponentsInChildren<Skills>())
        {
            s.Attacker = this;
        }
        Invoke("DestroySkill", 3f);  // 사용한 스킬이펙트 삭제
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
    public IEnumerator isUseSkillstate()  // 스킬 사용상태를 변경하여 몬스터의 공격애니메이션 작동을 관리
    {
        yield return new WaitForSeconds(0.05f);

        isUseSkillState = false;  // 스킬 사용상태 false
    }
    public IEnumerator Bomb()
    {
        // 폭탄 오브젝트 던짐
        //var b = this.GetComponent<Bomber>().bomb;
        //var b = Instantiate(this.GetComponent<Bomber>().bomb, this.transform.position, this.transform.rotation);
        //b.GetComponent<Rigidbody>().AddForce(this.transform.forward * 10f, ForceMode.Impulse);
        Debug.Log("Enemy스크립트 Explosion 실행");
        yield return new WaitForSeconds(0.5f);
    }
}
