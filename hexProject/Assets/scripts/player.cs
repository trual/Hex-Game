using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class player : MonoBehaviour {

	// list of tiles the player has selected 
	public List<GameObject> selectedTiles; 


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	///add add tile to tile list
	public void addTile(GameObject To){
		selectedTiles.Add (To);
	}

	///print list of tiles
	public void printTiles(){
		for (int i = 0; i < selectedTiles.Count; i++) {
			Debug.Log (selectedTiles[i].name);
		}
	
	}

	// sums all the energy of the selected tiles and returns it
	public float getSelectedEnergy(){
		float selectedEnergy = 0;
		for (int i = 0; i < selectedTiles.Count; i++) {
			selectedEnergy += selectedTiles [i].GetComponent<tile> ().currentEnergy;
			Debug.Log (selectedTiles[i].name);
		}
		return selectedEnergy;

	}



}



