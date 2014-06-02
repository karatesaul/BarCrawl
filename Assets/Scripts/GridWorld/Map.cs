using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	public MapRow[] map;

	public int width = 10;
	public int height = 4;

	public Tile prefab;
	//prefab
	
	void Start () {

		//loop one: let tiles in arrays know where they are
		for (int x=0; x < width; x++) {
			for (int y=0; y < height; y++) {
				if(this[x,y] != null)
				{
					this[x,y].x = x;
					this[x,y].y = y;
					this[x,y].placed = true;
				}
			}
		}

		//loop two: check for other tiles and place them in arrays based on position if appropriate
		Tile[] tiles = FindObjectsOfType<Tile>();

		//Debug.Log (tiles.Length);

		foreach (Tile tile in tiles)
		{

			if(!tile.placed)
			{
				int x = GridManager.getX(tile.transform.position);
				int y = GridManager.getY(tile.transform.position);

				if(x >= width || y >= height || x < 0 || y < 0)
					continue;
			
				if(this[x,y] == null)
				{
					this[x,y] = tile;
					tile.x = x;
					tile.y = y;
					tile.placed = true;
				}
			}
		}

		//loop three: move placed tiles to proper positions, fill empty slots with empty tiles
		for (int x=0; x < width; x++) {
			for (int y=0; y < height; y++) {
				if(this[x,y] == null)
				{
					this[x,y] = (Tile)Instantiate(prefab, new Vector3(GridManager.tileSizeX * x, GridManager.tileSizeY * y, -0.1f), Quaternion.identity);

					this[x,y].x = x;
					this[x,y].y = y;
					this[x,y].placed = true;
				}
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
