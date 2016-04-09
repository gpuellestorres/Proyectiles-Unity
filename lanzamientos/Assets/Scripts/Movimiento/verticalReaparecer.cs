using UnityEngine;
using System.Collections;

public class verticalReaparecer : MonoBehaviour {

    public float posicionInicio = -5;
    public float posicionReinicio = 5;
    public float velocidad = 3;
    public float tiempoRefresco = 0.01f;
    float t0;

	// Use this for initialization
	void Start () {
        t0 = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
        float tiempoActual = Time.timeSinceLevelLoad;

        if (tiempoActual >= t0 + tiempoRefresco)
        {
            float posicionY = transform.position.y + velocidad * (tiempoActual - t0);

            if (velocidad > 0 && posicionY>=posicionReinicio)
            {
                posicionY = posicionInicio;
            }
            else if (velocidad < 0 && posicionY <= posicionReinicio)
            {
                posicionY = posicionInicio;
            }

            transform.position = new Vector2(transform.position.x, posicionY);
            
            t0 = tiempoActual;
        }
	}
}
