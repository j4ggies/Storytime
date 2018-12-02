using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController : MonoBehaviour {

	public GameObject[] Spreads;

	private int spreadIndex = -1;

	private float startTime = -1;
	private bool turnedPage = false;
	private bool zoomedOut = true;

	private Vector3 initialCameraPosition;
	private Quaternion initialCameraRotation;
	private Matrix4x4 initialProjectionMatrix;

    private Vector3 desiredCameraPosition = new Vector3(1.25f, 4.5f, -23.8f); //TODO: this X and Y coordinate should follow player
	private Quaternion desiredCameraRotation = Quaternion.identity;
	private float orthoSize = 7;
	private Matrix4x4 desiredProjectionMatrix;

	void Start () {
		initialCameraPosition = Camera.main.transform.position;
		initialCameraRotation = Camera.main.transform.rotation;
		initialProjectionMatrix = Camera.main.projectionMatrix;

		float aspect = Camera.main.aspect;
		desiredProjectionMatrix = Matrix4x4.Ortho(-orthoSize * aspect, orthoSize * aspect, -orthoSize, orthoSize, 0.3f, 1000f);

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

	public void BeginAnimation()
	{
		startTime = Time.time;
	}
	

	void Update () {
		if (Input.GetKeyDown(KeyCode.N))
		{
			BeginAnimation();
		}

		Animate();
	}

	void Animate()
	{
        if (startTime < 0 || spreadIndex == Spreads.Length)
		{
			return;
		}

        if (zoomedOut)
		{

			if (!turnedPage)
			{
				turnedPage = true;
				TurnPage();
			}

			float delta = (Time.time - startTime - 1) * 1f; //Time since animation began
			float clampedDelta = Mathf.Clamp(delta, 0, 1); //Ensure we are in the range [0, 1]

			Camera.main.transform.position = Vector3.Lerp(initialCameraPosition, desiredCameraPosition, clampedDelta);
			Camera.main.transform.rotation = Quaternion.Lerp(initialCameraRotation, desiredCameraRotation, clampedDelta);
			Camera.main.projectionMatrix = MatrixLerp(initialProjectionMatrix, desiredProjectionMatrix, clampedDelta);

			if (delta >= 1)
			{
				startTime = -1;
				zoomedOut = false;
			}
		} else
		{
			float delta = (Time.time - startTime) * 1f; //Time since animation began
			float clampedDelta = Mathf.Clamp(delta, 0, 1); //Ensure we are in the range [0, 1]

			Camera.main.transform.position = Vector3.Lerp(desiredCameraPosition, initialCameraPosition, clampedDelta);
			Camera.main.transform.rotation = Quaternion.Lerp(desiredCameraRotation, initialCameraRotation, clampedDelta);
			Camera.main.projectionMatrix = MatrixLerp(desiredProjectionMatrix, initialProjectionMatrix, clampedDelta);

			if (delta >= 1)
			{
				startTime = Time.time;
				zoomedOut = true;
				turnedPage = false;
			}
		}
	}

	Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float time)
	{
		Matrix4x4 ret = new Matrix4x4();
		for (int i = 0; i < 16; i++)
		{
			ret[i] = Mathf.Lerp(from[i], to[i], time);
		}
		return ret;
	}

    public void NewSpread () {
        BeginAnimation();
    }
}
