using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject monoPrefab;
    public float Speed;
    GameObject[] monoPrefabObjTemp = new GameObject[2];

    Transform thisTransform;

    public bool moveBool = false;

    Rigidbody thisRigidbody;
    BoxCollider thisBoxCollider;
    MeshRenderer thisMeshRenderer;
    KeyCode beforeKeyCode; //屏蔽前一个按键

    public int playerState = 0; //有两种状态
    GameObject tempMono;
    Vector3 beforePos = Vector3.zero;

    public InputField InputField;
    private Material mat;
    private Vector4 color1;
    private Vector4 color2;

    private void Start()
    {
        thisTransform = this.transform;
        thisRigidbody = this.GetComponent<Rigidbody>();
        thisBoxCollider = this.GetComponent<BoxCollider>();
        mat = this.GetComponentInChildren<MeshRenderer> ().sharedMaterial;
        color1 = mat.GetColor ("_Color1");
        color2 = mat.GetColor ("_Color2");
        mat.SetColor ("_Color", color1);
    }

    private void Update()
    {
        if (!moveBool)
        {
            if (Input.GetKeyDown(KeyCode.W))
                StartCoroutine(MoveInfo(Vector3.forward, KeyCode.W));
            else if (Input.GetKeyDown(KeyCode.S))
                StartCoroutine(MoveInfo(Vector3.back, KeyCode.S));
            else if (Input.GetKeyDown(KeyCode.D))
                StartCoroutine(MoveInfo(Vector3.right, KeyCode.D));
            else if (Input.GetKeyDown(KeyCode.A))
                StartCoroutine(MoveInfo(Vector3.left, KeyCode.A));

            if (Input.GetKeyDown(KeyCode.J))
                if (playerState == 0)
                {
                    mat.SetColor ("_Color", color2);
                    playerState = 1;
                }
                else
                {
                    thisBoxCollider.isTrigger = false;
                    mat.SetColor ("_Color", color1);
                    playerState = 0;
                }
        }
    }

    IEnumerator MoveInfo(Vector3 moveDir, KeyCode keyCode)
    {
        if (playerState == 0 && beforeKeyCode == keyCode) //如果为創建狀態并且為前一个按键，退出协程
            yield break;

        beforePos = thisTransform.position; //记录起始位置
        beforeKeyCode = keyCode;
        thisRigidbody.isKinematic = false;
        thisRigidbody.AddForce(moveDir * 100 *Speed);
        if (playerState == 1)
            thisBoxCollider.isTrigger = false;
        StartCoroutine(MoveInfo2());

        moveBool = true;
    }

    IEnumerator MoveInfo2()
    {
        yield return new WaitForSeconds(0.05f);
        while (thisRigidbody.velocity != Vector3.zero)
            yield return null;
        CreatObj();
    }

    void CreatObj()
    {
        moveBool = false;
        thisBoxCollider.isTrigger = true;
        thisRigidbody.isKinematic = true;

        if (beforePos.x != thisTransform.position.x) //位置修正
        {
            beforePos.x = AxisCorrect(thisTransform.position.x);
            thisTransform.position = beforePos;
        }
        else if (beforePos.z != thisTransform.position.z)
        {
            beforePos.z = AxisCorrect(thisTransform.position.z);
            thisTransform.position = beforePos;
        }

        monoPrefabObjTemp[1] = monoPrefabObjTemp[0];
        if (monoPrefabObjTemp[1] != null)
            monoPrefabObjTemp[1].GetComponent<BoxCollider>().isTrigger = false;

        if (playerState == 0) //Creat的狀態下
        {
            monoPrefabObjTemp[0] = Instantiate(monoPrefab);
            monoPrefabObjTemp[0].transform.position = thisTransform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (monoPrefabObjTemp[0] == other.gameObject)
            thisBoxCollider.isTrigger = false;
    }

    //public void TestButton1()
    //{
    //    InputField.text = AxisCorrect(float.Parse(InputField.text)).ToString();
    //}

    int AxisCorrect(float beforeValue)
    {
        if (beforeValue > 0)
        {
            if (Mathf.Abs(beforeValue - (int)beforeValue) < 0.5f)
                return (int)beforeValue;
            else if (Mathf.Abs((int)beforeValue + 1 - beforeValue) < 0.5f)
                return (int)beforeValue + 1;
            else //用来确保所有路径都返回值，没啥用
                return (int)beforeValue;
        }
        else if (beforeValue < 0)
        {
            if (Mathf.Abs((int)beforeValue - beforeValue) < 0.5f)
                return (int)beforeValue;
            else if (Mathf.Abs(beforeValue - ((int)beforeValue - 1)) < 0.5f)
                return (int)beforeValue - 1;
            else //用来确保所有路径都返回值，没啥用
                return (int)beforeValue;
        }
        else //用来确保所有路径都返回值，没啥用
            return (int)beforeValue;
    }
}