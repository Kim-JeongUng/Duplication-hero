using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerLine : MonoBehaviour
{

    LineRenderer lr;
    Vector3 pos1, pos2;

    //public GameObject LaserEffect;  // 라인렌더러

    void Start()
    {
        lr = GetComponent<LineRenderer>();

        pos1 = gameObject.GetComponent<Transform>().position;

        //lr.SetPosition(0, pos1);
        //lr.SetPosition(1, GameObject.Find("Player").GetComponent<Transform>().position);

        //StartCoroutine(SetTarget());
    }
    private void Update()
    {
        StartCoroutine(SetTarget());
    }

    public IEnumerator SetTarget()
    {
        pos1 = gameObject.GetComponent<Transform>().position;

        lr.SetPosition(0, pos1);
        lr.SetPosition(1, GameObject.Find("Player").GetComponent<Transform>().position);

        yield return new WaitForSeconds(3f); // 스킬 쿨타임이 됐으면 1초뒤 dangermark를 활성화한다.


        transform.parent.gameObject.SetActive(false);
    }
    /*
    TrailRenderer tr;
    public Vector3 EndPosition;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<TrailRenderer>();

        tr.startColor = new Color(1, 0, 0, 0.7f);
        tr.endColor = new Color(1, 0, 0, 0.7f);
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, EndPosition, Time.deltaTime * 3.5f);
    }*/
}

/*
Enemy 스크립트에 작성하기

public LayerMask layerMask;
public GameObject DangerMarker;

void DangerMarkerShoot()
{
    Vector3 NewPosition = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
    Physics.Raycast(NewPosition, transform.forward, out RaycastHit hit, 30f, layerMask);

    if (hit.transform.CompareTag("Wall"))
    {
        GameObject DangerMargerClone = Instantiate(DangerMarker, NewPosition, transform.rotation);
        DangerMargerClone.GetComponent<DangerLine>().EndPosition = hit.point;
    }
}*/