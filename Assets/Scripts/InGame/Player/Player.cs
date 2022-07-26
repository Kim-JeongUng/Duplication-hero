using UnityEngine;
using ThirteenPixels.Soda;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

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
	[SerializeField] private GameController gameController;
	float lastShootTime;
	public float m_DoubleClickSecond = 0.25f;
	private bool m_IsOneClick = false;
	private double m_Timer = 0;
	//[SerializeField] protected MultipleObjectPooling skillPooling;  스킬 오브젝트 풀
	private GameObject skill;

	private void EditorMode() => hp = isEditorMode ? 100000 : hp; //체력 10000

	public void UseSkill()  // 스킬 버튼 입력 시
    {
		if (GameManager.instance.gameData.haveSkill())  // 현재 스킬을 획득한 상태이면
		{
			bool isPassive = false; //따라다니는 스킬
			switch (GameManager.instance.gameData.nowSkillName)  // 획득한 스킬의 이벤트 발생
			{
				case "Fire":
					Debug.Log("Fire");
					break;
				case "Barrier":
					Debug.Log("Barrier");
					isPassive = true;
					StartCoroutine(Invincible());
					break;
				case "Water":
					Debug.Log("Water");
					break;
				case "Healing":
					Debug.Log("Healing");
					isPassive = true;
					hp += 30;
					break;
				default:
					//Debug.Log("ERROR");
					Debug.Log(GameManager.instance.gameData.nowSkillName);
					break;
			}
			int num = GameManager.instance.gameData.SkillNameSet.IndexOf(GameManager.instance.gameData.nowSkillName);
			Debug.Log(num);
			num = num == -1? 0 : num;
			Debug.Log("playerskill");
			skill = isPassive?Instantiate(GameManager.instance.gameData.SkillResource[num], this.transform.position,this.transform.rotation,this.transform): Instantiate(GameManager.instance.gameData.SkillResource[num], this.transform.position, this.transform.rotation);
			foreach (MultipleObjectsMake c in skill.GetComponentsInChildren<MultipleObjectsMake>())
            {
				c.Attacker = this;
            }
			tag = "PlayerSkill";
			/*
			// 스킬오브젝트 오브젝트 풀에서 꺼내옴
			skill = skillPooling.GetPooledObject(GameManager.instance.gameData.nowSkillName);
			Debug.Log(skill.name);
			skill.transform.position = this.gameObject.transform.position;  // 스킬구슬 생성 위치 설정
			skill.transform.rotation = this.transform.rotation;
			*/
			SkillImage.sprite = DefaultSkillImage;  // 스킬버튼 이미지 변경
			GameManager.instance.gameData.nowSkillName = "";

			//StartCoroutine(endskill());
			//skill.SetActive(false);
			
			Invoke("DestroySkill", 1f);  // 사용한 스킬이펙트 삭제

			//Invoke("Returnskillpool", 1f);
		}
		else
		{
			Debug.Log("스킬없음");
		}
	}
	/* 스킬 오브젝트 풀
	public void Returnskillpool()  // 사용한 스킬이펙트 비활성화
    {
		skill.SetActive(false);
	}
	*/
	public void Revive()
    {
		OnDie = false;
		hp = 30; 
		skill = Instantiate(GameManager.instance.gameData.SkillResource[6], this.transform.position, this.transform.rotation);
		Destroy(skill,1f);
		//Invoke("DestroySkill", 1f);  // 사용한 스킬이펙트 삭제
	}
	public IEnumerator Invincible(float time=1f) //무적
    {
		isInvincible = true;
		yield return new WaitForSeconds(time);
		isInvincible = false;
	}
	public void DestroySkill()  // 사용한 스킬이펙트 삭제
	{
		Destroy(skill);
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
		GameManager.instance.player = this;
		CharacterDatas character = DataManager.instance.Load();

		speed = character.Speed;
		maxHp = character.HP;
		hp = character.HP;
		attackSpeed = character.AS;
		damage = character.AD;
		ap = character.AP;
		coin = character.Coin;
		animator.SetFloat("Speed", speed / 3);
		animator.SetFloat("AttackSpeed", attackSpeed);
		Debug.Log("HIgame");

		if(animator != null)
			animator = GetComponentInChildren<Animator>();
		if (shooter == null)
			shooter = GetComponentInChildren<Shooter>();
		if (aimer == null)
			aimer = GetComponentInChildren<PlayerAimer>();
        if (GameManager.instance.gameData.nowSkillName != "")
        {
			SkillImage.sprite = Resources.Load<Transform>(string.Format("SkillBubble/{0}", GameManager.instance.gameData.nowSkillName)).GetChild(0).GetComponent<SpriteRenderer>().sprite;  // 스킬버튼 이미지 변경
		}
        if (GameManager.instance.gameData.nowProgressLevel!=0 || GameManager.instance.gameData.DeadCount > 0 )
        {
			hp = GameManager.instance.gameData.nowHP;
		}
		EditorMode();
	}

	protected override void Death(Entity killer)
	{
		onPlayerDeath.Raise();
		Debug.Log("Player is DEAD");
		input.value = new Vector2(0, 0);
		GameManager.instance.gameData.DeadCount++;
		GameController.instance.OpenContinuePannel();
		gameObject.SetActive(false);
	}

	/// <summary>
	/// Adds coins on Enemy death
	/// </summary>
	/// <param name="amount"></param>
	public void AddCoins(int amount)
	{
		//coins += amount;
		GameManager.instance.gameData.acquiredCoins += amount;
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
		if (m_IsOneClick && ((Time.time - m_Timer) > m_DoubleClickSecond) || Time.timeScale==0)
		{
			m_IsOneClick = false;
		}

		if (Input.GetMouseButtonDown(0)) //공격처리
		{
			if (!m_IsOneClick)
			{
				m_Timer = Time.time;
				m_IsOneClick = true;
			}
			else if (m_IsOneClick && ((Time.time - m_Timer) < m_DoubleClickSecond))
			{
				m_IsOneClick = false;
				UseSkill();
			}
		}
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
						//shooter.Shoot(new DamageReport(damage, this)); (원거리)
					}
				}
				else
					aimer.ResetTarget();
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
		if (other.gameObject.CompareTag("Skill"))
		{
			if (GameManager.instance.gameData.haveSkill())  // 현재 스킬을 획득한 상태이면 무시
				return;
			else
			{
				GameManager.instance.gameData.nowSkillName = other.gameObject.name;  // 획득한 스킬구슬의 스킬이미지 이름 저장
				SkillImage.sprite = other.gameObject.GetComponent<SpriteRenderer>().sprite;  // 스킬버튼 이미지 변경
																							 //Destroy(other.transform.parent.gameObject);  // 드랍된 스킬구슬 파괴
				other.transform.parent.gameObject.SetActive(false);  // 오브젝트 풀링
			}
		}
		else if (other.gameObject.CompareTag("Gate"))
		{
			if (GameManager.instance.gameData.nowProgressLevel == GameManager.instance.gameData.EndProgressLevel) //게임종료
			{
				GameController.instance.OpenResultPannel();
			}
			else
			{
				GameManager.instance.gameData.nowHP = hp;
				gameController.nextStage();
			}
		}
		else if (other.gameObject.CompareTag("Item"))
		{
			if (other.transform.parent == null) { 
				GameManager.instance.gameData.acquiredItems.Add(other.GetComponent<presetItemdata>().itemData);
				Destroy(other.gameObject);
			}
			else
			{
				GameManager.instance.gameData.acquiredItems.Add(other.transform.parent.GetComponent<presetItemdata>().itemData);
				Destroy(other.transform.parent.gameObject);
			}
		}
	}
    public void OnTriggerStay(Collider other)
    {
		if (other.gameObject.CompareTag("Skill"))
		{
			if (GameManager.instance.gameData.haveSkill())  // 현재 스킬을 획득한 상태이면 무시
				return;
			else
			{
				GameManager.instance.gameData.nowSkillName = other.gameObject.name;  // 획득한 스킬구슬의 스킬이미지 이름 저장
				SkillImage.sprite = other.gameObject.GetComponent<SpriteRenderer>().sprite;  // 스킬버튼 이미지 변경
				//Destroy(other.transform.parent.gameObject);   // 드랍된 스킬구슬 파괴
				other.transform.parent.gameObject.SetActive(false);
			}
		}
	}
}
