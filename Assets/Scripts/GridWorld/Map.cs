using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	public MapRow[] map;

	public int width = 10;
	public int height = 4;

	public Tile prefab;
	//prefab

	// Use this for initialization
	void Start () {
		for (int x=0; x < width; x++) {
			for (int y=0; y < height; y++) {
				if(this[x,y] == null)
					this[x,y] = (Tile)Instantiate(prefab, new Vector3(GridManager.tileSizeX * x, GridManager.tileSizeY * y, -0.1f), Quaternion.identity);
				else
					this[x,y].transform.position = new Vector3(GridManager.tileSizeX * x, GridManager.tileSizeY * y, -0.1f);

			}
		}

		GridManager.instance.map = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Tile this[int x, int y]
	{
		get
		{
			return map[y].tiles[x];
		}
		set
		{

			map[y].tiles[x] = value;
		}
	}
}
