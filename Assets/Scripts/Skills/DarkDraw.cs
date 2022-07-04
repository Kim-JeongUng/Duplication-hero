using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkDraw : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnParticleCollision(GameObject hit)
    {
        if (hit.transform.CompareTag("Enemy"))
        {
            hit.transform.GetComponent<Enemy>().TakeDamage(new DamageReport(GameManager.instance.player.Damage * 0.2f * (GameManager.instance.player.Ap * 0.01f + 1), GameManager.instance.player));
        }
    }
}
