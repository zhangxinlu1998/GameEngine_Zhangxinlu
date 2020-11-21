using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public GameObject monoPrefab; //生成方块预制体
    public Transform monoPool; //方块物体池
    public List<GameObject> monoList = new List<GameObject>();
    public float speed = 1;

    Transform thisTransform;
    GameObject[] monoPrefabObjTemp = new GameObject[2];
    Rigidbody thisRigidbody;
    BoxCollider thisBoxCollider;
    MeshRenderer thisMeshRenderer;
    Vector3 beforePos = Vector3.zero;

    bool moveBool = false;
    int playerState = 0; //有两种状态
    float pushMoveBottomTime = 0; //记录按下时间

    private Material mat;
    private Vector4 color1;
    private Vector4 color2;

    private void Start()
    {
        thisTransform = this.transform;
        thisRigidbody = this.GetComponent<Rigidbody>();
        thisBoxCollider = this.GetComponent<BoxCollider>();
        thisMeshRenderer = this.GetComponent<MeshRenderer>();

        mat = this.GetComponentInChildren<MeshRenderer>().material;
        color1 = mat.GetColor("_Color1");
        color2 = mat.GetColor("_Color2");
        mat.SetColor("_Color", color1);
    }

    private void Update()
    {
        if (!moveBool)
        {
            if (Input.GetKeyDown(KeyCode.W))
                StartCoroutine(MoveInfo(Vector3.forward, KeyCode.W, 3));
            else if (Input.GetKeyDown(KeyCode.S))
                StartCoroutine(MoveInfo(Vector3.back, KeyCode.S, 4));
            else if (Input.GetKeyDown(KeyCode.D))
                StartCoroutine(MoveInfo(Vector3.right, KeyCode.D, 1));
            else if (Input.GetKeyDown(KeyCode.A))
                StartCoroutine(MoveInfo(Vector3.left, KeyCode.A, 2));

            if (Input.GetKeyDown(KeyCode.J))
                if (playerState == 0)
                {
                    mat.SetColor("_Color", color2);
                    playerState = 1;
                }
                else
                {
                    thisBoxCollider.isTrigger = false;
                    mat.SetColor("_Color", color1);
                    playerState = 0;
                }
        }
    }

    IEnumerator MoveInfo(Vector3 moveDir, KeyCode keyCode, int rotStateIndex)
    {
        beforePos = thisTransform.position; //记录起始位置
        pushMoveBottomTime = Time.unscaledTime;
        thisRigidbody.isKinematic = false;
        thisRigidbody.AddForce(moveDir * 100 * speed);
        StartCoroutine(Camera.main.gameObject.GetComponent<MainCam>().Rot(rotStateIndex));
        if (playerState == 1)
            thisBoxCollider.isTrigger = false;
        moveBool = true;
        StartCoroutine(MoveInfo2());
        yield return null;
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

        //StopCoroutine(Camera.main.gameObject.GetComponent<MainCam>().Rot(0)); //先停止协程
        //StartCoroutine(Camera.main.gameObject.GetComponent<MainCam>().Rot(0)); //复位相机旋转
        if (Time.unscaledTime - pushMoveBottomTime < 0.1f) //判断是否为旁边的方块
            return;

        thisBoxCollider.isTrigger = true;
        thisRigidbody.isKinematic = true;
        pushMoveBottomTime = 0;
        monoPrefabObjTemp[1] = monoPrefabObjTemp[0];
        if (monoPrefabObjTemp[1] != null)
            monoPrefabObjTemp[1].GetComponent<BoxCollider>().isTrigger = false;

        if (playerState == 0) //Creat的狀態下
        {
            if (monoList.Count > 0)
            {
                monoPrefabObjTemp[0] = monoList[0];
                monoPrefabObjTemp[0].SetActive(true);
                monoPrefabObjTemp[0].GetComponent<BoxCollider>().isTrigger = true;
                monoPrefabObjTemp[0].transform.parent = null;
                monoList.RemoveAt(0);
            }
            else
                monoPrefabObjTemp[0] = Instantiate(monoPrefab);
            monoPrefabObjTemp[0].transform.position = thisTransform.position;
        }
    }

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

    private void OnTriggerExit(Collider other)
    {
        if (monoPrefabObjTemp[0] == other.gameObject)
            thisBoxCollider.isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Mono" && playerState == 1)
        {
            //StopCoroutine(Camera.main.gameObject.GetComponent<MainCam>().Rot(0)); //先停止协程
            //StartCoroutine(Camera.main.gameObject.GetComponent<MainCam>().Rot(0)); //复位相机旋转
            if (Time.unscaledTime - pushMoveBottomTime > 0.1f)
                StartCoroutine(DestroyMono(collision.gameObject));
        }
    }

    IEnumerator DestroyMono(GameObject monoObj)
    {
        while (moveBool != false) //避免没停稳就关闭物体
            yield return null;

        monoObj.transform.parent = monoPool;
        monoList.Add(monoObj);
        monoObj.SetActive(false);
        pushMoveBottomTime = 0;
    }
}