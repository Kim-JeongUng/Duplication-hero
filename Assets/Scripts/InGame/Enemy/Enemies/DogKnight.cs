using System.IO;
using UnityEngine;
using System.Collections;

public class DogKnight : WalkingEnemy
{
	[SerializeField] EnemyAimer aimer;
	public GameObject Skill;
	public Animator anim;
	public GameObject Weapon;

	public float lastShootTime;
	protected new void Awake()
	{
		base.Awake();
		isBossMonster = true;
		if (aimer == null)
			aimer = GetComponentInChildren<EnemyAimer>();
		if(anim == null)
			anim = GetComponentInChildren<Animator>();
		//StartCoroutine(MonsterRoutine());
	}

	protected void Update()
	{
		if (aimer.Target != null)
		{
			aimer.FollowTarget();
			if (Vector3.Distance(aimer.Target.position, this.transform.position) < 2f)
			{
				if (Time.time - lastShootTime >= (1 / attackSpeed))
				{
					anim.PlayInFixedTime("Attack01");
					lastShootTime = Time.time;
					//shooter.Shoot(new DamageReport(damage, this)); (원거리)
				}
			}
			else
				aimer.ResetTarget();
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
	protected override void Death(Entity killer)
	{
		Debug.Log("------------Boss Dead-------------");

		base.Death(killer);
		//랜덤아이템
		UserItemData Randitem = DataManager.instance.PickRandomItem();
		//DataManager.instance.UserGetItem(Randitem); result에서 처리
		DropItem(Randitem);
	}
	public void isSlashStart() => Weapon.GetComponent<CapsuleCollider>().enabled = true;
	public void isSlashStop() => Weapon.GetComponent<CapsuleCollider>().enabled = false;
}
