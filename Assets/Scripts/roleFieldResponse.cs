using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roleFieldResponse : MonoBehaviour {
    public GameObject informationBar;//信息面板预制件
    private GameObject showRoleInformation;//实例化的信息面板
    public List<Material> informatinMaterial = new List<Material>();//角色卡解释说明的材质

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseEnter()
    {
        showRoleInformation = GameObject.Instantiate(informationBar) as GameObject;
        switch (this.tag)
        {
            case "A.情敌":
                showRoleInformation.GetComponent<Renderer>().material = informatinMaterial[0];
                break;
            case "B.同学":
                showRoleInformation.GetComponent<Renderer>().material = informatinMaterial[1];
                break;
            case "C.同学":
                showRoleInformation.GetComponent<Renderer>().material = informatinMaterial[2];
                break;
            case "D.同学":
                showRoleInformation.GetComponent<Renderer>().material = informatinMaterial[3];
                break;
            case "E.同学":
                showRoleInformation.GetComponent<Renderer>().material = informatinMaterial[4];
                break;
            case "F.讨厌":
                showRoleInformation.GetComponent<Renderer>().material = informatinMaterial[5];
                break;
            case "G.同学":
                showRoleInformation.GetComponent<Renderer>().material = informatinMaterial[6];
                break;
            case "H.情敌":
                showRoleInformation.GetComponent<Renderer>().material = informatinMaterial[7];
                break;
            case "I.基友":
                showRoleInformation.GetComponent<Renderer>().material = informatinMaterial[8];
                break;
            case "J.同学":
                showRoleInformation.GetComponent<Renderer>().material = informatinMaterial[9];
                break;
            case "K.同学":
                showRoleInformation.GetComponent<Renderer>().material = informatinMaterial[10];
                break;
            case "L.同学":
                showRoleInformation.GetComponent<Renderer>().material = informatinMaterial[11];
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