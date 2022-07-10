using UnityEngine;

public class Golem : WalkingEnemy
{
	protected override void Death(Entity killer)
	{
		Debug.Log("------------Golem Dead-------------");
		
		// 확률에따라 스킬구슬, 장비아이템, 꽝 3가지 중에서 랜덤 실행

		base.Death(killer);

		if (OnDie == true)
			DropItem(getSkill); // 스킬아이템 드랍
	}
}
