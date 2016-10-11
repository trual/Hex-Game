using UnityEngine;
using System.Collections;

public enum tileTeam { Green, Neutral, Red };


public class tile : MonoBehaviour {

	// I think i should move these color gradients to a game script????????????????????????
	public Gradient colorG;
	public Gradient colorR;


	// these are the hex cordinate grid 
	public int x;
	public int z;
	//
	public float currentEnergy;
	//Max Energy will depend on tile type
	public float maxEnergy = 1; 
	//tile type

	public tileTeam tileType;
	public  bool isSelected = false; 

	/*
	 * this is generally considered bad because every hex has to have an extra script on it
	 * it is best to minimize the amount of scripts in this stile game because there are so
	 * many hexs
	 * 
	void OnMouseDown(){
		this.currentEnergy = 1;
		print (this.transform.position);
		print (transform.parent.GetComponent<hexGeneration> ().decimalToGrid (this.transform.position));
		print ("\n" + this.currentEnergy);
		print (this.tileType);
		this.setEnergyGradient ();
	}
	*/


	//would be best to move to another olaction
	public void setEnergyGradient(){
		Material myMat = GetComponent<Renderer> ().sharedMaterial;



		if (tileType == tileTeam.Red) {
			myMat.color = colorR.Evaluate (currentEnergy/maxEnergy);
		} else {
			myMat.color = colorG.Evaluate (currentEnergy/maxEnergy);
		}
		GetComponent<Renderer> ().sharedMaterial = myMat;
	}

	public tileTeam getTileType(){
		return this.tileType;
	}



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
