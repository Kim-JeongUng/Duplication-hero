using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    public static PlayerHpBar Instance // singlton     
    {
        get
        {
            if ( instance == null )
            {
                instance = FindObjectOfType<PlayerHpBar> ( );
                if ( instance == null )
                {
                    var instanceContainer = new GameObject ( "PlayerHpBar" );
                    instance = instanceContainer.AddComponent<PlayerHpBar> ( );
                }
            }
            return instance;
        }
    }
    private static PlayerHpBar instance;

    public Transform player;
    public Slider hpBar;
    public float maxHp;
    public float currentHp;

    public GameObject HpLineFolder;

    public Text playerHpText;
    float unitHp = 200f;

    // Start is called before the first frame update
    void Start ()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
        hpBar.value = currentHp / maxHp;
        playerHpText.text = "" + currentHp;
    }

    public void GetHpBoost ( )
    {
        maxHp += 150;
        currentHp += 150;
        float scaleX = ( 1000f / unitHp ) / ( maxHp / unitHp );
        HpLineFolder.GetComponent<HorizontalLayoutGroup> ( ).gameObject.SetActive ( false );

        foreach ( Transform child in HpLineFolder.transform)
        {
            child.gameObject.transform.localScale = new Vector3 ( scaleX, 1, 1 );
        }

        HpLineFolder.GetComponent<HorizontalLayoutGroup> ( ).gameObject.SetActive ( true );
    }
}
