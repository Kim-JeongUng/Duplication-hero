using UnityEngine;

public class Bird : FlyingEnemy
{
    [SerializeField] private Animator anim;
    private void Start() {
        anim = GetComponent<Animator>();
    }
    
    public override void Animation_Run(bool isRun){
        anim.SetBool("isRun", isRun);
    }

    public override void Animation_Attack()
    {
        anim.SetTrigger("attack_01");
    }
}