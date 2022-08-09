using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : WalkingEnemy
{
	[SerializeField] EnemyAimer aimer;
	[SerializeField] private Animator anim;

	public GameObject bomb;  // ���� ��ź ������Ʈ

	public float lastShootTime;

	protected new void Awake()
	{
		base.Awake();

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
			aimer.FollowTarget();
			/*
			if (Vector3.Distance(aimer.Target.position, this.transform.position) < 10f)  // ĳ���Ϳ��� �Ÿ��� 10�����̸� ��ź ����
			{
				if (Time.time - lastShootTime >= (1 / attackSpeed))
				{
					anim.PlayInFixedTime("attack01");
					lastShootTime = Time.time;

					var b = Instantiate(bomb, this.transform.position, this.transform.rotation);
					b.GetComponent<Rigidbody>().AddForce(Vector3.up * 50f);

					//shooter.Shoot(new DamageReport(damage, this)); (���Ÿ�)
				}
			}
			else
				aimer.ResetTarget();*/
			
			if (isUseSkillState == true)
			{
				//anim.SetTrigger("attack01");
			}
			
			//EnemySkill();

			/*if (Time.time - lastShootTime >= (1 / skillcool))  // ���� �Ϲݰ���
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
		DropItem(getSkill); // ��ų������ ���
	}
}
