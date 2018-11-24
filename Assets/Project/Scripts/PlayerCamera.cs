using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public GameObject player;
    public float offset = 1.3f;

	// Update is called once per frame
	void FixedUpdate () {
        if (player != null)
            transform.position = new Vector3(0, player.transform.position.y + offset, transform.position.z);
	}
}
