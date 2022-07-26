using UnityEngine;

public class Bird : FlyingEnemy
{
    [SerializeField] private Animator anim;

    private new void Awake() {
        base.Awake();
        anim = GetComponent<Animator>();
    }
    public override void Animation_Run(bool isRun){
        anim.SetBool("isRun", isRun);
    }

    public override void Animation_Attack()
    {
        anim.SetTrigger("attack_01");
    }

    protected override void Death(Entity killer)
    {
        Debug.Log("------------Bird Dead-------------");
        // 확률에따라 스킬구슬, 장비아이템, 꽝 3가지 중에서 랜덤 실행

        base.Death(killer);
        DropItem(getSkill); // 스킬아이템 드랍
    }
}