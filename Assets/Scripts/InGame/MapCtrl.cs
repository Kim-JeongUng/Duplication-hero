using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCtrl : MonoBehaviour
{
    [SerializeField] private GameObject[] normalMaps;
    [SerializeField] private GameObject[] BossMaps;
    [SerializeField] private GameObject[] SpecialMaps;
    private GameObject curMap;
        
    void Start()
    {
        curMap = normalMaps[Random.Range(0, normalMaps.Length)];
        curMap.SetActive(true);
    }

    public void MapChange(int stageNum){
        curMap.SetActive(false);
        if(stageNum % 10 == 0){
            //boss stage
            curMap = BossMaps[Random.Range(0, BossMaps.Length)];
        }
        else{   //normal stage   (need add special stage - if needed)
            curMap = normalMaps[Random.Range(0, normalMaps.Length)];
        }
        curMap.SetActive(true);
    }
}
