using UnityEngine;
using ThirteenPixels.Soda;
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

    [SerializeField] public MultipleObjectPooling multipleobjectpooling;

    private Rigidbody rb;  // 아이템구슬

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
        enemyHandler.componentCache.RemoveEnemy(this);
        Destroy(gameObject);
    }

    public void DropItem(GameObject getSkill)  // 아이템 구슬 생성
    {
        var itemGo = multipleobjectpooling.GetPooledObject(getSkill.name);
        itemGo.transform.position = this.gameObject.transform.position;  // 스킬구슬 생성 위치 설정

        rb = itemGo.GetComponent<Rigidbody>();
        rb.AddForce(transform.up * 5f, ForceMode.Impulse);
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
}
