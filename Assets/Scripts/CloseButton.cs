using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour {
    public GameObject rule;

	// Use this for initialization
	void Start () {
        //手动赋予拥有本脚本的按钮一个监听器
        transform.GetComponent<Button>().onClick.AddListener(OnClick);
	}
    //当按钮被点击时，激活规则说明窗口
    public void OnClick() {
        rule.SetActive(false);
    }
}
