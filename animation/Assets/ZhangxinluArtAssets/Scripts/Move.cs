using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform head;
    [Header("Root")]
    public float turnSpeed;
    public float moveSpeed;
    public float turnAcceleration;
    public float moveAcceleration;
    public float minDstToTarget;
    public float maxDstToTarget;
    public float maxAngToTarget;
    Vector3 currentVelocity;
    float currentAngularVelocity;

    void Start()
    {
        
    }
    void RootMotionUpdate()
    {
        //头部旋转
        Quaternion currentLocalRotation = head.localRotation;
        head.localRotation = Quaternion.identity;
        Vector3 targetWorldDir = target.position - head.position;
        Vector3 targetLocalLookDir = head.InverseTransformDirection(targetWorldDir);
        targetLocalLookDir = Vector3.RotateTowards(Vector3.forward, targetLocalLookDir, Mathf.Deg2Rad * 60, 0);
        Quaternion targetLocalRotation = Quaternion.LookRotation(targetLocalLookDir, Vector3.up);
        head.localRotation = Quaternion.Slerp(currentLocalRotation, targetLocalRotation, 1 - Mathf.Exp(-3 * Time.deltaTime));


        //身体跟随
        Vector3 towardTarget = target.position - transform.position;
        Vector3 towardTargetProject = Vector3.ProjectOnPlane(towardTarget, transform.up);
        float angToTarget = Vector3.SignedAngle(transform.forward, towardTargetProject, transform.up);

        float targetAngularVelocity = 0;
        if (Mathf.Abs(angToTarget) > maxAngToTarget)
        {
            if (angToTarget > 0)
                targetAngularVelocity = turnSpeed;
            else
                targetAngularVelocity = -turnSpeed;
        }
        currentAngularVelocity = Mathf.Lerp(currentAngularVelocity, targetAngularVelocity, 
            1 - Mathf.Exp(-turnAcceleration * Time.deltaTime));
        transform.Rotate(0, Time.deltaTime * currentAngularVelocity, 0, Space.World);

        Vector3 targetVelocity = Vector3.zero;
        if(Mathf.Abs(angToTarget)<90)
        {
            float dstToTarget = Vector3.Distance(transform.position, target.position);
            if (dstToTarget > maxDstToTarget)
                targetVelocity = moveSpeed * towardTargetProject.normalized;
            else if(dstToTarget<minDstToTarget)
                targetVelocity = moveSpeed * -towardTargetProject.normalized;
            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, 1 - Mathf.Exp(-moveAcceleration * Time.deltaTime));
            transform.position += currentVelocity * Time.deltaTime;
        }
    }
    // Update is called once per frame
    void Update()
    {
        RootMotionUpdate();
    }
}
