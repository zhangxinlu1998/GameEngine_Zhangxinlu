using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickBtn : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();
    private Animator ac;
    public GameObject Player;

    void Start()
    {
        AddClickEvents();
        ac = Player.GetComponent<Animator>();
    }

    void AddClickEvents()
    {
        int x = 0;
        foreach (Button item in buttons)
        {
            int y = x;
            item.onClick.AddListener(() => ClickEvent2(y));
            x++;
        }
    }

    void ClickEvent2(int a)
    {
        Debug.Log(buttons[a].name);
        switch (buttons[a].name)
        {
            case "Button01":
                ClickBtn1();
                break;
            case "Button02":
                ClickBtn2();
                break;
            case "Button03":
                ClickBtn3();
                break;
            case "Button04":
                ClickBtn4();
                break;
            case "Button05":
                ClickBtn5();
                break;
            default:
                break;
        }
    }

    void ClickBtn1()
    {
        ac.SetBool("dance01", true);
        ac.SetBool("dance02", false);
        ac.SetBool("dance03", false);
        ac.SetBool("dance04", false);
    }
    void ClickBtn2()
    {
        ac.SetBool("dance02", true);
        ac.SetBool("dance01", false);
        ac.SetBool("dance03", false);
        ac.SetBool("dance04", false);

    }
    void ClickBtn3()
    {
        ac.SetBool("dance03", true);
        ac.SetBool("dance02", false);
        ac.SetBool("dance01", false);
        ac.SetBool("dance04", false);
    }
    void ClickBtn4()
    {
        ac.SetBool("dance04", true);
        ac.SetBool("dance02", false);
        ac.SetBool("dance03", false);
        ac.SetBool("dance01", false);
    }
    void ClickBtn5()
    {
        ac.SetBool("dance01", false);
        ac.SetBool("dance02", false);
        ac.SetBool("dance03", false);
        ac.SetBool("dance04", false);
    }

}