using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class gameMaster : MonoBehaviour {


    public int point;

    public Text pointText;

	
	// Update is called once per frame
	void Update () {

        pointText.text = ("Points: " + point);
		
	}
}
