using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mouseManager : MonoBehaviour {

	public Gradient colorG;
	player ps;

	/*
	public void setEnergyGradient(Material myMat){
		
		myMat.color = colorG.Evaluate (currentEnergy);
		GetComponent<Renderer> ().sharedMaterial = myMat;

	}
	*/

	// Use this for initialization
	void Start () {
		ps = gameObject.GetComponent<player> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		//Debug.Log (Input.mousePosition);

		// this is a ray, orign and a direction
		// origin is the camera direction is the mous
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		//something that stores hit info
		RaycastHit hitInfo;

		//out operates like a pointer
		if (Physics.Raycast (ray, out hitInfo )) {
			GameObject ourHitObject = hitInfo.collider.transform.gameObject;
			if (Input.GetMouseButtonDown(0)) {	
				Debug.Log ("raycast hit:" + ourHitObject.name);
				//List<tile> listOTiles = new List<tile> (); 
				//listOTiles = hexGeneration.instance.getNeighbors (ourHitObject.GetComponent<tile> ());
				//hexGeneration.instance.changeTileColor (listOTiles);
			}


			//this is selecting tiles and adding them to my selected list
			if (Input.GetMouseButtonDown (0)) {
				Debug.Log("tile Energy " + ourHitObject.GetComponent<tile> ().currentEnergy);
				if (ourHitObject.GetComponent<tile> ().getTileType () == tileTeam.Red && !ourHitObject.GetComponent<tile> ().isSelected) {
					selectTile (ourHitObject);
				}
			}

			//this will "aatack a a nuetral tile
			// checks if tile is nuetral and current tile list has more energy than that tile
			// then sets tile to players tile
			if (Input.GetMouseButtonDown (1)) {
				//Debug.Log ("right button clicked");
				if (ourHitObject.GetComponent<tile> ().getTileType () == tileTeam.Neutral) {
					Debug.Log ("right button clicked nuetral");
					attackTile ( ourHitObject);
				}

			//need to set all selected tiles to false          //////////////// tthere must be a better way to select these withoyt using get component
				for (int i = 0; i < ps.selectedTiles.Count; i++) {
					ps.selectedTiles [i].GetComponent<tile>().isSelected = false; 
				}
			/// empty selected tile list
			ps.selectedTiles.Clear();
			}






		}
			
	}

	void selectTile(GameObject ourHitObject){
		
			//add clicked tile to player selected tile list
			ps.addTile(ourHitObject);



			Debug.Log (ourHitObject.GetComponent<tile> ().currentEnergy);
			/////////  what is the difference between renderer and mesh renderer?

			//Material myMat = ourHitObject.GetComponent<Renderer> ().sharedMaterial;

			//myMat.color = Color.red;
			//Debug.Log ("raycast hit:" + ourHitObject.name);

			//ps.printTiles ();

			ourHitObject.GetComponent<tile> ().isSelected = true;

		

	}



	// attacks tile
	//if selected tiles energy is > than target tile woho tile
	// else remove the current tiles energy fom the target tile
	void attackTile( GameObject ourTargetObject){
		//check if selected energy is great than target energy
		float playerEnergy = ps.getSelectedEnergy();
		float targetEnergy = ourTargetObject.GetComponent<tile> ().currentEnergy;
		Material myMat = ourTargetObject.GetComponent<Renderer> ().sharedMaterial;

		if (playerEnergy > targetEnergy) {
			float leftOver = playerEnergy - targetEnergy;
			ourTargetObject.GetComponent<tile> ().currentEnergy = leftOver;
			ourTargetObject.GetComponent<tile> ().tileType = tileTeam.Red;

			myMat.color = ourTargetObject.GetComponent<tile> ().colorR.Evaluate (leftOver);

			/// need to remove enrgy from player energy 
			for (int i = 0; i < ps.selectedTiles.Count; i++) {
				ps.selectedTiles [i].GetComponent<tile>().currentEnergy = 0f; 
				ps.selectedTiles [i].GetComponent<tile> ().setEnergyGradient ();
			}
			Debug.Log ("leftover engery = " + leftOver );
		}

	}
}
