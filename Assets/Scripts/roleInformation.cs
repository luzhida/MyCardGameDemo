using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//附在角色卡预制件上面的脚本
public class roleInformation : MonoBehaviour {
    public GameObject test;
    public static GameObject showRoleInformation;
    public List<Material> roleMaterial = new List<Material>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //实现鼠标悬停在角色卡时，通过判断该预制件当前的标签，实例化信息面板的预制件
    //随后将不同的材质赋给他，来向玩家说明不同卡片的作用
    void OnMouseEnter()
    {
        showRoleInformation = GameObject.Instantiate(test) as GameObject;
        switch (this.tag)
        {
            case "A.情敌":
                showRoleInformation.GetComponent<Renderer>().material = roleMaterial[0];
                break;
            case "B.同学":
                showRoleInformation.GetComponent<Renderer>().material = roleMaterial[1];
                break;
            case "C.同学":
                showRoleInformation.GetComponent<Renderer>().material = roleMaterial[2];
                break;
            case "D.同学":
                showRoleInformation.GetComponent<Renderer>().material = roleMaterial[3];
                break;
            case "E.同学":
                showRoleInformation.GetComponent<Renderer>().material = roleMaterial[4];
                break;
            case "F.讨厌":
                showRoleInformation.GetComponent<Renderer>().material = roleMaterial[5];
                break;
            case "G.同学":
                showRoleInformation.GetComponent<Renderer>().material = roleMaterial[6];
                break;
            case "H.情敌":
                showRoleInformation.GetComponent<Renderer>().material = roleMaterial[7];
                break;
            case "I.基友":
                showRoleInformation.GetComponent<Renderer>().material = roleMaterial[8];
                break;
            case "J.同学":
                showRoleInformation.GetComponent<Renderer>().material = roleMaterial[9];
                break;
            case "K.同学":
                showRoleInformation.GetComponent<Renderer>().material = roleMaterial[10];
                break;
            case "L.同学":
                showRoleInformation.GetComponent<Renderer>().material = roleMaterial[11];
                break;
            default:
                break;
        }
    }

    //鼠标离开时破坏创建的信息面板实例
    void OnMouseExit()
    {
        Destroy(showRoleInformation);
    }
}