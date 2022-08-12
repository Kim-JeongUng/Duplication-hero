using UnityEngine;
using UnityEngine.AI;

public abstract class WalkingEnemy : Enemy
{
    float movingStateTimer = 0;
    public abstract void Animation_Run(bool isRun);
    //public abstract void Animation_Attack();
    NavMeshAgent agent;

    protected new void Awake()
    {
        base.Awake();
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;
    }

    protected void FixedUpdate()
    {
        switch (walkingState)
        {
            case MovingState.MOVING:
                if (Time.time - movingStateTimer >= movingTime)
                {
                    walkingState = MovingState.STAYING;
                    movingStateTimer = Time.time;
                    Animation_Run(false);
                }
                else
                {
                    if (this.GetComponent<NavMeshAgent>().enabled == true)
                    {
                        Animation_Run(true);
                        agent.destination = player.componentCache.position;
                    }
                }
                break;
            case MovingState.STAYING:
                if(Time.time - movingStateTimer >= waitingTime)
                {
                    if (touchingPlayer != null)  // 캐릭터랑 근접했을 때
                    {
                        //Animation_Attack();  근접 공격
                        if (touchingPlayer.TakeDamage(new DamageReport(damage * touchDamageMultiplier, this)))
                            touchingPlayer = null;
                        movingStateTimer = Time.time;
                    }
                    else
                    {
                        walkingState = MovingState.MOVING;
                        movingStateTimer = Time.time + Random.Range(0, randomTime);
                    }
                }
                else
                {
                    if(this.GetComponent<NavMeshAgent>().enabled == true)
                        agent.destination = transform.position;
                }
                break;
            default:
                break;
        }
    }

    protected new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == Tags.playerTag)
        {
            walkingState = MovingState.STAYING;
            movingStateTimer = Time.time;
        }
    }
}
