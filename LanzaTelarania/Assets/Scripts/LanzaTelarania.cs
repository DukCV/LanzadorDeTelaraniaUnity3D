using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanzaTelarania : MonoBehaviour
{
    private LineRenderer linea;
    private Vector3 puntoDeAgarre;
    public LayerMask objetoConAgarre;

    public Transform canion;
    public Transform camara;
    public Transform jugador;

    public Animator anim;

    private SpringJoint unionDeResorte;

    public float distanciaMaxima = 100f;

    private bool telaraniaLanzada;
    private bool lanzadorActivo;

    private void Awake()
    {
        linea = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Controles();
        Animaciones();

    }

    private void Controles()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LanzarTelarania();
        }

        if (Input.GetMouseButtonUp(0))
        {
            NoLanzarTelarania();
        }
    }

    private void Animaciones()
    {
        anim.SetBool("Telarania", lanzadorActivo);
    }

    private void LateUpdate()
    {
        DibujarTelarania();
    }

    private void LanzarTelarania()
    {
        RaycastHit hit;
        telaraniaLanzada = Physics.Raycast(camara.position, camara.forward, out hit, distanciaMaxima, objetoConAgarre);
        if (telaraniaLanzada)
        {
            lanzadorActivo = true;
            puntoDeAgarre = hit.point;
            unionDeResorte = jugador.gameObject.AddComponent<SpringJoint>();
            unionDeResorte.autoConfigureConnectedAnchor = false;
            unionDeResorte.connectedAnchor = puntoDeAgarre;

            float distanciaDesdeElPunto = Vector3.Distance(jugador.position, puntoDeAgarre);

            unionDeResorte.maxDistance = distanciaDesdeElPunto * 0.8f;
            unionDeResorte.minDistance = distanciaDesdeElPunto * 0.25f;

            unionDeResorte.spring = 4.5f; //? configuracion de la fuerza del resorte.
            unionDeResorte.damper = 7f; //? configuracion del amortiguador
            unionDeResorte.massScale = 4.5f; //? configuracion de la escala de la masa y el tensor de inercia

            linea.positionCount = 2;
        }
    }

    private void NoLanzarTelarania()
    {
        lanzadorActivo = false;
        linea.positionCount = 0;
        Destroy(unionDeResorte);
    }

    private void DibujarTelarania()
    {

        if (!unionDeResorte)
        {
            return;
        }
        linea.SetPosition(0, canion.position);
        linea.SetPosition(1, puntoDeAgarre);
    }
}
