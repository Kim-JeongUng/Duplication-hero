using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerLine : MonoBehaviour
{
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
    }
}

/*
Enemy ��ũ��Ʈ�� �ۼ��ϱ�

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