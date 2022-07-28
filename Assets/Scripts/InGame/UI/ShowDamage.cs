using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowDamage : MonoBehaviour
{
    public float damage;
    public float damageForcePower = 1000.0f;
    TMP_Text damageText;
    Rigidbody rb;
    private void Awake() {
        rb = GetComponent<Rigidbody>();
        damageText = GetComponent<TMP_Text>();
    }
    void Start()
    {
        rb.AddForce(new Vector3(Random.Range(0.1f,10.0f),0,Random.Range(1.0f,10.0f))
                                * damageForcePower*Time.deltaTime);
        Destroy(this.gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        damageText.text = damage.ToString();
    }
}
