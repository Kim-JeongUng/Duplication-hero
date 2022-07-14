using System.IO;
using UnityEngine;

public class Dragon : WalkingEnemy
{
	[SerializeField] EnemyAimer aimer;

	float lastShootTime;
	float lastSkillTime;
	
	protected new void Awake()
	{
		base.Awake();
		
		if (aimer == null)
			aimer = GetComponentInChildren<EnemyAimer>();
	}

	protected void Update()
	{
		if (aimer.Target != null)
		{
			walkingState = MovingState.STAYING;
			aimer.FollowTarget();

			//EnemySkill();

			if (Time.time - lastShootTime >= (1 / attackSpeed))  // 몬스터 일반공격
			{
				lastShootTime = Time.time;
				RandomSkill();
				//shooter.Shoot(new DamageReport(damage, this));
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
	protected override void Death(Entity killer)
	{
		Debug.Log("------------Boss Dead-------------");

		base.Death(killer);

		if(OnDie == true)
			DropItem(getSkill); // 스킬아이템 드랍
	}
	public void RandomSkill()
    {
		int RanNum = Random.Range(0, 2);
		Debug.Log(RanNum);
		switch (RanNum)
        {
			case 0:
				TeleportSkill();
				break;
			case 1:	
				MonsterSkill();
				break;
			default:
				break;
		}
    }
	public void TeleportSkill()
    {
		Debug.Log("telpo");
		this.transform.position = player.value.transform.position;
    }
	public void MonsterSkill()
    {
		Debug.Log("skill");
		EnemySkill();
    }
}
