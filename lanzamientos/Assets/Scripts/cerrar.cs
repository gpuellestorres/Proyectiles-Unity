using UnityEngine;
using System.Collections;

public class cerrar : MonoBehaviour {

    public float tiempoDobleCierre = 1;
    float t0=-1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape))
        {
            if (t0 == -1)
            {
                t0 = Time.timeSinceLevelLoad;
            }
            else if (Time.timeSinceLevelLoad < t0 + tiempoDobleCierre)
            {
                Application.Quit();
            }
            else {
                t0 = -1;
            }
        }

	}
}
