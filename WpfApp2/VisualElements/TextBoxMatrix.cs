using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2.VisualElements
{
    internal class TextBoxMatrix
    {
        public int Size { get; set; }
        public Canvas Canvas { get; set; }
        public List<TextBox> TextBoxes { get; set; }
        public bool IsMatrixValid { get; set; }
        public TextBoxMatrix(int Size)
        {
            this.Size = Size;
            Canvas = new Canvas();
            TextBoxes = new List<TextBox>();
            IsMatrixValid = true;
        }
        public void BuildMatrix()
        {
            TextBoxes.Clear();
            for (int i = 0; i < Size * Size; i++)
            {
                TextBox textBox = new TextBox();
                TextBoxes.Add(textBox);
            }
            int marginTop = 100, marginLeft = 50;
            for (int i = 0; i < Size * Size; i++)
            {
                TextBoxes[i].Text = "0";
                TextBoxes[i].HorizontalAlignment = HorizontalAlignment.Left;
                TextBoxes[i].VerticalAlignment = VerticalAlignment.Top;
                TextBoxes[i].TextWrapping = TextWrapping.Wrap;
                TextBoxes[i].Height = 20;
                TextBoxes[i].Width = 50;
                if (i % Size == i / Size)
                    TextBoxes[i].IsReadOnly = true;
                if (marginLeft >= Size * 100)
                {
                    marginLeft = 50;
                    marginTop += 50;
                    TextBoxes[i].Margin = new Thickness(marginLeft, marginTop, 0, 0);
                    Canvas.Children.Add(TextBoxes[i]);
                    marginLeft += 100;
                }
                else
                {
                    TextBoxes[i].Margin = new Thickness(marginLeft, marginTop, 0, 0);
                    Canvas.Children.Add(TextBoxes[i]);
                    marginLeft += 100;
                }
            }
        }
        public int[,] GetMatrix()
        {
            IsMatrixValid = true;
            int[] elem = new int[Size * Size];
            string pattern = @"[0-9]";
            int[,] array = new int[Size, Size];
            for (int i = 0; i < TextBoxes.Count; i++)
                if (!Regex.IsMatch(TextBoxes[i].Text, pattern))
                {
                    IsMatrixValid = false;
                    return null;
                }
            for (int i = 0; i < Size * Size; i++)
                elem[i] = Convert.ToInt32(TextBoxes[i].Text);
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    array[i, j] = elem[(i * Size) + j];
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    if (array[i, j] != 0 && array[j, i] != 0)
                        if (array[i, j] != array[j, i])
                            IsMatrixValid = false;
            return array;
        }
    }
}
