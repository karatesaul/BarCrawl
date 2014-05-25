using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public bool isPassable = true;
	public bool placed = false;

	public int x, y;

	//this is fully uneccessary, I believe.
	/*
	void Start()
	{
		if (placed)
			return;

		//attempt to place it in now

		Map map = GridManager.instance.map;
		if (map == null)
			return;

		x = GridManager.getX(transform.position);
		y = GridManager.getY(transform.position);
		
		if(x >= map.width || y >= map.height || x < 0 || y < 0)
			return;
		
		if (!GridManager.instance.isPassableMapOnly (x, y))
			return;

		//it seems to be a blank spot
		map[x,y] = this;
		placed = true;
	}
	//*/
}
