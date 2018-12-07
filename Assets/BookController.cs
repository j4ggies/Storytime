using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController : MonoBehaviour {

	public GameObject[] Spreads;
    public GameObject player;
    public GameObject Base;

    private GameObject[] respawns;
    private Camera m_mainCamera;

    private Vector3 initialCameraPosition;
    private Quaternion initialCameraRotation;
    private Matrix4x4 initialProjectionMatrix;
    private Matrix4x4 ortho;
    private Matrix4x4 perspective;
    private float fov = 60f;
    private float near = 0.3f;
    private float far = 1000f;
    private float orthographicSize = 1f;
    private float aspect;
    private bool orthoOn;

    private Vector3 initalCameraPos;
    private bool rotating = false;
    private enum Targets
    {
        PLAYER,
        SOURCE
    }
    private Targets panTarget = Targets.PLAYER;
    private bool followTarget = false;

    private Vector3 offset = new Vector3(0, 0, -1);

    private void Start()
    {
        player.SetActive(false);
        player.transform.position = Base.transform.position;

        respawns = GameObject.FindGameObjectsWithTag("Respawn");

        m_mainCamera = Camera.main;
        initalCameraPos = new Vector3(m_mainCamera.transform.position.x, m_mainCamera.transform.position.y, m_mainCamera.transform.position.z);

        aspect = (float)Screen.width / (float)Screen.height;
        ortho = Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far);
        perspective = Matrix4x4.Perspective(fov, aspect, near, far);
        m_mainCamera.projectionMatrix = perspective;
        orthoOn = false;

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

    private void Update()
    {
        if (PauseMenu.GamePaused) return;

        if (Input.GetKeyDown(KeyCode.N))
        {
            TurnPage();

            orthoOn = !orthoOn;
            if (orthoOn)
            {
                panTarget = Targets.PLAYER;
                RotateCamera(new Vector3(0, 0, 0), 1f);
                BlendToMatrix(ortho, 1f);

                if (spreadIndex < respawns.Length)
                {
                    player.transform.position = respawns[spreadIndex].transform.position;
                    player.gameObject.SetActive(true);
                }

                FollowTarget(player.transform.position + offset, 0.3f);
            }
            else
            {
                panTarget = Targets.SOURCE;
                followTarget = false;
                FollowTarget(initalCameraPos, 0.3f);
                BlendToMatrix(perspective, 1f);
                RotateCamera(new Vector3(30, 0, 0), 1f);
            }
        }
    }

    private void LateUpdate()
    {
        if (followTarget)
        {
            Vector3 finalPosition = player.transform.GetChild(0).position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(m_mainCamera.transform.position, finalPosition, 1);
            m_mainCamera.transform.position = smoothedPosition;

            m_mainCamera.transform.LookAt(player.transform.GetChild(0));
        }
    }

    private IEnumerator RotateObject(GameObject _gameObject, Quaternion newRotation, float duration)
    {
        yield return new WaitForSeconds(1f);

        if (panTarget == Targets.SOURCE)
        {
            yield return new WaitForSeconds(0.3f);
        }

        if (rotating)
        {
            yield break;
        }
        rotating = true;

        Quaternion currentRot = _gameObject.transform.rotation;

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            _gameObject.transform.rotation = Quaternion.Lerp(currentRot, newRotation, counter / duration);
            yield return null;
        }
        rotating = false;
    }

    private Coroutine RotateCamera(Vector3 rotationTo, float duration)
    {
        Quaternion newRotation = Quaternion.Euler(rotationTo);
        return StartCoroutine(RotateObject(m_mainCamera.gameObject, newRotation, duration));
    }

    private static Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float time)
    {
        Matrix4x4 ret = new Matrix4x4();
        for (int i = 0; i < 16; i++)
            ret[i] = Mathf.Lerp(from[i], to[i], time);
        return ret;
    }

    private IEnumerator LerpFromTo(Matrix4x4 src, Matrix4x4 dest, float duration)
    {
        yield return new WaitForSeconds(1f);

        if (panTarget == Targets.SOURCE)
        {
            yield return new WaitForSeconds(0.3f);
        }

        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            m_mainCamera.projectionMatrix = MatrixLerp(src, dest, (Time.time - startTime) / duration);
            yield return 1;
        }
        m_mainCamera.projectionMatrix = dest;
    }

    private Coroutine BlendToMatrix(Matrix4x4 targetMatrix, float duration)
    {
        return StartCoroutine(LerpFromTo(m_mainCamera.projectionMatrix, targetMatrix, duration));
    }

    private IEnumerator _FollowTarget(Vector3 endPos, float duration)
    {
        yield return new WaitForSeconds(1f);

        if (panTarget == Targets.PLAYER)
        {
            yield return new WaitForSeconds(1f);
        }

        float counter = 0;
        Vector3 startingPos = m_mainCamera.transform.position;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            m_mainCamera.gameObject.transform.position = Vector3.Lerp(startingPos, endPos, counter / duration);
            yield return null;
        }
        if (panTarget == Targets.PLAYER)
        {
            followTarget = true;
        }
    }

    private Coroutine FollowTarget(Vector3 endPos, float duration)
    {
        return StartCoroutine(_FollowTarget(endPos, duration));
    }



    private int spreadIndex = -1;
    //private float startTime = -1;
    //private bool turnedPage = false;
    //private bool zoomedOut = true;

    //private Vector3 initialCameraPosition;
    //private Quaternion initialCameraRotation;
    //private Matrix4x4 initialProjectionMatrix;

    //   private Vector3 desiredCameraPosition = new Vector3(1.25f, 4.5f, -23.8f); //TODO: this X and Y coordinate should follow player
    //private Quaternion desiredCameraRotation = Quaternion.identity;
    //private float orthoSize = 7;
    //private Matrix4x4 desiredProjectionMatrix;


    //void Start () {
    //       Player.SetActive(false);
    //       Player.transform.position = Base.transform.position;

    //       respawns = GameObject.FindGameObjectsWithTag("Respawn");

    //       initialCameraPosition = Camera.main.transform.position;
    //	initialCameraRotation = Camera.main.transform.rotation;
    //	initialProjectionMatrix = Camera.main.projectionMatrix;

    //	float aspect = Camera.main.aspect;
    //	desiredProjectionMatrix = Matrix4x4.Ortho(-orthoSize * aspect, orthoSize * aspect, -orthoSize, orthoSize, 0.3f, 1000f);

    //       foreach (GameObject obj in Spreads)
    //	{
    //		obj.transform.position = new Vector3(+8.5f, 0, 0);

    //		Spread spread = obj.GetComponent<Spread>();
    //		spread.SetLeftPageVisibility(false);
    //		if (!obj.Equals(Spreads[0]))
    //		{
    //			spread.SetRightPageVisibility(false);
    //		}
    //	}
    //}

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


    //void Update () {
           

    //	if (Input.GetKeyDown(KeyCode.N))
    //	{
    //		BeginAnimation();
    //	}

    	
    //}

    //void Animate()
    //{
    //       if (startTime < 0 || spreadIndex == Spreads.Length)
    //	{
    //		return;
    //	}

    //       if (zoomedOut)
    //	{

    //		if (!turnedPage)
    //		{
    //			turnedPage = true;
    //			TurnPage();
    //		}

    //		float delta = (Time.time - startTime - 1) * 1f; //Time since animation began
    //		float clampedDelta = Mathf.Clamp(delta, 0, 1); //Ensure we are in the range [0, 1]

    //		Camera.main.transform.position = Vector3.Lerp(initialCameraPosition, desiredCameraPosition, clampedDelta);
    //		Camera.main.transform.rotation = Quaternion.Lerp(initialCameraRotation, desiredCameraRotation, clampedDelta);
    //		Camera.main.projectionMatrix = MatrixLerp(initialProjectionMatrix, desiredProjectionMatrix, clampedDelta);

    //		if (delta >= 1)
    //		{
    //			startTime = -1;
    //			zoomedOut = false;
    //		}

    //           if (spreadIndex < respawns.Length)
    //           {
    //               Player.transform.position = respawns[spreadIndex].transform.position;
    //               Player.gameObject.SetActive(true);
    //           }

    //       } else
    //	{
    //		float delta = (Time.time - startTime) * 1f; //Time since animation began
    //		float clampedDelta = Mathf.Clamp(delta, 0, 1); //Ensure we are in the range [0, 1]

    //		Camera.main.transform.position = Vector3.Lerp(desiredCameraPosition, initialCameraPosition, clampedDelta);
    //		Camera.main.transform.rotation = Quaternion.Lerp(desiredCameraRotation, initialCameraRotation, clampedDelta);
    //		Camera.main.projectionMatrix = MatrixLerp(desiredProjectionMatrix, initialProjectionMatrix, clampedDelta);

    //		if (delta >= 1)
    //		{
    //			startTime = Time.time;
    //			zoomedOut = true;
    //			turnedPage = false;
    //		}

    //           Player.SetActive(false);
    //           Player.transform.position = Base.transform.position;
    //       }
    //}
}
