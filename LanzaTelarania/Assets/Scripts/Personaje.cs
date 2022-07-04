using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    Transform tr;
    Rigidbody rg;
    Animator anim;


    public Transform camaraHombro;
    public Transform camaraPosicion;
    private Transform cam;

    public bool tocandoPiso;
    public bool saltoActivo;

    private float rotacionY = 0f;
    public float velocidad = 6;
    public float FuerzaDeSalto = 6;
    public float velocidadDeRotacion = 25;
    public float anguloMinimo = -70;
    public float anguloMaximo = 90;
    public float velociadaDeCamara = 140;


    // Use this for initialization
    void Start()
    {
        tr = this.transform;
        rg = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Controles();
        ControlCamara();
    }

    private void Controles()
    {
        if (Input.GetButton("Jump"))
        {
            saltoActivo = true;
        }

        RaycastHit hit;
        tocandoPiso = Physics.Raycast(this.tr.position, -tr.up, out hit, .2f);
        if (tocandoPiso)
        {
            if (saltoActivo)
            {
                rg.AddForce(tr.up * FuerzaDeSalto);
                saltoActivo = false;
            }
        }

        Vector3 sp = rg.velocity;

        float deltaX = Input.GetAxis("Horizontal");
        float deltaZ = Input.GetAxis("Vertical");
        float deltaT = Time.deltaTime;

        Vector3 side = velocidad * deltaX * deltaT * tr.right;
        Vector3 forward = velocidad * deltaZ * deltaT * tr.forward;

        Vector3 endSpeed = side + forward;

        endSpeed.y = sp.y;


        rg.velocity = endSpeed;
    }



    private void ControlCamara()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float deltaT = Time.deltaTime;

        rotacionY += mouseY * deltaT * velocidadDeRotacion;
        float xrot = mouseX * deltaT * velocidadDeRotacion;

        tr.Rotate(0, xrot, 0);

        rotacionY = Mathf.Clamp(rotacionY, anguloMinimo, anguloMaximo);

        Quaternion localRotation = Quaternion.Euler(-rotacionY, 0, 0);
        camaraHombro.localRotation = localRotation;

        cam.position = Vector3.Lerp(cam.position, camaraPosicion.position, velociadaDeCamara * deltaT);
        cam.rotation = Quaternion.Lerp(cam.rotation, camaraPosicion.rotation, velociadaDeCamara * deltaT);
    }
}
