using System.IO;
using UnityEngine;

public class Archer : WalkingEnemy
{
	[SerializeField] Shooter shooter;
	[SerializeField] EnemyAimer aimer;
	float lastShootTime;

	//드랍아이템 코드
	public GameObject itemPrefab;  // 스킬드랍 버블 아이템
	public System.Action onDie;
	private Rigidbody rb;

	public GameObject getSkill;  // Archer가 보유한 스킬


	protected override void Death(Entity killer)
	{
		shooter.Dispose();
		// 확률에따라 스킬구슬, 장비아이템, 꽝 3가지 중에서 랜덤 실행
		this.DropItem();  // 스킬아이템 드랍

		base.Death(killer);
		this.onDie();
	}
	public void DropItem()
	{
		// bubble
		var itemGo = Instantiate<GameObject>(this.itemPrefab);
		itemGo.transform.position = this.gameObject.transform.position;

		// Skill Image
		//skillitem = Instantiate(Resources.Load<Sprite>("Skills/Fire"), itemGo.gameObject.transform.position, Quaternion.Euler(70, 0, 0));
		//skillitem.(GameObejct)sprite.SetParent(itemGo.transform, false);

		// 스킬이미지 생성해서 버블에 넣는거
		//itemPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/Bubble"));  // 버블생성
		//getSkill.gameObject.transform.GetChild(0).gameObject.SetActive(true);  // 스킬이미지 활성화

		rb = itemGo.GetComponent<Rigidbody>();
		rb.AddForce(transform.up * 5f, ForceMode.Impulse);
		//itemGo.SetActive(false);
		this.onDie = () =>
		{
			itemGo.SetActive(true);
		};
	}

	protected new void Awake()
	{
		base.Awake();
		
		if (shooter == null)
			shooter = GetComponentInChildren<Shooter>();
		if (aimer == null)
			aimer = GetComponentInChildren<EnemyAimer>();
	}

	protected void Update()
	{
		if (aimer.Target != null)
		{
			walkingState = MovingState.STAYING;
			aimer.FollowTarget();
			if (Time.time - lastShootTime >= (1 / attackSpeed))
			{
				lastShootTime = Time.time;
				shooter.Shoot(new DamageReport(damage, this));
			}
		}
	}
	protected new void FixedUpdate()
	{
		base.FixedUpdate();
		if(walkingState == MovingState.STAYING)
		{
			if (aimer.Target == null)
				aimer.Aim();
			else if (!aimer.IsVisible())
				aimer.ResetTarget();
		}
		
	}
}
