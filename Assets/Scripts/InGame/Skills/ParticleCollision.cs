using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Player")
        {
            other.transform.GetComponent<Player>().TakeDamage(new DamageReport(10, GameManager.instance.player));
        }
        if (other.tag == "Enemy")
        {
            other.transform.GetComponent<Enemy>().TakeDamage(new DamageReport(10, GameManager.instance.player));
        }
    }
}
