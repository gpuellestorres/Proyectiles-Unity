using UnityEngine;
using System.Collections;

public class estrella : MonoBehaviour {

    Vector2 posicionInicial;

	// Use this for initialization
	void Start () {

        posicionInicial = transform.position;
	}

    public bool encontrada()
    {
        if (posicionInicial.x == transform.position.x &&
            posicionInicial.y == transform.position.y) return false;
        return true;
    }

    public void volverPosicionInicial()
    {
        transform.position = posicionInicial;
    }
}
