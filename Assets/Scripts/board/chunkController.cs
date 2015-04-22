using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class chunkController : MonoBehaviour {

	[HideInInspector] public List <Vector3> gridPositions = new List <Vector3> ();   
	int columns = 7;                                         
	int rows = 7;  
	float chunkWidth;
	float chunkHeight;
	public int chunkRow;
	public int chunkColumn;
	public bool hasPlayer; 

	void Awake(){
		hasPlayer = false;
	}

	public void InitialiseList ()
	{
		//Clear our list gridPositions.
		gridPositions.Clear ();

		chunkWidth = gameObject.transform.localScale.x * 10;
		chunkHeight = gameObject.transform.localScale.z * 10;
		int initialX = Mathf.RoundToInt (columns / 2) - columns +1;
		int initialY = Mathf.RoundToInt (rows / 2) - rows +1;
		
		//Loop through x axis (columns).
		for(int x = initialX; x < columns + initialX; x++)
		{
			//Within each column, loop through y axis (rows).
			for(int y = initialY; y < rows + initialY; y++)
			{
				//At each index add a new Vector3 to our list with the x and y coordinates of that position.
				gridPositions.Add (new Vector3(x*chunkWidth/columns, y*chunkHeight/rows, 0f));
			}
		}
	}

	public Vector3 RandomPosition ()
	{
		//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
		int randomIndex = Random.Range (0, gridPositions.Count);
		
		//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
		Vector3 randomPosition = gridPositions[randomIndex];
		
		//Remove the entry at randomIndex from the list so that it can't be re-used.
		gridPositions.RemoveAt (randomIndex);
		
		//Return the randomly selected Vector3 position.
		return randomPosition;
	}

	public void SetChunkRow(int row){
		chunkRow = row;
	}

	public void SetChunkColumn(int column){
		chunkColumn = column;
	}

	void OnTriggerEnter(Collider other){
		if (gameObject.tag.Equals ("Respawn") && other.gameObject.tag.Equals ("Enemy"))
			other.gameObject.transform.SetParent (gameObject.transform);
		
		if (gameObject.tag.Equals ("Respawn") && other.gameObject.tag.Equals ("Player")) {
			//other.gameObject.transform.SetParent (gameObject.transform);
			hasPlayer = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (gameObject.tag.Equals ("Respawn") && other.gameObject.tag.Equals ("Player"))
			hasPlayer = false;
	}

	public bool isplayeronChunk(){
		return hasPlayer;
	}

}
