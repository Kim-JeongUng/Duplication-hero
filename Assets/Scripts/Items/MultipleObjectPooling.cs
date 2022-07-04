using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleObjectPooling : MonoBehaviour
{
    //public int SkillNum;
    public GameObject[] poolPrefabs;
    public int poolingCount;

    private Dictionary<string, List<GameObject>> pooledObjects = new Dictionary<string, List<GameObject>>();

    public void Awake()
    {
        //poolPrefabs = new GameObject[SkillNum];
        //poolingCount = new int[SkillNum];

        CreateMultiplePoolObjects();
    }
    public void CreateMultiplePoolObjects()
    {
        for (int i = 0; i < poolPrefabs.Length; i++)
        {
            for (int j = 0; j < poolingCount; j++)
            {
                if (!pooledObjects.ContainsKey(poolPrefabs[i].name))  // Dictionary에 해당 키가없으면 실행
                {
                    List<GameObject> newList = new List<GameObject>();
                    pooledObjects.Add(poolPrefabs[i].name, newList);
                }

                GameObject newDoll = Instantiate(poolPrefabs[i], transform);
                newDoll.SetActive(false);
                pooledObjects[poolPrefabs[i].name].Add(newDoll);
            }
        }
    }
    
    public void AddObject(string _name)
    {
        for (int i = 0; i < poolPrefabs.Length; i++)
        {
            if(poolPrefabs[i].name == _name)  // 프리팹에서 동일한 것을 찾아서 생성
            {
                GameObject newDoll = Instantiate(poolPrefabs[i], transform);
                pooledObjects[poolPrefabs[i].name].Add(newDoll);
                return;
            }
        }
    }
    public GameObject GetPooledObject(string _name)
    {
        if (pooledObjects.ContainsKey(_name))
        {
            for (int i = 0; i < pooledObjects[_name].Count; i++)
            {
                if (!pooledObjects[_name][i].activeSelf)  // 비활성화된 오브젝트에 접근
                {
                    pooledObjects[_name][i].SetActive(true);  // 활성화 후 반환
                    return pooledObjects[_name][i];
                }
            }
            // 비활성화된 오브젝트가 없으면 (= 모두 활성화 상태이면)
            int beforeCreateCount = pooledObjects[_name].Count;

            //CreateMultiplePoolObjects();
            AddObject(_name);

            return pooledObjects[_name][beforeCreateCount];
        }
        else
        {
            return null;
        }
    }

    public static void ReturnPooled(string _name)
    {

    }
}