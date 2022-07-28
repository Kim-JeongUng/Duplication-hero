using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
	[SerializeField] Transform thisObject;
	[SerializeField] Entity entity;
	[SerializeField] Slider hpBar;
	[SerializeField] Slider backHpBar;
	[SerializeField] GameObject HpLineFolder;
	[SerializeField] TMP_Text thisObjectHpText;
	[SerializeField] float unitHp = 200f;
	[SerializeField] GameObject damageText;
	[SerializeField] GameObject damageShowCanvas;
	float defaultHp = 1000f;
	float yPos;
	float currentHp;
	bool backHpHit;
	bool isPlayer;
	float damage = 0f;
	
	private void Awake() {
		yPos = transform.position.y;
		if(entity.CompareTag("Player")){
			isPlayer = true;
			GetHpBoost();
		}
        if (damageShowCanvas == null)
        {
			damageShowCanvas = GameObject.Find("DamageShowCanvas");
		}
	}
	private void Start() {
		currentHp = entity.Hp;
	}
	private void Update()
	{
		if(isPlayer)
			transform.position = new Vector3(thisObject.position.x, yPos, thisObject.position.z);
		else
			transform.rotation = Quaternion.Euler(new Vector3(-90, -this.transform.root.rotation.y, 0));

        hpBar.value = Mathf.Lerp(hpBar.value, entity.Hp / entity.MaxHp, Time.deltaTime * 5f);

		if(currentHp!=entity.Hp){
			damage = currentHp - entity.Hp;
			GameObject dT = Instantiate(damageText, new Vector3(entity.transform.position.x, damageShowCanvas.transform.position.y, entity.transform.position.z), 
											damageShowCanvas.transform.rotation, damageShowCanvas.transform);
			dT.GetComponent<ShowDamage>().damage = this.damage;

			currentHp = entity.Hp;
			Invoke("BackHp", 0.8f);
		}

		if(backHpHit){
			backHpBar.value = Mathf.Lerp(hpBar.value, entity.Hp / entity.MaxHp, Time.deltaTime * 10f);
			if(hpBar.value >= backHpBar.value - 0.01f){
				backHpHit = false;
				backHpBar.value = hpBar.value;
			}
		}

        thisObjectHpText.text = "" + entity.Hp;
	}
	void BackHp(){
		backHpHit = true;
	}

	public void GetHpBoost()
    {
        float scaleX = ( defaultHp / unitHp ) / ( entity.MaxHp / unitHp );
        HpLineFolder.GetComponent<HorizontalLayoutGroup> ( ).gameObject.SetActive ( false );
		Debug.Log(scaleX);
        foreach ( Transform child in HpLineFolder.transform)
        {
            child.gameObject.transform.localScale = new Vector3 ( scaleX, 1, 1 );
        }

        HpLineFolder.GetComponent<HorizontalLayoutGroup> ( ).gameObject.SetActive ( true );
    }
}
