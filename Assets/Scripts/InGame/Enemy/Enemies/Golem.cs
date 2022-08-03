using UnityEngine;

public class Golem : WalkingEnemy
{
	[SerializeField] EnemyAimer aimer;
	[SerializeField] private Animator anim;

	protected new void Awake()
	{
		base.Awake();
		anim = GetComponent<Animator>();

		isBossMonster = true;
		if (aimer == null)
			aimer = GetComponentInChildren<EnemyAimer>();
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

			if (base.isdanger == false)  // 위험표시 비 실행 중일때만 캐릭터 따라다님
			{
				aimer.FollowTarget();
			}
			if (isUseSkillState == true)
			{
				anim.SetTrigger("attack01");
			}
			//EnemySkill();

			/*else if (Time.time - lastShootTime >= (1 / attackSpeed))  // 몬스터 일반공격
			{
				lastShootTime = Time.time;
				shooter.Shoot(new DamageReport(damage, this));
			}*/
		}
	}

	protected override void Death(Entity killer)
	{
		Debug.Log("------------Golem Dead-------------");
		
		// 확률에따라 스킬구슬, 장비아이템, 꽝 3가지 중에서 랜덤 실행

		base.Death(killer);
		DropItem(getSkill); // 스킬아이템 드랍
	}
}
