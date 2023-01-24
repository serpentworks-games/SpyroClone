using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class TargetTracker : MonoBehaviour
{
    [SerializeField] float targetRadius;
    [SerializeField] LayerMask layersToTarget;
    [SerializeField] CinemachineStateDrivenCamera stateDrivenCamera;

    Collider activeTarget;
    GameObject targetFollow;

    bool lockedToTarget = false;
    int targetIndex;
    List<Collider> targetList = new List<Collider>();

    CinemachineTargetGroup targetCamGroup;

    private void Start()
    {
        targetFollow = new();
        targetFollow.name = "TargetFollow";

        targetCamGroup = stateDrivenCamera.GetComponentInChildren<CinemachineTargetGroup>();
        targetCamGroup.AddMember(targetFollow.transform, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, targetRadius, layersToTarget, QueryTriggerInteraction.Ignore);

        targetList.Clear();

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                targetList.Add(col);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SetTargetting();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchTarget();
        }

        if (lockedToTarget)
        {
            targetFollow.transform.position = Vector3.Lerp(targetFollow.transform.position, activeTarget.bounds.center, Time.deltaTime * 4f);
        }
    }

    private void SetTargetting()
    {
        if (!HasTarget())
        {
            lockedToTarget = false;
            return;
        }

        lockedToTarget = !lockedToTarget;

        if (lockedToTarget)
        {
            activeTarget = ClosestTarget();
        }
    }

    private void SwitchTarget()
    {
        if(lockedToTarget)
        {
            SortTargetList();
            if(targetIndex < (targetList.Count - 1))
            {
                targetIndex = targetIndex + 1;
            } else {
                targetIndex = targetList.Count - 1;
            }
            activeTarget = targetList[targetIndex];
        }
    }

    private void SortTargetList()
    {
        targetList = targetList.OrderBy(x => Camera.main.WorldToScreenPoint(x.transform.position)).ToList();
    }

    private bool HasTarget()
    {
        if (targetList.Count > 0) { return true; }
        return false;
    }

    private Collider ClosestTarget()
    {
        Collider bestTarget = null;
        targetList = targetList.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToList();
        bestTarget = targetList[0];
        return bestTarget;
    }


}
