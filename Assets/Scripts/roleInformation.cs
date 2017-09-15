using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//附在角色卡预制件上面的脚本
public class roleInformation : MonoBehaviour {
    public List<Material> roleMaterial = new List<Material>();
    Text introductions;//鼠标悬停于动作卡上时显示的解释文本
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

    //实现鼠标悬停在角色卡时，通过判断该预制件当前的标签，实例化信息面板的预制件
    //随后将不同的材质赋给他，来向玩家说明不同卡片的作用
    void OnMouseEnter()
    {
        switch (this.tag)
        {
            case "A.情敌":
                image.material = roleMaterial[0];
                introductions.text = "角色A：人物属性为擅长吵闹，当他使用吵闹动作卡时，" +
                    "非讨厌对应关系的赖床者沉睡度减少5%，愤怒值增加5%。并且场上每多出一" +
                    "张其他的角色卡，则吵闹动作卡的唤醒效率增加1%";
                break;
            case "B.同学":
                image.material = roleMaterial[1];
                introductions.text = "角色B：人物属性为擅长撒娇，当他使用撒娇动作卡时，" +
                    "除人物属性为讨厌撒娇的赖床者以外，沉睡度均会减少5%，愤怒值增加1%。" +
                    "但是场上上每多出一张其他的角色卡，则吵闹动作卡的唤醒效率减少1%。";
                break;
            case "C.同学":
                image.material = roleMaterial[2];
                introductions.text = "角色C：人物属性为擅长硬拽，当他使用硬拽动作卡时，" +
                    "非讨厌及基友对应关系的赖床者沉睡度减少10%，愤怒值增加10%。当他对讨" +
                    "厌对应关系的赖床者使用硬拽动作卡时，赖床者的沉睡度将减少15%，愤怒值" +
                    "增加20%。当他对基友对应关系的赖床者使用硬拽动作卡时，赖床者的沉睡度将减少10%，愤怒值增加1%。";
                break;
            case "D.同学":
                image.material = roleMaterial[3];
                introductions.text = "角色D：人物属性为擅长放弃，当他使用放弃动作卡时，" +
                    "非讨厌及基友对应关系的赖床者沉睡度减少5%，愤怒值增加0%。当他对讨厌" +
                    "对应关系的赖床者使用放弃动作卡时，赖床者的沉睡度将增加5%，愤怒值增" +
                    "加1%。当他对基友对应关系的赖床者使用放弃动作卡时，赖床者的沉睡度将减少10%，愤怒值增加0%。";
                break;
            case "E.同学":
                image.material = roleMaterial[4];
                introductions.text = "角色E：人物属性为喜欢撒娇，使用在他身上的动作卡有50%的几率变为撒娇动作卡";
                break;
            case "F.讨厌":
                image.material = roleMaterial[5];
                introductions.text = "角色F：人物属性为喜欢吵闹，使用在他身上的动作卡有50%的几率变为吵闹动作卡";
                break;
            case "G.同学":
                image.material = roleMaterial[6];
                introductions.text = "角色G：人物属性为喜欢硬拽，使用在他身上的动作卡有50%的几率变为硬拽动作卡";
                break;
            case "H.情敌":
                image.material = roleMaterial[7];
                introductions.text = "角色H：人物属性为喜欢放弃，使用在他身上的动作卡有50%的几率变为放弃动作卡";
                break;
            case "I.基友":
                image.material = roleMaterial[8]; ;
                introductions.text = "角色I：人物属性为特殊对应关系强化与赖床者处" +
                    "于基友关系的该角色卡上所打出的任何动作卡，赖床者的响应为沉睡度减少" +
                    "8%，愤怒值增加0%。与赖床者处于讨厌关系的角色卡上所打出的任何动作卡，" +
                    "赖床者的响应为沉睡度减少10%，愤怒值增加10%。与赖床者处于情敌关系的角" +
                    "色卡上所打出的撒娇动作卡，赖床者的响应为沉睡度减少5%，愤怒值增加5%。";
                break;
            case "J.同学":
                image.material = roleMaterial[9];
                introductions.text = "角色J：人物属性为没睡醒的家伙。当在该角色卡上打出" +
                    "放弃动作卡时，赖床者将替换为角色J，且沉睡度和愤怒值将重置为50%";
                break;
            case "K.同学":
                image.material = roleMaterial[10];
                introductions.text = "角色K：人物属性为多动症，场上的该角色卡有1/3的可能，" +
                    "将玩家在其他角色卡身上打出的动作卡转移成在自己身上打出。";
                break;
            case "L.同学":
                image.material = roleMaterial[11];
                introductions.text = "角色L：人物属性为和事佬，该角色卡可以将在自己身上打" +
                    "出的动作卡，转化为将赖床者愤怒值减少5%的动作卡——“讲道理”。";
                break;
            default:
                break;
        }
    }

    //鼠标离开时破坏创建的信息面板实例
    void OnMouseExit()
    {
        image.material = initialMaterial;
    }
}