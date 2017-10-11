using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roleFieldResponse : MonoBehaviour {
    public List<Material> informatinMaterial = new List<Material>();//角色卡解释说明的材质
    Text introductions;//鼠标悬停于动作卡上时显示的解释文本
    private Image image;//用来存储场景中的image组件
    public Material initialMaterial;//战场的初始材质
    public int Relation = 0;//象征角色与Boss之间关系的数字
    private string CurTag = "Untagged";//记录角色卡区域当前的标签
    public int gameNum;//用来记录当前是第几关

    // Use this for initialization
    void Start () {
        introductions = GameObject.Find("Canvas/introduction").GetComponent<Text>();
        image = GameObject.Find("Canvas/Image").GetComponent<Image>();
        

    }

    // Update is called once per frame
    void Update () {
        //当角色卡打出改变角色卡区域标签时，调用根据角色卡与Boss之间的关系，赋予角色区域不同的数字的方法
        if (this.tag != CurTag)
        {
            RBRelation();
            //将CurTag改为当前标签
            CurTag = this.tag; 
        }

        if (this.tag == actionCardControl.curRFTag) {
            if (actionCardControl.AddOrReduce)
            {
                if (Relation != 1)
                    Relation -= 1;
            }
            else {
                if(Relation != 4)
                    Relation += 1;
            }
            actionCardControl.curRFTag = "";
        }

    }

    void OnMouseEnter()
    {
        switch (this.tag)
        {
            case "A":
                image.material = informatinMaterial[0];
                introductions.text = "角色A：人物属性为擅长吵闹，当他使用吵闹动作卡时，" +
                    "非讨厌对应关系的赖床者沉睡度减少5%，愤怒值增加5%。并且场上每多出一" +
                    "张其他的角色卡，则吵闹动作卡的唤醒效率增加1%";
                break;
            case "B":
                image.material = informatinMaterial[1];
                introductions.text = "角色B：人物属性为擅长撒娇，当他使用撒娇动作卡时，" +
                    "除人物属性为讨厌撒娇的赖床者以外，沉睡度均会减少5%，愤怒值增加1%。" +
                    "但是场上上每多出一张其他的角色卡，则吵闹动作卡的唤醒效率减少1%。";
                break;
            case "C":
                image.material = informatinMaterial[2];
                introductions.text = "角色C：人物属性为擅长硬拽，当他使用硬拽动作卡时，" +
                    "非讨厌及基友对应关系的赖床者沉睡度减少10%，愤怒值增加10%。当他对讨" +
                    "厌对应关系的赖床者使用硬拽动作卡时，赖床者的沉睡度将减少15%，愤怒值" +
                    "增加20%。当他对基友对应关系的赖床者使用硬拽动作卡时，赖床者的沉睡度将减少10%，愤怒值增加1%。";
                break;
            case "D":
                image.material = informatinMaterial[3];
                introductions.text = "角色D：人物属性为擅长放弃，当他使用放弃动作卡时，" +
                    "非讨厌及基友对应关系的赖床者沉睡度减少5%，愤怒值增加0%。当他对讨厌" +
                    "对应关系的赖床者使用放弃动作卡时，赖床者的沉睡度将增加5%，愤怒值增" +
                    "加1%。当他对基友对应关系的赖床者使用放弃动作卡时，赖床者的沉睡度将减少10%，愤怒值增加0%。";
                break;
            case "E":
                image.material = informatinMaterial[4];
                introductions.text = "角色E：人物属性为喜欢撒娇，使用在他身上的动作卡有50%的几率变为撒娇动作卡";
                break;
            case "F":
                image.material = informatinMaterial[5];
                introductions.text = "角色F：人物属性为喜欢吵闹，使用在他身上的动作卡有50%的几率变为吵闹动作卡";
                break;
            case "G":
                image.material = informatinMaterial[6];
                introductions.text = "角色G：人物属性为喜欢硬拽，使用在他身上的动作卡有50%的几率变为硬拽动作卡";
                break;
            case "H":
                image.material = informatinMaterial[7];
                introductions.text = "角色H：人物属性为喜欢放弃，使用在他身上的动作卡有50%的几率变为放弃动作卡";
                break;
            case "I":
                image.material = informatinMaterial[8];
                introductions.text = "角色I：人物属性为特殊对应关系强化与赖床者处" +
                    "于基友关系的该角色卡上所打出的任何动作卡，赖床者的响应为沉睡度减少" +
                    "8%，愤怒值增加0%。与赖床者处于讨厌关系的角色卡上所打出的任何动作卡，" +
                    "赖床者的响应为沉睡度减少10%，愤怒值增加10%。与赖床者处于情敌关系的角" +
                    "色卡上所打出的撒娇动作卡，赖床者的响应为沉睡度减少5%，愤怒值增加5%。";
                break;
            case "J":
                image.material = informatinMaterial[9];
                introductions.text = "角色J：人物属性为没睡醒的家伙。当在该角色卡上打出" +
                    "放弃动作卡时，赖床者将替换为角色J，且沉睡度和愤怒值将重置为50%";
                break;
            case "K":
                image.material = informatinMaterial[10];
                introductions.text = "角色K：人物属性为多动症，场上的该角色卡有1/3的可能，" +
                    "将玩家在其他角色卡身上打出的动作卡转移成在自己身上打出。";
                break;
            case "L":
                image.material = informatinMaterial[11];
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

    //根据当前关卡，调用不同的赋值方法
    void RBRelation() {
        switch (gameNum) {
            case 1:
                game1();
                break;
            case 2:
                game2();
                break;
            case 3:
                game3();
                break;
            case 4:
                game4();
                break;
            case 5:
                game5();
                break;
            case 6:
                game6();
                break;
            case 7:
                game7();
                break;
            case 8:
                game8();
                break;
            case 9:
                game9();
                break;
            case 10:
                //第十关全是同学关系
                Relation = 3;
                break;
            case 11:
                game11();
                break;
            case 12:
                game12();
                break;
            default:
                break;
        }
    }

    //根据角色卡与Boss之间的关系，赋予角色区域不同的数字
    void game1()
    {
        if (this.tag == "K")
            Relation = 4;
        else if (this.tag == "B")
            Relation = 1;
        else if (this.tag == "C" || this.tag == "H" || this.tag == "F")
            Relation = 2;
        else
            Relation = 3;
    }

    void game2()
    {
        if (this.tag == "G")
            Relation = 4;
        else if (this.tag == "H" || this.tag == "E")
            Relation = 1;
        else if (this.tag == "J" || this.tag == "C")
            Relation = 2;
        else
            Relation = 3;
    }

    void game3()
    {
        if (this.tag == "A")
            Relation = 4;
        else if (this.tag == "B")
            Relation = 1;
        else if (this.tag == "C")
            Relation = 2;
        else
            Relation = 3;
    }

    void game4()
    {
        if (this.tag == "A" || this.tag == "H")
        {
            //2代表情敌关系
            Relation = 2;
        }
        else if (this.tag == "F")
        {
            //1代表讨厌关系
            Relation = 1;
        }
        else if (this.tag == "G")
        {
            //4代表基友关系
            Relation = 4;
        }
        else
        {
            //3代表同学关系
            Relation = 3;
        }
    }

    void game5() {
        if (this.tag == "C")
            Relation = 4;
        else if (this.tag == "A")
            Relation = 1;
        else if (this.tag == "E" || this.tag == "K" || this.tag == "D")
            Relation = 2;
        else
            Relation = 3;
    }

    void game6() {
        if (this.tag == "F" || this.tag == "G")
            Relation = 4;
        else if (this.tag == "I")
            Relation = 1;
        else if (this.tag == "J")
            Relation = 2;
        else
            Relation = 3;
    }

    void game7() {
        if (this.tag == "E")
            Relation = 4;
        else if (this.tag == "D")
            Relation = 1;
        else if (this.tag == "J" || this.tag == "K")
            Relation = 2;
        else
            Relation = 3;
    }

    void game8() {
        if (this.tag == "A")
            Relation = 4;
        else if (this.tag == "G" || this.tag == "F")
            Relation = 2;
        else if (this.tag == "C" || this.tag == "D")
            Relation = 1;
        else
            Relation = 3;
    }

    void game9() {
        if (this.tag == "F" || this.tag == "K" || this.tag == "I" ||
            this.tag == "L" || this.tag == "H")
            Relation = 3;
        else
            Relation = 2;
    }

    void game11() {
        if (this.tag == "L" || this.tag == "I")
            Relation = 1;
        if (this.tag == "J")
            Relation = 2;
        if (this.tag == "D")
            Relation = 4;
        else
            Relation = 3;
    }

    void game12()
    {
        if (this.tag == "D" || this.tag == "K")
            Relation = 1;
        if (this.tag == "L")
            Relation = 2;
        if (this.tag == "A" || this.tag == "B")
            Relation = 4;
        else
            Relation = 3;
    }
}