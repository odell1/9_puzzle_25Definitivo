using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public class PuzzleVisualizer : MonoBehaviour
{
    public GameObject piezaPrefab;
    public Sprite[] sprites;

    private GameObject[,] piezasVisuales = new GameObject[3, 3];
    private List<Nodo> solucion;
    private int pasoActual = 0;
    // Constante para el número de movimientos de barajado . Ojo! 
    private const int NUM_MOVIMIENTOS_BARAJADO = 10;
    private const int LIMITE_MAXIMO_PROFUNDIDAD_ITERATIVA = 25; //Límite de bucles infinitos
    private Nodo estadoActual;
    private Nodo estadoGuardado;

    public TextMeshProUGUI pasoTexto;

    void Start()
    {
        pasoTexto.text = "¡Ordenado!";
        
        CrearVisualizacion(); // Creamos la visualización inicial


        // Crear instancia de Piezas y ejecutar su lógica
        Piezas piezas = new Piezas();

        int[,] estadoInicial = {{0,1,2},
                        {3,4,5},
                        {6,7,8}};//Creo el array

        estadoActual = new Nodo(estadoInicial);
        ActualizarVisualizacion(estadoActual);


       // StartCoroutine(BarajarUsandoMovimientosValidos(estadoInicial));

    }


    /// <summary>
    /// ///////////////////
    /// </summary>
    /// <param name="estadoInicial"></param>
    /// <returns></returns>
    IEnumerator BarajarUsandoMovimientosValidos(int[,] estadoInicialMeta)
    {
        pasoTexto.text = "Barajando con movimientos legales...";
        yield return new WaitForSeconds(0.5f);

        //Creamos un nodo con el estado inicial (meta)
        Nodo nodoActual = new Nodo(estadoInicialMeta);
        ActualizarVisualizacion(nodoActual);

        System.Random rand = new System.Random(); // Usamos System.Random para consistencia

        for (int k = 0; k < NUM_MOVIMIENTOS_BARAJADO; k++)
        {
            // Clonar el nodo actual (para evitar modificar el nodo padre) por si usamos el nodo
            Nodo nodoTemporal = new Nodo(nodoActual.nodo);



            nodoTemporal.hijos.Clear(); // Limpiamos la lista de hijos antes de expandir
            nodoTemporal.Expandir();    // Esto puebla nodoTemporal.hijos

            if (nodoTemporal.hijos.Count == 0)
            {
                // Si por alguna razón no hay movimientos válidos, salimos.
                break;
            }


            int indiceAleatorio = rand.Next(0, nodoTemporal.hijos.Count);

            //El nodo seleccionado es el nuevo estado actual
            nodoActual = nodoTemporal.hijos[indiceAleatorio];


            estadoActual = nodoActual;
            ActualizarVisualizacion(nodoActual);


            yield return new WaitForSeconds(0.05f);
        }

        // Una vez barajado, iniciamos la búsqueda de la solución.
        pasoTexto.text = "¡Barajado!";
        yield return new WaitForSeconds(0.5f);




    }//BarajarUsandoMovimientosValidos


/// <summary>
/// ///////////////
/// 
/// </summary>
 private void IniciarBusqueda(Nodo nodoRaiz, string tipoBusqueda)
    {
        Piezas piezas = new Piezas();
        solucion = null;
        pasoActual = 0;

        pasoTexto.text = $"Buscando solución ({tipoBusqueda})...";

        //Clonar el nodo raíz para que los algoritmos no modifiquen el Costo/Padre del estadoActual.
        Nodo nodoRaizClonado = new Nodo(nodoRaiz.nodo);
        nodoRaizClonado.Costo = 0;
        nodoRaizClonado.padre = null;
        
        //Llamar al método de búsqueda
        switch (tipoBusqueda)
        {
            case "Anchura":
                solucion = piezas.BusquedaAnchura(nodoRaizClonado);
                break;
            case "Profundidad": 
                solucion = piezas.BusquedaProfundidad(nodoRaizClonado);
                break;
            case "A*":
                solucion = piezas.BuscaAsterisco(nodoRaizClonado);
                break;
            default:
                pasoTexto.text = "Error: Tipo de búsqueda no reconocido.";
                return;
        }

        if (solucion != null && solucion.Count > 0)
        {
            pasoTexto.text = $"Solución encontrada! Pasos ({tipoBusqueda}): {solucion.Count - 1}";
            solucion.Reverse(); 
            StartCoroutine(MostrarSolucionPasoAPaso());
        }
        else
        {
            pasoTexto.text = $"¡ERROR! No se pudo encontrar solución ({tipoBusqueda}).";
        }
    }//IniciarBusqueda



    ////////////////////////////////////
    void CrearVisualizacion()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject pieza = Instantiate(piezaPrefab, transform);
                piezasVisuales[i, j] = pieza;
            }
        }
    }
    /// <summary>
    /// /////////////////////
    /// </summary>
    /// <param name="nodo"></param>
    void ActualizarVisualizacion(Nodo nodo)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int valor = nodo.nodo[i, j];
                piezasVisuales[i, j].GetComponent<Image>().sprite = sprites[valor];
            }
        }
    }

    IEnumerator MostrarSolucionPasoAPaso()
    {
        while (pasoActual < solucion.Count)
        {
            ActualizarVisualizacion(solucion[pasoActual]);
            pasoActual++;
            yield return new WaitForSeconds(0.2f); // Espera 2 segundos entre pasos
        }
    }

    IEnumerator SimularBarajadoYBuscar(int[,] estadoInicial)
    {
        pasoTexto.text = "Barajando con intercambios...";
        yield return new WaitForSeconds(0.5f);

        // int[,] matrizActual = ClonarMatriz(estadoInicial);

        System.Random rand = new System.Random();
        //Número de intercambios
        for (int k = 0; k < 3; k++)
        {
            int pos1D_A = rand.Next(0, 9);
            int pos1D_B = rand.Next(0, 9);

            // Comprobamos que no sean iguales
            while (pos1D_A == pos1D_B)
            {
                pos1D_B = rand.Next(0, 9);
            }

            int filaA = pos1D_A / 3;//Calculo fila y columna Destino
            int colA = pos1D_A % 3;
            int filaB = pos1D_B / 3;//Calculo fila y columna Origen
            int colB = pos1D_B % 3;



            if (estadoInicial[filaA, colA] != 0 && estadoInicial[filaB, colB] != 0)
            {
                //Realizar el intercambio en la matriz de simulación
                int temp = estadoInicial[filaA, colA];
                estadoInicial[filaA, colA] = estadoInicial[filaB, colB];
                estadoInicial[filaB, colB] = temp;

                ActualizarVisualizacion(new Nodo(estadoInicial));
                yield return new WaitForSeconds(0.05f);



            }
            else
            {
                k--; // No contamos este intercambio si alguno es el espacio vacío. Práctica no muy elegante, pero funcional.
            }

            // Una vez finalizada la simulación y visualización del barajado:
            pasoTexto.text = "¡Barajado Finalizado!";
            yield return new WaitForSeconds(0.5f);

            pasoTexto.text = "Iniciando búsqueda de solución...";
            Nodo nodoBarajado = new Nodo(estadoInicial);
            IniciarBusqueda(nodoBarajado);
        }
    }//SimularBarajadoYBuscar

    private void IniciarBusqueda(Nodo nodoBarajado)
    {

        Piezas piezas = new Piezas();
        pasoTexto.text = "Buscando solución...";

        // Llamada al método de solución
        //solucion = piezas.BusquedaAnchura(nodoBarajado); 
        //solucion = piezas.BusquedaProfundidad(nodoBarajado);
        //solucion =piezas.BusquedaProfundidadAcotada(nodoBarajado, 20);
        solucion = piezas.BuscaAsterisco(nodoBarajado);

        if (solucion != null && solucion.Count > 0)
        {
            pasoTexto.text = "Solución encontrada! Pasos: " + solucion.Count;
            solucion.Reverse();
            //Inicia la corutina de visualización de la solución
            StartCoroutine(MostrarSolucionPasoAPaso());
        }
        else
        {
            pasoTexto.text = "¡ERROR! No se pudo encontrar solución. Estado insoluble.";
        }
    }



/////////////////////////
/// Botones de la interfaz
/// 
/// 
public void OnClick_Barajar()
    {
        Debug.Log("Botón Barajar pulsado.");
        if (estadoActual == null)
        {
            pasoTexto.text = "Error: Inicializa el estado primero.";
            return;
        }
       
        StopAllCoroutines(); //Detiene cualquier visualización de solución en curso
        pasoActual = 0;
        StartCoroutine(BarajarUsandoMovimientosValidos(estadoActual.nodo));
    }//OnClick_Barajar

/// <summary>
/// ///
/// </summary>
    public void OnClick_GuardarPosicion()
    {
        estadoGuardado = new Nodo(estadoActual.nodo);
        pasoTexto.text = "Posición guardada.";
    }//OnClick_GuardarPosicion

/// <summary>
/// //
/// </summary>
    public void OnClick_CargarPosicion()
    {
        if (estadoGuardado != null)
        {
            estadoActual = new Nodo(estadoGuardado.nodo);
            ActualizarVisualizacion(estadoActual);
            pasoTexto.text = "Posición cargada.";
        }
        else
        {
            pasoTexto.text = "No hay posición guardada.";
        }
    }//OnClick_CargarPosicion
public void OnClick_BusquedaAnchura()
    {
        StopAllCoroutines();
        IniciarBusqueda(estadoActual, "Anchura");
    }

    public void OnClick_BusquedaAStar()
    {
        StopAllCoroutines();
        IniciarBusqueda(estadoActual, "A*");
    }

    public void OnClick_ProfundidadIterativa()
    {
        StopAllCoroutines();
        //StartCoroutine(BusquedaProfundidadIterativaCoroutine(estadoActual));
    }
  
    }