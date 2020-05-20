using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Komiwojazer
{
    static class Defined
    {
        public const int startNode = 2;
        public const int nodeNumber = 4;
    }
   
    class Komiwojazer
    {
       
        static int NodeNumber = Defined.nodeNumber;
        Random rnd = new Random();

        public void Random(Random random)
        {
            rnd = random;
        }
        void nearest_neighbour(int[,]graph, int startNode)
        {
            Console.WriteLine("=========================================================================== ");
            Console.WriteLine("     \t\t\t--Algorytm Nearest Neighbour--");
            Console.WriteLine("===========================================================================");
            string path = "Ścieżka: ";

            bool[] visited = new bool[NodeNumber];
            for (int i = 0; i < NodeNumber; i++)
            {
                visited[i] = false;
            }

            int currentNode = startNode;
            int minCost = int.MaxValue;
            visited[currentNode] = true;
            int tmp = currentNode;
            int currentCost = 0;
            int endCost = 0;

            for(int j=0;j< NodeNumber-1;j++)
            {

                for (int i = 0; i < NodeNumber; i++)
                {
                    if (visited[i] == false && graph[currentNode, i] < minCost && graph[currentNode, i] != 0)
                    {
                        minCost = graph[currentNode, i];
                        tmp = i;
                    }
                }
                Console.WriteLine("Koszt drogi " + currentNode + " --> " + tmp + " = " + graph[currentNode, tmp]);
                path += currentNode + " --> ";
                currentCost += graph[currentNode, tmp];
                Console.WriteLine("Aktualny koszt: " + currentCost);
                currentNode = tmp;
                visited[currentNode] = true;
                minCost = int.MaxValue;
            }
            path += currentNode;
            endCost = currentCost + graph[currentNode, startNode];
            Console.WriteLine("Koszt drogi " + currentNode + " --> " + startNode + " = " + graph[currentNode, startNode]);
            Console.WriteLine("===========================================================================");
            Console.WriteLine("\t\t\tKoszt Koncowy: " + endCost);
            Console.WriteLine("\t\t\t" + path + " --> " + startNode);
            Console.WriteLine("===========================================================================");

        }
        void permutationFunc(int [,] graph , int[,] next_graph)
        {
            for(int i = 0 ; i< NodeNumber;i++)
            {
                for(int j=0; j < NodeNumber; j++)
                {
                    next_graph[i, j] = graph[i, j];
                }
                
            }

            int i1 = (int)(rnd.Next(0, NodeNumber - 2) );
            int j1 = (int)(rnd.Next(0, NodeNumber - 1) );
            int i2 = (int)(rnd.Next(0, NodeNumber - 2) );
            int j2 = (int)(rnd.Next(0, NodeNumber - 1) );

            if (j2 >= i2) j2++;
            if (j1 >= i1) j1++; // i1 = 2, j1 = 1., i2 = 0, j2 = 3 [2,1] i [0,3]
                      
           
            
            
            //tmp = 6
            int tmp = next_graph[i1,j1]; //[2,1] = 6   tmp = 6 
            //tmp2 = 5 
            int tmp2 = next_graph[i2, j2];  // [0,3] = 5 tmp2 = 5

            next_graph[i1, j1] = tmp2;

            next_graph[j1, i1] = tmp2;

            next_graph[j2, i2] = tmp;
            next_graph[i2, j2] = tmp;                             

            
        }
        int sumDistance(int[,] graph)
        {
            int distance = 0;
            for(int i = 0; i < NodeNumber-1;i++)
            {
                distance += graph[i, i + 1];                
            }
                        
            distance += graph[NodeNumber - 1, 0];

            return distance;
        }
        void annealing(int [,] graph, int[,] next_graph)
        {
            Console.WriteLine("===========================================================================");
            Console.WriteLine("            \t\t--Algorytm Annealing--");
            Console.WriteLine("===========================================================================");

            int iteration = -1;            
            double proba;
            double alpha = 0.999;
            double temperature = 400.0;
            double epsilon = 0.01;
            int delta;
            int distance = sumDistance(graph);


            while (temperature > epsilon)
            {
                iteration++;

                permutationFunc(graph, next_graph);

                delta = sumDistance(next_graph) - distance;

                if (delta < 0)
                {
                    graph = next_graph;
                    distance = delta + distance;
                }
                else
                {
                    proba = rnd.NextDouble();
                    if (proba < Math.Exp(-delta / temperature))
                    {
                        graph = next_graph;
                        distance = delta + distance;
                    }
                }
                temperature *= alpha;
                if (iteration % 400 == 0) // drukujemy co 400 iteracji
                {

                    if (iteration == 0)
                    {
                        Console.WriteLine("Iteracja = " + iteration + "  \t\tDystans z " + Defined.startNode + " --> " + Defined.startNode + " = " + distance + ",\tTemperatura = " + temperature);

                    }
                    else
                    {
                        Console.WriteLine("Iteracja = " + iteration + "  \tDystans z " + Defined.startNode + " --> " + Defined.startNode + " = " + distance + ",\tTemperatura = " + temperature);

                    }
                }

            }
            Console.WriteLine("===========================================================================");
            Console.WriteLine("     \t\t\tNajmniejszy Koszt = " + distance);
            Console.WriteLine("===========================================================================");
        }
             
        public static void Main()
        {
            
            //Przykładowy graf dla z 6 nodami. Poniżej Macierz Sąsiedztwa 
            int[,] graph2 = new int[,] {     /*0  1  2  3  4  5*/
            /* 0  */                        { 0, 2, 3, 2, 1, 5 },
            /* 1 */                         { 2, 0, 6, 2, 5, 1 },
            /* 2  */                        { 3, 6, 0, 3, 2, 7 },
            /* 3  */                        { 2, 2, 3, 0, 5, 1 },
            /* 4  */                        { 1, 5, 2, 5, 0, 9 },
            /* 5  */                        { 5, 1, 7, 1, 9, 0 },
};

            int[,] graph = new int[,] {     /*0  1  2  3  
                                  /* 0 */   { 0, 2, 3, 5},
                                  /* 1 */   { 2, 0, 6, 1},
                                  /* 2 */   { 3, 6, 0, 7},
                                  /* 3 */   { 5, 1, 7, 0},


                                  
            };
            int[,] next_graph = new int[Defined.nodeNumber, Defined.nodeNumber];

            Komiwojazer t = new Komiwojazer();
            
            t.nearest_neighbour(graph,Defined.startNode);
            t.annealing(graph, next_graph);

            
        }
    }
}


 
