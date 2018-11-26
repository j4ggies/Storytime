using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController : MonoBehaviour {

	public GameObject[] Spreads;

	private int spreadIndex = -1;
	
	void Start () {
		foreach (GameObject spread in Spreads)
		{
			spread.transform.position = new Vector3(+8.5f, 0, 0);
		}
	}
	
	public void TurnPage()
	{
		if (spreadIndex < Spreads.Length - 1)
		{
			Spreads[spreadIndex + 1].GetComponent<Spread>().Turn();
		}

		if (spreadIndex >= 0)
		{
			Spreads[spreadIndex].GetComponent<Spread>().Turn();
		}

		spreadIndex++;
	}
	

	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			TurnPage();
		}
	}
}
