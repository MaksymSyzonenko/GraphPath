using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfApp2.Math
{
    class ShortestPathFinder
    {
        private int[,] graph;
        private int numCities;

        public ShortestPathFinder(int[,] adjacencyMatrix)
        {
            graph = adjacencyMatrix;
            numCities = adjacencyMatrix.GetLength(0);
        }

        public string FindShortestPath(int startCity, int endCity)
        {
            int[] distances = new int[numCities];
            int[] previousCities = new int[numCities];
            bool[] visited = new bool[numCities];

            for (int i = 0; i < numCities; i++)
            {
                distances[i] = int.MaxValue;
                previousCities[i] = -1;
                visited[i] = false;
            }

            distances[startCity] = 0;

            for (int i = 0; i < numCities - 1; i++)
            {
                int minDistance = int.MaxValue;
                int currentCity = -1;

                for (int j = 0; j < numCities; j++)
                {
                    if (!visited[j] && distances[j] < minDistance)
                    {
                        minDistance = distances[j];
                        currentCity = j;
                    }
                }

                if (currentCity == -1)
                    break;

                visited[currentCity] = true;

                for (int j = 0; j < numCities; j++)
                {
                    if (!visited[j] && graph[currentCity, j] != 0 && distances[currentCity] + graph[currentCity, j] < distances[j])
                    {
                        distances[j] = distances[currentCity] + graph[currentCity, j];
                        previousCities[j] = currentCity;
                    }
                }
            }

            if (distances[endCity] == int.MaxValue)
            {
                return "Путь не найден.";
            }

            var path = new List<int>();
            int city = endCity;

            while (city != -1)
            {
                path.Insert(0, city);
                city = previousCities[city];
            }

            for (int i = 0; i < path.Count; i++)
                path[i] = ++path[i];
            string pathString = string.Join(" -> ", path);
            return $"Кратчайший путь из города {++startCity} в город {++endCity} равен: {distances[--endCity]}! \n Путь: {pathString}.";
        }
    }

}
