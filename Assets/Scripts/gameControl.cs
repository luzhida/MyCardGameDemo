using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameControl : MonoBehaviour
{
    public Transform card01;//用来确定角色卡的手牌位置的场景隐藏游戏物体
    public Transform card02;//用来确定角色卡的手牌位置的场景隐藏游戏物体
    private float xOffset;//用来保持角色卡之间保持的固定间距
    private List<GameObject> cards = new List<GameObject>();//用来存储手牌卡片的List集合
    public GameObject roleCard;//用来实例化的卡片预制件
    public List<GameObject> roleDeck = new List<GameObject>();//存储初始化卡组的List集合
    public List<Material> roleMaterials = new List<Material>();//存储所需的角色卡材质的List集合
    public static bool CardGenerating = true;//系统随机发放卡牌的阶段
    public bool PlayCard;//出牌对战阶段
    public Button buttonPrefab;//标志回合结束，并标明下回合开始将一张角色卡加入手牌的结束回合按钮

    public static float startSleepyLevel;//开始时的沉睡度
    public static float startAngryLevel;//开始的愤怒值
    public static float sleepyLevelRemaining;//游戏过程中变化的沉睡度
    public static float angryLevelRemaining;//游戏过程中变化的愤怒值
    public Image sleepyLevelFG;//沉睡度百分比条
    public Image angryLevelFG;//愤怒值百分比条

    int i;//roleDeck数组的下标
    public List<GameObject> roleField = new List<GameObject>();//角色卡置入战场后的区域
    public static int j;//roleField数组的下标
    private List<int> result = new List<int>(12);//存储为赋予不同角色卡材质产生的随机数
    int someNum;//为赋予不同角色卡材质产生的随机数
    public static int positionOfK;//由于K角色卡的特俗效果，因此要对他的位置进行记录
    public static bool KinBattlefield;//判断角色卡K是否在场上
    private bool LinBattlefield;//判断角色卡L是否处于战场中
    public static bool UseSkillOfL;//决定是否使用L的技能
    public Button UseOrNot;//是否使用L角色技能的按钮
    Text text;//记录场景中显示的变化的沉睡度
    Text text1;//记录场景中显示的变化的愤怒值
    public static bool canUseActionCard;//判断能否使用动作卡
    public static bool canUseRoleCard;//判断是否能使用角色卡
    private bool winOrNot;//判断是否胜利
    public static bool loseOrNot;//判断是否输了

    public int gameNum;//表明这是第几关
    private int recordRoleNum;//为第九关记录角色卡的使用数量
    public static bool classmate, rialInLove, hatedMan, friend;//第十关为了判断对应关系而创建的布尔值
    private int turnNum;//表明当前是角色卡的第几回合
    public GameObject hiddenSkill;//隐藏技能的提示面板
    public Text SkillText;//隐藏技能的提示文本
    public static bool hiddenSkillsOn;//判断隐藏技能是否打开的布尔值
    public static List<string> Boss = new List<string>(12);//记录每关击倒Boss的角色卡标签
    public GameObject BossButton;//存储Boss选择按钮的游戏物体
    public static bool G1, G2, G3, G4, G5, G6, G7, G8, G9, G10, G11;//判断某角色卡是否能激活某Boss卡的布尔值
    public static int count;//Boss卡的计数器
    public Button ButtonTest;//暂时为了跳关而存在的按钮
    bool BossIsActive;//判断Boss按钮是否激活的布尔值

    void Start()
    {
        recordRoleNum = 0;
        i = 0;
        initLevel();
        angryLevelRemaining = startAngryLevel;//将初始沉睡度赋给变化沉睡度
        sleepyLevelRemaining = startSleepyLevel;//将初始愤怒值赋给变化愤怒值
        xOffset = card02.position.x - card01.position.x;
        BuildDeck();//创建初始卡组
        //如果当前不处于第七关，则游戏开始时从卡组中将四张角色卡置入手牌
        if (gameNum != 7)
        {
            for (int i = 0; i < 4; i++)
            {
                AddRoleCard();
            }
        }//第七关开局只有两张角色卡
        else {
            AddRoleCard();
            AddRoleCard();
        }
        
        //如果点击了场景中的结束（角色卡)按钮，则表示结束回合，执行敌方回合的方法
        buttonPrefab.onClick.AddListener(delegate ()
        {
            EnemyTurn();
        });

        UseOrNot.onClick.AddListener(delegate ()
        {
            //如果点击了使用L角色技能的按钮，则判断L是否在场上
            if (LinBattlefield)
            {
                UseSkillOfL = true;
            }
        });
        ButtonTest.onClick.AddListener(delegate ()
         {
             //被点击就跳关
             winOrNot = true;
             winOrLose();
         });
        if (gameNum == 5)
            j = 5;
        else
            j = 0;
        KinBattlefield = false;
        LinBattlefield = false;
        UseSkillOfL = false;
        canUseActionCard = true;
        canUseRoleCard = true;
        winOrNot = false;
        loseOrNot = false;
        if (gameNum == 11)
            hiddenSkill.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //在场景中显示变化的沉睡度
        text = GameObject.Find("Canvas/sleepyLevel").GetComponent<Text>();
        text.text = sleepyLevelRemaining.ToString() + "%";
        //在场景中显示变化的愤怒值
        text1 = GameObject.Find("Canvas/angryLevel").GetComponent<Text>();
        text1.text = angryLevelRemaining.ToString() + "%";
        //如果正处于角色卡的发牌阶段，则判断是否处在第七关
        if (CardGenerating)
        {
            //如果当前关卡是第七关，则产生一个随机数
            if (gameNum == 7)
            {
                someNum = Random.Range(0, 2);
                //如果随机数为1，表示正常发牌，发一张角色卡入手
                if (someNum == 1)
                {
                    AddRoleCard();
                }//否则修改ActionCardGenerating为真，即调用发动作卡方法
                else
                {
                    actionCardControl.ActionCardGenerating = true;
                }
            }//不在第七关依旧正常发牌
            else
            {
                AddRoleCard();
            }//角色卡发牌阶段结束
            CardGenerating = false;
        }

        //如果当前是第十一关且角色L在场上，则随时检查沉睡度及愤怒值是否达到99%
        if (gameNum == 11 && LinBattlefield) {
            if (sleepyLevelRemaining ==99 && angryLevelRemaining == 99) {
                hiddenSkillsOn = true;
            }
        }

        //Boss卡选择按钮被激活时，才能执行
        if (BossIsActive) {
            //可置入战场的Boss已无或角色卡区域已满，则关闭按钮
            if (j >= 5 || count <= 0){
                BossButton.SetActive(false);
                BossIsActive = false;
            }
        }

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
            //如果当前是第九关，则需要打出十二张动作卡后使沉睡度归零才能获胜,否则判为失败
            if (gameNum == 9) {
                if (recordRoleNum == 12) {
                    winOrNot = true;
                    winOrLose();
                    Boss.Add(roleField[actionCardControl.k - 1].tag);
                }
                else
                {
                    loseOrNot = true;
                    winOrLose();
                }
            }
            Boss.Add(roleField[actionCardControl.k - 1].tag);
            winOrNot = true;
            winOrLose();
        }
        //如果当前为第十关，并且角色卡已经打出了四张以上的场合
        if (gameNum == 10 && j >= 4)
        {
            //判断每个角色卡的对应关系
            for (int k = 0; k < gameControl.j; k++)
            {
                roleFieldResponse theScript = roleField[k].GetComponent<roleFieldResponse>();
                switch (theScript.Relation)
                {
                    case 1:
                        hatedMan = true;
                        break;
                    case 2:
                        rialInLove = true;
                        break;
                    case 3:
                        classmate = true;
                        break;
                    case 4:
                        friend = true;
                        break;
                    default:
                        break;
                }
            }
            //当四种对应关系都存在于场上时，游戏胜利
            if (hatedMan && rialInLove && classmate && friend)
            {
                Boss.Add(roleField[actionCardControl.k].tag);
                winOrNot = true;
                winOrLose();
            }
        }
        //判断玩家是否输了比赛
        if (sleepyLevelRemaining >= 100)
        {
            loseOrNot = true;
            winOrLose();
        }
        else if (angryLevelRemaining >= 100) {
            SceneManager.LoadScene("title");
        }
        //显示沉睡度和愤怒值的百分比条
        sleepyLevelFG.fillAmount = sleepyLevelRemaining / 100;
        angryLevelFG.fillAmount = angryLevelRemaining / 100;
    }

    //初始化沉睡度和愤怒值
    void initLevel() {
        //根据当前是第几关来赋值
        switch (gameNum) {
            case 1:
                startSleepyLevel = 35;
                startAngryLevel = 50;
                break;
            case 2:
                startSleepyLevel = 50;
                startAngryLevel = 50;
                break;
            case 3:
                startSleepyLevel = 50;
                startAngryLevel = 40;
                break;
            case 4:
                startSleepyLevel = 40;
                startAngryLevel = 60;
                break;
            case 5:
                startSleepyLevel = 50;
                startAngryLevel = 50;
                break;
            case 6:
                startSleepyLevel = 50;
                startAngryLevel = 80;
                break;
            case 7:
                startSleepyLevel = 20;
                startAngryLevel = 40;
                break;
            case 8:
                startSleepyLevel = 80;
                startAngryLevel = 70;
                break;
            case 9:
                startSleepyLevel = 50;
                startAngryLevel = 60;
                break;
            case 10:
                startSleepyLevel = 50;
                startAngryLevel = 50;
                break;
            case 11:
                startSleepyLevel = 50;
                startAngryLevel = 50;
                break;
            case 12:
                startSleepyLevel = 80;
                startAngryLevel = 80;
                break;
            default:
                break;
        }
    }

    private void BuildDeck()
    {
        //因为关卡五一开始就会将五张角色卡置入场景中，故卡组少五张
        if (gameNum == 5) {
            for (int i = 0; i < 7; i++)
            {
                //下标越小的roleDeck[]，y轴的位置越高
                int j = 6 - i;
                roleDeck[j] = GameObject.Instantiate(roleCard) as GameObject;
                //每创建一张卡片，将他的Y轴坐标上移
                float y = (float)i / 10 - 10;
                roleDeck[j].transform.position = new Vector3(30, y, -10);
            }
        }
        else
        {
            //角色卡一共十二张，故卡组也为十二张
            for (int i = 0; i < 12; i++)
            {
                //下标越小的roleDeck[]，y轴的位置越高
                int j = 11 - i;
                roleDeck[j] = GameObject.Instantiate(roleCard) as GameObject;
                //每创建一张卡片，将他的Y轴坐标上移
                float y = (float)i / 10 - 10;
                roleDeck[j].transform.position = new Vector3(30, y, -10);
            }
        }
    }

    private void EnemyTurn()
    {
        //角色卡抽完判负
        if (i == 12)
        {
            loseOrNot = true;
            winOrLose();
        }
        else if (gameNum == 5 && i == 7) {
            loseOrNot = true;
            winOrLose();
        }
        else
        {
            //根据不同关卡将沉睡度及愤怒值变化
            //当前关卡为十一关，隐藏技能被打开时，沉睡度及愤怒值改为每回合结束后减10
            if (gameNum == 11 && hiddenSkillsOn)
            {
                sleepyLevelRemaining -= 10;
                angryLevelRemaining -= 10;
            }
            else {
                switch (gameNum)
                {
                    case 2:
                        sleepyLevelRemaining += 3;
                        angryLevelRemaining += 5;
                        break;
                    case 1:
                        sleepyLevelRemaining += 10;
                        break;
                    case 5:
                        sleepyLevelRemaining += 8;
                        break;
                    case 7:
                        sleepyLevelRemaining += 2;
                        break;
                    case 8:
                        break;
                    case 10:
                        sleepyLevelRemaining += 3;
                        angryLevelRemaining += 3;
                        break;
                    case 11:
                        sleepyLevelRemaining += 2;
                        break;
                    default:
                        sleepyLevelRemaining += 5;
                        break;
                }
            }
            //虽然第二关Boss属性为过度响应，然而每回合却必须把沉睡度及愤怒值的增减量控制在20%以内，否则下回合将重置愤怒值和沉睡度
            if (gameNum == 3 && (System.Math.Abs(sleepyLevelRemaining - startSleepyLevel) > 20 ||
                System.Math.Abs(angryLevelRemaining - startAngryLevel) > 20))
            {
                sleepyLevelRemaining = startSleepyLevel;
                angryLevelRemaining = startAngryLevel;
            }
            else if (gameNum == 8)
            {
                if (!actionCardControl.abandon8 || !actionCardControl.coquetry8
               || !actionCardControl.noisy8 || !actionCardControl.pull8) {
                    sleepyLevelRemaining = startSleepyLevel;
                    angryLevelRemaining = startAngryLevel;
                }
                actionCardControl.abandon8 = actionCardControl.coquetry8
                            = actionCardControl.noisy8 = actionCardControl.pull8 = false;
            }
            else {
                //每回合结束会记录当前回合的沉睡度及愤怒值为初始的沉睡度及愤怒值
                startSleepyLevel = sleepyLevelRemaining;
                startAngryLevel = angryLevelRemaining;
            }//如果当前关卡是第十关，则每回合结束时将判断对应关系的布尔值归零
            if (gameNum == 10)
            {
                hatedMan = rialInLove = classmate = friend = false;
            }
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
            else
            {
                canUseRoleCard = true;
            }
            canUseActionCard = true;
            //如果当前是十一关，则激活隐藏技能提示面板，每回合输出提示语句，共四句
            if (gameNum == 11) 
                hiddenSkills();
        }
    }

    void AddRoleCard()
    {
        //角色卡数不能大于8，否则就不会发牌
        if (cards.Count < 8) {
            //关卡五一开始就会将五张角色卡置入场地中，因此将相应的数字赋入result数组中
            if (gameNum == 5) {
                result.Add(4);
                result.Add(8);
                result.Add(3);
                result.Add(1);
                result.Add(10);
            }
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
                    roleDeck[i].tag = "A";
                    break;
                case 1:
                    roleDeck[i].tag = "B";
                    break;
                case 2:
                    roleDeck[i].tag = "C";
                    break;
                case 3:
                    roleDeck[i].tag = "D";
                    break;
                case 4:
                    roleDeck[i].tag = "E";
                    break;
                case 5:
                    roleDeck[i].tag = "F";
                    break;
                case 6:
                    roleDeck[i].tag = "G";
                    break;
                case 7:
                    roleDeck[i].tag = "H";
                    break;
                case 8:
                    roleDeck[i].tag = "I";
                    break;
                case 9:
                    roleDeck[i].tag = "J";
                    break;
                case 10:
                    roleDeck[i].tag = "K";
                    break;
                case 11:
                    roleDeck[i].tag = "L";
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

    private void enterBattlefield()
    {
        //由于点击鼠标后，预制件会被破坏，因此无法执行脚本中的OnMouseExit函数，故在点击后就直接将showRoleInformation破坏
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //定义一条射线，这条射线从摄像机屏幕射向鼠标所在位置
        RaycastHit hit; //声明一个碰撞的点(暂且理解为碰撞的交点)
        if (Physics.Raycast(ray, out hit)) //如果真的发生了碰撞，ray这条射线在hit点与别的物体碰撞了
        {
            //如果碰撞物不带有四种动作卡及Untagged的标签且角色卡处于能使用的情况下，
            //则将碰撞物的材质、标签依次赋予作为角色卡场地的预制件
            if (hit.collider.gameObject.tag != "abandon" && hit.collider.gameObject.tag != "coquetry"
                && hit.collider.gameObject.tag != "noisy" && hit.collider.gameObject.tag != "pull"
                && hit.collider.gameObject.tag != "Untagged" && canUseRoleCard)
            {
                //当回合中使用了角色卡后就不可以在使用动作卡了
                canUseActionCard = false;
                //当角色卡能使用时才会执行下面的代码
                if (canUseRoleCard)
                {
                    //如果当前是第九关，则记录角色卡的使用数
                    if (gameNum == 9)
                        recordRoleNum++;
                    //如果K角色卡被打出
                    if (hit.collider.gameObject.tag == "K")
                    {
                        KinBattlefield = true;//表示K被置入战场
                        positionOfK = j;//将List集合的下标记录下来，即记录了角色K的位置
                    }
                    //当角色卡L被置入战场之后，将LinBattlefield改为true
                    else if (hit.collider.gameObject.tag == "L")
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
                //如果当前是第十二关，则执行将BOSS卡置入战场的方法
                if (gameNum == 12)
                {
                    count = 0;
                    BossEnter();
                    //如果该角色卡曾经击倒过一个以上的Boss，则激活Boss按钮
                    if (count > 0)
                    {
                        BossButton.SetActive(true);
                        BossIsActive = true;
                    }
                }
            }

        }
    }

    //boss进入战场
    void BossEnter() {
        //遍历Boss字符串集合，查找曾经由打出角色卡所击败的Boss，将其置入战场
        for (int i = 0; i < Boss.Count; i++) {
            if (Boss[i] == roleField[j - 1].tag) {
                switch (i + 1) {
                    case 1:
                        G1 = true;
                        count++;
                        break;
                    case 2:
                        G2 = true;
                        count++;
                        break;
                    case 3:
                        G3 = true;
                        count++;
                        break;
                    case 4:
                        G4 = true;
                        count++;
                        break;
                    case 5:
                        G5 = true;
                        count++;
                        break;
                    case 6:
                        G6 = true;
                        count++;
                        break;
                    case 7:
                        G7 = true;
                        count++;
                        break;
                    case 8:
                        G8 = true;
                        count++;
                        break;
                    case 9:
                        G9 = true;
                        count++;
                        break;
                    case 10:
                        G10 = true;
                        count++;
                        break;
                    case 11:
                        G11 = true;
                        count++;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    //胜利或者失败后的操作
    void winOrLose() {
        if (winOrNot) {
            switch (gameNum) {
                case 1:
                    SceneManager.LoadScene("game2");
                    break;
                case 2:
                    SceneManager.LoadScene("game3");
                    break;
                case 3:
                    SceneManager.LoadScene("game4");
                    break;
                case 4:
                    SceneManager.LoadScene("game5");
                    break;
                case 5:
                    SceneManager.LoadScene("game6");
                    break;
                case 6:
                    SceneManager.LoadScene("game7");
                    break;
                case 7:
                    SceneManager.LoadScene("game8");
                    break;
                case 8:
                    SceneManager.LoadScene("game9");
                    break;
                case 9:
                    SceneManager.LoadScene("game10");
                    break;
                case 10:
                    SceneManager.LoadScene("game11");
                    break;
                case 11:
                    SceneManager.LoadScene("game12");
                    break;
                default:
                    break;
            }
        }
        if (loseOrNot)
        {
            switch (gameNum)
            {
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
                case 6:
                    SceneManager.LoadScene("game6", LoadSceneMode.Single);
                    break;
                case 7:
                    SceneManager.LoadScene("game7", LoadSceneMode.Single);
                    break;
                case 8:
                    SceneManager.LoadScene("game8", LoadSceneMode.Single);
                    break;
                case 9:
                    SceneManager.LoadScene("game9", LoadSceneMode.Single);
                    break;
                case 10:
                    SceneManager.LoadScene("game10", LoadSceneMode.Single);
                    break;
                case 11:
                    SceneManager.LoadScene("game11", LoadSceneMode.Single);
                    break;
                case 12:
                    SceneManager.LoadScene("game12", LoadSceneMode.Single);
                    break;
                default:
                    break;
            }
        }
    }

    //每次选择角色卡结束回合时，输出隐藏技能的提示句子
    void hiddenSkills()
    {
        turnNum++;
        //隐藏技能的提示每关只有四句
        if (turnNum < 5)
        {
            switch (turnNum) {
                case 1:
                    SkillText.text = "\n你觉得第三关难吗？如果觉得难，那可能这关也不简单哦。";
                    break;
                case 2:
                    SkillText.text = SkillText.text + "\n喂，别再L面前提99哦，他前阵子刚考了99。";
                    break;
                case 3:
                    SkillText.text = SkillText.text + "\n你听说过置之死地而后生吗？";
                    break;
                case 4:
                    SkillText.text = SkillText.text + "\n有时候真的想舒舒服服的，不用思考就能赢得一局。";
                    break;
                default:
                    break;
            }
        }
    }
}
    /*private void OnGUI()
    {
        //赢时输出的信息以及创建再玩一次的按钮
        if (winOrNot)
        {
            GUI.Box(new Rect(600, 400, 180, 40), "艹，吵醒老子你要死啊！！！");
            //创建再玩一次的按钮
            if (GUI.Button(new Rect(600, 450, 180, 40),
                "有种再玩一次，劳资打死你"))
            {
                initData();
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
                initData();
                Application.LoadLevel("title");
            }
        }
    }*/

    /*void initData()
    {
        startSleepyLevel = 35;
        startAngryLevel = 50;
        j = 0;
        KinBattlefield = false;
        LinBattlefield = false;
        UseSkillOfL = false;
        canUseActionCard = true;
        canUseRoleCard = true;
        winOrNot = false;
        loseOrNot = false;
    }*/
