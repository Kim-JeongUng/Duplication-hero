using System.IO;
using UnityEngine;
using System.Collections;

public class Dragon : WalkingEnemy
{
	[SerializeField] EnemyAimer aimer;
	public GameObject Skill;

	float lastShootTime;
	float lastSkillTime;
	protected new void Awake()
	{
		base.Awake();
		isBossMonster = true;
		if (aimer == null)
			aimer = GetComponentInChildren<EnemyAimer>();
		StartCoroutine(MonsterRoutine());
	}

	protected void Update()
	{
		if (aimer.Target != null)
		{
			walkingState = MovingState.STAYING;
			aimer.FollowTarget();

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
		DataManager.instance.UserGetItem(Randitem);
		DropItem(Randitem);
	}
	IEnumerator MonsterRoutine()
    {
		while (hp > 0)
		{
			yield return new WaitForSeconds(skillcool);
			RandomSkill();
		}
    }
	public void RandomSkill()
    {
		int RanNum = Random.Range(0, 2);
		switch (RanNum)
        {
			case 0:
				StartCoroutine(TeleportSkill());
				break;
			case 1:
				StartCoroutine(MonsterSkill());
				break;
			default:
				break;
		}
    }

	IEnumerator TeleportSkill()
    {
		var tempPlayerPos = player.value.transform.position;
		GameObject Item = Instantiate(Resources.Load<GameObject>(string.Format("Prefabs/Warning")), tempPlayerPos, Quaternion.identity);
		yield return new WaitForSeconds(1f);
		Destroy(Item);
		this.transform.position = tempPlayerPos;
	}
	IEnumerator MonsterSkill()
    {
		Skill.SetActive(true);
		yield return new WaitForSeconds(1f);
		Skill.SetActive(false);
	}
}
