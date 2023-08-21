using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfApp2.Math;
using WpfApp2.VisualElements;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int numberofTowns = 0;
        int firstTown;
        int secondTown;
        TextBoxMatrix matrix;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string pattern = @"\b[2-6]{1}\b";
            if (!Regex.IsMatch(towns.Text, pattern))
            {
                MessageBox.Show("Вы не правильно ввели количество городов. ");
                return;
            }
            numberofTowns = Convert.ToInt32(towns.Text);
            matrix = new TextBoxMatrix(numberofTowns);
            matrix.BuildMatrix();
            RootGrid.Children.Add(matrix.Canvas);
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (matrix is null)
            {
                MessageBox.Show("Инициализируйте матрицу весов!");
                return;
            }
            string newPattern = @"\b[1-6]{1}\b";
            if (!(Regex.IsMatch(firsttown.Text, newPattern) && Regex.IsMatch(secondtown.Text, newPattern)))
            {
                MessageBox.Show("Вы не правильно ввели номер города. ");
                return;
            }
            firstTown = Convert.ToInt32(firsttown.Text);
            secondTown = Convert.ToInt32(secondtown.Text);
            if (!(firstTown < numberofTowns || secondTown > numberofTowns || firstTown > numberofTowns || secondTown < numberofTowns))
            {
                MessageBox.Show("Вы ввели номер не существующего города. ");
                return;
            }
            var array = matrix.GetMatrix();
            if(!matrix.IsMatrixValid)
            {
                MessageBox.Show("Вы не правильно заполнили матрицу весов графа. ");
                return;
            }    
            var shortestPathFinder = new ShortestPathFinder(array);
            string result = shortestPathFinder.FindShortestPath(firstTown - 1, secondTown - 1);
            MessageBox.Show(result);
        }
        private void Button_Click_3(object sender, RoutedEventArgs e) => MessageBox.Show("Инструкция! Для начала введите количество городов в первое поле(от 2 до 6). Далее нажмите кнопку \" Готово \" и заполните матрицу весов графа. Следующим шагом введите в два поля по одному номеру города(первый будет стартом, второй финишем), и нажмите кнопку \" Расчитать\". Чтобы увидеть граф, нажмите кнопку \" Показать граф\".");
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (matrix is null)
            {
                MessageBox.Show("Инициализируйте матрицу весов!");
                return;
            }
            var array = matrix.GetMatrix();
            if (!matrix.IsMatrixValid)
            {
                MessageBox.Show("Вы не правильно заполнили матрицу весов графа. ");
                return;
            }
            Dictionary<int, Brush> cityColorMap = new Dictionary<int, Brush>
            {
                { 0, Brushes.Red },
                { 1, Brushes.DarkBlue },
                { 2, Brushes.DarkGreen },
                { 3, Brushes.Orange },
                { 4, Brushes.Purple },
                { 5, Brushes.Yellow },
            };
            GraphsImage graphsImage = (bool)checkBox_Colors.IsChecked ? 
                new GraphsImage(numberofTowns, array, (bool)checkBox_Distances.IsChecked, cityColorMap) :
                new GraphsImage(numberofTowns, array, (bool)checkBox_Distances.IsChecked);
            graphsImage.Build();
            Window window = new Window
            {
                Background = Brushes.LightGray,
                Content = graphsImage.Canvas
            };
            window.Show();
        }
    }
}
