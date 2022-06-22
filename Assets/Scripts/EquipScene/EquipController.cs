using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using ThirteenPixels.Soda;

public class EquipController : Entity
{
    // Start is called before the first frame update
    public void GoHome()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void SetState() {
        speed += 10;
        Debug.Log(speed);
    }
    public void Awake()
    {
        SetState();
    }
    protected override void Death(Entity killer)
    {
        throw new System.NotImplementedException();
    }
}
