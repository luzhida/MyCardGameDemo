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
        this.transform.position = new Vector3(halfScreenW-625, halfScreenH-275, 5);
        Debug.Log(this.transform.position);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
