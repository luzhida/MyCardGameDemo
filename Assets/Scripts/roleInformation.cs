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
            case "A":
                image.material = roleMaterial[0];
                introductions.text = "角色A：人物属性为擅长吵闹，当他使用吵闹动作卡时，同" +
                    "学及情敌关系的赖床者沉睡度减少5%，愤怒值增加5%。并且场上每多出一张其" +
                    "他的角色卡，则吵闹动作卡的唤醒效率增加1%。讨厌关系时，此限制规则变为" +
                    "每多出一张其他的角色卡，则吵闹动作卡的唤醒效率减少1%。基友关系时，每" +
                    "多出一张角色卡，唤醒效率增加2%。";
                break;
            case "B":
                image.material = roleMaterial[1];
                introductions.text = "角色B：人物属性为擅长撒娇，当他使用撒娇动作卡时，同" +
                    "学、情敌关系的赖床者，沉睡度及愤怒值均会减少5%。讨厌关系，场上每多出" +
                    "一张其他的角色卡，则撒娇的唤醒效率减少1%。基友关系时，响应翻倍，撒娇" +
                    "会使沉睡度及愤怒值均会减少10%。当他与赖床者处于情敌关系且打出了撒娇动" +
                    "作卡时，将场上的角色卡全部从游戏中除外。";
                break;
            case "C":
                image.material = roleMaterial[2];
                introductions.text = "角色C：人物属性为擅长硬拽，当他使用硬拽动作卡时，" +
                    "非讨厌及基友对应关系的赖床者沉睡度减少10%，愤怒值增加10%。当他对讨" +
                    "厌对应关系的赖床者使用硬拽动作卡时，赖床者的沉睡度将减少15%，愤怒值" +
                    "增加20%。当他对基友对应关系的赖床者使用硬拽动作卡时，赖床者的沉睡度将减少10%，愤怒值增加1%。";
                break;
            case "D":
                image.material = roleMaterial[3];
                introductions.text = "角色D：人物属性为擅长放弃，当他使用放弃动作卡时，" +
                    "非讨厌及基友对应关系的赖床者沉睡度减少5%，愤怒值增加0%。当他对讨厌" +
                    "对应关系的赖床者使用放弃动作卡时，赖床者的沉睡度将减少5%，愤怒值增" +
                    "加1%。当他对基友对应关系的赖床者使用放弃动作卡时，赖床者的沉睡度将减少10%，愤怒值增加0%。";
                break;
            case "E":
                image.material = roleMaterial[4];
                introductions.text = "角色E：人物属性为喜欢撒娇，使用在他身上的动作卡有50%的几率变为撒娇动作卡";
                break;
            case "F":
                image.material = roleMaterial[5];
                introductions.text = "角色F：人物属性为喜欢吵闹，使用在他身上的动作卡有50%的几率变为吵闹动作卡";
                break;
            case "G":
                image.material = roleMaterial[6];
                introductions.text = "角色G：人物属性为喜欢硬拽，使用在他身上的动作卡有50%的几率变为硬拽动作卡";
                break;
            case "H":
                image.material = roleMaterial[7];
                introductions.text = "角色H：人物属性为喜欢放弃，使用在他身上的动作卡有50%的几率变为放弃动作卡";
                break;
            case "I":
                image.material = roleMaterial[8]; ;
                introductions.text = "角色I：特殊对应关系强化。与赖床者处于基友关系的该角色卡上所打出的任何动作卡，赖床者" +
                    "的响应为沉睡度减少16%，愤怒值增加0%。与赖床者处于讨厌关系的角色卡上所打出的任何动作卡，赖" +
                    "床者的响应为沉睡度增加10%，愤怒值增加20%。与赖床者处于情敌关系的角色卡上所打出的撒娇动作" +
                    "卡，赖床者的响应为沉睡度增加10%，愤怒值增加5%，其余卡打出后的响应与同学关系一致。";
                break;
            case "J":
                image.material = roleMaterial[9];
                introductions.text = "角色J：人物属性为没睡醒的家伙。当在同学关系的该角色卡上打出放弃动作卡时，" +
                    "角色J将代替沉睡者成为此关BOSS，且沉睡度及愤怒值重置为50%";
                break;
            case "K":
                image.material = roleMaterial[10];
                introductions.text = "角色K：人物属性为多动症，场上的该角色卡有1/3的可能，" +
                    "将玩家在其他角色卡身上打出的动作卡转移成在自己身上打出。";
                break;
            case "L":
                image.material = roleMaterial[11];
                introductions.text = "角色L：人物属性为和事佬，该角色卡可以将在自己身上打" +
                    "出的动作卡，转化为将赖床者愤怒值减少8%的动作卡——“讲道理”。";
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