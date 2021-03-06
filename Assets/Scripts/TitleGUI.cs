﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleGUI : MonoBehaviour {
    public Button GoToRule;//进入rule场景的按钮
    Text story;//介绍故事背景的文本

    public GUISkin customSkin;//自己制作的GUISkin

	// Use this for initialization
	void Start () {
        story = GameObject.Find("Canvas/story").GetComponent<Text>();
        story.text = "2017年5月21日，在当天早上8:30就要进行英语四六级考试的情况下，四川大学江安校区西苑一舍6单元601寝室的十二位" +
            "同学分别出现了不同程度的赖床现象。在这十万火急的危机之下，作为601寝室基友们的602寝室男儿站了出来，他们将发起对601寝" +
            "室的起床大作战。";
        //如果点击了场景中的了解规则按钮，则进入规则解说界面
        GoToRule.onClick.AddListener(delegate () {
            SceneManager.LoadScene("game1");
        });
    }
}
