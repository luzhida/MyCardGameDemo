using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossButton : MonoBehaviour {
    public List<Material> BossMaterials = new List<Material>();
    public List<GameObject> roleFields = new List<GameObject>();//角色卡区域

    // Use this for initialization
    void Start () {
        //手动赋予拥有本脚本的按钮一个监听器
        transform.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void Update()
    {
        //只有代表该Boss的布尔值为真时，按钮才可以使用
        switch (this.tag)
        {
            case "G1":
                if (gameControl.G1)
                    GetComponent<Button>().interactable = true;
                break;
            case "G2":
                if (gameControl.G2)
                    GetComponent<Button>().interactable = true;
                break;
            case "G3":
                if (gameControl.G3)
                    GetComponent<Button>().interactable = true;
                break;
            case "G4":
                if (gameControl.G4)
                    GetComponent<Button>().interactable = true;
                break;
            case "G5":
                if (gameControl.G5)
                    GetComponent<Button>().interactable = true;
                break;
            case "G6":
                if (gameControl.G6)
                    GetComponent<Button>().interactable = true;
                break;
            case "G7":
                if (gameControl.G7)
                    GetComponent<Button>().interactable = true;
                break;
            case "G8":
                if (gameControl.G8)
                    GetComponent<Button>().interactable = true;
                break;
            case "G9":
                if (gameControl.G9)
                    GetComponent<Button>().interactable = true;
                break;
            case "G10":
                if (gameControl.G10)
                    GetComponent<Button>().interactable = true;
                break;
            case "G11":
                if (gameControl.G11)
                    GetComponent<Button>().interactable = true;
                break;
        }
    }
    //当按钮被点击时，将Boss卡置入场地
    public void OnClick()
    {
        switch (this.tag)
        {
            case "G1":
                roleFields[gameControl.j].GetComponent<Renderer>().material = BossMaterials[0];
                roleFields[gameControl.j].tag = this.tag;
                gameControl.j++;
                gameControl.count--;
                gameControl.G1 = false;
                break;
            case "G2":
                roleFields[gameControl.j].GetComponent<Renderer>().material = BossMaterials[1];
                roleFields[gameControl.j].tag = this.tag;
                gameControl.j++;
                gameControl.count--;
                gameControl.G2 = false;
                break;
            case "G3":
                roleFields[gameControl.j].GetComponent<Renderer>().material = BossMaterials[2];
                roleFields[gameControl.j].tag = this.tag;
                gameControl.j++;
                gameControl.count--;
                gameControl.G3 = false;
                break;
            case "G4":
                roleFields[gameControl.j].GetComponent<Renderer>().material = BossMaterials[3];
                roleFields[gameControl.j].tag = this.tag;
                gameControl.j++;
                gameControl.count--;
                gameControl.G4 = false;
                break;
            case "G5":
                roleFields[gameControl.j].GetComponent<Renderer>().material = BossMaterials[4];
                roleFields[gameControl.j].tag = this.tag;
                gameControl.j++;
                gameControl.count--;
                gameControl.G5 = false;
                break;
            case "G6":
                roleFields[gameControl.j].GetComponent<Renderer>().material = BossMaterials[5];
                roleFields[gameControl.j].tag = this.tag;
                gameControl.j++;
                gameControl.count--;
                gameControl.G6 = false;
                break;
            case "G7":
                roleFields[gameControl.j].GetComponent<Renderer>().material = BossMaterials[6];
                roleFields[gameControl.j].tag = this.tag;
                gameControl.j++;
                gameControl.count--;
                gameControl.G7 = false;
                break;
            case "G8":
                roleFields[gameControl.j].GetComponent<Renderer>().material = BossMaterials[7];
                roleFields[gameControl.j].tag = this.tag;
                gameControl.j++;
                gameControl.count--;
                gameControl.G8 = false;
                break;
            case "G9":
                roleFields[gameControl.j].GetComponent<Renderer>().material = BossMaterials[8];
                roleFields[gameControl.j].tag = this.tag;
                gameControl.j++;
                gameControl.count--;
                gameControl.G9 = false;
                break;
            case "G10":
                roleFields[gameControl.j].GetComponent<Renderer>().material = BossMaterials[9];
                roleFields[gameControl.j].tag = this.tag;
                gameControl.j++;
                gameControl.count--;
                gameControl.G10 = false;
                break;
            case "G11":
                roleFields[gameControl.j].GetComponent<Renderer>().material = BossMaterials[10];
                roleFields[gameControl.j].tag = this.tag;
                gameControl.j++;
                gameControl.count--;
                gameControl.G11 = false;
                break;
        }
    }
}
