using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleParralax : MonoBehaviour {

    [SerializeField]
    private GameObject parallaxTarget;
    [SerializeField]
    private float tileHeight = 3.6f;

    private int childCountValue;
    private Transform parallaxTargetTransform;
    
	void Awake () {
		childCountValue = transform.childCount;
        parallaxTargetTransform = parallaxTarget.transform;
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < childCountValue; i++) {
            Transform currentTile = transform.GetChild(i).gameObject.transform;
            if (parallaxTargetTransform.position.y - currentTile.position.y >= tileHeight + 0.6f)
                currentTile.position = new Vector2(0, currentTile.position.y + childCountValue * tileHeight);
        }
	}
}
