using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//动作卡预制件上面的脚本
public class actionInformation : MonoBehaviour {
    public GameObject informationBar;//信息面板预制件
    public static GameObject showActionInformation;//实例化的信息面板
    public List<Material> actionMaterial = new List<Material>();//用于动作卡解释说明的材质
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //实现鼠标悬停在动作卡时，通过判断该预制件当前的标签，实例化信息面板的预制件
    //随后将不同的材质赋给他，来向玩家说明不同卡片的作用
    void OnMouseEnter()
    {
        
        switch (this.tag)
        {
            case "abandon":
                showActionInformation = GameObject.Instantiate(informationBar) as GameObject;
                showActionInformation.GetComponent<Renderer>().material = actionMaterial[0];
                break;
            case "coquetry":
                showActionInformation = GameObject.Instantiate(informationBar) as GameObject;
                showActionInformation.GetComponent<Renderer>().material = actionMaterial[1];
                break;
            case "noisy":
                showActionInformation = GameObject.Instantiate(informationBar) as GameObject;
                showActionInformation.GetComponent<Renderer>().material = actionMaterial[2];
                break;
            case "pull":
                showActionInformation = GameObject.Instantiate(informationBar) as GameObject;
                showActionInformation.GetComponent<Renderer>().material = actionMaterial[3];
                break;
            default:
                break;
        }
    }

    //鼠标离开时破坏信息面板实例
    void OnMouseExit()
    {
        Destroy(showActionInformation);
    }
}