using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.value < 0.1f)
		{
			GetComponent<Light>().intensity = Random.Range(0.95f, 1f);
		} else
		{
			GetComponent<Light>().intensity = 1;
		}
	}
}
