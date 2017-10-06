using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class actionCardControl : MonoBehaviour {
    public Transform card01;//用来确定动作卡的手排位置的场景隐藏游戏物体
    public Transform card03;//用来确定动作卡的手排位置的场景隐藏游戏物体
    private float xOffset;//用来保持动作卡卡之间保持的固定间距
    private List<GameObject> cards = new List<GameObject>();//用来存储手牌卡片的List集合
    public GameObject actionCard;//用来实例化的卡片预制件
    public List<Material> actionMaterials = new List<Material>();//存储所需的动作卡材质的List集合
    public List<GameObject> actionField = new List<GameObject>();//动作卡置入战场后的区域
    public List<GameObject> actionDeck = new List<GameObject>();//存储初始化动作卡卡组的List集合
    int i;//actionDeck[]的下标
    public static int k;//actionField数组的下标,由于每次回合开始都会从左至右再依次打出动作卡，故设置为静态变量
    int someNum;//为赋予不同动作卡材质产生的随机数
    private List<int> result = new List<int>(48);//存储为赋予不同动作卡材质产生的随机数
    public Button buttonPrefab;//标志回合结束，并标明下回合开始将两张动作卡卡加入手牌的结束回合按钮
    private bool ActionCardGenerating;//表明现在是否为动作卡发牌阶段的布尔值
    public static int canUseActionCardAmount;//能使用的动作卡数量

    public Material initialMaterial;//战场的初始材质
    public List<Material> actionCardMaterials = new List<Material>();
    public GameObject enemy;
    private bool updateRoleField;//判断是否有某个角色卡从游戏中被除外
    Text sleepyVariableQuantity;//沉睡度的变化量
    Text angryVariableQuantity;//愤怒值的变化量
    public List<GameObject> roleField = new List<GameObject>();
    public Material J;//J角色的材质
    private int temporaryNumber;//为了进行数据交换而临时设置的数
    private bool changeForK;//判断是否因为K的特殊技能而产生了某些改变
    public static string curRFTag;//当前角色卡区域的标签
    public static bool AddOrReduce;//判断沉睡度是增加还是减少了15%
    public int gameNum;//表明这是第几关
    private int startSleepyLevel;//第二关开始时的沉睡度
    private int startAngryLevel;//第二关开始的愤怒值

    // Use this for initialization
    void Start () {
        i = 0;
        k = 0;
        canUseActionCardAmount = gameControl.j;
        xOffset = card01.position.x - card03.position.x;
        //创建初始动作卡卡组
        BuildDeck();
        //游戏开始时从卡组中将两张角色卡置入手牌
        for (int i = 0; i < 2; i++)
        {
            AddActionCard();
        }
        //如果点击了场景中的结束（动作卡)按钮，则表示结束回合，执行敌方回合的方法
        buttonPrefab.onClick.AddListener(delegate () {
            EnemyTurn();
        });
        sleepyVariableQuantity = GameObject.Find("Canvas/sleepyVariableQuantity").GetComponent<Text>();
        angryVariableQuantity = GameObject.Find("Canvas/angryVariableQuantity").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        //动作卡发牌阶段每次将两张动作卡加入手牌，那之后动作卡发牌阶段结束
        if (ActionCardGenerating)
        {
            AddActionCard();
            AddActionCard();
            ActionCardGenerating = false;
        }

        //首先判断是否点击了鼠标左键
        if (Input.GetMouseButtonDown(0))
        {
            //执行将动作卡置入战场的方法
            enterBattlefield();
        }
        //手牌数改变后，执行该方法使手牌位置相应的改动
        UpdateShow();
        //因为在对应关系及人物技能使得角色卡从游戏中除外时，更新角色卡区域
        if (updateRoleField)
        {
           updateRoleFields();
        }
    }

    private void BuildDeck()
    {
        //角色卡一共48张，故卡组也为48张
        for (int i = 0; i < 48; i++)
        {
            //下标越小的actionDeck[]，y轴的位置越高
            int j = 47 - i;
            actionDeck[j] = GameObject.Instantiate(actionCard) as GameObject;
            //每创建一张卡片，将他的Y轴坐标上移
            float y = (float)i / 10 - 10;
            actionDeck[j].transform.position = new Vector3(-8, y, -8);
        }
    }

    public void AddActionCard()
    {
        //动作卡不能大于6，否则就不会发牌
        if (cards.Count < 6)
        {
            do
            {
                someNum = Random.Range(0, actionMaterials.Count);
            } while (result.Contains(someNum));
            result.Add(someNum);
            Vector3 toPosition = card03.position - new Vector3(xOffset, 0, 0) * cards.Count;
            iTween.MoveTo(actionDeck[i], toPosition, 1f);
            actionDeck[i].GetComponent<Renderer>().material = actionMaterials[someNum];
            int fuck = (someNum + 1) % 4;
            switch (fuck)
            {
                case 0:
                    actionDeck[i].tag = "pull";
                    break;
                case 1:
                    actionDeck[i].tag = "abandon";
                    break;
                case 2:
                    actionDeck[i].tag = "coquetry";
                    break;
                case 3:
                    actionDeck[i].tag = "noisy";
                    break;
                default:
                    break;
            }
            actionDeck[i].transform.rotation = Quaternion.Euler(90, 0, 0);
            cards.Add(actionDeck[i]);
            i++;
        }
    }

    //失败后的操作
    void loseGame()
    {
        switch (gameNum) {
            case 1:
                SceneManager.LoadScene("game1", LoadSceneMode.Single);
                break;
            case 2:
                SceneManager.LoadScene("game2", LoadSceneMode.Single);
                break;
            case 3:
                SceneManager.LoadScene("game3", LoadSceneMode.Single);
                break;
            case 4:
                SceneManager.LoadScene("game4", LoadSceneMode.Single);
                break;
            case 5:
                SceneManager.LoadScene("game5", LoadSceneMode.Single);
                break;
            default:
                break;
        }
    }

    private void enterBattlefield()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //定义一条射线，这条射线从摄像机屏幕射向鼠标所在位置
        RaycastHit hit; //声明一个碰撞的点(暂且理解为碰撞的交点)
        if (Physics.Raycast(ray, out hit)) //如果真的发生了碰撞，ray这条射线在hit点与别的物体碰撞了
        {
            //如果碰撞物带有四种动作卡的标签，则将碰撞物的材质、标签依次赋予作为动作卡场地的预制件
            if (hit.collider.gameObject.tag == "abandon" || hit.collider.gameObject.tag == "coquetry"
                || hit.collider.gameObject.tag == "noisy" || hit.collider.gameObject.tag == "pull")
            {
                //当可以使用动作卡且可使用数不为0时，下面的代码才会被执行
                if (gameControl.canUseActionCard && canUseActionCardAmount != 0) {
                    //当动作卡被使用后，当回合也无法再使用角色卡
                    gameControl.canUseRoleCard = false;
                    //产生随机数，当随机数等于0时，执行将动作卡移至角色卡J区域打出的操作（与角色J的特殊技能有关）
                    someNum = Random.Range(0, 3);
                    if (someNum == 0 && gameControl.KinBattlefield)
                    {
                        changeForK = true;
                        temporaryNumber = k;
                        k = gameControl.positionOfK;//将要添加材质的动作卡区域改为K角色卡下的动作卡区域
                        //如果L角色的技能被使用，则最近一次打出的动作卡会被转换成为将愤怒值减8的动作卡“讲道理”
                        if (gameControl.UseSkillOfL)
                        {
                            actionField[k].GetComponent<Renderer>().material = actionCardMaterials[4];
                            actionField[k].tag = "makeSense";
                            //L角色的技能只能改变一张动作卡
                            gameControl.UseSkillOfL = false;
                        }
                        else
                        {
                            actionField[k].GetComponent<Renderer>().material = hit.collider.gameObject.GetComponent<Renderer>().material;
                            actionField[k].tag = hit.collider.gameObject.tag;
                        }
                    }
                    else
                    {
                        //如果L角色的技能被使用，则最近一次打出的动作卡会被转换成为将愤怒值减8的动作卡“讲道理”
                        if (gameControl.UseSkillOfL)
                        {
                            actionField[k].GetComponent<Renderer>().material = actionCardMaterials[4];
                            actionField[k].tag = "makeSense";
                            //L角色的技能只能改变一张动作卡
                            gameControl.UseSkillOfL = false;
                        }
                        else
                        {
                            actionField[k].GetComponent<Renderer>().material = hit.collider.gameObject.GetComponent<Renderer>().material;
                            actionField[k].tag = hit.collider.gameObject.tag;
                        }
                    }
                    //遍历存储手牌的List集合，将标签与碰撞物一致的破坏，实现类似打出手牌的效果
                    for (int i = 0; i < cards.Count; i++)
                    {
                        if (cards[i].GetComponent<Renderer>().material == hit.collider.gameObject.GetComponent<Renderer>().material)
                        {
                            Destroy(cards[i]);
                            cards.RemoveAt(i);
                            break;
                        }
                    }
                    canUseActionCardAmount--;
                    actionCardResponse();
                    k++;
                }
                
            }
            
        }
    }

    private void EnemyTurn()
    {
        //动作卡抽完判负
        if (i == 48)
        {
            loseGame();
        }
        else
        {
            //虽然第二关Boss属性为过度响应，然而每回合却必须把沉睡度及愤怒值的增减量控制在20%以内，否则下回合将重置愤怒值和沉睡度
            if (gameNum == 3 && (System.Math.Abs(gameControl.sleepyLevelRemaining - gameControl.startSleepyLevel) > 20 ||
                System.Math.Abs(gameControl.angryLevelRemaining - gameControl.startAngryLevel) > 20))
            {
                gameControl.sleepyLevelRemaining = gameControl.startSleepyLevel;
                gameControl.angryLevelRemaining = gameControl.startAngryLevel;
            }
            else
            {
                //每回合结束会记录当前回合的沉睡度及愤怒值为初始的沉睡度及愤怒值
                gameControl.startSleepyLevel = gameControl.sleepyLevelRemaining;
                gameControl.startAngryLevel = gameControl.angryLevelRemaining;
            }
            //将变化沉睡度增加
            //根据不同关卡定义增加量
            if (gameNum == 2)
            {
                gameControl.sleepyLevelRemaining += 3;
                gameControl.angryLevelRemaining += 5;
            }
            else if (gameNum == 1)
            {
                gameControl.sleepyLevelRemaining += 10;
            }
            else
            {
                gameControl.sleepyLevelRemaining += 5;
            }
            //表示结束敌人回合，再次进入动作卡发牌阶段
            ActionCardGenerating = true;
            //当结束回合之后，下回合打出的动作卡会依次重新置入动作卡区域
            k = 0;
            //每次结束回合后，令可使用的动作卡数等于当前战场上的角色卡数
            canUseActionCardAmount = gameControl.j;
            //当角色卡区域被置满角色卡后，则无法再打出角色卡
            if (gameControl.j > 4)
            {
                gameControl.canUseRoleCard = false;
            }
            else
            {
                gameControl.canUseRoleCard = true;
            }
            gameControl.canUseActionCard = true;
        }
    }

    void UpdateShow()
    {
        //将每张手牌移动到对应位置
        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 toPosition = card03.position - new Vector3(xOffset, 0, 0) * i;

            iTween.MoveTo(cards[i], toPosition, 0.5f);
        }
    }

    //响应打出的不同动作卡
    void actionCardResponse()
    {
        //当动作卡置入战场改变动作卡区域的标签后，通过判断动作卡区域的标签，
        //再判断其上方动作卡区域的标签，来实行不同的相应动作
        switch (actionField[k].tag)
        {
            case "abandon":
                abandon();
                RecordRFTag();
                break;
            case "coquetry":
                coquetry();
                RecordRFTag();
                break;
            case "noisy":
                //第一关沉睡者的特殊属性为免疫吵闹
                if (gameNum != 4) {
                    noisy();
                    RecordRFTag();
                }
                break;
            case "pull":
                pull();
                RecordRFTag();
                break;
            case "makeSense":
                makeSense();
                RecordRFTag();
                break;
            default:
                break;
        }
        if (changeForK) {
            k = temporaryNumber;
            changeForK = false;
        }
    }

    void abandon()
    {
        roleFieldResponse theScript = roleField[k].GetComponent<roleFieldResponse>();
        switch (theScript.Relation)
        {
            //处于讨厌关系
            case 1:
                HatedManRsAbandon();
                break;
            //处于情敌关系
            case 2:
                RialInLoveRsAbandon();
                break;
            //处于同学关系
            case 3:
                ClassmateRsAbandon();
                break;
            //处于基友关系
            case 4:
                FriendRsAbandon();
                break;
            default:
                break;
        }
    }

    void coquetry()
    {
        roleFieldResponse theScript = roleField[k].GetComponent<roleFieldResponse>();
        switch (theScript.Relation)
        {
            //处于讨厌关系
            case 1:
                HatedManRsCoquetry();
                break;
            //处于情敌关系
            case 2:
                RialInLoveRsCoquetry();
                break;
            //处于同学关系
            case 3:
                ClassmateRsCoquetry();
                break;
            //处于基友关系
            case 4:
                FriendRsCoquetry();
                break;
            default:
                break;
        }
    }

    void noisy()
    {
        roleFieldResponse theScript = roleField[k].GetComponent<roleFieldResponse>();
        switch (theScript.Relation)
        {
            //处于讨厌关系
            case 1:
                HatedManRsNoisy();
                break;
            //处于情敌关系
            case 2:
                RialInLoveRsNoisy();
                break;
            //处于同学关系
            case 3:
                ClassmateRsNoisy();
                break;
            //处于基友关系
            case 4:
                FriendRsNoisy();
                break;
            default:
                break;
        }
    }

    void pull()
    {
        roleFieldResponse theScript = roleField[k].GetComponent<roleFieldResponse>();
        switch (theScript.Relation)
        {
            //处于讨厌关系
            case 1:
                HatedManRsPull();
                break;
            //处于情敌关系
            case 2:
                RialInLoveRsPull();
                break;
            //处于同学关系
            case 3:
                ClassmateRsPull();
                break;
            //处于基友关系
            case 4:
                FriendRsPull();
                break;
            default:
                break;
        }
    }

    void makeSense()
    {
        gameControl.angryLevelRemaining -= 8;
        sleepyVariableQuantity.text = "0";
        angryVariableQuantity.text = "↓8";
    }

    //同学关系下的各角色卡对放弃动作卡的响应方法
    void ClassmateRsAbandon() {

        switch (roleField[k].tag)
        {
            case "D":
                gameControl.sleepyLevelRemaining -= 5;
                angryVariableQuantity.text = "0";
                sleepyVariableQuantity.text = "↓5";
                break;
            case "E":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为撒娇的效果
                if (someNum == 0)
                {
                    NormalAbOrCqResponse();
                    actionField[k].tag = "coquetry";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[1];
                }
                else
                {
                    NormalAbOrCqResponse();
                }
                break;
            case "F":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为吵闹的效果
                if (someNum == 0)
                {
                    if (gameNum != 1)
                        NormalNsOrPlResponse();
                    actionField[k].tag = "noisy";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[2];
                }
                else
                {
                    NormalAbOrCqResponse();
                }
                break;
            case "G":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为硬拽的效果
                if (someNum == 0)
                {
                    NormalNsOrPlResponse();
                    actionField[k].tag = "pull";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[3];
                }
                else
                {
                    NormalAbOrCqResponse();
                }
                break;
            //J角色卡的特殊技能为当他不处于讨厌或情敌关系时，玩家对他打出放弃卡时，他会替代沉睡者成为boss
            case "J":
                enemy.GetComponent<Renderer>().material = J;
                roleField[k].GetComponent<Renderer>().material = initialMaterial;
                roleField[k].tag = "Untagged";
                gameControl.sleepyLevelRemaining = 50;
                gameControl.angryLevelRemaining = 50;
                gameControl.j--;
                updateRoleField = true;
                gameControl.canUseActionCard = false;
                break;
            default:
                NormalAbOrCqResponse();
                break;
        }
    }

    //同学关系下的各角色卡对撒娇动作卡的响应方法
    void ClassmateRsCoquetry()
    {
        switch (roleField[k].tag)
        {
            case "B":
                gameControl.sleepyLevelRemaining = gameControl.sleepyLevelRemaining - 5;
                sleepyVariableQuantity.text = "↓5";
                gameControl.angryLevelRemaining -= 5;
                angryVariableQuantity.text = "↓5";
                break;
            case "F":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为吵闹的效果
                if (someNum == 0)
                {
                    if (gameNum != 1)
                        NormalNsOrPlResponse();
                    actionField[k].tag = "noisy";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[2];
                }
                else
                {
                    NormalAbOrCqResponse();
                }
                break;
            case "G":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为硬拽的效果
                if (someNum == 0)
                {
                    NormalNsOrPlResponse();
                    actionField[k].tag = "pull";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[3];
                }
                else
                {
                    NormalAbOrCqResponse();
                }
                break;
            case "H":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为放弃的效果
                if (someNum == 0)
                {
                    NormalAbOrCqResponse();
                    actionField[k].tag = "abandon";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[0];
                }
                else
                {
                    NormalAbOrCqResponse();
                }
                break;
            default:
                NormalAbOrCqResponse();
                break;
        }
    }

    //同学关系下的各角色卡对吵闹动作卡的响应方法
    void ClassmateRsNoisy()
    {
        switch (roleField[k].tag)
        {
            case "A":
                gameControl.sleepyLevelRemaining = gameControl.sleepyLevelRemaining - 5 - gameControl.j;
                sleepyVariableQuantity.text = "↓" + (5 + gameControl.j).ToString();
                gameControl.angryLevelRemaining += 5;
                angryVariableQuantity.text = "↑5";
                break;
            case "E":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为撒娇的效果
                if (someNum == 0)
                {
                    NormalAbOrCqResponse();
                    actionField[k].tag = "coquetry";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[1];
                }
                else
                {
                    NormalNsOrPlResponse();
                }
                break;
            case "G":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为硬拽的效果
                if (someNum == 0)
                {
                    NormalNsOrPlResponse();
                    actionField[k].tag = "pull";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[3];
                }
                else
                {
                    NormalNsOrPlResponse();
                }
                break;
            case "H":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为放弃的效果
                if (someNum == 0)
                {
                    NormalAbOrCqResponse();
                    actionField[k].tag = "abandon";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[0];
                }
                else
                {
                    NormalNsOrPlResponse();
                }
                break;
            default:
                NormalNsOrPlResponse();
                break;
        }
    }

    //同学关系下的各角色卡对硬拽动作卡的响应方法
    void ClassmateRsPull()
    {
        switch (roleField[k].tag)
        {
            case "C":
                gameControl.sleepyLevelRemaining -= 10;
                sleepyVariableQuantity.text = "↓10";
                gameControl.angryLevelRemaining += 10;
                angryVariableQuantity.text = "↑10";
                break;
            case "E":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为撒娇的效果
                if (someNum == 0)
                {
                    NormalAbOrCqResponse();
                    actionField[k].tag = "coquetry";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[1];
                }
                else
                {
                    NormalNsOrPlResponse();
                }
                break;
            case "F":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为吵闹的效果
                if (someNum == 0)
                {
                    if (gameNum != 1)
                        NormalNsOrPlResponse();
                    actionField[k].tag = "noisy";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[2];
                }
                else
                {
                    NormalNsOrPlResponse();
                }
                break;
            case "H":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为放弃的效果
                if (someNum == 0)
                {
                    NormalAbOrCqResponse();
                    actionField[k].tag = "abandon";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[0];
                }
                else
                {
                    NormalNsOrPlResponse();
                }
                break;
            default:
                NormalNsOrPlResponse();
                break;
        }
    }

    //情敌关系下的各角色卡对放弃动作卡的响应方法
    void RialInLoveRsAbandon()
    {
        switch (roleField[k].tag)
        {
            case "D":
                gameControl.sleepyLevelRemaining -= 5;
                angryVariableQuantity.text = "0";
                sleepyVariableQuantity.text = "↓5";
                break;
            case "E":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为撒娇的效果
                if (someNum == 0)
                {
                    NormalAbOrCqResponse();
                    actionField[k].tag = "coquetry";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[1];
                    //当情敌关系的角色卡打出撒娇时，将角色卡区域还原为初始材质，并去除标签，类似将该名角色卡从游戏中除外
                    roleField[k].GetComponent<Renderer>().material = initialMaterial;
                    roleField[k].tag = "Untagged";
                    gameControl.j--;
                    updateRoleField = true;
                    gameControl.canUseActionCard = false;
                }
                else
                {
                    NormalAbOrCqResponse();
                }
                break;
            case "F":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为吵闹的效果
                if (someNum == 0)
                {
                    if (gameNum != 1)
                        NormalNsOrPlResponse();
                    actionField[k].tag = "noisy";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[2];
                }
                else
                {
                    NormalAbOrCqResponse();
                }
                break;
            case "G":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为硬拽的效果
                if (someNum == 0)
                {
                    NormalNsOrPlResponse();
                    actionField[k].tag = "pull";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[3];
                }
                else
                {
                    NormalAbOrCqResponse();
                }
                break;
            default:
                NormalAbOrCqResponse();
                break;
        }
    }

    //情敌关系下的各角色卡对撒娇动作卡的响应方法
    void RialInLoveRsCoquetry()
    {
        switch (roleField[k].tag)
        {
            case "B":
                gameControl.sleepyLevelRemaining = gameControl.sleepyLevelRemaining - 5;
                sleepyVariableQuantity.text = "↓5";
                gameControl.angryLevelRemaining -= 5;
                angryVariableQuantity.text = "↓5";
                //当情敌关系的B角色卡打出撒娇时，将战场中的角色卡全部从游戏中除外
                for (int i = k; i >= 0; i--) {
                    roleField[i].GetComponent<Renderer>().material = initialMaterial;
                    roleField[i].tag = "Untagged";
                    gameControl.j--;
                    gameControl.canUseActionCard = false;
                }
                break;
            case "F":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为吵闹的效果
                if (someNum == 0)
                {
                    if (gameNum != 1)
                        NormalNsOrPlResponse();
                    actionField[k].tag = "noisy";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[2];
                }
                else
                {
                    NormalAbOrCqResponse();
                    //当情敌关系的角色卡打出撒娇时，将角色卡区域还原为初始材质，并去除标签，类似将该名角色卡从游戏中除外
                    roleField[k].GetComponent<Renderer>().material = initialMaterial;
                    roleField[k].tag = "Untagged";
                    gameControl.j--;
                    updateRoleField = true;
                    gameControl.canUseActionCard = false;
                }
                break;
            case "G":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为硬拽的效果
                if (someNum == 0)
                {
                    NormalNsOrPlResponse();
                    actionField[k].tag = "pull";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[3];
                }
                else
                {
                    NormalAbOrCqResponse();
                    //当情敌关系的角色卡打出撒娇时，将角色卡区域还原为初始材质，并去除标签，类似将该名角色卡从游戏中除外
                    roleField[k].GetComponent<Renderer>().material = initialMaterial;
                    roleField[k].tag = "Untagged";
                    gameControl.j--;
                    updateRoleField = true;
                    gameControl.canUseActionCard = false;
                }
                break;
            case "H":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为放弃的效果
                if (someNum == 0)
                {
                    NormalAbOrCqResponse();
                    actionField[k].tag = "abandon";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[0];
                }
                else
                {
                    NormalAbOrCqResponse();
                    //当情敌关系的角色卡打出撒娇时，将角色卡区域还原为初始材质，并去除标签，类似将该名角色卡从游戏中除外
                    roleField[k].GetComponent<Renderer>().material = initialMaterial;
                    roleField[k].tag = "Untagged";
                    gameControl.j--;
                    updateRoleField = true;
                    gameControl.canUseActionCard = false;
                }
                break;
            case "I":
                //当情敌关系的角色卡打出撒娇时，将角色卡区域还原为初始材质，并去除标签，类似将该名角色卡从游戏中除外
                roleField[k].GetComponent<Renderer>().material = initialMaterial;
                roleField[k].tag = "Untagged";
                gameControl.j--;
                updateRoleField = true;
                gameControl.canUseActionCard = false;
                gameControl.sleepyLevelRemaining += 10;
                sleepyVariableQuantity.text = "↑10";
                gameControl.angryLevelRemaining += 5;
                angryVariableQuantity.text = "↑5";
                break;
            default:
                NormalAbOrCqResponse();
                //当情敌关系的角色卡打出撒娇时，将角色卡区域还原为初始材质，并去除标签，类似将该名角色卡从游戏中除外
                roleField[k].GetComponent<Renderer>().material = initialMaterial;
                roleField[k].tag = "Untagged";
                gameControl.j--;
                updateRoleField = true;
                gameControl.canUseActionCard = false;
                break;
        }
    }

    //情敌关系下的各角色卡对吵闹动作卡的响应方法
    void RialInLoveRsNoisy()
    {
        switch (roleField[k].tag)
        {
            case "A":
                gameControl.sleepyLevelRemaining = gameControl.sleepyLevelRemaining - 5 - gameControl.j;
                sleepyVariableQuantity.text = "↓" + (5 + gameControl.j).ToString();
                gameControl.angryLevelRemaining += 5;
                angryVariableQuantity.text = "↑5";
                break;
            case "E":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为撒娇的效果
                if (someNum == 0)
                {
                    NormalAbOrCqResponse();
                    actionField[k].tag = "coquetry";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[1];
                    //当情敌关系的角色卡打出撒娇时，将角色卡区域还原为初始材质，并去除标签，类似将该名角色卡从游戏中除外
                    roleField[k].GetComponent<Renderer>().material = initialMaterial;
                    roleField[k].tag = "Untagged";
                    gameControl.j--;
                    updateRoleField = true;
                    gameControl.canUseActionCard = false;
                }
                else
                {
                    NormalNsOrPlResponse();
                }
                break;
            case "G":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为硬拽的效果
                if (someNum == 0)
                {
                    NormalNsOrPlResponse();
                    actionField[k].tag = "pull";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[3];
                }
                else
                {
                    NormalNsOrPlResponse();
                }
                break;
            case "H":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为放弃的效果
                if (someNum == 0)
                {
                    NormalAbOrCqResponse();
                    actionField[k].tag = "abandon";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[0];
                }
                else
                {
                    NormalNsOrPlResponse();
                }
                break;
            default:
                NormalNsOrPlResponse();
                break;
        }
    }

    //情敌关系下的各角色卡对硬拽动作卡的响应方法
    void RialInLoveRsPull()
    {
        switch (roleField[k].tag)
        {
            case "C":
                gameControl.sleepyLevelRemaining -= 10;
                sleepyVariableQuantity.text = "↓10";
                gameControl.angryLevelRemaining += 10;
                angryVariableQuantity.text = "↑10";
                break;
            case "E":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为撒娇的效果
                if (someNum == 0)
                {
                    NormalAbOrCqResponse();
                    actionField[k].tag = "coquetry";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[1];
                    //当情敌关系的角色卡打出撒娇时，将角色卡区域还原为初始材质，并去除标签，类似将该名角色卡从游戏中除外
                    roleField[k].GetComponent<Renderer>().material = initialMaterial;
                    roleField[k].tag = "Untagged";
                    gameControl.j--;
                    updateRoleField = true;
                    gameControl.canUseActionCard = false;
                }
                else
                {
                    NormalNsOrPlResponse();
                }
                break;
            case "F":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为吵闹的效果
                if (someNum == 0)
                {
                    if (gameNum != 1)
                        NormalNsOrPlResponse();
                    actionField[k].tag = "noisy";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[2];
                }
                else
                {
                    NormalNsOrPlResponse();
                }
                break;
            case "H":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为放弃的效果
                if (someNum == 0)
                {
                    NormalAbOrCqResponse();
                    actionField[k].tag = "abandon";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[0];
                }
                else
                {
                    NormalNsOrPlResponse();
                }
                break;
            default:
                NormalNsOrPlResponse();
                break;
        }
    }

    //基友关系下的各角色卡对放弃动作卡的响应方法
    void FriendRsAbandon()
    {
        switch (roleField[k].tag)
        {
            case "D":
                gameControl.sleepyLevelRemaining -= 10;
                angryVariableQuantity.text = "0";
                sleepyVariableQuantity.text = "↓10";
                break;
            case "E":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为撒娇的效果
                if (someNum == 0)
                {
                    NormalFriendResponse();
                    actionField[k].tag = "coquetry";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[1];
                }
                else
                {
                    NormalFriendResponse();
                }
                break;
            case "F":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为吵闹的效果
                if (someNum == 0)
                {
                    if (gameNum != 1)
                        NormalNsOrPlResponse();
                    actionField[k].tag = "noisy";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[2];
                }
                else
                {
                    NormalFriendResponse();
                }
                break;
            case "G":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为硬拽的效果
                if (someNum == 0)
                {
                    NormalFriendResponse();
                    actionField[k].tag = "pull";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[3];
                }
                else
                {
                    NormalFriendResponse();
                }
                break;
            case "I":
                gameControl.sleepyLevelRemaining -= 16;
                sleepyVariableQuantity.text = "↓16";
                angryVariableQuantity.text = "0";
                break;
            default:
                NormalFriendResponse();
                break;
        }
    }

    //基友关系下的各角色卡对撒娇动作卡的响应方法
    void FriendRsCoquetry()
    {
        switch (roleField[k].tag)
        {
            case "B":
                //处于基友关系时，响应翻倍
                gameControl.sleepyLevelRemaining = gameControl.sleepyLevelRemaining - 10;
                sleepyVariableQuantity.text = "↓10";
                gameControl.angryLevelRemaining -= 10;
                angryVariableQuantity.text = "↓10";
                break;
            case "F":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为吵闹的效果
                if (someNum == 0)
                {
                    if (gameNum != 1)
                        NormalNsOrPlResponse();
                    actionField[k].tag = "noisy";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[2];
                }
                else
                {
                    NormalFriendResponse();
                }
                break;
            case "G":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为硬拽的效果
                if (someNum == 0)
                {
                    NormalFriendResponse();
                    actionField[k].tag = "pull";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[3];
                }
                else
                {
                    NormalFriendResponse();
                }
                break;
            case "H":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为放弃的效果
                if (someNum == 0)
                {
                    NormalFriendResponse();
                    actionField[k].tag = "abandon";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[0];
                }
                else
                {
                    NormalFriendResponse();
                }
                break;
            case "I":
                gameControl.sleepyLevelRemaining -= 16;
                sleepyVariableQuantity.text = "↓16";
                angryVariableQuantity.text = "0";
                break;
            default:
                NormalFriendResponse();
                break;
        }
    }

    //基友关系下的各角色卡对吵闹动作卡的响应方法
    void FriendRsNoisy()
    {
        switch (roleField[k].tag)
        {
            case "A":
                gameControl.sleepyLevelRemaining = gameControl.sleepyLevelRemaining - 5 - gameControl.j * 2;
                sleepyVariableQuantity.text = "↓" + (5 + gameControl.j * 2).ToString();
                gameControl.angryLevelRemaining += 5;
                angryVariableQuantity.text = "↑5";
                break;
            case "E":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为撒娇的效果
                if (someNum == 0)
                {
                    NormalFriendResponse();
                    actionField[k].tag = "coquetry";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[1];
                }
                else
                {
                    NormalFriendResponse();
                }
                break;
            case "G":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为硬拽的效果
                if (someNum == 0)
                {
                    NormalFriendResponse();
                    actionField[k].tag = "pull";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[3];
                }
                else
                {
                    NormalFriendResponse();
                }
                break;
            case "H":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为放弃的效果
                if (someNum == 0)
                {
                    NormalFriendResponse();
                    actionField[k].tag = "abandon";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[0];
                }
                else
                {
                    NormalFriendResponse();
                }
                break;
            case "I":
                gameControl.sleepyLevelRemaining -= 16;
                sleepyVariableQuantity.text = "↓16";
                angryVariableQuantity.text = "0";
                break;
            default:
                NormalFriendResponse();
                break;
        }
    }

    //基友关系下的各角色卡对硬拽动作卡的响应方法
    void FriendRsPull()
    {
        switch (roleField[k].tag)
        {
            case "C":
                gameControl.sleepyLevelRemaining -= 10;
                sleepyVariableQuantity.text = "↓10";
                angryVariableQuantity.text = "0";
                break;
            case "E":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为撒娇的效果
                if (someNum == 0)
                {
                    NormalFriendResponse();
                    actionField[k].tag = "coquetry";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[1];
                }
                else
                {
                    NormalFriendResponse();
                }
                break;
            case "F":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为吵闹的效果
                if (someNum == 0)
                {
                    if (gameNum != 1)
                        NormalNsOrPlResponse();
                    actionField[k].tag = "noisy";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[2];
                }
                else
                {
                    NormalFriendResponse();
                }
                break;
            case "H":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为放弃的效果
                if (someNum == 0)
                {
                    NormalFriendResponse();
                    actionField[k].tag = "abandon";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[0];
                }
                else
                {
                    NormalFriendResponse();
                }
                break;
            case "I":
                gameControl.sleepyLevelRemaining -= 16;
                sleepyVariableQuantity.text = "↓16";
                angryVariableQuantity.text = "0";
                break;
            default:
                NormalFriendResponse();
                break;
        }
    }

    //讨厌关系下的各角色卡对放弃动作卡的响应方法
    void HatedManRsAbandon()
    {
        switch (roleField[k].tag)
        {
            case "D":
                gameControl.sleepyLevelRemaining += 5;
                sleepyVariableQuantity.text = "↑5";
                gameControl.angryLevelRemaining += 1;
                angryVariableQuantity.text = "↑1";
                break;
            case "E":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为撒娇的效果
                if (someNum == 0)
                {
                    NormalHatedManRs();
                    actionField[k].tag = "coquetry";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[1];
                }
                else
                {
                    gameControl.sleepyLevelRemaining += 5;
                    sleepyVariableQuantity.text = "↑5";
                    gameControl.angryLevelRemaining -= 5;
                    angryVariableQuantity.text = "↓5";
                }
                break;
            case "F":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为吵闹的效果
                if (someNum == 0)
                {
                    if (gameNum != 1)
                        NormalNsOrPlResponse();
                    actionField[k].tag = "noisy";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[2];
                }
                else
                {
                    gameControl.sleepyLevelRemaining += 5;
                    sleepyVariableQuantity.text = "↑5";
                    gameControl.angryLevelRemaining -= 5;
                    angryVariableQuantity.text = "↓5";
                }
                break;
            case "G":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为硬拽的效果
                if (someNum == 0)
                {
                    NormalHatedManRs();
                    actionField[k].tag = "pull";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[3];
                }
                else
                {
                    gameControl.sleepyLevelRemaining += 5;
                    sleepyVariableQuantity.text = "↑5";
                    gameControl.angryLevelRemaining -= 5;
                    angryVariableQuantity.text = "↓5";
                }
                break;
            case "I":
                gameControl.sleepyLevelRemaining -= 10;
                sleepyVariableQuantity.text = "↓10";
                gameControl.angryLevelRemaining += 20;
                angryVariableQuantity.text = "↑20";
                break;
            default:
                gameControl.sleepyLevelRemaining += 5;
                sleepyVariableQuantity.text = "↑5";
                gameControl.angryLevelRemaining -= 5;
                angryVariableQuantity.text = "↓5";
                break;
        }
    }

    //讨厌关系下的各角色卡对撒娇动作卡的响应方法
    void HatedManRsCoquetry()
    {
        switch (roleField[k].tag)
        {
            case "B":
                //讨厌关系下，战场每多一名其他角色，唤醒效率减1
                gameControl.sleepyLevelRemaining = gameControl.sleepyLevelRemaining - 5 + gameControl.j;
                sleepyVariableQuantity.text = "↓" + (5 - gameControl.j).ToString();
                gameControl.angryLevelRemaining += 1;
                angryVariableQuantity.text = "↑1";
                break;
            case "F":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为吵闹的效果
                if (someNum == 0)
                {
                    if (gameNum != 1)
                        NormalNsOrPlResponse();
                    actionField[k].tag = "noisy";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[2];
                }
                else
                {
                    NormalHatedManRs();
                }
                break;
            case "G":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为硬拽的效果
                if (someNum == 0)
                {
                    NormalHatedManRs();
                    actionField[k].tag = "pull";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[3];
                }
                else
                {
                    NormalHatedManRs();
                }
                break;
            case "H":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为放弃的效果
                if (someNum == 0)
                {
                    gameControl.sleepyLevelRemaining += 5;
                    sleepyVariableQuantity.text = "↑5";
                    gameControl.angryLevelRemaining -= 5;
                    angryVariableQuantity.text = "↓5";
                    actionField[k].tag = "abandon";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[0];
                }
                else
                {
                    NormalHatedManRs();
                }
                break;
            case "I":
                gameControl.sleepyLevelRemaining -= 10;
                sleepyVariableQuantity.text = "↓10";
                gameControl.angryLevelRemaining += 20;
                angryVariableQuantity.text = "↑20";
                break;
            default:
                NormalHatedManRs();
                break;
        }
    }

    //讨厌关系下的各角色卡对吵闹动作卡的响应方法
    void HatedManRsNoisy()
    {
        switch (roleField[k].tag)
        {
            case "A":
                gameControl.sleepyLevelRemaining = gameControl.sleepyLevelRemaining - 5 + gameControl.j;
                sleepyVariableQuantity.text = "↓" + (5 - gameControl.j).ToString();
                gameControl.angryLevelRemaining += 1;
                angryVariableQuantity.text = "↑1";
                break;
            case "E":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为撒娇的效果
                if (someNum == 0)
                {
                    NormalHatedManRs();
                    actionField[k].tag = "coquetry";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[1];
                }
                else
                {
                    NormalHatedManRs();
                }
                break;
            case "G":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为硬拽的效果
                if (someNum == 0)
                {
                    NormalHatedManRs();
                    actionField[k].tag = "pull";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[3];
                }
                else
                {
                    NormalHatedManRs();
                }
                break;
            case "H":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为放弃的效果
                if (someNum == 0)
                {
                    gameControl.sleepyLevelRemaining += 5;
                    sleepyVariableQuantity.text = "↑5";
                    gameControl.angryLevelRemaining -= 5;
                    angryVariableQuantity.text = "↓5";
                    actionField[k].tag = "abandon";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[0];
                }
                else
                {
                    NormalHatedManRs();
                }
                break;
            case "I":
                gameControl.sleepyLevelRemaining -= 10;
                sleepyVariableQuantity.text = "↓10";
                gameControl.angryLevelRemaining += 20;
                angryVariableQuantity.text = "↑20";
                break;
            default:
                NormalHatedManRs();
                break;
        }
    }

    //讨厌关系下的各角色卡对硬拽动作卡的响应方法
    void HatedManRsPull()
    {
        switch (roleField[k].tag)
        {
            case "C":
                gameControl.sleepyLevelRemaining -= 15;
                sleepyVariableQuantity.text = "↓15";
                gameControl.angryLevelRemaining += 20;
                angryVariableQuantity.text = "↑20";
                break;
            case "E":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为撒娇的效果
                if (someNum == 0)
                {
                    NormalHatedManRs();
                    actionField[k].tag = "coquetry";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[1];
                }
                else
                {
                    NormalHatedManRs();
                }
                break;
            case "F":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为吵闹的效果
                if (someNum == 0)
                {
                    if (gameNum != 1)
                        NormalNsOrPlResponse();
                    actionField[k].tag = "noisy";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[2];
                }
                else
                {
                    NormalHatedManRs();
                }
                break;
            case "H":
                someNum = Random.Range(0, 2);
                //当随机数为0时，触发该角色卡改变动作卡为放弃的效果
                if (someNum == 0)
                {
                    gameControl.sleepyLevelRemaining += 5;
                    sleepyVariableQuantity.text = "↑5";
                    gameControl.angryLevelRemaining -= 5;
                    angryVariableQuantity.text = "↓5";
                    actionField[k].tag = "abandon";
                    actionField[k].GetComponent<Renderer>().material = actionCardMaterials[0];
                }
                else
                {
                    NormalHatedManRs();
                }
                break;
            case "I":
                gameControl.sleepyLevelRemaining -= 10;
                sleepyVariableQuantity.text = "↓10";
                gameControl.angryLevelRemaining += 20;
                angryVariableQuantity.text = "↑20";
                break;
            default:
                NormalHatedManRs();
                break;
        }
    }

    //在游戏过程中某角色卡被移除出战场后，进行位置的更新
    void updateRoleFields()
    {
        for (int i = 0; i < 5; i++)
        {
            //updateRoleField防止在更新从游戏中除外的角色卡区域后，再次进入该循环
            while (roleField[i].tag == "Untagged" && updateRoleField)
            {
                //在将某角色卡移除以后，修改曾记录的K的角色卡位置
                if (gameControl.KinBattlefield && gameControl.positionOfK > i)
                {
                    gameControl.positionOfK--;
                }
                for (int j = i; j < 5; j++)
                {
                    if (j != 4)
                    {
                        roleField[j].GetComponent<Renderer>().material = roleField[j + 1].GetComponent<Renderer>().material;
                        roleField[j].tag = roleField[j + 1].tag;
                    }
                    else {
                        roleField[j].GetComponent<Renderer>().material = initialMaterial;
                        roleField[j].tag = "Untagged";
                    }
                }
                updateRoleField = false;
            }
        }
    }

    //不涉及人物特殊技能的情况下，同学关系者撒娇及放弃动作卡的响应
    private void NormalAbOrCqResponse() {
        //第二关的沉睡者的属性为过度响应，对各种动作卡的响应都会大幅度改变
        if (gameNum == 3)
        {
            gameControl.sleepyLevelRemaining += 6;
            sleepyVariableQuantity.text = "↑6";
            gameControl.angryLevelRemaining -= 10;
            angryVariableQuantity.text = "↓10";
        }
        else {
            gameControl.sleepyLevelRemaining += 3;
            sleepyVariableQuantity.text = "↑3";
            gameControl.angryLevelRemaining -= 5;
            angryVariableQuantity.text = "↓5";
        }
    }

    //不涉及人物特殊技能的情况下，同学关系者对吵闹及硬拽动作卡的响应
    private void NormalNsOrPlResponse()
    {
        //第二关的沉睡者的属性为过度响应，对各种动作卡的响应都会大幅度改变
        if (gameNum == 3)
        {
            gameControl.sleepyLevelRemaining -= 6;
            sleepyVariableQuantity.text = "↓6";
            gameControl.angryLevelRemaining += 10;
            angryVariableQuantity.text = "↑10";
        }
        else {
            gameControl.sleepyLevelRemaining -= 3;
            sleepyVariableQuantity.text = "↓3";
            gameControl.angryLevelRemaining += 1;
            angryVariableQuantity.text = "↑1";
        }
    }

    //不涉及人物技能的情况下，基友关系者打出任何动作卡的响应
    private void NormalFriendResponse() {
        //第二关的沉睡者的属性为过度响应，对各种动作卡的响应都会大幅度改变
        if (gameNum == 3)
        {
            gameControl.sleepyLevelRemaining -= 10;
            sleepyVariableQuantity.text = "↓10";
            angryVariableQuantity.text = "0";
        }
        else
        {
            gameControl.sleepyLevelRemaining -= 8;
            sleepyVariableQuantity.text = "↓8";
            angryVariableQuantity.text = "0";
        }
    }

    //不涉及人物技能的情况下，讨厌关系者打出除放弃以外的动作卡响应
    private void NormalHatedManRs() {
        //第二关的沉睡者的属性为过度响应，对各种动作卡的响应都会大幅度改变
        if (gameNum == 3)
        {
            gameControl.sleepyLevelRemaining -= 10;
            sleepyVariableQuantity.text = "↓10";
            gameControl.angryLevelRemaining += 20;
            angryVariableQuantity.text = "↑20";
        }
        else
        {
            gameControl.sleepyLevelRemaining -= 8;
            sleepyVariableQuantity.text = "↓8";
            gameControl.angryLevelRemaining += 5;
            angryVariableQuantity.text = "↑5";
        }
    }

    //当一回合内使沉睡度减少10%或者增加15%时，记录完成该变化的角色卡标签
    void RecordRFTag()
    {
        if (gameControl.sleepyLevelRemaining - gameControl.startSleepyLevel >= 15)
        {
            curRFTag = roleField[k].tag;
            //true表示增加
            AddOrReduce = true;
        }
        if (gameControl.startSleepyLevel - gameControl.sleepyLevelRemaining >= 10) {
            curRFTag = roleField[k].tag;
            //false表示减少
            AddOrReduce = false;
        }
    }
}
