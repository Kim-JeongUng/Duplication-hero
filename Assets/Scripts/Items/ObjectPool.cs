using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �Ѱ��� ������Ʈ�� Ǯ�� �Ҷ� ���
// ���� ������Ʈ�� ��������� Dictionary�� ���� ����Ǯ���� �ʿ��ϴ�
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
//------------------
//-----����Ǯ��---------
/*
[System.Serializable]
public class ObjectInfo
{
    public string objectName;
    public GameObject perfab;
    public int count;
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField]
    ObjectInfo[] objectInfos = null;

    [Header("������Ʈ Ǯ�� ��ġ")]
    [SerializeField]
    Transform tfPoolParent;

    public List<Queue<GameObject>> objectPoolList;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        objectPoolList = new List<Queue<GameObject>>();
        ObjectPoolState();
    }

    void ObjectPoolState()
    {
        if (objectInfos != null)
        {
            for (int i = 0; i < objectInfos.Length; i++)
            {
                objectPoolList.Add(InsertQueue(objectInfos[i]));
            }
        }
    }

    Queue<GameObject> InsertQueue(ObjectInfo perfab_objectInfo)
    {
        Queue<GameObject> test_queue = new Queue<GameObject>();

        for (int i = 0; i < perfab_objectInfo.count; i++)
        {
            GameObject objectClone = Instantiate(perfab_objectInfo.perfab) as GameObject;
            objectClone.SetActive(false);
            objectClone.transform.SetParent(tfPoolParent);
            test_queue.Enqueue(objectClone);
        }

        return test_queue;
    }
}*/