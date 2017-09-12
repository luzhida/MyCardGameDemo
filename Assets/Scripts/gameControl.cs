using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameControl : MonoBehaviour {
    public Transform card01;//用来确定角色卡的手牌位置的场景隐藏游戏物体
    public Transform card02;//用来确定角色卡的手牌位置的场景隐藏游戏物体
    private float xOffset;//用来保持角色卡之间保持的固定间距
    private List<GameObject> cards = new List<GameObject>();//用来存储手牌卡片的List集合
    public GameObject roleCard;//用来实例化的卡片预制件
    public List<GameObject> roleDeck = new List<GameObject>();//存储初始化卡组的List集合
    public List<Material> roleMaterials = new List<Material>();//存储所需的角色卡材质的List集合
    public bool CardGenerating = true;//系统随机发放卡牌的阶段
    public bool PlayCard;//出牌对战阶段
    public Button buttonPrefab;//标志回合结束，并标明下回合开始将一张角色卡加入手牌的结束回合按钮

    public static float startSleepyLevel = 20;//开始时的沉睡度
    public static float startAngryLevel = 50;//开始的愤怒值
    public static float sleepyLevelRemaining;//游戏过程中变化的沉睡度
    public static float angryLevelRemaining;//游戏过程中变化的愤怒值
    private float sleepyPercent;//开始沉睡度的百分比
    private float angryPercent;//开始愤怒值的百分比
    public Texture2D levelBG;//沉睡度和愤怒值百分比条的背景
    public Texture2D levelFG;//沉睡度和愤怒值百分比条的显示
    private float levelFGMaxWidth;//沉睡度和愤怒值满值后百分比条的长度

    int i=0;//roleDeck数组的下标
    public List<GameObject> roleField = new List<GameObject>();//角色卡置入战场后的区域
    public static int j = 0;//roleField数组的下标
    private List<int> result = new List<int>(12);//存储为赋予不同角色卡材质产生的随机数
    int someNum;//为赋予不同角色卡材质产生的随机数
    public static int positionOfK;//由于K角色卡的特俗效果，因此要对他的位置进行记录
    public static bool KinBattlefield = false;//判断角色卡K是否在场上
    private bool LinBattlefield = false;//判断角色卡L是否处于战场中
    public static bool UseSkillOfL = false;//决定是否使用L的技能
    public Button UseOrNot;//是否使用L角色技能的按钮
    Text text;//记录场景中显示的变化的沉睡度
    Text text1;//记录场景中显示的变化的愤怒值
    public static bool canUseActionCard = true;//判断能否使用动作卡
    public static bool canUseRoleCard = true;//判断是否能使用角色卡
    private bool winOrNot = false;//判断是否胜利
    private bool loseOrNot = false;//判断是否输了

    void Start () {
        angryLevelRemaining = startAngryLevel;//将初始沉睡度赋给变化沉睡度
        sleepyLevelRemaining = startSleepyLevel;//将初始愤怒值赋给变化愤怒值
        levelFGMaxWidth = levelFG.width;//让显示的百分比条等于他的最大值
        xOffset = card02.position.x - card01.position.x;
        BuildDeck();//创建初始卡组
        //游戏开始时从卡组中将四张角色卡置入手牌
        for (int i = 0; i < 4; i++)
        {
            AddRoleCard();
        }
        //如果点击了场景中的结束（角色卡)按钮，则表示结束回合，执行敌方回合的方法
        buttonPrefab.onClick.AddListener(delegate () {
            EnemyTurn();
        });
        
        UseOrNot.onClick.AddListener(delegate () {
            //如果点击了使用L角色技能的按钮，则判断L是否在场上
            if (LinBattlefield) {
                UseSkillOfL = true;
            }
        });


    }
	
	// Update is called once per frame
	void Update () {
        //沉睡度及愤怒值所要显示的百分比
        sleepyPercent = sleepyLevelRemaining / 100 * 100;
        angryPercent = angryLevelRemaining / 100 * 100;
        //在场景中显示变化的沉睡度
        text = GameObject.Find("Canvas/sleepyLevel").GetComponent<Text>();
        text.text = sleepyLevelRemaining.ToString() + "%";
        //在场景中显示变化的愤怒值
        text1 = GameObject.Find("Canvas/angryLevel").GetComponent<Text>();
        text1.text = angryLevelRemaining.ToString() + "%";
        //如果正处于角色卡的发牌阶段，则执行发牌方法
        if (CardGenerating)
            AddRoleCard();
        //首先判断是否点击了鼠标左键
        if (Input.GetMouseButtonDown(0))
        { 
            //执行将角色卡置入战场的方法
            enterBattlefield();
        }
        //手牌数改变后，执行该方法使手牌位置相应的改动
        UpdateShow();

        //判断玩家是否赢得了游戏
        if (sleepyLevelRemaining <= 0)
        {
            winOrNot = true;
            

        }

        //判断玩家是否输了比赛
        if (sleepyLevelRemaining >= 100) {
            loseOrNot = true;

        }
        
    }

    private void BuildDeck()
    {
        //角色卡一共十二张，故卡组也为十二张
        for (int i = 0; i < 12; i++)
        {
            //下标越小的roleDeck[]，y轴的位置越高
            int j = 11 - i;
            roleDeck[j] = GameObject.Instantiate(roleCard) as GameObject;
            //每创建一张卡片，将他的Y轴坐标上移
            float y = (float)i / 10 - 10;
            roleDeck[j].transform.position = new Vector3(31, y, -15);
        }
    }

    private void EnemyTurn()
    {
        //将变化沉睡度增加
        sleepyLevelRemaining += 5;
        //表示结束敌人回合，再次进入发牌阶段
        CardGenerating = true;
        actionCardControl.k = 0;
        //每次结束回合后，令可使用的动作卡数等于当前战场上的角色卡数
        actionCardControl.canUseActionCardAmount = j;
        //当角色卡区域被置满角色卡后，则无法再打出角色卡
        if (j > 4)
        {
            canUseRoleCard = false;
        }
        else {
            canUseRoleCard = true;
        }
        canUseActionCard = true;
    }

    public void AddRoleCard() {
        //创建不重复的随机数
        do
        {
            someNum = Random.Range(0, roleMaterials.Count);
        } while (result.Contains(someNum));
        result.Add(someNum);
        //根据手牌数，将置入的手牌移至与card01相应的位置
        Vector3 toPosition = card01.position + new Vector3(xOffset, 0, 0) * cards.Count;
        iTween.MoveTo(roleDeck[i], toPosition, 1f);
        //将随机的角色卡材质赋予从卡组置入的手牌
        roleDeck[i].GetComponent<Renderer>().material = roleMaterials[someNum];
        //根据随机数判断是哪张角色卡，并将该角色卡的名称作为标签赋给置入的手牌
        switch (someNum)
        {
            case 0:
                roleDeck[i].tag = "A.情敌";
                break;
            case 1:
                roleDeck[i].tag = "B.同学";
                break;
            case 2:
                roleDeck[i].tag = "C.同学";
                break;
            case 3:
                roleDeck[i].tag = "D.同学";
                break;
            case 4:
                roleDeck[i].tag = "E.同学";
                break;
            case 5:
                roleDeck[i].tag = "F.讨厌";
                break;
            case 6:
                roleDeck[i].tag = "G.同学";
                break;
            case 7:
                roleDeck[i].tag = "H.情敌";
                break;
            case 8:
                roleDeck[i].tag = "I.基友";
                break;
            case 9:
                roleDeck[i].tag = "J.同学";
                break;
            case 10:
                roleDeck[i].tag = "K.同学";
                break;
            case 11:
                roleDeck[i].tag = "L.同学";
                break;
            default:
                break;
        }
        //改变roleDeck的角度，让他看上去像是手牌
        roleDeck[i].transform.rotation = Quaternion.Euler(90, 0, 0);
        //将置入手牌中的roleDeck添加进存储手牌的List集合
        cards.Add(roleDeck[i]);
        //发完牌后，将CardGennerating改为false，表明发牌阶段结束
        CardGenerating = false;
        i++;
    }

    void UpdateShow()
    {
        //将每张手牌移动到对应位置
        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 toPosition = card01.position + new Vector3(xOffset, 0, 0) * i;

            iTween.MoveTo(cards[i], toPosition, 0.5f);
        }
    }

    private void enterBattlefield() {
        //由于点击鼠标后，预制件会被破坏，因此无法执行脚本中的OnMouseExit函数，故在点击后就直接将showRoleInformation破坏
        Destroy(roleInformation.showRoleInformation);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //定义一条射线，这条射线从摄像机屏幕射向鼠标所在位置
        RaycastHit hit; //声明一个碰撞的点(暂且理解为碰撞的交点)
        if (Physics.Raycast(ray, out hit)) //如果真的发生了碰撞，ray这条射线在hit点与别的物体碰撞了
        {
            //如果碰撞物不带有四种动作卡及Untagged的标签且角色卡处于能使用的情况下，
            //则将碰撞物的材质、标签依次赋予作为角色卡场地的预制件
            if (hit.collider.gameObject.tag != "abandon" && hit.collider.gameObject.tag != "coquetry"
                && hit.collider.gameObject.tag != "noisy" && hit.collider.gameObject.tag != "pull"
                && hit.collider.gameObject.tag != "Untagged" && canUseRoleCard) {
                //当回合中使用了角色卡后就不可以在使用动作卡了
                canUseActionCard = false;
                //当角色卡能使用时才会执行下面的代码
                if (canUseRoleCard) {
                    //如果K角色卡被打出
                    if (hit.collider.gameObject.tag == "K.同学")
                    {
                        KinBattlefield = true;//表示K被置入战场
                        positionOfK = j;//将List集合的下标记录下来，即记录了角色K的位置
                    }
                    //当角色卡L被置入战场之后，将LinBattlefield改为true
                    else if (hit.collider.gameObject.tag == "L.同学")
                    {
                        LinBattlefield = true;
                    }
                    roleField[j].GetComponent<Renderer>().material = hit.collider.gameObject.GetComponent<Renderer>().material;
                    roleField[j].tag = hit.collider.gameObject.tag;
                    j++;
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
                }
                //角色卡一回合只能使用一张
                canUseRoleCard = false;
            }
            
        }
    }

    //实现沉睡度和愤怒值的百分比条
    private void OnGUI()
    {
        float newSleepyBarWidth = (sleepyPercent / 100) * levelFGMaxWidth;
        float newAngryBarWidth = (angryPercent / 100) * levelFGMaxWidth;
        GUI.BeginGroup(new Rect(120, 140, levelBG.width, levelBG.height));
        GUI.DrawTexture(new Rect(0, 0, levelBG.width, levelBG.height), levelBG);
        GUI.BeginGroup(new Rect(5, 6, newSleepyBarWidth, levelFG.height));
        GUI.DrawTexture(new Rect(0, 0, levelFG.width, levelFG.height), levelFG);
        GUI.EndGroup();
        GUI.EndGroup();
        GUI.BeginGroup(new Rect(120, 140 + levelBG.height, levelBG.width, levelBG.height));
        GUI.DrawTexture(new Rect(0, 0, levelBG.width, levelBG.height), levelBG);
        GUI.BeginGroup(new Rect(5, 6, newAngryBarWidth, levelFG.height));
        GUI.DrawTexture(new Rect(0, 0, levelFG.width, levelFG.height), levelFG);
        GUI.EndGroup();
        GUI.EndGroup();
        //赢时输出的信息以及创建再玩一次的按钮
        if (winOrNot) {
            GUI.Box(new Rect(600, 400, 180, 40), "艹，吵醒老子你要死啊！！！");
            //创建再玩一次的按钮
            if (GUI.Button(new Rect(600, 450, 180, 40),
                "有种再玩一次，劳资打死你"))
            {
                Application.LoadLevel("title");
            }
        }
        //输时输出的信息以及创建再玩一次的按钮
        if (loseOrNot)
        {
            GUI.Box(new Rect(600, 400, 180, 40), " 呼~~~~~~~ZZzz...........");
            //创建再玩一次的按钮
            if (GUI.Button(new Rect(560, 450, 250, 40),
                "胜败乃兵家常事，大侠请重新来过"))
            {
                Application.LoadLevel("title");
            }
        }
    }

}