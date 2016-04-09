using UnityEngine;
using System.Collections;

public class girar : MonoBehaviour {

    public float tiempoRefresco = 0.01f;
    public float velocidadGiro = 0;

    public bool izquierda = true;

    float t0 = 0;

	// Use this for initialization
	void Start () {
        t0 = Time.timeSinceLevelLoad;

	}
	
	// Update is called once per frame
	void Update () {
        float tiempoActual = Time.timeSinceLevelLoad;
        if (tiempoActual >= t0 + tiempoRefresco)
        {
            if (izquierda)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + velocidadGiro * (tiempoActual - t0));
            }
            else
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - velocidadGiro * (tiempoActual - t0));
            }

            t0 = tiempoActual;
        }
	}
}
