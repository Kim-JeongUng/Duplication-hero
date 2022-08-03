using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupInfo : MonoBehaviour
{
    [SerializeField] private Image bg;

    [SerializeField]
    public TMPro.TextMeshProUGUI infoText;

    Coroutine disapear = null;

    float disapperTime = 2f;

    public void OnEnable()
    {
        if (disapear != null)
        {
            StopCoroutine(disapear);
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 0.94f);
        }

        disapperTime = 1.5f;

        disapear = StartCoroutine(CloseInfo());
    }

    IEnumerator CloseInfo()
    {
        float time = 0;
        while (time < disapperTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        while (bg.color.a > 0.1f)
        {
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, bg.color.a - (5f * Time.deltaTime));
            infoText.color = new Color(bg.color.r, bg.color.g, bg.color.b, bg.color.a - (5f * Time.deltaTime));
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
