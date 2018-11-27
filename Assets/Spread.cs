using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spread : MonoBehaviour {

	public GameObject LeftPage;
	public GameObject RightPage;
	public Transform SpinePoint; //Center of rotation
    public string[] Colors;
    public string DefaultColor;
    public GameObject Player;

	private enum SpreadState
	{
		UNOPENED, //both pages on right
		OPEN, //left page on left, right page on right
		CLOSED //both pages on left
	}

	private float rotationStartTime = -1; //This is the number we base our animations off of. When the animation starts we set this to current time.
	private SpreadState state;

	void Start () {
		LeftPage.transform.RotateAround(SpinePoint.position, Vector3.forward, -180); //Our spread is initially open, so we need to close it (set it to UNOPENED)
		state = SpreadState.UNOPENED;
		SetPopupVisibility(false);
	}

	public void SetLeftPageVisibility(bool visibility)
	{
		LeftPage.GetComponent<MeshRenderer>().enabled = visibility;
	}

	public void SetRightPageVisibility(bool visibility)
	{
		RightPage.GetComponent<MeshRenderer>().enabled = visibility;
	}
	
	void Update () {
		Animate();
	}

	void SetPopupVisibility(bool visible)
	{
		PopupGroup[] popUpGroups = GetComponentsInChildren<PopupGroup>();
		foreach (PopupGroup group in popUpGroups)
		{
			foreach (MeshRenderer r in group.gameObject.GetComponentsInChildren<MeshRenderer>())
			{
				r.enabled = visible;
			}
		}
	}

	public void Turn()
	{
		rotationStartTime = Time.time; //Begin animation

		//Make every PopupGroup pop up
		PopupGroup[] popUpGroups = GetComponentsInChildren<PopupGroup>();
		foreach (PopupGroup group in popUpGroups)
		{
			group.Pop();
		}

		if (state == SpreadState.UNOPENED)
		{
			SetPopupVisibility(true);
			SetLeftPageVisibility(true);
			SetRightPageVisibility(true);
		} else if (state == SpreadState.OPEN)
		{
			SetRightPageVisibility(false);
		}
	}

	void Animate()
	{
		//If we set our rotationStartTime then we should be animating
		if (rotationStartTime >= 0)
		{
			float delta = Time.time - rotationStartTime; //Time since animation began
			float clampedDelta = Mathf.Clamp(delta, 0, 1); //Ensure we are in the range [0, 1]

			float desiredAngle;
			float deltaAngle;

			if (state == SpreadState.UNOPENED)
			{
				desiredAngle = clampedDelta.Remap(0, 1, -180, 0); //[0, 1] -> [-180, 0]
				deltaAngle = desiredAngle - LeftPage.transform.rotation.eulerAngles.z;
				LeftPage.transform.RotateAround(SpinePoint.position, Vector3.forward, deltaAngle);
			} else if (state == SpreadState.OPEN)
			{
				desiredAngle = clampedDelta.Remap(0, 1, 0, 180); //[0, 1] -> [0, 180]
				deltaAngle = desiredAngle - RightPage.transform.rotation.eulerAngles.z;
				RightPage.transform.RotateAround(SpinePoint.position, Vector3.forward, deltaAngle);
			}

			//If 1 second elapsed, end our animation
			if (delta >= 1)
			{
				state++;
				rotationStartTime = -1;

				if (state == SpreadState.OPEN)
				{

				} 
                if (state == SpreadState.CLOSED)
				{
					SetPopupVisibility(false);
					SetLeftPageVisibility(false);
					SetRightPageVisibility(false);
				}

                //Enable player rigidbody
                if (Player != null) Player.GetComponent<Rigidbody2D>().simulated = true;

                //Disable non-default groups
                foreach (string color in Colors)
                {
                    if (color == DefaultColor) continue;
                    foreach (GameObject o in GameObject.FindGameObjectsWithTag(color))
                    {
                        if (o.transform.parent.tag != "SwitchToken")
                        {
                            o.SetActive(false);
                        }
                    }
                }
			}
		}
	}
}
