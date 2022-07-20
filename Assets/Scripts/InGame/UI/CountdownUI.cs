using UnityEngine;
using System.Collections;
using TMPro;
using ThirteenPixels.Soda;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] GlobalGameState gameState;
    [SerializeField] TextMeshProUGUI countdown;
    [SerializeField] GameObject canvas;

    private void OnEnable()
    {
        gameState.onChange.AddResponse(StartTimer);
    }

    private void OnDisable()
    {
        gameState.onChange.RemoveResponse(StartTimer);
    }

    private void StartTimer(GameState gameState)
    {
        if(gameState==GameState.STARTED)
            StartCoroutine(StartTimerCoroutine());
    }

    private IEnumerator StartTimerCoroutine()
    {
        if (GameManager.instance.gameData.isBossStage)
            GameController.instance.OpenBossPannel();
        else
        {
            GameController.instance.OpenProgressPannel();
        }
        canvas.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            countdown.text = i.ToString();
            yield return new WaitForSecondsRealtime(1);
            if (GameManager.instance.gameData.isBossStage)
                GameController.instance.CloseBossPannel();
            else
                GameController.instance.CloseProgressPannel();
            Time.timeScale = 0;
        }
        canvas.SetActive(false);
        Time.timeScale = 1;
    }
}
