using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float currentvelocity;
    public float smoothtime;
    public float walkspeed;
    private Animator ac;
    private Transform cameratransform;
    private Rigidbody rb;


    AnimatorStateInfo info;
    void Start()
    {
        ac = this.GetComponent<Animator>();//获取角色状态机，命名为ac
        cameratransform = Camera.main.transform;//获取摄像机当前位置
        rb = this.GetComponent<Rigidbody>();//获取角色刚体组件命名为rb
        
    }

    // Update is called once per frame
    void Update(){
        info = ac.GetCurrentAnimatorStateInfo(0);
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));//把玩家输入指令转为一个二维向量
        Vector2 inputdir = input.normalized;
        float targetrotation=Mathf.Atan2(inputdir.x, inputdir.y) * Mathf.Rad2Deg+cameratransform.eulerAngles.y;//计算旋转
        if (inputdir!=Vector2.zero)//当输入不为0（用户有所输入时），下面为位移的计算
        {   
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetrotation,ref currentvelocity,smoothtime);//计算角色旋转欧拉角
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * walkspeed);//更新角色当前位置
            ac.SetBool("move?",true);//在走路时，开启角色漫步动画
            ac.SetBool("dance01", false);//在走路时，关闭角色所有跳舞动画
            ac.SetBool("dance02", false);
            ac.SetBool("dance03", false);
            ac.SetBool("dance04", false);
        }
        else
        {
            ac.SetBool("move?",false);//非走路状态，关闭走路动画
        }


        if (Input.GetKeyDown(KeyCode.LeftShift))//当玩家按下shift时，走路速度调整为2倍速，重新计算和匹配角色位移
        {
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetrotation, ref currentvelocity, smoothtime);
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * walkspeed*2);
            ac.SetBool("run", true);//播放跑步动画
            ac.SetBool("dance01", false);
            ac.SetBool("dance02", false);
            ac.SetBool("dance03", false);
            ac.SetBool("dance04", false);

        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))//检测玩家未按下shift，回归走路的参数状态
        {
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * walkspeed);
            ac.SetBool("run", false);
        }
    }
}
