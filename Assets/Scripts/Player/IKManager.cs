using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKManager : MonoBehaviour
{
    [Header("Head")]
    [SerializeField] Transform lookPoint;

    [SerializeField]
    Camera cam;
    [SerializeField]
    PlayerMovement playerMovement;

    [Header("Arms")]
    [SerializeField] Transform rArm, lArm;
    [SerializeField] Transform rPos, lPos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 collisionPoint;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitinfo, 30))
        {
            collisionPoint = hitinfo.point;
        }
        else
        {
            collisionPoint = cam.transform.forward * 30;
        }
        lookPoint.position = collisionPoint;

        rArm.position = rPos.position;
        lArm.position = lPos.position;

        rArm.rotation = rPos.rotation;
        lArm.rotation = lPos.rotation;
    }
}
