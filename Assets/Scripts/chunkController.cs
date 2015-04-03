using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class chunkController : MonoBehaviour {

	[HideInInspector] public List <Vector3> gridPositions = new List <Vector3> ();   
	int columns = 5;                                         
	int rows = 5;  
	float chunkWidth;
	float chunkHeight;

	public void InitialiseList ()
	{
		//Clear our list gridPositions.
		gridPositions.Clear ();

		chunkWidth = gameObject.transform.localScale.x * 10;
		chunkHeight = gameObject.transform.localScale.z * 10;
		
		//Loop through x axis (columns).
		for(int x = -2; x < 3; x++)
		{
			//Within each column, loop through y axis (rows).
			for(int y = -2; y < 3; y++)
			{
				//At each index add a new Vector3 to our list with the x and y coordinates of that position.
				gridPositions.Add (new Vector3(x*chunkWidth/5, y*chunkHeight/5, 0f));
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


}
