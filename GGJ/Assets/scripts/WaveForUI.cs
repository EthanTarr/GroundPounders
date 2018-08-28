using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveForUI : MonoBehaviour {

	public GameObject Square;
	public float minFloorPlacement;
	public float maxFloorPlacement;

    public float yPos = 5;
    public bool vertical;

	// Use this for initialization
	void Awake () {
		generateUISquares();
	}

	void generateUISquares() {
		float width = Square.transform.localScale.x;
		float spaceToFill = Mathf.Abs(minFloorPlacement) + Mathf.Abs(maxFloorPlacement) / width;
		for (float i = -spaceToFill / 2; i < spaceToFill / 2f; i++) {
			GameObject piece = Instantiate (Square, new Vector3 (width * i, transform.localPosition.y, 0), transform.rotation);
            piece.transform.parent = this.transform;
		}
        if (vertical)
            transform.eulerAngles = new Vector3(0, 0, 90);
	}
}
