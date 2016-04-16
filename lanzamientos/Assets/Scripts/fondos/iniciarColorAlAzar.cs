using UnityEngine;
using System.Collections;

public class iniciarColorAlAzar : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value, 79f / 255f);
    }
}
