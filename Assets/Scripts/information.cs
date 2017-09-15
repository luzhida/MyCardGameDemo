using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class information : MonoBehaviour {
    private float halfScreenW;//屏幕的一半宽
    private float halfScreenH;//屏幕的一半高

    // Use this for initialization
    void Start () {
        halfScreenH = Screen.height / 2;
        halfScreenW = Screen.width / 2;
        this.GetComponent<RectTransform>().localPosition = new Vector3(50, 16, 5);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
