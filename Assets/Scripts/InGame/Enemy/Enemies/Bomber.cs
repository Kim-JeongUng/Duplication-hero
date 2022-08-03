using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : WalkingEnemy
{
	[SerializeField] EnemyAimer aimer;
	[SerializeField] private Animator anim;

	public GameObject bomb;  // 던질 폭탄 오브젝트


	public float lastShootTime;
	protected new void Awake()
	{
		base.Awake();

		if (aimer == null)
			aimer = GetComponent<EnemyAimer>();
		if (anim == null)
			anim = GetComponent<Animator>();
		//StartCoroutine(MonsterRoutine());
	}
	public override void Animation_Run(bool isRun)
	{
		anim.SetBool("isRun", isRun);
	}

	protected void Update()
	{
		if (aimer.Target != null)
		{
			walkingState = MovingState.STAYING;
			aimer.FollowTarget();

			if (isUseSkillState == true)
			{
				anim.SetTrigger("attack01");
			}

			//EnemySkill();

			/*if (Time.time - lastShootTime >= (1 / skillcool))  // 몬스터 일반공격
			{
				lastShootTime = Time.time;
				RandomSkill();
				//shooter.Shoot(new DamageReport(damage, this));
			}*/
		}
	}
	protected new void FixedUpdate()
	{
		base.FixedUpdate();
		if (walkingState == MovingState.STAYING)
		{
			if (aimer.Target == null)
				aimer.Aim();
			else if (!aimer.IsVisible())
				aimer.ResetTarget();
		}
	}
	protected override void Death(Entity killer)
	{
		Debug.Log("------------Bomber Dead-------------");

		base.Death(killer);
		DropItem(getSkill); // 스킬아이템 드랍
	}
}
