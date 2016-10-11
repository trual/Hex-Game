using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class hexGeneration : MonoBehaviour {

	public static hexGeneration instance;

	// how much energy is updated / tick 
	public float updateEnergy;

	void Awake (){
		instance = this;
	}

	public GameObject tile;
	public int Xsize;
	public int Zsize;
	float height = 2;
	float width = Mathf.Sqrt(3)/2 ;

	private tile[,] allTiles;

	public Material sourceMat;

	void hexGrid(){

		allTiles = new tile [Xsize,Zsize];

		for (int x = 0; x < Xsize; x++) {
			for (int z = 0; z < Zsize; z++) {
				Vector3 temp = gridToDecimal (x,z);

				//creates our game objext hex
				GameObject go = GameObject.Instantiate(tile, temp, Quaternion.identity) as GameObject;
				// I don't remember
				go.GetComponent<Renderer>().sharedMaterial = new Material (sourceMat);
				// names the game object
				go.name = "hex_" + x + "_" + z;
				go.GetComponent<tile> ().x = x;
				go.GetComponent<tile> ().z = z;

				// set the parent of this object to the current transform
				go.transform.SetParent (this.transform);

				if (x == 8 && z == 8) {
					go.GetComponent<tile> ().tileType = tileTeam.Red;
				} else {
					go.GetComponent<tile> ().tileType = tileTeam.Neutral;
				}
				go.GetComponent<tile> ().currentEnergy = Random.value;
				go.GetComponent<tile> ().setEnergyGradient ();

				allTiles [x,z] = go.GetComponent<tile> (); 

			}
		}
	}

	//changes x,y cordinates to their actual location
	public Vector3 gridToDecimal(float x, float z){
		Vector3 temp = new Vector3();
		temp.x = (z%2f)*width+x*width*2;
		temp.y = 0;
		temp.z = z*height*(.75f);
		return temp;
	}


	//transforms actual location to xz cordinates
	public Vector3 decimalToGrid(Vector3 vec){
		Vector3 temp = new Vector3 ();
		temp.y = 0;
		temp.z = Mathf.Round (vec.z / (.75f) / height);
		temp.x = Mathf.Round ((vec.x - (temp.z % 2f) * width) / 2f / width);
		return temp;
	}

	// Use this for initialization
	void Start () {
		hexGrid ();
		InvokeRepeating ("generateEnergy", 2, 2f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	/// Invoke Repeating calls this function
	///  this function generates the energy every t seconds
	/// 
	/// there needs to be a max here  
	/// </summary>
	void generateEnergy(){
		Debug.Log ("generating!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		foreach (Transform child in transform) {
			if (child.GetComponent<tile> ().tileType == tileTeam.Red) {
				// add equalizer function
				//Debug.Log(child.GetComponent<tile> ().name);
				equalizeEnergy (child.GetComponent<tile> ());
				child.GetComponent<tile> ().currentEnergy += updateEnergy;
				//Debug.Log("tiles current engery : " + child.GetComponent<tile> ().currentEnergy);
				child.GetComponent<tile> ().setEnergyGradient ();
			}
		}
		/// update players selected energery,  
		/// if player has selected tiles sum energy and set new selected energy
		/// maybe not,  player should save a reference to these tiles and attack 
		/// tile should call on those references 
	}


	// checsks if child is on the same team
	// add tile to needy neighbor list
	// updates totalt percent need
	//checks if most needy tile
	void howNeedy(tile tempTile, tile currentTile,List<tile> needyNeighbors, List<float> needs, ref float totalPercentNeed,
		ref float mostNeedy){
		if (tempTile.tileType == currentTile.tileType &&
		    tempTile.currentEnergy < currentTile.currentEnergy && tempTile.currentEnergy < tempTile.maxEnergy) {
			// add tile to list of needy neighbros
			needyNeighbors.Add (tempTile);
			//calculate %need
			float tempP = (tempTile.maxEnergy - tempTile.currentEnergy) /
			tempTile.maxEnergy;
			///add to list of needs
			needs.Add (tempP); 
			// add to total percent need
			totalPercentNeed += tempP;
			//Debug.Log ("total Percent need : " + totalPercentNeed);
			// check if most need tile
			if (tempP > mostNeedy) {
				mostNeedy = tempP;
				//mostNeedyTile = needs.Count - 1;
			}
		}
		return;
	}


	// Equalize energy
	void equalizeEnergy (tile currentTile){
		/// should I use get niehgbors or should I write an optimized function I"m going to write an optimized function
		/// create a list neighbors that need energy
		List<tile> needyNeighbors = new List<tile> ();

		//the percent needed by every tile
		float totalPercentNeed = 0;
		//the tile with the highest percent need
		float mostNeedy = 0f;
		//int mostNeedyTile;   /// i don't think i need this
		//list of the need
		List<float> needs = new List<float> ();


		tile tempTile;


		/////////////////////////////////////////////  hey write this bottom part with functions silly head
		if (currentTile.x - 1 >= 0) {
			//tile is neighbor and in need
			tempTile = allTiles [currentTile.x - 1, currentTile.z];
			howNeedy (tempTile, currentTile, needyNeighbors, needs, ref totalPercentNeed, ref mostNeedy);
		} 

		// cehck if tile is odd or even
		if (currentTile.z % 2 > 0) {
			//tile is odd
			if ( currentTile.z + 1 < Zsize) {
				tempTile = allTiles [currentTile.x, currentTile.z + 1];
				howNeedy (tempTile, currentTile, needyNeighbors, needs,ref totalPercentNeed,ref mostNeedy);
				
			} 
			//add top right 
			if ( currentTile.x + 1 < Xsize && currentTile.z + 1 < Zsize) {
				tempTile = allTiles [currentTile.x + 1, currentTile.z + 1];
				howNeedy (tempTile, currentTile, needyNeighbors, needs,ref totalPercentNeed,ref mostNeedy);
			} 

		} else {
			//tile is even 
			//add top left -1, +1
			if (currentTile.x - 1 >= 0 && currentTile.z + 1 < Zsize) {
				tempTile = allTiles [currentTile.x - 1, currentTile.z + 1];
				howNeedy (tempTile, currentTile, needyNeighbors, needs,ref totalPercentNeed,ref mostNeedy);
			} 
			//add top right 
			if ( currentTile.z + 1 < Zsize) {
				tempTile = allTiles [currentTile.x, currentTile.z + 1];
				howNeedy (tempTile, currentTile, needyNeighbors, needs,ref totalPercentNeed,ref mostNeedy);
			} 
		}

		// add  right tile

		if (currentTile.x + 1 < Xsize) {
			tempTile = allTiles [currentTile.x + 1, currentTile.z];
			howNeedy (tempTile, currentTile, needyNeighbors, needs,ref totalPercentNeed,ref mostNeedy);
		} 


		// add bottom right and bottom left
		if (currentTile.z % 2 > 0) {
			//tile is odd
			// bottom right +1 -1 
			if (currentTile.x + 1 < Xsize && currentTile.z - 1 >= 0) {
				tempTile = allTiles [currentTile.x + 1, currentTile.z - 1];
				howNeedy (tempTile, currentTile, needyNeighbors, needs,ref totalPercentNeed,ref mostNeedy);
			} 
			if (currentTile.z >= 0) {
				tempTile = allTiles [currentTile.x, currentTile.z - 1];
				howNeedy (tempTile, currentTile, needyNeighbors, needs,ref totalPercentNeed,ref mostNeedy);
			} 

		} else {
			//tile is even 
			if (currentTile.z - 1 >= 0) {
				tempTile = allTiles [currentTile.x, currentTile.z - 1];
				howNeedy (tempTile, currentTile, needyNeighbors, needs,ref totalPercentNeed,ref mostNeedy);
			}
			if (currentTile.z -1 >= 0 && currentTile.x - 1 >= 0) {
				tempTile = allTiles [currentTile.x - 1, currentTile.z - 1];
				howNeedy (tempTile, currentTile, needyNeighbors, needs,ref totalPercentNeed,ref mostNeedy);
			} 

		}

		//Debug.Log ("total Percent need2 : " + totalPercentNeed);

		/// check if list is empty
		if (needyNeighbors.Count > 0) {
			/// we have a list of needy neighbors
			// add currentiles need to the needy neighbours 
			if (currentTile.currentEnergy < currentTile.maxEnergy) {
				totalPercentNeed += ((currentTile.maxEnergy - currentTile.currentEnergy) /
					currentTile.maxEnergy + mostNeedy);
				needs.Add ((currentTile.maxEnergy - currentTile.currentEnergy) /
				currentTile.maxEnergy + mostNeedy);
			} else {
				totalPercentNeed += mostNeedy;
				needs.Add (mostNeedy);
			}
			needyNeighbors.Add (currentTile);



			for (int i = 0; i < needyNeighbors.Count; i++) {
				//Debug.Log ("needy amount : " + needs [i] + " total needy percent : "+ totalPercentNeed + " current tile energy : " + currentTile.currentEnergy);
				if (i != needyNeighbors.Count - 1) {
					//updates neighbors energy
					needyNeighbors [i].currentEnergy += (needs [i] / totalPercentNeed) * currentTile.currentEnergy;
					needyNeighbors [i].setEnergyGradient ();
				} else {
					//sets current tilees energy
					needyNeighbors [i].currentEnergy = (needs [i] / totalPercentNeed) * currentTile.currentEnergy;
					needyNeighbors [i].setEnergyGradient ();
				}
			}


		}


	}




	public List<tile> getNeighbors(tile currentTile){
		/// add left hex,top left, top right, right, bot right, bot left, 
		/// 
		///need to check if even or odd row

		List<tile> neighbors = new List<tile> ();
		//add left 
		// need to check if out of bounds
		if (currentTile.x - 1 >= 0) {
			neighbors.Add (allTiles [currentTile.x - 1, currentTile.z]);
		} else {
			neighbors.Add (null);
		}

		// cehck if tile is odd or even
		if (currentTile.z % 2 > 0) {
			//tile is odd
			if ( currentTile.z + 1 < Zsize) {
				neighbors.Add (allTiles [currentTile.x, currentTile.z + 1]);
			} else {
				neighbors.Add (null);
			}
			//add top right 
			if ( currentTile.x + 1 < Xsize && currentTile.z + 1 < Zsize) {
				neighbors.Add (allTiles [currentTile.x + 1, currentTile.z + 1]);
			} else {
				neighbors.Add (null);
			}

		} else {
			//tile is even 
			//add top left -1, +1
			if (currentTile.x - 1 >= 0 && currentTile.z + 1 < Zsize) {
				neighbors.Add (allTiles [currentTile.x - 1, currentTile.z + 1]);
			} else {
				neighbors.Add (null);
			}
			//add top right 
			if ( currentTile.z + 1 < Zsize) {
				neighbors.Add (allTiles [currentTile.x, currentTile.z + 1]);
			} else {
				neighbors.Add (null);
			}
		}

		// add  right tile

		if (currentTile.x + 1 < Xsize) {
			neighbors.Add (allTiles [currentTile.x + 1, currentTile.z]);
		} else {
			neighbors.Add (null);
		}


		// add bottom right and bottom left
		if (currentTile.z % 2 > 0) {
			//tile is odd
			// bottom right +1 -1 
			if (currentTile.x + 1 < Xsize && currentTile.z - 1 >= 0) {
				neighbors.Add (allTiles [currentTile.x + 1, currentTile.z - 1]);
			} else {
				neighbors.Add (null);
			}
			if (currentTile.z >= 0) {
				neighbors.Add (allTiles [currentTile.x, currentTile.z - 1]);
			} else {
				neighbors.Add (null);
			}

		} else {
			//tile is even 
			if (currentTile.z - 1 >= 0) {
				neighbors.Add (allTiles [currentTile.x, currentTile.z - 1]);
			} else {
				neighbors.Add (null);
			} 
			if (currentTile.z -1 >= 0 && currentTile.x - 1 >= 0) {
				neighbors.Add (allTiles [currentTile.x - 1, currentTile.z - 1]);
			} else {
				neighbors.Add (null);
			}

		}

		return neighbors;

	}


	public void changeTileColor(List<tile> tiles){
		foreach (var item in tiles) {
			if (item != null) {
				item.currentEnergy = item.maxEnergy;
				item.setEnergyGradient ();
			}
		}
	}


}
