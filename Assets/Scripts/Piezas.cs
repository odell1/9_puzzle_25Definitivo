using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Piezas 
{
    // Start is called before the first frame update
    public void Start()
    {
        //Cuando se inicie el juego de rigor por aquí empezamos!
        // Creo el nodo

        int[,] piezas = {{1,2,3},
                        {4,5,8},
                        {6,0,7}};//Creo el array
                        
        

        Nodo root=new Nodo(piezas);// Creo el objeto con el array

        Debug.Log("¡Anchura! ");
        List<Nodo> solucion = BusquedaAnchura(root);
    
        if (solucion.Count > 0)
            {
              //  Debug.Log("Imprimiendo la solución: ");
                solucion.Reverse();
                for (int i = 0; i < solucion.Count; i++)
                {
                   // solucion[i].Imprime();
                }
            }
            else
            {
                 //Debug.Log("No hemos encontrado la solucion");
            }

    }//Start

    /////////////
    /// Búsqueda en anchura
    /// ////////
    public List<Nodo> BusquedaAnchura(Nodo root)
    {
        //Variables para el algoritmo
        List<Nodo> Abiertos = new List<Nodo>();//Nodos que faltan por visitar
        List<Nodo> Cerrados = new List<Nodo>();// Nodos que ya he visitado
        List<Nodo> CaminoSoluccion = new List<Nodo>(); // Lista con el camino
        bool encontrado = false;
        Abiertos.Add(root);//añado el raíz
        int contador = 0;

        while (Abiertos.Count > 0 && !encontrado)
        {
            Nodo actual = Abiertos[0]; //cogemos el primer elemento
            Abiertos.RemoveAt(0);//eliminamos el elemento
            Cerrados.Add(actual);//ya visitamos este nodo
            //Tratamos el nodo actual. 
            if (actual.EsMeta())
            {
                // Console.WriteLine("Hemos encontrado el nodo solución!!!!");
                encontrado = true;
                //tenemos que devolver el camino a la solución, es decir, la lista de los nodos
                // por los que tenemos que pasar
                Trazo(CaminoSoluccion, actual);
                //Console.WriteLine("Número de pasos de la solución: "+ CaminoSoluccion.Count());
                return CaminoSoluccion;
                //break;//Salimos del todo
            }//if

            //Expandimos el nodo actual
            actual.Expandir();
            for (int i = 0; i < actual.hijos.Count; i++)// Recorremos todos los hijos
            {
                Nodo hijoActual = actual.hijos[i];
                if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual))
                {
                    // hijoActual.Imprime();
                    Abiertos.Add(hijoActual);// Metemos como una posible solución
                    contador++;
                    //  Console.WriteLine("Número de nodos metidos: "+contador);

                }//if

            }//for
            //if (contador == 50000) break;
        }//while

        //No hemos encontrado solución
        Debug.Log("No hemos encontrado solución, Alma de cántaro.");
        return null;




    }//BusquedaAnchura



    /// <summary>
    /// Búsqueda en profundidad
    /// </summary>
    /// <param name="lista"></param>
    /// <param name="hijoActual"></param>
    /// <returns></returns>
    /// /////////////
    /// Búsqueda en profundidad (DFS)
    /// ////////
    public List<Nodo> BusquedaProfundidad(Nodo root)
    {


        List<Nodo> Abiertos = new List<Nodo>();
        List<Nodo> Cerrados = new List<Nodo>();// Nodos que ya he visitado
        List<Nodo> CaminoSoluccion = new List<Nodo>(); // Lista con el camino
        bool encontrado = false;
        Abiertos.Add(root);// añado el raíz (Push)
        int contador = 0;

        while (Abiertos.Count > 0 && !encontrado)
        {
           
            int ultimoIndice = Abiertos.Count - 1;
            Nodo actual = Abiertos[ultimoIndice]; //cogemos el último elemento (LIFO)
            Abiertos.RemoveAt(ultimoIndice);      //eliminamos el último elemento (Pop)

            Cerrados.Add(actual);// ya visitamos este nodo

            //Tratamos el nodo actual. 
            if (actual.EsMeta())
            {

                encontrado = true;
                Trazo(CaminoSoluccion, actual);

                return CaminoSoluccion;
            }

            //Expandimos el nodo actual
            actual.Expandir();
            for (int i = 0; i < actual.hijos.Count; i++)// Recorremos todos los hijos
            {
                Nodo hijoActual = actual.hijos[i];
                if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual))
                {

                    Abiertos.Add(hijoActual);// Metemos como una posible solución (Push)
                    contador++;


                }
            }

        }//while


        return null;
    }//BusquedaProfundidad





    /////////////
    /// Búsqueda en Profundidad Acotada 
    /// ////////
    public List<Nodo> BusquedaProfundidadAcotada(Nodo root, int limite)
    {
        //Variables para el algoritmo
        // Abiertos se usa como una Pila (Stack LIFO)
        List<Nodo> Abiertos = new List<Nodo>();
        List<Nodo> Cerrados = new List<Nodo>(); // Nodos que ya he visitado
        List<Nodo> CaminoSolucion = new List<Nodo>(); // Lista con el camino
        bool encontrado = false;

        // Inicializar el costo/profundidad del nodo raíz (asumiendo que Nodo.Costo guarda la profundidad)
        root.Costo = 0;
        Abiertos.Add(root); // añado el raíz

        while (Abiertos.Count > 0 && !encontrado)
        {
            
            int ultimoIndice = Abiertos.Count - 1;
            Nodo actual = Abiertos[ultimoIndice];
            Abiertos.RemoveAt(ultimoIndice);

            Cerrados.Add(actual); // ya visitamos este nodo

  
            if (actual.EsMeta())
            {
                UnityEngine.Debug.Log("¡Hemos encontrado el nodo solución!");
                encontrado = true;
                Trazo(CaminoSolucion, actual);
                return CaminoSolucion;
            }

          
            if (actual.Costo < limite)
            {
                // Expandimos el nodo actual
                actual.Expandir();
                for (int i = 0; i < actual.hijos.Count; i++)// Recorremos todos los hijos
                {
                    Nodo hijoActual = actual.hijos[i];

                    // Asignar el costo/profundidad al hijo
                    hijoActual.Costo = actual.Costo + 1;

                    if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual))
                    {
                        // Metemos como una posible solución (Push)
                        Abiertos.Add(hijoActual);
                    }
                }
            }
            // Si actual.Costo == limite, el nodo no se expande y la búsqueda se detiene en ese camino.
        }

        // No hemos encontrado solución
        Debug.Log("No se encontró solución dentro del límite de profundidad: " + limite);
        return null;
    }//Búsqueda en prundidad acotada


    /// <summary>
    /// Búsqueda A*
    /// </summary>
    /// <param name="lista"></param>
    /// <param name="hijoActual"></param>
    /// <returns></returns>
    public List<Nodo> BuscaAsterisco(Nodo root)
    {
        List<Nodo> abiertos = new List<Nodo>(); //Por visitar
   List<Nodo> Abiertos = new List<Nodo>(); // Nodos por visitar (Cola de Prioridad)
    List<Nodo> Cerrados = new List<Nodo>(); // Nodos visitados
    List<Nodo> CaminoSolucion = new List<Nodo>();
    bool encontrado = false;

    // Inicializar el nodo raíz
    root.Costo = 0;
    root.calculaManhattan(); // Calcular h(n) inicial
    Abiertos.Add(root);

        while (Abiertos.Count > 0 && !encontrado)
        {

            Abiertos = Abiertos.OrderBy(n => n.Costo + n.Heuristica).ToList();//Ordenamos por coste+heurística. 

            // Cogemos el menor
            Nodo actual = Abiertos[0];
            Abiertos.RemoveAt(0);


            if (actual.EsMeta())
            {
                Debug.Log("¡Solución A* encontrada!");
                Trazo(CaminoSolucion, actual);
                return CaminoSolucion;
            }

            Cerrados.Add(actual); // Marcar como visitado


            actual.Expandir();
            for (int i = 0; i < actual.hijos.Count; i++)
            {
                Nodo hijoActual = actual.hijos[i];

                //Asignar Coste (g(n)): Profundidad
                hijoActual.Costo = actual.Costo + 1;

                //Calcular Heurística (h(n)): Piezas mal colocadas
                hijoActual.calculaManhattan();


                if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual))
                {
                    Abiertos.Add(hijoActual);
                }

            }//for
        }//while
        return null;
    }//BuscaAsterisco


    /// <summary>
    /// Búsqueda Voraz (Greedy Search)
    /// Ordena los abiertos solo por la Heurística (h(n)), ignora el Costo (g(n)).
    /// </summary>
    public List<Nodo> BusquedaVoraz(Nodo root)
    {
        List<Nodo> Abiertos = new List<Nodo>(); // Nodos por visitar (Cola de Prioridad simulada)
        List<Nodo> Cerrados = new List<Nodo>(); // Nodos visitados
        List<Nodo> CaminoSolucion = new List<Nodo>();
        bool encontrado = false;

        // Inicializar el nodo raíz
        root.Costo = 0;
        root.calculaManhattan(); // *** Usamos Manhattan ***
        Abiertos.Add(root);

        while (Abiertos.Count > 0 && !encontrado)
        {
            // *** CAMBIO CLAVE: Ordenamos solo por Heuristica (h(n)) ***
            // La Voraz solo se fija en la proximidad a la meta, no en el costo del camino
            Abiertos = Abiertos.OrderBy(n => n.Heuristica).ToList();//----------- nos fijamos solamente en la heurística

            // Cogemos el menor
            Nodo actual = Abiertos[0];
            Abiertos.RemoveAt(0);

            if (actual.EsMeta())
            {
                Debug.Log("¡Solución Voraz encontrada!");
                Trazo(CaminoSolucion, actual);
                return CaminoSolucion;
            }

            Cerrados.Add(actual); // Marcar como visitado

            actual.Expandir();
            for (int i = 0; i < actual.hijos.Count; i++)
            {
                Nodo hijoActual = actual.hijos[i];

                // Asignar Coste (g(n)): Profundidad 
                hijoActual.Costo = actual.Costo + 1;

                // Calcular Heurística (h(n))
                hijoActual.calculaManhattan(); // *** Usamos Manhattan ***

                if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual))
                {
                    Abiertos.Add(hijoActual);
                }
            }//for
        }//while
        return null;
    }//BusquedaVoraz





    //////////////////////////////

    /// <summary>
    /// ///////
    /// </summary>
    /// <param name="lista"></param>
    /// <param name="hijoActual"></param>
    /// <returns></returns>
    private bool Contiene(List<Nodo> lista,Nodo hijoActual)
    {
        /*   for (int i = 0; i < lista.Count; i++)
           {
               if (lista[i].EsMismoNodo(hijoActual.nodo))
                   return true;
           }

           return false;
        */
        foreach (Nodo nodo in lista)
        {
            if(nodo.EsMismoNodo(hijoActual.nodo)) { return true; }
        }
        return false;
    }//Contiene



    //Metodo que hace la solucion
    public void Trazo(List<Nodo> camino, Nodo n)
    {
        Console.WriteLine("Trazando el camino: ");
        Nodo actual = n;
        camino.Add(actual);
        while (actual.padre != null)
        {
            actual = actual.padre;
            camino.Add(actual);
        }

    }



    private int[,] GenerarMatrizAleatoria()
    {
        int[,] matriz = new int[3, 3];
        List<int> numeros = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        System.Random rand = new System.Random();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int index = rand.Next(numeros.Count);
                matriz[i, j] = numeros[index];
                numeros.RemoveAt(index);
            }
        }

        return matriz;
    }


}//Piezas