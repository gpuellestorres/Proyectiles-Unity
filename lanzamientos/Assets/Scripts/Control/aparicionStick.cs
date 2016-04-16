using UnityEngine;
using System.Collections;
using System;

public class aparicionStick : MonoBehaviour {

    public bool ejecutar = true;
    float valorTransparencia = 0.5f;
    bool subir = true;

    public float velocidad = 0.01f;

    SpriteRenderer elRenderer;

	// Use this for initialization
	void Start () {
        elRenderer = GetComponent<SpriteRenderer>();

        if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.WindowsEditor)
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
                valorTransparencia+= velocidad * Time.deltaTime;
                if (valorTransparencia >= 0.6f) subir = !subir;
            }
            else
            {
                valorTransparencia -= velocidad * Time.deltaTime;
                if (valorTransparencia <= 0.01f) subir = !subir;
            }
        }

	}

    internal void finEjecucion()
    {
        ejecutar = false;
        transform.position = new Vector2(-20, -20);
    }
}
