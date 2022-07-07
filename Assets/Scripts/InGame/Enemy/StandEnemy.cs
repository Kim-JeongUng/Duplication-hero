using UnityEngine;

public abstract class StandEnemy : Enemy
{
    float movingStateTimer = 0;

    protected new void Awake()
    {
        base.Awake();
    }

    protected void FixedUpdate()
    {
       /* switch (walkingState)
        {
            case MovingState.STAYING:
                if(Time.time - movingStateTimer >= waitingTime)
                {
                    if (touchingPlayer != null)
                    {
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
                }
                break;
            default:
                break;
        }*/
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
