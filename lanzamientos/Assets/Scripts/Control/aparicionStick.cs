using UnityEngine;
using System.Collections;
using System;

public class aparicionStick : MonoBehaviour {

    public bool ejecutar = true;
    float valorTransparencia = 0.5f;
    bool subir = true;

    SpriteRenderer elRenderer;

	// Use this for initialization
	void Start () {
        elRenderer = GetComponent<SpriteRenderer>();

        if (Application.platform != RuntimePlatform.Android)
        {
            finEjecucion();
        }
    }
	
	// Update is called once per frame
	void Update () {        
        if (ejecutar)
        {
            elRenderer.color = new Color(1f, 1f, 1f, valorTransparencia);
            
            if (subir)
            {
                valorTransparencia+=0.005f;
                if (valorTransparencia >= 0.6f) subir = !subir;
            }
            else
            {
                valorTransparencia-=0.005f;
                if (valorTransparencia <= 0.3f) subir = !subir;
            }
        }

	}

    internal void finEjecucion()
    {
        ejecutar = false;
        transform.position = new Vector2(-20, -20);
    }
}
