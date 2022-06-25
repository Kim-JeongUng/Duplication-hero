using UnityEngine;

public abstract class Entity : MonoBehaviour
{
	protected enum MovingState
	{
		MOVING,
		STAYING
	}

	[SerializeField] protected float speed;
	[SerializeField] protected float maxHp;
	[SerializeField] [ReadOnly] protected float hp;
	[SerializeField] protected float attackSpeed;
	[SerializeField] protected float damage;
	[SerializeField] protected float ap;
	[SerializeField] protected int coin;
	protected MovingState walkingState = MovingState.STAYING;

	//public GameObject itemPrefab;
	//public System.Action onDie;

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

	protected void Awake()
	{
		hp = maxHp;
	}

	public bool TakeDamage(DamageReport damageReport)  // 데미지 받음
	{
		hp -= damageReport.damage;
		if (hp <= 0)  // 죽은경우
		{
			//this.DropItem();
			Death(damageReport.attacker);
			//this.onDie();

			return true;
		}
		return false;
	}
	/*
	public void DropItem()
	{
		var itemGo = Instantiate<GameObject>(this.itemPrefab);
		itemGo.transform.position = this.gameObject.transform.position;
		itemGo.SetActive(false);
		this.onDie = () =>
		{
			itemGo.SetActive(true);
		};
	}*/

	/// <summary>
	/// Execute on Entity death
	/// </summary>
	/// <param name="killer">Reference to killer</param>
	protected abstract void Death(Entity killer);
}
