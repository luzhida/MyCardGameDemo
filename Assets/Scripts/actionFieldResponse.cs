using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class actionFieldResponse : MonoBehaviour {
    public GameObject role;
    public static bool actionCardCanResponse;//用来确保不会反复调用动作卡的响应方法
    public GameObject informationBar;//信息面板预制件
    private GameObject showActionInformation;//实例化的信息面板
    public List<Material> informatinMaterial = new List<Material>();//动作卡解释说明的材质
    public Material initialMaterial;//角色卡区域的初始材质
    int randomNumber;//某些角色卡的特殊能力为将打在自己身上的动作卡随机进行改变，故设置该随机数
    //某些角色卡的特殊能力为将打在自己身上的动作卡随机进行改变，故设置动作卡卡材质的List集合
    //到时改变动作卡材质，模拟动作卡改变效果
    public List<Material> actionCardMaterials = new List<Material>();
    public GameObject enemy;
    public Material J;//J角色卡的材质，因为J的特殊技能为代替沉睡者
    private bool updateRoleField;//判断是否有某个角色卡从游戏中被除外
    public List<GameObject> roleFields = new List<GameObject>();//作为角色卡区域的游戏物体
    private int i;//角色卡区域游戏物体的List集合的下标

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        actionCardResponse();
        if (updateRoleField)
        {
            updateRoleFields();
        }
    }

    void actionCardResponse() {
        if (actionCardCanResponse) {
            //当动作卡置入战场改变动作卡区域的标签后，通过判断动作卡区域的标签，
            //再判断其上方动作卡区域的标签，来实行不同的相应动作，还未完成，暂做测试用
            //执行打出放弃动作卡时的响应方法
            if (this.tag == "abandon")
            {
                abandon();
                //每次响应完后修改actionCardCanResponse，避免重复响应
                actionCardCanResponse = false;
            }
            //执行打出撒娇动作卡时的响应方法
            if (this.tag == "coquetry")
            {
                coquetry();
                //每次响应完后修改actionCardCanResponse，避免重复响应
                actionCardCanResponse = false;
            }
            //执行打出吵闹动作卡时的响应方法
            if (this.tag == "noisy")
            {
                noisy();
                //每次响应完后修改actionCardCanResponse，避免重复响应
                actionCardCanResponse = false;
            }
            //执行打出硬拽动作卡时的响应方法
            if (this.tag == "pull")
            {
                pull();
                //每次响应完后修改actionCardCanResponse，避免重复响应
                actionCardCanResponse = false;
            }
            //执行打出讲道理动作卡时的响应方法
            if (this.tag == "makeSense")
            {
                makeSense();
                //每次响应完后修改actionCardCanResponse，避免重复响应
                actionCardCanResponse = false;
            }
            
        }
    }

    //实现鼠标悬停在动作卡时，通过判断该预制件当前的标签，实例化信息面板的预制件
    //随后将不同的材质赋给他，来向玩家说明不同卡片的作用
    void OnMouseEnter()
    {
        showActionInformation = GameObject.Instantiate(informationBar) as GameObject;
        switch (this.tag)
        {
            case "abandon":
                showActionInformation.GetComponent<Renderer>().material = informatinMaterial[0];
                break;
            case "coquetry":
                showActionInformation.GetComponent<Renderer>().material = informatinMaterial[1];
                break;
            case "noisy":
                showActionInformation.GetComponent<Renderer>().material = informatinMaterial[2];
                break;
            case "pull":
                showActionInformation.GetComponent<Renderer>().material = informatinMaterial[3];
                break;
            case "makeSense":
                showActionInformation.GetComponent<Renderer>().material = informatinMaterial[4];
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

    void abandon()
    {
        switch (role.tag)
        {
            case "A.情敌":
                gameControl.sleepyLevelRemaining += 3;
                gameControl.angryLevelRemaining -= 5;
                break;
            case "B.同学":
                gameControl.sleepyLevelRemaining += 3;
                gameControl.angryLevelRemaining -= 5;
                break;
            case "C.同学":
                gameControl.sleepyLevelRemaining += 3;
                gameControl.angryLevelRemaining -= 5;
                break;
            case "D.同学":
                gameControl.sleepyLevelRemaining -= 5;
                break;
            case "E.同学":
                randomNumber = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为撒娇的效果
                Debug.Log(randomNumber);
                if (randomNumber == 0)
                {
                    gameControl.sleepyLevelRemaining += 3;
                    gameControl.angryLevelRemaining -= 5;
                    this.tag = "coquetry";
                    this.GetComponent<Renderer>().material = actionCardMaterials[1];
                }
                else {
                    gameControl.sleepyLevelRemaining += 3;
                    gameControl.angryLevelRemaining -= 5;
                }
                break;
            case "F.讨厌":
                randomNumber = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为吵闹的效果
                if (randomNumber == 0)
                {
                    gameControl.sleepyLevelRemaining -= 8;
                    gameControl.angryLevelRemaining += 5;
                    this.tag = "noisy";
                    this.GetComponent<Renderer>().material = actionCardMaterials[2];
                }
                else
                {
                    gameControl.sleepyLevelRemaining += 5;
                    gameControl.angryLevelRemaining -= 5;
                }
                break;
            case "G.同学":
                randomNumber = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为硬拽的效果
                if (randomNumber == 0)
                {
                    gameControl.sleepyLevelRemaining -= 3;
                    gameControl.angryLevelRemaining += 1;
                    this.tag = "pull";
                    this.GetComponent<Renderer>().material = actionCardMaterials[3];
                }
                else
                {
                    gameControl.sleepyLevelRemaining += 3;
                    gameControl.angryLevelRemaining -= 5;
                }
                break;
            case "H.情敌":
                gameControl.sleepyLevelRemaining += 3;
                gameControl.angryLevelRemaining -= 5;
                break;
            case "I.基友":
                gameControl.sleepyLevelRemaining -= 8;
                break;
            //J角色卡的特殊技能为当他不处于讨厌或情敌关系时，玩家对他打出放弃卡时，他会替代沉睡者成为boss
            case "J.同学":
                enemy.GetComponent<Renderer>().material = J;
                role.GetComponent<Renderer>().material = initialMaterial;
                role.tag = "Untagged";
                gameControl.sleepyLevelRemaining = 50;
                gameControl.angryLevelRemaining = 50;
                gameControl.j--;
                updateRoleField = true;
                break;
            case "K.同学":
                gameControl.sleepyLevelRemaining += 3;
                gameControl.angryLevelRemaining -= 5;
                break;
            case "L.同学":
                gameControl.sleepyLevelRemaining += 3;
                gameControl.angryLevelRemaining -= 5;
                break;
            default:
                break;
        }
    }

    void coquetry()
    {
        switch (role.tag)
        {
            case "A.情敌":
                gameControl.sleepyLevelRemaining -= 3;
                gameControl.angryLevelRemaining += 5;
                //当情敌关系的角色卡打出撒娇时，将角色卡区域还原为初始材质，并去除标签，类似将该名角色卡从游戏中除外
                role.GetComponent<Renderer>().material = initialMaterial;
                role.tag = "Untagged";
                gameControl.j--;
                updateRoleField = true;
                break;
            case "B.同学":
                gameControl.sleepyLevelRemaining -= 5;
                gameControl.angryLevelRemaining += 1;
                break;
            case "C.同学":
                gameControl.sleepyLevelRemaining += 3;
                gameControl.angryLevelRemaining -= 5;
                break;
            case "D.同学":
                gameControl.sleepyLevelRemaining += 3;
                gameControl.angryLevelRemaining -= 5;
                break;
            case "E.同学":
                gameControl.sleepyLevelRemaining += 3;
                gameControl.angryLevelRemaining -= 5;
                break;
            case "F.讨厌":
                randomNumber = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为吵闹的效果
                if (randomNumber == 0)
                {
                    gameControl.sleepyLevelRemaining -= 8;
                    gameControl.angryLevelRemaining += 5;
                    this.tag = "noisy";
                    this.GetComponent<Renderer>().material = actionCardMaterials[2];
                }
                else
                {
                    gameControl.sleepyLevelRemaining -= 8;
                    gameControl.angryLevelRemaining += 5;
                }
                break;
            case "G.同学":
                randomNumber = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为硬拽的效果
                if (randomNumber == 0)
                {
                    gameControl.sleepyLevelRemaining -= 3;
                    gameControl.angryLevelRemaining += 1;
                    this.tag = "pull";
                    this.GetComponent<Renderer>().material = actionCardMaterials[3];
                }
                else
                {
                    gameControl.sleepyLevelRemaining += 3;
                    gameControl.angryLevelRemaining -= 5;
                }
                break;
            case "H.情敌":
                randomNumber = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为放弃的效果
                if (randomNumber == 0)
                {
                    gameControl.sleepyLevelRemaining += 3;
                    gameControl.angryLevelRemaining -= 5;
                    this.tag = "abandon";
                    this.GetComponent<Renderer>().material = actionCardMaterials[0];
                }
                else
                {
                    gameControl.sleepyLevelRemaining -= 3;
                    gameControl.angryLevelRemaining += 5;
                    //当情敌关系的角色卡打出撒娇时，将角色卡区域还原为初始材质，并去除标签，类似将该名角色卡从游戏中除外
                    role.GetComponent<Renderer>().material = initialMaterial;
                    role.tag = "Untagged";
                    gameControl.j--;
                    updateRoleField = true;
                }
                break;
            case "I.基友":
                gameControl.sleepyLevelRemaining -= 8;
                break;
            case "J.同学":
                gameControl.sleepyLevelRemaining += 3;
                gameControl.angryLevelRemaining -= 5;
                break;
            case "K.同学":
                gameControl.sleepyLevelRemaining += 3;
                gameControl.angryLevelRemaining -= 5;
                break;
            case "L.同学":
                gameControl.sleepyLevelRemaining += 3;
                gameControl.angryLevelRemaining -= 5;
                break;
            default:
                break;
        }
    }

    void noisy()
    {
        switch (role.tag)
        {
            case "A.情敌":
                gameControl.sleepyLevelRemaining = gameControl.sleepyLevelRemaining - 5 - gameControl.j;
                gameControl.angryLevelRemaining += 5;
                break;
            case "B.同学":
                gameControl.sleepyLevelRemaining -= 3;
                gameControl.angryLevelRemaining += 1;
                break;
            case "C.同学":
                gameControl.sleepyLevelRemaining -= 3;
                gameControl.angryLevelRemaining += 1;
                break;
            case "D.同学":
                gameControl.sleepyLevelRemaining -= 3;
                gameControl.angryLevelRemaining += 1;
                break;
            case "E.同学":
                randomNumber = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为撒娇的效果
                if (randomNumber == 0)
                {
                    gameControl.sleepyLevelRemaining += 3;
                    gameControl.angryLevelRemaining -= 5;
                    this.tag = "coquetry";
                    this.GetComponent<Renderer>().material = actionCardMaterials[1];
                }
                else
                {
                    gameControl.sleepyLevelRemaining -= 3;
                    gameControl.angryLevelRemaining += 1;
                }
                break;
            case "F.讨厌":
                gameControl.sleepyLevelRemaining -= 8;
                gameControl.angryLevelRemaining += 5;
                break;
            case "G.同学":
                randomNumber = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为硬拽的效果
                if (randomNumber == 0)
                {
                    gameControl.sleepyLevelRemaining -= 3;
                    gameControl.angryLevelRemaining += 1;
                    this.tag = "pull";
                    this.GetComponent<Renderer>().material = actionCardMaterials[3];
                }
                else
                {
                    gameControl.sleepyLevelRemaining -= 3;
                    gameControl.angryLevelRemaining += 1;
                }
                break;
            case "H.情敌":
                randomNumber = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为放弃的效果
                if (randomNumber == 0)
                {
                    gameControl.sleepyLevelRemaining += 3;
                    gameControl.angryLevelRemaining -= 5;
                    this.tag = "abandon";
                    this.GetComponent<Renderer>().material = actionCardMaterials[0];
                }
                else
                {
                    gameControl.sleepyLevelRemaining -= 3;
                    gameControl.angryLevelRemaining += 1;
                }
                break;
            case "I.基友":
                gameControl.sleepyLevelRemaining -= 8;
                break;
            case "J.同学":
                gameControl.sleepyLevelRemaining -= 3;
                gameControl.angryLevelRemaining += 1;
                break;
            case "K.同学":
                gameControl.sleepyLevelRemaining -= 3;
                gameControl.angryLevelRemaining += 1;
                break;
            case "L.同学":
                gameControl.sleepyLevelRemaining -= 3;
                gameControl.angryLevelRemaining += 1;
                break;
            default:
                break;
        }
    }

    void pull() {
        switch (role.tag)
        {
            case "A.情敌":
                gameControl.sleepyLevelRemaining -= 3;
                gameControl.angryLevelRemaining += 1;
                break;
            case "B.同学":
                gameControl.sleepyLevelRemaining -= 3;
                gameControl.angryLevelRemaining += 1;
                break;
            case "C.同学":
                gameControl.sleepyLevelRemaining -= 10;
                gameControl.angryLevelRemaining += 10;
                break;
            case "D.同学":
                gameControl.sleepyLevelRemaining += 5;
                break;
            case "E.同学":
                randomNumber = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为撒娇的效果
                if (randomNumber == 0)
                {
                    gameControl.sleepyLevelRemaining += 3;
                    gameControl.angryLevelRemaining -= 5;
                    this.tag = "coquetry";
                    this.GetComponent<Renderer>().material = actionCardMaterials[1];
                }
                else
                {
                    gameControl.sleepyLevelRemaining -= 3;
                    gameControl.angryLevelRemaining += 1;
                }
                break;
            case "F.讨厌":
                randomNumber = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为吵闹的效果
                if (randomNumber == 0)
                {
                    gameControl.sleepyLevelRemaining -= 8;
                    gameControl.angryLevelRemaining += 5;
                    this.tag = "noisy";
                    this.GetComponent<Renderer>().material = actionCardMaterials[2];
                }
                else
                {
                    gameControl.sleepyLevelRemaining -= 8;
                    gameControl.angryLevelRemaining += 5;
                }
                break;
            case "G.同学":
                gameControl.sleepyLevelRemaining -= 3;
                gameControl.angryLevelRemaining += 1;
                break;
            case "H.情敌":
                randomNumber = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为放弃的效果
                if (randomNumber == 0)
                {
                    gameControl.sleepyLevelRemaining += 3;
                    gameControl.angryLevelRemaining -= 5;
                    this.tag = "abandon";
                    this.GetComponent<Renderer>().material = actionCardMaterials[0];
                }
                else
                {
                    gameControl.sleepyLevelRemaining -= 3;
                    gameControl.angryLevelRemaining += 1;
                }
                break;
            case "I.基友":
                gameControl.sleepyLevelRemaining -= 8;
                break;
            case "J.同学":
                gameControl.sleepyLevelRemaining -= 3;
                gameControl.angryLevelRemaining += 1;
                break;
            case "K.同学":
                gameControl.sleepyLevelRemaining -= 3;
                gameControl.angryLevelRemaining += 1;
                break;
            case "L.同学":
                gameControl.sleepyLevelRemaining -= 3;
                gameControl.angryLevelRemaining += 1;
                break;
            default:
                break;
        }
    }

    void makeSense() {
        gameControl.angryLevelRemaining -= 8;
    }

    //在游戏过程中某角色卡被移除出战场后，进行位置的更新
    void updateRoleFields()
    {
        for (int i = 0; i < 5; i++)
        {
            while (roleFields[i].tag == "Untagged")
            {
                for (int j = i; j < 5; j++)
                {
                    roleFields[j].GetComponent<Renderer>().material = roleFields[j + 1].GetComponent<Renderer>().material;
                    roleFields[j].tag = roleFields[j + 1].tag;
                }
            }
            updateRoleField = false;
        }
    }
}