using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rule : MonoBehaviour {
    Text introductionOfRule;//规则说明文本
    private float halfScreenW;//屏幕的一半宽
    private float halfScreenH;//屏幕的一半高
    public Button enterGame;//进入游戏界面的按钮

    // Use this for initialization
    void Start () {
        halfScreenW = Screen.width / 2;
        halfScreenH = Screen.height / 2;
        introductionOfRule = GameObject.Find("Canvas/rule").GetComponent<Text>();
        introductionOfRule.text = "《睡你麻痹起来High》为一款卡牌闯关类游戏。卡牌分为角色卡12种，分别为602寝室的12位不同成员。" +
            "动作卡分为4种，分别为：吵闹、硬拽、撒娇、放弃。" +
            "\n游戏初始状态为玩家统一持有两种由12张角色卡以及12组4种动作卡共计60张卡的卡组。游戏开始时，将会随机将四张角" +
            "色卡和两张动作卡置入玩家手牌" +
            "\n点击屏幕左侧写有结束回合的任一按键，都表示结束当前回合。但角色卡的结束键表明下回合将一张角色卡置入手牌，动" +
            "作卡的结束键表明下回合将两张动作卡置入手牌" +
            "\n玩家想要过关，所要做的就是使用不同角色，通过不同的角色使出不同的动作卡来将沉睡度降为0。每张角色卡都有特殊" +
            "的技能，且为公开情报" +
            "\n玩家每回合若打出了角色卡则无法使用动作卡，反之亦然。每回合只能打出一张角色卡，而每回合能打出的动作卡数等于" +
            "战场中的角色卡数" +
            "\n角色卡与沉睡者之间有四种对应关系，分别为同学、基友、情敌以及讨厌。当对处于情敌关系的角色卡打出撒娇动作卡时，" +
            "该角色卡会从游戏中除外，且当回合无法再出卡。游戏中的对应关系非公开情报" +
            "\n每关Boss有沉睡度和愤怒值两个属性，会根据玩家不同角色打出的动作卡而改变。沉睡值归零则通关，达到一百则失败。" +
            "而当愤怒值达到一百时，玩家将会被踢回第一关重新开始" +
            "\n处于同学关系的角色卡，在打出硬拽和吵闹动作卡时，沉睡度均减少3%，愤怒值则增加1%，打出放弃和撒娇动作卡时，" +
            "沉睡度均增加3%，愤怒值则减少5%" +
            "\n处于基友关系的角色卡上所打出的除放弃动作卡，赖床者的响应为沉睡度减少5%，愤怒值增加1%。打出放弃动作卡时，" +
            "沉睡度将减少10%，愤怒值增加5%" +
            "\n处于讨厌关系的角色卡上所打出的除放弃动作卡，赖床者的响应为沉睡度减少8%，愤怒值增加5%。打出放弃动作卡时，" +
            "沉睡度增加5%，愤怒值减少5%" +
            "\n处于情敌关系的角色卡上所打出的撒娇动作卡，赖床者的响应为沉睡度减少3%，愤怒值增加5%，打出其余动作卡时，" +
            "响应与同学关系一致";

        //如果点击了场景中的进入游戏按钮，则进入游戏界面
        enterGame.onClick.AddListener(delegate () {
            Application.LoadLevel("game");
        });
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
}