using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class puntero : MonoBehaviour {

    Camera camera;

    public float velocidadJoystick=5;
    float t0;

    Vector2 posicionAnteriorAndroid;

    float arriba, abajo, izquierda, derecha;

    public float deadZoneAndroid = 0.5f;

    Vector2 centro;
    bool empezar = false;

    public Transform flechaControlIzq, flechaControlDer;
    SpriteRenderer flechaIzqRend, flechaDerRend;
    Color original, alternativo;

    pelota Pelota;

    // Use this for initialization
    void Start() {

        Pelota = FindObjectOfType<pelota>();

        camera = FindObjectOfType<Camera>();
        t0 = Time.timeSinceLevelLoad;

        arriba = camera.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
        abajo = camera.ScreenToWorldPoint(new Vector2(0, 0)).y;
        izquierda = camera.ScreenToWorldPoint(new Vector2(0, 0)).x;
        derecha = camera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;

        if (Application.platform == RuntimePlatform.Android)
        {
            flechaControlDer = Instantiate(flechaControlDer);
            flechaControlIzq = Instantiate(flechaControlIzq);

            flechaControlDer.name = "flechaControlDer(Clone)";
            flechaControlIzq.name = "flechaControlIzq(Clone)";

            flechaIzqRend = flechaControlIzq.GetComponent<SpriteRenderer>();
            flechaDerRend = flechaControlDer.GetComponent<SpriteRenderer>();

            original = flechaDerRend.color;
            alternativo = new Color(original.a, original.g, original.b, 190f / 255);

            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float t1 = Time.timeSinceLevelLoad;

        string objetoColision = "";

        if (Input.touchCount > 0)
        {
            foreach (RaycastHit2D hit in Physics2D.RaycastAll(camera.ScreenToWorldPoint(Input.touches[0].position), Vector3.forward))
                if (hit.collider.name.Equals("flechaControlDer(Clone)") || hit.collider.name.Equals("flechaControlIzq(Clone)"))
                {
                    objetoColision = hit.collider.gameObject.name;
                }
        }
        else if(Application.platform==RuntimePlatform.Android)
        {
            flechaIzqRend.color = original;
            flechaDerRend.color = original;
        }
        if (Input.touchCount > 1)
        {
            foreach (RaycastHit2D hit in Physics2D.RaycastAll(camera.ScreenToWorldPoint(Input.touches[1].position), Vector3.forward))
                if (hit.collider.name.Equals("flechaControlDer(Clone)") || hit.collider.name.Equals("flechaControlIzq(Clone)"))
                {
                    objetoColision = hit.collider.gameObject.name;
                }
        }
        if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse X") != 0) && Application.platform != RuntimePlatform.Android)
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector3 position = camera.ScreenToWorldPoint(mousePosition);

            if (Application.platform != RuntimePlatform.Android) transform.position = new Vector3(position.x, position.y, -1);
        }
        else if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && Application.platform != RuntimePlatform.Android)
        {
            transform.position = new Vector3(transform.position.x + Input.GetAxis("Horizontal") * velocidadJoystick * (t1 - t0),
                transform.position.y + Input.GetAxis("Vertical") * velocidadJoystick * (t1 - t0), -1);
        }
        else if (objetoColision.Equals("flechaControlDer(Clone)") || objetoColision.Equals("flechaControlIzq(Clone)") && !Pelota.movimiento)
        {
            float posX = transform.position.x;
            float posY = transform.position.y;
            if (objetoColision.Equals("flechaControlIzq(Clone)"))
            {
                posX -= 0.03f;
                flechaIzqRend.color = alternativo;
            }
            else
            {
                flechaDerRend.color = original;
            }
            if (objetoColision.Equals("flechaControlDer(Clone)"))
            {
                posX += 0.03f;
                flechaDerRend.color = alternativo;
            }
            else
            {
                flechaIzqRend.color = original;
            }

            if (posX <= Pelota.transform.position.x)
            {
                posX = Pelota.transform.position.x;
            }
            else if (posX >= Pelota.transform.position.x + 5)
            {
                posX = Pelota.transform.position.x + 1.99f;
            }

            posY = Pelota.transform.position.x + Pelota.transform.position.y - posX + 5;
            transform.position = new Vector2(posX, posY);
        }
        /*
        if (Input.touchCount > 0 &&
        //(Input.GetTouch(0).position.x < Screen.width / 2 && Input.GetTouch(0).position.y < Screen.height / 2))
        (Input.GetTouch(0).position.x < Screen.width / 2))
        {
            Touch touchActual = Input.GetTouch(0);
            if (touchActual.phase != TouchPhase.Began && empezar)
            {
                Vector2 nuevaPosicion = camera.ScreenToWorldPoint(touchActual.position);
                Vector2 movimiento = new Vector2(nuevaPosicion.x - centro.x, nuevaPosicion.y - centro.y);

                float x = movimiento.x;
                float y = movimiento.y;

                transform.position = new Vector2(transform.position.x + x / 15, transform.position.y + y / 15);
            }
            else if (touchActual.phase == TouchPhase.Began)
            {
                centro = camera.ScreenToWorldPoint(touchActual.position);
                empezar = true;
            }
        }
        else if (Input.touchCount > 1 &&
        (Input.GetTouch(1).position.x < Screen.width / 2 && Input.GetTouch(1).position.y < Screen.height / 2))
        {
            Touch touchActual = Input.GetTouch(1);
            if (touchActual.phase != TouchPhase.Began && empezar)
            {
                Vector2 nuevaPosicion = camera.ScreenToWorldPoint(touchActual.position);
                Vector2 movimiento = new Vector2(nuevaPosicion.x - centro.x, nuevaPosicion.y - centro.y);

                float x = movimiento.x;
                float y = movimiento.y;

                transform.position = new Vector2(transform.position.x + x / 15, transform.position.y + y / 15);
            }
            else if (touchActual.phase == TouchPhase.Began)
            {
                centro = camera.ScreenToWorldPoint(touchActual.position);
                empezar = true;
            }
        }//*/
        else
        {
            empezar = false;
        }

        if (transform.position.x < izquierda + 0.2f || transform.position.x > derecha - 0.2f ||
            transform.position.y < abajo + 0.2f || transform.position.y > arriba - 0.2f)
        {
            float y = transform.position.y;
            if (y < abajo + 0.2f) y = abajo + 0.2f;
            else if (y > arriba - 0.2f) y = arriba - 0.2f;

            float x = transform.position.x;
            if (x < izquierda + 0.2f) x = izquierda + 0.2f;
            else if (x > derecha - 0.2f) x = derecha - 0.2f;

            transform.position = new Vector3(x, y, -1);
        }

        t0 = t1;
    }
    
}
