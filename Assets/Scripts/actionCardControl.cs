using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    int i = 0;//actionDeck[]的下标
    public static int k = 0;//actionField数组的下标,由于每次回合开始都会从左至右再依次打出动作卡，故设置为静态变量
    int someNum;//为赋予不同动作卡材质产生的随机数
    private List<int> result = new List<int>(48);//存储为赋予不同动作卡材质产生的随机数
    public Button buttonPrefab;//标志回合结束，并标明下回合开始将两张动作卡卡加入手牌的结束回合按钮
    private bool ActionCardGenerating;//表明现在是否为动作卡发牌阶段的布尔值
    public Material makeSense;//由L角色技能所生成的特殊动作卡“讲道理”
    public static int canUseActionCardAmount = 0;//能使用的动作卡数量

    // Use this for initialization
    void Start () {
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
            actionDeck[j].transform.position = new Vector3(-8, y, -15);
        }
    }

    public void AddActionCard()
    {
        do {
            someNum = Random.Range(0, actionMaterials.Count);
        } while (result.Contains(someNum));
        result.Add(someNum);
        Vector3 toPosition = card03.position - new Vector3(xOffset, 0, 0) * cards.Count;
        iTween.MoveTo(actionDeck[i], toPosition, 1f);
        actionDeck[i].GetComponent<Renderer>().material = actionMaterials[someNum];
        int fuck = (someNum+1) % 4;
        //Debug.Log(someNum);
        //Debug.Log(fuck);
        switch (fuck) {
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
        // CardGenerating = false;
        i++;
    }

    private void enterBattlefield()
    {
        //由于点击鼠标后，预制件会被破坏，因此无法执行脚本中的OnMouseExit函数，故在点击后就直接将showActionInformation破坏
        Destroy(actionInformation.showActionInformation);
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
                    //将动作卡改为能被响应
                    actionFieldResponse.actionCardCanResponse = true;
                    //产生随机数，当随机数等于0时，执行将动作卡移至角色卡J区域打出的操作（与角色J的特殊技能有关）
                    someNum = Random.Range(0, 3);
                    if (someNum == 0 && gameControl.KinBattlefield)
                    {
                        //如果L角色的技能被使用，则最近一次打出的动作卡会被转换成为将愤怒值减8的动作卡“讲道理”
                        if (gameControl.UseSkillOfL)
                        {
                            actionField[gameControl.positionOfK].GetComponent<Renderer>().material = makeSense;
                            actionField[gameControl.positionOfK].tag = "makeSense";
                            //L角色的技能只能改变一张动作卡
                            gameControl.UseSkillOfL = false;
                        }
                        else
                        {
                            actionField[gameControl.positionOfK].GetComponent<Renderer>().material = hit.collider.gameObject.GetComponent<Renderer>().material;
                            actionField[gameControl.positionOfK].tag = hit.collider.gameObject.tag;
                        }
                    }
                    else
                    {
                        //如果L角色的技能被使用，则最近一次打出的动作卡会被转换成为将愤怒值减8的动作卡“讲道理”
                        if (gameControl.UseSkillOfL)
                        {
                            actionField[k].GetComponent<Renderer>().material = makeSense;
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
                    k++;
                    canUseActionCardAmount--;
                }
                
            }
            
        }
    }

    private void EnemyTurn()
    {
        //将变化沉睡度增加
        gameControl.sleepyLevelRemaining += 5;
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

    void UpdateShow()
    {
        //将每张手牌移动到对应位置
        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 toPosition = card03.position - new Vector3(xOffset, 0, 0) * i;

            iTween.MoveTo(cards[i], toPosition, 0.5f);
        }
    }

    /*暂时无用的，可能有用的判断语句群
                if (GUI.Button(new Rect(470, 400, 40, 40), "1"))
                {
                    actionField[0].GetComponent<Renderer>().material = hit.collider.gameObject.GetComponent<Renderer>().material;
                    actionField[0].tag = hit.collider.gameObject.tag;
                }
                if (GUI.Button(new Rect(555, 400, 40, 40), "2"))
                {
                    actionField[1].GetComponent<Renderer>().material = hit.collider.gameObject.GetComponent<Renderer>().material;
                    actionField[1].tag = hit.collider.gameObject.tag;
                }
                if (GUI.Button(new Rect(640, 400, 40, 40), "3"))
                {
                    actionField[2].GetComponent<Renderer>().material = hit.collider.gameObject.GetComponent<Renderer>().material;
                    actionField[2].tag = hit.collider.gameObject.tag;
                }
                if (GUI.Button(new Rect(730, 400, 40, 40), "4"))
                {
                    actionField[3].GetComponent<Renderer>().material = hit.collider.gameObject.GetComponent<Renderer>().material;
                    actionField[3].tag = hit.collider.gameObject.tag;
                }
                if (GUI.Button(new Rect(820, 400, 40, 40), "5"))
                {
                    actionField[4].GetComponent<Renderer>().material = hit.collider.gameObject.GetComponent<Renderer>().material;
                    actionField[4].tag = hit.collider.gameObject.tag;
                }*/
}
