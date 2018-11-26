using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController : MonoBehaviour {

	public GameObject[] Spreads;

	private int spreadIndex = -1;
	
	void Start () {
		foreach (GameObject obj in Spreads)
		{
			obj.transform.position = new Vector3(+8.5f, 0, 0);

			Spread spread = obj.GetComponent<Spread>();
			spread.SetLeftPageVisibility(false);
			if (!obj.Equals(Spreads[0]))
			{
				spread.SetRightPageVisibility(false);
			}
		}
	}
	
	public void TurnPage()
	{
		if (spreadIndex < Spreads.Length - 1)
		{
			Spread nextSpread = Spreads[spreadIndex + 1].GetComponent<Spread>();
			nextSpread.Turn();
		}

		if (spreadIndex >= 0)
		{
			Spread currentSpread = Spreads[spreadIndex].GetComponent<Spread>();
			currentSpread.Turn();

			if (spreadIndex == Spreads.Length - 1)
			{
				currentSpread.SetRightPageVisibility(true);
			}
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
