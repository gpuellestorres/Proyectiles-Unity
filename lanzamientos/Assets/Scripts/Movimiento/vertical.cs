using UnityEngine;
using System.Collections;

public class vertical : MonoBehaviour {

    public float velocidad = 3;
    public float tiempoRefresco = 0.01f;
    float t0=0;

    public float limiteSuperior = 4.5f;
    public float limiteInferior = -4.5f;

    public int direccion = 1;

    // Use this for initialization
    void Start () {
        t0 = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
        float tiempoActual = Time.timeSinceLevelLoad;
        if ( tiempoActual >= tiempoRefresco + t0)
        {
            float tiempoTranscurrido = tiempoActual - t0;

            if (tiempoTranscurrido > 0.05f)
            {
                tiempoTranscurrido = 0.05f;
            }

            float posicionY = transform.position.y + velocidad * tiempoTranscurrido * direccion;

            if (direccion == 1)
            {
                if (posicionY >= limiteSuperior)
                {
                    posicionY = limiteSuperior;
                    direccion = -1;
                }
            }
            else
            {
                if (posicionY <= limiteInferior)
                {
                    posicionY = limiteInferior;
                    direccion = 1;
                }
            }

            transform.position = new Vector2(transform.position.x,posicionY);

            t0 = tiempoActual;
        }
	}
}
