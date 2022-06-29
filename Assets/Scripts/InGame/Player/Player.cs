﻿using UnityEngine;
using ThirteenPixels.Soda;
using UnityEngine.UI;
using System;

public class Player : Entity
{
	[SerializeField] bool isEditorMode;
	[SerializeField] GameEvent onPlayerDeath;
	[SerializeField] GlobalVector2 input;
	[SerializeField] Shooter shooter;
	[SerializeField] PlayerAimer aimer;
	[SerializeField][ReadOnly] long coins = 0;
	public Image SkillImage;
	public Sprite DefaultSkillImage;

	[SerializeField]
	private Animator animator;

	float lastShootTime;
	private void EditorMode() => hp = isEditorMode ? 10000 : 100; //체력 10000

	public void UseSkill()  // 스킬 버튼 입력 시
    {
		if (GameManager.instance.gameData.haveSkill())  // 현재 스킬을 획득한 상태이면
		{
			switch (GameManager.instance.gameData.nowSkillName)  // 획득한 스킬의 이벤트 발생
			{
				case "Fire":
					Debug.Log("Fire");
					break;
				case "Barrier":
					Debug.Log("Barrier");
					break;
				case "Water":
					Debug.Log("Water");
					break;
				default:
					//Debug.Log("ERROR");
					Debug.Log(GameManager.instance.gameData.nowSkillName);
					break;
			}
			int num = GameManager.instance.gameData.SkillNameSet.IndexOf(GameManager.instance.gameData.nowSkillName);
			Debug.Log(num);
			num = num == -1? 0 : num;
			Instantiate(GameManager.instance.gameData.SkillResource[num], this.transform.position,this.transform.rotation);
			SkillImage.sprite = DefaultSkillImage;  // 스킬버튼 이미지 변경
			GameManager.instance.gameData.nowSkillName = "";
		}
		else
		{
			Debug.Log("스킬없음");
		}
	}
	private void OnEnable()
	{
		input.onChange.AddResponse(CheckMovementState);
	}

	private void OnDisable()
	{
		input.onChange.RemoveResponse(CheckMovementState);
	}

	protected new void Awake()
	{
		base.Awake();

		CharacterDatas character = CharacterData.instance.Load();
		speed = character.Speed;
		maxHp = character.HP;
		attackSpeed = character.AS;
		damage = character.AD;
		ap = character.AP;
		coin = character.Coin;

		if(animator != null)
			animator = GetComponentInChildren<Animator>();
		if (shooter == null)
			shooter = GetComponentInChildren<Shooter>();
		if (aimer == null)
			aimer = GetComponentInChildren<PlayerAimer>();

		EditorMode();
	}

	protected override void Death(Entity killer)
	{
		onPlayerDeath.Raise();
		Debug.Log("Player is DEAD");
		gameObject.SetActive(false);
	}

	/// <summary>
	/// Adds coins on Enemy death
	/// </summary>
	/// <param name="amount"></param>
	public void AddCoins(int amount)
	{
		coins += amount;
	}

	/// <summary>
	/// Called when Input Vector2 is changed
	/// </summary>
	/// <param name="direction"></param>
	private void CheckMovementState(Vector2 direction)
	{
		if(walkingState == MovingState.STAYING && direction != Vector2.zero)
			walkingState = MovingState.MOVING;
		else
		{
			if (walkingState == MovingState.MOVING && direction == Vector2.zero)
				walkingState = MovingState.STAYING;
		}
	}

	private void Update()
	{
		if (walkingState == MovingState.STAYING)
		{
			if (aimer.Target != null)
			{
				aimer.FollowTarget();
				if (Vector3.Distance(aimer.Target.position, this.transform.position) < 2f)
				{
					if (Time.time - lastShootTime >= (1 / attackSpeed))
					{
						animator.PlayInFixedTime("Slash");
						lastShootTime = Time.time;
						shooter.Shoot(new DamageReport(damage, this));
					}
				}
			}
		}
	}
	private void FixedUpdate()
	{
		if (aimer.Target == null)
			aimer.Aim();
		else if (!aimer.IsVisible())
			aimer.ResetTarget();
	}

    public void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "Item")
        {
			if (GameManager.instance.gameData.haveSkill())  // 현재 스킬을 획득한 상태이면 무시
				return;
			else
			{
				GameManager.instance.gameData.nowSkillName = other.gameObject.name;  // 획득한 스킬구슬의 스킬이미지 이름 저장
				SkillImage.sprite = other.gameObject.GetComponent<SpriteRenderer>().sprite;  // 스킬버튼 이미지 변경
				Destroy(other.transform.parent.gameObject);  // 드랍된 스킬구슬 파괴
			}
        }
    }
    public void OnTriggerStay(Collider other)
    {
		if (other.gameObject.tag == "Item")
		{
			if (GameManager.instance.gameData.haveSkill())  // 현재 스킬을 획득한 상태이면 무시
				return;
			else
			{
				GameManager.instance.gameData.nowSkillName = other.gameObject.name;  // 획득한 스킬구슬의 스킬이미지 이름 저장
				SkillImage.sprite = other.gameObject.GetComponent<SpriteRenderer>().sprite;  // 스킬버튼 이미지 변경
				Destroy(other.transform.parent.gameObject);   // 드랍된 스킬구슬 파괴
			}
		}
	}
}