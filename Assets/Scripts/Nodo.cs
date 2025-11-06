using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;


// Clase Nodo
public class Nodo 
{
    //Propiedades -- objetos, variables
    public int[,] nodo = new int[3, 3];
    public List<Nodo> hijos=new List<Nodo> ();// Lista para almacenar los hijos
    public Nodo padre; //Referencia al padre

    public int Heuristica;

    public int Costo { get; internal set; }
    public int manhattan { get; private set; }


    //Constructor
    public Nodo(int[,] aux)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                this.nodo[i, j] = aux[i, j];
            }
        }
        padre=null;
    }//Nodo .Constructor


    //Método inicializa. Que nos crea por defecto el nodo meta
    public void Inicializa(int[,] aux)
    {
        int indice = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                aux[i,j]=indice;
                indice++;
            }//2 for
        }//1 for
    }// Inicializa

    //Método para imprimir el nodo actual
    public void Imprime()
    {
        string str = "";
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                str += this.nodo[i, j];
                str += " ";
            }//2 for
            str += "\n";
        }//1 for
        Console.WriteLine(str);//Imprimimos la variable. 
    }//Imprime

    public void Imprime(int [,] aux)
    {

        string str = "";
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                str += aux[i, j];
                str += " ";
            }//2 for
            str += "\n";
        }//1 for
        Console.WriteLine(str);//Imprimimos la variable. 
    }//Imprime

    public bool EsMeta()
    {
        int indice = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (this.nodo[i, j] != indice) { 
                    return false; 
                }//if
                indice++;
            }//2 for
        }//1 for
        return true;
    }//EsMeta


    //Método que compara el array de este objeto con un array que se le pasa como parámetro
    public bool EsMismoNodo(int[,] aux)
    {
       
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (this.nodo[i, j] != aux[i,j])
                {
                    return false;
                }//if
               
            }//2 for
        }//1 for
        return true;


    }//EsMismoNodo

    //////////////// 
    /// Método para expandir un nodo
    /// Creamos los nodos hijos a partir de este nodo
    /// 
    public void Expandir()
    {
        int fila=0, columna = 0;//Para buscar el hueco

        for(int i = 0;i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if(nodo[i, j] == 0){
                    fila = i;columna = j;
                    //Expandimos todos los nodos y los metemos en una lista los hijos
                    MueveDerecha(nodo,fila,columna);
                    MueveIzquierda(nodo,fila,columna);
                    MueveArriba(nodo,fila,columna);
                    MueveAbajo(nodo, fila, columna);
                    break;
                }//if 

            }//for 2º
        }//for 1º

        //Console.WriteLine("imprimiendo los hijos");
        //foreach(Nodo objetos in hijos)
        //{
        //    Console.WriteLine("Imprimiendo uno hijete...");
        //    objetos.Imprime();
        //}
    }//Expandir

    private void MueveAbajo(int[,] nodoaux, int fila, int columna)
    {
        if (fila < 2)// Comprobamos que podemos mover a la izquierda
        {
            int[,] destino = new int[3, 3];
            Copiar(nodoaux, destino);
            int temporal = destino[fila + 1, columna];
            destino[fila + 1, columna] = 0;
            destino[fila, columna] = temporal;
            //Ahora creo el objeto que voy a guardar como hijo
            Nodo hijo = new Nodo(destino);
            hijo.padre = this;// Le indico quien es el padre 
            hijos.Add(hijo);
         //   Imprime(destino);

        }// if columna
    }

    private void MueveArriba(int[,] nodoaux, int fila, int columna)
    {
        if (fila > 0)// Comprobamos que podemos mover a la izquierda
        {
            int[,] destino = new int[3, 3];
            Copiar(nodoaux, destino);
            int temporal = destino[fila-1, columna];
            destino[fila-1, columna] = 0;
            destino[fila, columna] = temporal;
            //Ahora creo el objeto que voy a guardar como hijo
            Nodo hijo = new Nodo(destino);
            hijo.padre = this;// Le indico quien es el padre 
            hijos.Add(hijo);
         //   Imprime(destino);

        }// if columna


    }//    MueveArriba

    private void MueveIzquierda(int[,] nodoaux, int fila, int columna)
    {
        if (columna > 0)// Comprobamos que podemos mover a la izquierda
        {
            int[,] destino = new int[3, 3];
            Copiar(nodoaux, destino);
            int temporal = destino[fila, columna-1 ];
            destino[fila, columna -1] = 0;
            destino[fila, columna] = temporal;
            //Ahora creo el objeto que voy a guardar como hijo
            Nodo hijo = new Nodo(destino);
            hijo.padre = this;// Le indico quien es el padre 
            hijos.Add(hijo);
         //   Imprime(destino);

        }// if columna

    }//MueveIzquierda

    private void MueveDerecha(int[,] nodoaux,int fila,int columna)
    {
        if (columna < 2)// Comprobamos que podemos mover a la derecha
        {
            int[,] destino = new int[3, 3];
            Copiar(nodoaux,destino);
            int temporal = destino[fila, columna+1];
            destino[fila, columna + 1] = 0;
            destino[fila, columna]= temporal;
            //Ahora creo el objeto que voy a guardar como hijo
            Nodo hijo=new Nodo(destino);
            hijo.padre = this;// Le indico quien es el padre 
            hijos.Add(hijo);
        //    Imprime(destino);

        }// if columna

    }//MueveDerecha

    /// <summary>
    /// /////// Método para copiar un array en otro
    /// </summary>
    /// <param name="nodoaux"></param>
    /// <param name="destino"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void Copiar(int[,] nodoaux, int[,] destino)
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                destino[i, j] = nodoaux[i, j]; //Copiamos el array en otro
            
        
    }//Copiar

        //Método que calcula las piezas mal colocadas
    public void calculaMalColocadas()
    {
        int indice = 0;
        int mal = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (this.nodo[i, j] != indice)
                {
                    mal++;
                }//if
                indice++;
            }//2 for
        }//1 for
        this.Heuristica = mal;
    }//calculaMalColocadas


}//Nodo 