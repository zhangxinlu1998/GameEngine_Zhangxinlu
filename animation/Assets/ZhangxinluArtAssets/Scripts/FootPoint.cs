using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPoint : MonoBehaviour
{
    public Transform[] footPointTransDisplay;
    public AnimationCurve footHeightAnimationCurve;
    public float moveSpeed = 5;
    public float moveDistance = 1;
    public float footHeight = 1;
    //public float moveCoefficient = 1; //移动系数
    public float gapTime = 0.25f; //间隔时间

    Transform[] footPointTrans;
    Vector3[] footDir = new Vector3[4];
    Vector3[] moveDirPos = new Vector3[4];
    float footDefaultHeight = 0;
    float[] footTimer = new float[4];
    bool footCtrlBool = false;
    bool isMove = false;

    private void Start()
    {
        footPointTrans = new Transform[4];
        for (int i = 0; i < 4; i++)
            footPointTrans[i] = this.transform.GetChild(i).transform;
        footDefaultHeight = footPointTrans[0].position.y;
    }

    private void Update()
    {
        if (Vector3.Distance(footPointTransDisplay[0].position, footPointTrans[0].position) > moveDistance
            && !isMove)
        {
            if (footCtrlBool)
            {
                StartCoroutine(FootCtrl(0));
                StartCoroutine(FootCtrl(2));
            }
            else
            {
                StartCoroutine(FootCtrl(1));
                StartCoroutine(FootCtrl(3));
            }
        }
    }

    IEnumerator FootCtrl(int footIndex)
    {
        isMove = true;
        footTimer[footIndex] = 0;
        footDir[footIndex] = footPointTransDisplay[footIndex].position;
        //moveDirPos[footIndex] = (footPointTrans[footIndex].position - footDir[footIndex]).normalized * moveCoefficient
        //    + footPointTransDisplay[footIndex].position;

        while (footTimer[footIndex] < 1)
        {
            footTimer[footIndex] += moveSpeed * Time.deltaTime;
            footDir[footIndex] = Vector3.MoveTowards(footDir[footIndex], footPointTrans[footIndex].position,
                footTimer[footIndex]);
            footDir[footIndex].y = footDefaultHeight + footHeightAnimationCurve.Evaluate(footTimer[footIndex]) * footHeight;
            footPointTransDisplay[footIndex].position = footDir[footIndex];
            yield return null;
        }
        yield return new WaitForSeconds(gapTime);

        if (footIndex == 0)
            footCtrlBool = false;
        if (footIndex == 1)
            footCtrlBool = true;
        isMove = false;
    }
}