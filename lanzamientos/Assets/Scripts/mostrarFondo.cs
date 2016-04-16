using UnityEngine;
using System.Collections;

public class mostrarFondo : MonoBehaviour {

    public Transform[] fondos;

	// Use this for initialization
	void Start () {
        int fondoElegido = (int)( Random.value * fondos.Length);

        if (fondoElegido == fondos.Length) fondoElegido -= 1;

        Instantiate(fondos[fondoElegido]);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
