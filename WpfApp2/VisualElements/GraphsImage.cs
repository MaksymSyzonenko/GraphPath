using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp2.VisualElements
{
    internal class GraphsImage
    {
        public int NumberOfTowns { get; set; }
        public int[,] Matrix { get; set; }
        public bool IsColoured { get; set; }
        public bool IsWithDistance { get; set; }
        public Canvas Canvas { get; set; }
        private List<Line> Lines { get; set; }
        private readonly Random rnd = new Random();
        private readonly Dictionary<int, Brush> cityColorMap;
        public GraphsImage(int numberOfTowns, int[,] matrix, bool isWithDistance, Dictionary<int, Brush> cityColorMap)
        {
            NumberOfTowns = numberOfTowns;
            Matrix = matrix;
            IsColoured = true;
            IsWithDistance = isWithDistance;
            this.cityColorMap = cityColorMap;
            Canvas = new Canvas();
        }
        public GraphsImage(int numberOfTowns, int[,] matrix, bool isWithDistance) 
        {
            NumberOfTowns = numberOfTowns;
            Matrix = matrix;
            IsColoured = false;
            IsWithDistance = isWithDistance;
            Canvas = new Canvas();
        }
        public void Build()
        {
            Lines = new List<Line>();
            var values = GetCoordinates();
            Canvas.Width = values.Item3;
            Canvas.Height = values.Item4;
            Canvas.VerticalAlignment = VerticalAlignment.Top;
            AddTowns(values.Item1, values.Item2);
            AddArrows(values.Item1, values.Item2);
            if (IsWithDistance)
                AddDistances();
        }
        private void AddTowns(List<int> coordinateLeft, List<int> coordinateTop)
        {
            var textBlocks = new List<TextBlock>();
            var ellipses = new List<Ellipse>();
            for (int i = 0; i < NumberOfTowns; i++)
            {
                var brush = IsColoured ? cityColorMap[i] : Brushes.Black;
                int count = i + 1;
                var ellipse = new Ellipse()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(coordinateLeft[i], coordinateTop[i], 0, 0),
                    Width = 70,
                    Height = 70,
                    StrokeThickness = 2,
                    Stroke = brush,
                    Visibility = Visibility.Visible
                };
                var textBlock = new TextBlock()
                {
                    Text = count.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(coordinateLeft[i] + 30, coordinateTop[i] + 18, 0, 0),
                    Foreground = brush,
                    Width = 50,
                    Height = 50,
                    FontSize = 20
                };
                textBlocks.Add(textBlock);
                ellipses.Add(ellipse);
            }
            for (int i = 0; i < NumberOfTowns; i++)
            {
                Canvas.Children.Add(ellipses[i]);
                Canvas.Children.Add(textBlocks[i]);
            }
        }
        private void AddArrows(List<int> coordinateLeft, List<int> coordinateTop)
        {
            var townFrom = new List<int>();
            var townTo = new List<int>();

            for (int i = 0; i < NumberOfTowns; i++)
                for (int j = 0; j < NumberOfTowns; j++)
                    if (Matrix[i, j] != 0)
                    {
                        townFrom.Add(i);
                        townTo.Add(j);
                    }

            for (int i = 0; i < townFrom.Count; i++)
                if (townFrom[i] != townTo[i])
                {
                    var brush = IsColoured ? cityColorMap[townFrom[i]] : Brushes.Black;
                    if (coordinateTop[townFrom[i]] < coordinateTop[townTo[i]])
                    {
                        Arrow arrow = new Arrow(coordinateLeft[townFrom[i]] + 35, coordinateTop[townFrom[i]] + 50, coordinateLeft[townTo[i]] + 35, coordinateTop[townTo[i]] + 15, brush);
                        Lines.Add(arrow.Lines[0]);
                        Canvas.Children.Add(arrow.Container);
                    }
                    else
                    {
                        Arrow arrow = new Arrow(coordinateLeft[townFrom[i]] + 35, coordinateTop[townFrom[i]] + 15, coordinateLeft[townTo[i]] + 35, coordinateTop[townTo[i]] + 50, brush);
                        Lines.Add(arrow.Lines[0]);
                        Canvas.Children.Add(arrow.Container);
                    }
                }
        }
        private void AddDistances()
        {
            var distance = new List<TextBlock>();
            for (int i = 0; i < NumberOfTowns; i++)
                for (int j = 0; j < NumberOfTowns; j++)
                    if (Matrix[i, j] != 0 && i != j)
                    {
                        var dist = new TextBlock()
                        {
                            Text = Matrix[i, j].ToString(),
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            Width = 50,
                            Height = 50,
                            FontSize = 20,
                        };
                        distance.Add(dist);
                    }

            for (int i = 0; i < distance.Count; i++)
            {
                double x = (Lines[i].X1 + Lines[i].X2) / 2;
                double y = (Lines[i].Y1 + Lines[i].Y2) / 2;
                distance[i].Margin = new Thickness(x - 20, y - 10, 0, 0);
            }

            for (int i = 0; i < distance.Count; i++)
                Canvas.Children.Add(distance[i]);
        }
        private (List<int>, List<int>, int, int) GetCoordinates()
        {
            var coordinateLeft = new List<int>(NumberOfTowns);
            var coordinateTop = new List<int>(NumberOfTowns);
            int rndTopCoordinate = 0;
            int maxTopCoordinate = 0;
            int coefficient = 50;

            for (int i = 0; i < NumberOfTowns; i++)
            {
                if (i == 0)
                {
                    rndTopCoordinate = rnd.Next(50, 650);
                    maxTopCoordinate = rndTopCoordinate;
                    coordinateLeft.Add(coefficient);
                    coordinateTop.Add(rndTopCoordinate);
                    coefficient += 100;
                }
                else
                {
                    if (rndTopCoordinate > 300)
                    {
                        rndTopCoordinate = rnd.Next(50, 300);
                        coordinateLeft.Add(coefficient);
                        coordinateTop.Add(rndTopCoordinate);
                        coefficient += 100;
                    }
                    else
                    {
                        rndTopCoordinate = rnd.Next(450, 550);
                        if (rndTopCoordinate > maxTopCoordinate)
                            maxTopCoordinate = rndTopCoordinate;
                        coordinateLeft.Add(coefficient);
                        coordinateTop.Add(rndTopCoordinate);
                        coefficient += 100;
                    }
                }
            }

            return (coordinateLeft, coordinateTop, coefficient, maxTopCoordinate);
        } 
    }
}
