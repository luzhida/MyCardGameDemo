using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//动作卡预制件上面的脚本
public class actionInformation : MonoBehaviour {
    public List<Material> actionMaterial = new List<Material>();//用于动作卡解释说明的材质
    Text introductions;//悬停于动作卡上而出现的解释文本
    private Image image;//用来存储场景中的image组件
    public Material initialMaterial;//战场的初始材质

    // Use this for initialization
    void Start () {
		introductions = GameObject.Find("Canvas/introduction").GetComponent<Text>();
        image = GameObject.Find("Canvas/Image").GetComponent<Image>();
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
                image.material = actionMaterial[0];
                introductions.text = "动作卡：放弃。走吧走吧都散了吧，这货没救了！";
                break;
            case "coquetry":
                image.material = actionMaterial[1];
                introductions.text = "动作卡：撒娇。模仿其女神撒娇似的声音来叫赖床者起床，起——床——啦。";
                break;
            case "noisy":
                image.material = actionMaterial[2];
                introductions.text = "动作卡：吵闹。在寝室大吵大闹，睡你麻痹，起来嗨！！！";
                break;
            case "pull":
                image.material = actionMaterial[3];
                introductions.text = "动作卡：硬拽。直接把赖床者拽起来，草泥马，赶紧给老子起来！";
                break;
            default:
                break;
        }
    }

    //鼠标离开时将image组件的材质变为初始材质
    void OnMouseExit()
    {
        image.material = initialMaterial;
    }
}