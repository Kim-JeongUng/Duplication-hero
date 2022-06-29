using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleObjectPooling : MonoBehaviour
{
    public GameObject[] poolPrefabs;
    public int poolingCount;

    private Dictionary<object, List<GameObject>> pooledObjects = new Dictionary<object, List<GameObject>>();

    public void Awake()
    {
        CreateMultiplePoolObjects();
    }
    public void CreateMultiplePoolObjects()
    {
        for (int i = 0; i < poolPrefabs.Length; i++)
        {
            for (int j = 0; j < poolingCount; j++)
            {
                if (!pooledObjects.ContainsKey(poolPrefabs[i].name))
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

    public GameObject GetPooledObject(string _name)
    {
        if (pooledObjects.ContainsKey(_name))
        {
            for (int i = 0; i < pooledObjects[_name].Count; i++)
            {
                if (!pooledObjects[_name][i].activeSelf)
                {
                    return pooledObjects[_name][i];
                }
            }

            int beforeCreateCount = pooledObjects[_name].Count;

            CreateMultiplePoolObjects();

            return pooledObjects[_name][beforeCreateCount];
        }
        else
        {
            return null;
        }
    }
}