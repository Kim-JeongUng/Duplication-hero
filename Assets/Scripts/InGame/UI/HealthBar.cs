using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
	[SerializeField] Transform player;
	[SerializeField] Entity entity;
	[SerializeField] Slider hpBar;
	[SerializeField] GameObject HpLineFolder;
	[SerializeField] TMP_Text playerHpText;
	[SerializeField] float unitHp = 200f;
	float defaultHp = 1000f;
	private void Awake() {
		GetHpBoost();
	}
	private void Update()
	{
		transform.position = player.position;
        hpBar.value = entity.Hp / entity.MaxHp;
        playerHpText.text = "" + entity.Hp;
	}

	public void GetHpBoost()
    {
        float scaleX = ( defaultHp / unitHp ) / ( entity.MaxHp / unitHp );
        HpLineFolder.GetComponent<HorizontalLayoutGroup> ( ).gameObject.SetActive ( false );

        foreach ( Transform child in HpLineFolder.transform)
        {
            child.gameObject.transform.localScale = new Vector3 ( scaleX, 1, 1 );
        }

        HpLineFolder.GetComponent<HorizontalLayoutGroup> ( ).gameObject.SetActive ( true );
    }
}
