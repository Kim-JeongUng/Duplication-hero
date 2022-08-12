using System.Collections.Generic;
using UnityEngine;
using ThirteenPixels.Soda;
public class Gate : MonoBehaviour
{
    [SerializeField] GameEvent levelPassed;
    [SerializeField] GameObject gateEffect;
    [SerializeField] GameObject gateDoorL;
    [SerializeField] GameObject gateDoorR;
    //[SerializeField] List<GameObject> disableThings;
    
    private void OnEnable()
    {
        levelPassed.onRaise.AddResponse(OpenTheGate);
    }
    private void OnDisable()
    {
        levelPassed.onRaise.RemoveResponse(OpenTheGate);
    }

    /// <summary>
    /// Opens the gate
    /// </summary>
    private void OpenTheGate()
    {
        // foreach (var block in disableThings)
        // {
        //     block.SetActive(false);
        // }
        SoundManager.instance.PlaySFXSound("Magic Spell_Coins_2");
        gateEffect.SetActive(true);
        gateDoorL.GetComponent<GateDoor>().Ldoor = true;
        gateDoorR.GetComponent<GateDoor>().Rdoor = true;
    }
}
