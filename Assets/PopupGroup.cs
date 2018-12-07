using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupGroup : MonoBehaviour {

	public GameObject Page; //Reference to the page that this group is attached to
	public Transform PivotPoint; //PopupGroups will rotate around the Page's X axis through this point

	private float xFactor; //This keeps track of the position relative to the spine. We use this to make groups closer to the spine take longer to pop up.
	private List<GameObject> colliderObjects = new List<GameObject>(); //When this group pops up, we need to instantiate a bunch of 2D physics colliders. We store references to them here.

	//Stuff needed for animation
	private bool down = true; //True if this group is currently down
	private float rotationStartTime = -1; //This is the number we base our animations off of. When the animation starts we set this to current time.
	private float angle = 0; //This keeps track of our current angle. Theoretically we can get rid of this and just use the GameObject's rotation.

	Bounds GetBounds()
	{
		Bounds bounds = new Bounds(transform.position, Vector3.one);
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in renderers)
		{
			bounds.Encapsulate(renderer.bounds);
		}
		return bounds;
	}

	// Use this for initialization
	void Start () {
		var bounds = GetBounds();
		xFactor = 0.5f + (bounds.center.x - Page.transform.position.x) / Page.transform.lossyScale.x;
	}
	
	void SetAngle(float t)
	{
		float desiredAngle = t.Remap(0, 1, 0, 90); //Map [0, 1] -> [0, 90]
		float deltaAngle = desiredAngle - angle;
		angle = desiredAngle;

		//Apply rotation
		Vector3 axis = Page.transform.TransformDirection(Vector3.left);
		transform.RotateAround(PivotPoint.position, axis, deltaAngle);
	}

	void CreateCollider(Transform child)
	{
		//2D colliders are instantiated in the XY plane.
		//We are attaching these colliders to 3D objects which initially exist in the XZ plane and then pop up to the XY plane.
		//Since the collider is a child of this 3D object, the collider rotates OUT of the XY plane and into the XZ plane.
		//This causes our 2D colliders to become 1D lines.
		//We use this "rotation fix" object to apply a sort of 90 degree phase shift
		//This makes it so that our colliders begin in the XZ plane with our object and then rotate into their correct position in the XY plane
		GameObject rotationFix = new GameObject("ColliderObject");
		rotationFix.transform.parent = child;
		rotationFix.transform.localPosition = Vector3.zero;
		rotationFix.transform.localScale = Vector3.one;
		rotationFix.transform.rotation = Quaternion.Euler(90, 0, 0);

        BoxCollider2D collider = rotationFix.AddComponent<BoxCollider2D>();
        collider.size = Vector2.one;
        colliderObjects.Add(rotationFix);

        if (child.tag == "SwitchToken")
        {
            collider.isTrigger = true;
            rotationFix.AddComponent<Flip>();
        }
        else if (child.tag == "Coin")
        {
            collider.isTrigger = true;
            child.gameObject.AddComponent<Coin>();
        }
        else if (child.tag == "EndPlatform")
        {
            collider.isTrigger = true;
        }
        else if (child.tag == "Block")
        {
            collider.gameObject.layer = 9; //Set layer as ground
        }
    }

	public void Pop()
	{
		rotationStartTime = Time.time; //This is what triggers our animation to begin
		Transform[] children = GetComponentsInChildren<Transform>(true);

		if (down)
		{
			//If we were down, then that means we are now popping up.
			//So we create our physics objects
			foreach (Transform child in children)
			{
                //Debug.Log(child.tag);
                if (child.gameObject.GetInstanceID() == gameObject.GetInstanceID() || 
                    child.tag == "Untagged" ||
                    child.parent.tag == "SwitchToken" ||
                    child.tag == "Respawn")
				{
					//Skip 
					continue;
				}

				CreateCollider(child);
			}
		} else
		{
			//We were up, so now we're popping down.
			//Destroy our physics objects so they don't interfere with the next page
			foreach (GameObject o in colliderObjects)
			{
				Destroy(o);
			}
			colliderObjects.Clear();
		}
	}

	void Animate()
	{
		//If we set our rotationStartTime then we should be animating (See: Pop())
		if (rotationStartTime >= 0)
		{
			float delta = Time.time - rotationStartTime; //Time since animation began
			
			float clampedDelta;

			//Get a value in the range [0, 1]
			//Adjusted based on whether we are popping up or down and our "xFactor" (distance from spine)
			if (down)
			{
				clampedDelta = Mathf.Clamp(delta * (1 + xFactor), 0, 1);
			}
			else
			{
				clampedDelta = 1 - Mathf.Clamp(delta * (2 - xFactor), 0, 1);
			}

			SetAngle(clampedDelta); //Set the rotation of this pop up group

			//If 1 second elapsed, end our animation
			if (delta >= 1)
			{
				down = !down;
				rotationStartTime = -1;
			}
		}
	}

	void Update () {
		Animate();
	}
}
