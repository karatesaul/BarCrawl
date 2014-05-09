using UnityEngine;
using System.Collections;

public class Bottle : MonoBehaviour {

	public int target_x;
	public int target_y;

	public Vector3 targetOffset = new Vector3 (.25f, .1f, 0);

	public bool spinClockwise;

	public float throwSpeed = 0.1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(spinClockwise)
			gameObject.transform.Rotate (0, 0, -10);
		else
			gameObject.transform.Rotate (0, 0, 10);

		Vector3 targetPos = (Vector3)GridManager.getTransformPosition (target_x, target_y) + targetOffset;
		Vector3 ownPos = gameObject.transform.position;

		ownPos.x = approach (targetPos.x, ownPos.x, throwSpeed);
		ownPos.y = approach (targetPos.y, ownPos.y, throwSpeed);

		if (ownPos.x == targetPos.x && ownPos.y == targetPos.y)
		{
			Destroy(this.gameObject);
		}

		gameObject.transform.position = ownPos;
	}

	private float approach(float target, float pos, float speed)
	{
		if (pos < target)
		{
			pos += speed;
			if(pos > target)
				pos = target;
		}

		if (pos > target)
		{
			pos -= speed;
			if(pos < target)
				pos = target;
		}

		return pos;
	}
}
