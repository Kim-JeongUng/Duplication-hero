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
	float defaultHp = 1000f;
	float yPos;
	float currentHp;
	bool backHpHit;
	private void Awake() {
		yPos = transform.position.y;
		currentHp = entity.Hp;
		if(entity.CompareTag("Player")){
			GetHpBoost();
		}
	}
	private void Update()
	{
		if(entity.CompareTag("Player"))
			transform.position = new Vector3(thisObject.position.x, yPos, thisObject.position.z);
		else
			transform.rotation = Quaternion.Euler(new Vector3(-90, -this.transform.root.rotation.y, 0));

        hpBar.value = Mathf.Lerp(hpBar.value, entity.Hp / entity.MaxHp, Time.deltaTime * 5f);

		if(currentHp!=entity.Hp){
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
