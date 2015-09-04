using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class DesertTile : MonoBehaviour
{
	public GameObject[] groundTiles;
	public GameObject[] blockingTiles;
	public GameObject cactus;
	public RoomManager.Count cactusCount = new RoomManager.Count(10, 30);
	
	// randomization constants
	public int bloomNum = 100;
	public RoomManager.Count bloomSize = new RoomManager.Count(3, 7);
	
	public const int BiomeNumber = 1;
	
	private Tile[,] tileMap;
	private int width;
	private int height;
	
	void Awake() {
		this.tileMap = this.GetComponent<RoomManager>().tileMap;
		this.height = this.tileMap.GetLength(0);
		this.width = this.tileMap.GetLength(1);
	}
	
	public GameObject getGroundTile() {
		return this.groundTiles[Random.Range(0, this.groundTiles.Length)];
	}
	
	public GameObject getBlockingTile() {
		return this.blockingTiles[Random.Range(0, this.blockingTiles.Length)];
	}
	
	public void RandomBlocking(List<Tile> region) {

		// Add Boulders
		for (int num = 0; num < bloomNum; num++) {
			
			Tile randomTile = region[Random.Range(0, region.Count)];
			BlockingExplosion(randomTile.x,
			                  randomTile.y,
			                  Random.Range (this.bloomSize.minimum, this.bloomSize.maximum + 1));
		}

		// Add cactuses
		int cactusNum = Random.Range(this.cactusCount.minimum, this.cactusCount.maximum);
		for (int num = 0; num < cactusNum; num++) {
			Tile cactusTile = region[Random.Range(0, region.Count)];
			while (cactusTile.item != null) {
				cactusTile = region[Random.Range(0, region.Count)];
			}
			this.GetComponent<RoomManager>().PlaceItem(cactus, cactusTile.x, cactusTile.y);
		}
	}
	
	private void BlockingExplosion(int x, int y, int level) {
		
		if (level < 1 || x < 0 || y < 0 || x >= width || y >= height) {
			return;
		}
		
		Tile tile = this.tileMap[x, y];
		
		if (tile.biome != DesertTile.BiomeNumber || tile.blocking == true) {
			return;
		}
		
		if (tile.item == null) {
			tile.blocking = true;
			this.GetComponent<RoomManager>().PlaceItem(this.getBlockingTile(), x, y);
		}
		
		for (int i = -1; i <= 1; i++) {
			for (int j = -1; j <= 1; j++) {
				if (Random.Range(0, 10) > 3 && (j != 0 || i == 1)) {
					BlockingExplosion(x + i, y + j, level - 1);
				}
			}
		}
	}
}