using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DropItem : MonoBehaviour
{
    private Vector3 direction;
    public void Shoot(Vector3 direction)
    {
        this.direction = direction;
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.Translate(direction);
    }
}


public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField]
    private GameObject poolingObjectPrefab;

    Queue<DropItem> poolingObjectQueue = new Queue<DropItem>();

    private void Awake()
    {
        Instance = this;

        Initialize(10);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    private DropItem CreateNewObject()
    {
        var newObj = Instantiate(poolingObjectPrefab).GetComponent<DropItem>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static DropItem GetObject()
    {
        if (Instance.poolingObjectQueue.Count > 0)
        {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public static void ReturnObject(DropItem obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }
}