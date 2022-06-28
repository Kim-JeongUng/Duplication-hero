using UnityEngine;
using ThirteenPixels.Soda;
using UnityEngine.UI;

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
	public string mySkill;

	private bool isSkill;

	float lastShootTime;
	private void EditorMode() => hp = isEditorMode ? 10000 : 100; //체력 10000

	public void UseSkill()  // 스킬 버튼 입력 시
    {
		if (isSkill)  // 현재 스킬을 획득한 상태이면
		{
			SkillImage.sprite = DefaultSkillImage;  // 스킬버튼 이미지 변경

			switch (mySkill)  // 획득한 스킬의 이벤트 발생
			{
				case "Fire":
					Debug.Log(mySkill);
					break;
				case "Water":
					Debug.Log(mySkill);
					break;
				case "Punch":
					Debug.Log(mySkill);
					break;
				default:
					//Debug.Log("ERROR");
					Debug.Log(mySkill);
					break;
			}
			isSkill = false;
		}
		else
		{
			mySkill = "NullSkill";
			Debug.Log(mySkill);
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
				if (Time.time - lastShootTime >= (1 / attackSpeed))
				{
					lastShootTime = Time.time;
					shooter.Shoot(new DamageReport(damage, this));
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
			if (isSkill)  // 현재 스킬을 획득한 상태이면 무시
				return;
			else
			{
				mySkill = other.gameObject.name + " Skill";  // 획득한 스킬구슬의 스킬이미지 이름 저장
				SkillImage.sprite = other.gameObject.GetComponent<SpriteRenderer>().sprite;  // 스킬버튼 이미지 변경
				isSkill = true;  // 스킬 획득한 상태
				Destroy(other.transform.parent.gameObject);
			}
        }
    }
}
