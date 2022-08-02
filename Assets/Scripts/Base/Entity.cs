using UnityEngine;

public abstract class Entity : MonoBehaviour
{
	protected enum MovingState
	{
		MOVING,
		STAYING
	}

	[SerializeField] protected float speed;
	[SerializeField] protected int maxHp;
	[SerializeField] [ReadOnly] protected int hp;
	[SerializeField] protected float attackSpeed;
	[SerializeField] protected float damage;
	[SerializeField] protected float ap;
	[SerializeField] protected int coin;
	public bool isInvincible = false;
	protected MovingState walkingState = MovingState.STAYING;

	//public GameObject itemPrefab;
	public bool OnDie = false;

	public float Speed
	{
		get { return speed; }
	}
	public float MaxHp
	{
		get { return maxHp; }
	}
	public float Hp
	{
		get { return hp; }
	}
	public float AttackSpeed
	{
		get { return attackSpeed; }
	}
	public float Damage
	{
		get { return damage; }
	}
	public float Ap
	{
		get { return ap; }
	}
	public float Coin
	{
		get { return coin; }
	}

	protected virtual void Awake()
	{
		if (CompareTag("Enemy"))
		{
			maxHp += (int)(maxHp * GameManager.instance.gameData.nowChapter * 0.1f);
			damage += (int)(damage * GameManager.instance.gameData.nowChapter * 0.1f);
			coin += (int)(coin * GameManager.instance.gameData.nowChapter * 0.1f);
		}
		hp = maxHp;
	}

	public bool TakeDamage(DamageReport damageReport)  // 데미지 받음
	{
		if (!isInvincible)
		{
			hp -= (int)damageReport.damage;
			if (hp <= 0 && OnDie == false)  // 죽은경우
			{
				Death(damageReport.attacker);
				OnDie = true;
				return true;
			}
			return false;
		}
        else
        {
			Debug.Log("무적");
			return false;
        }
	}

	/// <summary>
	/// Execute on Entity death
	/// </summary>
	/// <param name="killer">Reference to killer</param>
	protected abstract void Death(Entity killer);
}
