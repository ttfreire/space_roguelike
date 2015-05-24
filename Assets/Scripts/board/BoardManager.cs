using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	// Using Serializable allows us to embed a class with sub properties in the inspector.
	[Serializable]
	public class Count
	{
		public int minimum;             //Minimum value for our Count class.
		public int maximum;             //Maximum value for our Count class.
		
		
		//Assignment constructor.
		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}
	

	public int columns;                                         //Number of columns in our game board.
	public int rows;                                            //Number of rows in our game board.

	public GameObject[] chunkTiles;                                 //Array of floor prefabs.
	public GameObject[] outerWallTiles;                             //Array of outer tile prefabs.
	public List<GameObject> chunkRoomTiles;
	public List<GameObject> chunkSpecialRoomTiles;
	public GameObject lockedRoomTile; 
	public GameObject[] enemies;
	public GameObject[] items;
	public GameObject key;
	public int totalRoomsOnLevel;
	public int numberOfRoomsToUnlockKey;
	public float chunkWidth;
	public float chunkHeight;

	public Transform boardHolder;                                  //A variable to store a reference to the transform of our Board object.
	public List <GameObject> gridPositions = new List <GameObject> ();   //A list of possible locations to place tiles.
	
	GameObject m_player;
	int chunkRowIndex = 0;
	int chunkColumnIndex = 0;
	//Clears our list gridPositions and prepares it to generate a new board.
	void InitialiseList ()
	{
		//Clear our list gridPositions.
		gridPositions.Clear ();

		//Loop through x axis (columns).
		for(int x = 0; x < columns; x++)
		{
			//Within each column, loop through y axis (rows).
			for(int y = 0; y < rows; y++)
			{
				//At each index add a new Vector3 to our list with the x and y coordinates of that position.
				gridPositions.Add (null);
			}
		}
	}
	
	
	//Sets up the outer walls and floor (background) of the game board.
	void BoardSetup ()
	{
		//Instantiate Board and set boardHolder to its transform.
		boardHolder = new GameObject ("Board").transform;
		chunkWidth *= 10f;
		chunkHeight *=10f;

		int spawnedRooms = 0;

		while (spawnedRooms < totalRoomsOnLevel) {
			int row = 0;
			int column = 0;
			GameObject toInstantiate;
			do{
				while((row == 0 && column == 0)){
					row = Random.Range (0,rows);
					column = Random.Range (0,columns);
				}
				int index = Random.Range (0,chunkRoomTiles.Count);
				toInstantiate = chunkRoomTiles[index];
				chunkRoomTiles.RemoveAt(index);
			}
			while(CheckIfChunkGroupSpawnPositionIsvalid(toInstantiate,column,row) == false);


			int gridIndex = GetGridPositionsIndex(row, column);

			GameObject instance =
				Instantiate (toInstantiate, new Vector3 (column*chunkWidth, row*chunkHeight, 5.0f), toInstantiate.transform.rotation) as GameObject;
			instance.transform.SetParent (boardHolder);

			spawnedRooms++;
		}


		//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
		for(int x = -1; x < columns+1; x++)
		{
			//Loop along y axis, starting from -1 to place floor or outerwall tiles.
			for(int y = -1; y < rows+1; y++)
			{
				if(CheckIfGridPositionIsEmpty(x, y)){
				//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
				GameObject toInstantiate;
				//if(gridPositions.Count == 0 )
				//	toInstantiate = lockedRoomTile;
				//else
				//Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
				if(x == -1 || x == columns || y == -1 || y == rows)
					toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
				else{
					do{
						toInstantiate = chunkTiles[Random.Range (1,chunkTiles.Length)];
					}
					while(CheckIfChunkGroupSpawnPositionIsvalid(toInstantiate,x,y) == false);
				}

				//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
				GameObject instance =
					Instantiate (toInstantiate, new Vector3 (x*chunkWidth, y*chunkHeight, 5.0f), toInstantiate.transform.rotation) as GameObject;
				}
				//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
				GameObject chunk = GetChunkAtGridPosition(x,y);
				if(chunk != null){
					if(x != -1 && x != columns && y != -1 && y != rows){
						chunk.AddComponent("chunkController");
						chunk.GetComponent<chunkController>().SetChunkRow(y);
						chunk.GetComponent<chunkController>().SetChunkColumn(x);
						if(gridPositions.Count == 0)
							chunk.GetComponent<chunkController>().hasPlayer = true;
						chunk.SetActive(false);
						gridPositions.Add(chunk);
					}
				}
			}
		}

		/*
		while (spawnedRooms < totalRoomsOnLevel) {
			int index = Random.Range(1,gridPositions.Count);
				int row = gridPositions[index].gameObject.GetComponent<chunkController>().chunkRow;
				int column = gridPositions[index].gameObject.GetComponent<chunkController>().chunkColumn;
				Destroy(gridPositions[index].gameObject);
				GameObject instance =
					Instantiate (randomRoomTile, new Vector3 (column*chunkWidth, row*chunkHeight, 5.0f), randomRoomTile.transform.rotation) as GameObject;
				instance.transform.SetParent (boardHolder);
				instance.AddComponent("chunkController");
				chunkController chunkCont = instance.GetComponent<chunkController>();

				instance.GetComponent<chunkController>().SetChunkRow(row);
				instance.GetComponent<chunkController>().SetChunkColumn(column);
				gridPositions[index] = instance;
				spawnedRooms++;
		}
		**/
		//gridPositions [0].SetActive (true);
	}
	
	
	//RandomPosition returns a random position from our list gridPositions.
	GameObject RandomChunk ()
	{
		//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
		int randomIndex = Random.Range (0, gridPositions.Count);
		
		//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
		GameObject randomPosition = gridPositions[randomIndex];
		
		//Remove the entry at randomIndex from the list so that it can't be re-used.
		//gridPositions.RemoveAt (randomIndex);
		
		//Return the randomly selected Vector3 position.
		return randomPosition;
	}

	//SetupScene initializes our level and calls the previous functions to lay out the game board
	public void SetupScene (int level)
	{
		//Creates the outer walls and floor.
		BoardSetup ();

	}
	
	

	void VisualizeActiveRows(){

		foreach (GameObject chunk in gridPositions) {
			if(chunk.GetComponent<chunkController>().hasPlayer){
				chunkRowIndex = chunk.GetComponent<chunkController>().chunkRow;
				chunkColumnIndex = chunk.GetComponent<chunkController>().chunkColumn;

				break;
			}
		}
		for(int i = 0; i < rows; i++){
			for(int j = 0; j < columns; j++){
				int index = GetGridPositionsIndex(i, j);
				if((i >= chunkRowIndex -1 && i <= chunkRowIndex +1) && (j >= chunkColumnIndex -1 && j <= chunkColumnIndex +1))
					gridPositions[index].SetActive(true);
				else
					gridPositions[index].SetActive(false);
				}
			}
	}

	void Update(){
		//ShowOnlyChunksOnCamera ();
		VisualizeActiveRows ();
	}

	void Awake(){
		m_player = GameObject.Find("Player");
	}
	public int FindPlayerChunkindex(){
		int playerRow = Mathf.FloorToInt(m_player.transform.position.y / chunkHeight);
		int playerColumn = Mathf.FloorToInt(m_player.transform.position.x / chunkWidth);
		return playerColumn + playerRow * columns;
	}

	bool CheckIfGridPositionIsEmpty(int col, int row){
		Vector3 targetPos = new Vector3 (40 * col, 30 * row, 5);
		GameObject[] allMovableThings = GameObject.FindGameObjectsWithTag("Respawn"); 
		foreach(GameObject current in allMovableThings) { 
			if(current.transform.position == targetPos) 
				return false; 
		} return true;
	}

	GameObject GetChunkAtGridPosition(int col, int row){
		Vector3 targetPos = new Vector3 (40 * col, 30 * row, 5);
		GameObject[] allMovableThings = GameObject.FindGameObjectsWithTag("Respawn"); 
		foreach(GameObject current in allMovableThings) { 
			if(current.transform.position == targetPos) 
				return current; 
		} return null;
	}

	bool CheckIfChunkGroupSpawnPositionIsvalid(GameObject group, int col, int row){
		int chunkGroupCol = group.GetComponent<chunkGroupController> ().columns;
		int chunkGroupRow = group.GetComponent<chunkGroupController> ().rows;

		if (col + chunkGroupCol > columns)
			return false;

		if (row + chunkGroupRow > rows)
			return false;

		for (int i = 0; i < chunkGroupCol; i++) {
			for(int j = 0; j < chunkGroupRow; j++){
				if(!CheckIfGridPositionIsEmpty(col+i,row+j))
					return false;
			}
		}

		return true;
	}

	int GetGridPositionsIndex (int row, int column){
		return (row + column * rows);
	}
}
