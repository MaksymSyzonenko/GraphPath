using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp2
{
    class Arrow
    {
        private double x1 { get; set; }
        private double y1 { get; set; }
        private double x2 { get; set; }
        private double y2 { get; set; }
        private Brush brush { get; set; }

        private List<Line> lines = new List<Line>();

        private Canvas container = new Canvas();

        public List<Line> Lines
        {
            get { return lines; }
        }

        public Canvas Container
        {
            get
            {
                for(int i = 0; i < lines.Count; i++)
                {
                    container.Children.Add(lines[i]);
                }
                return container;
            }
        }


        public Arrow(double x1_, double y1_, double x2_, double y2_, Brush brush_)
        {
            x1 = x1_;
            y1 = y1_;
            x2 = x2_;
            y2 = y2_;
            brush = brush_;

            double d = System.Math.Sqrt(System.Math.Pow(x2 - x1, 2) + System.Math.Pow(y2 - y1, 2));

            double X = x2 - x1;
            double Y = y2 - y1;

            double X3 = x2 - (X / d) * 25;
            double Y3 = y2 - (Y / d) * 25;

            double Xp = y2 - y1;
            double Yp = x1 - x2;

            double X4 = X3 + (Xp / d) * 5;
            double Y4 = Y3 + (Yp / d) * 5;
            double X5 = X3 - (Xp / d) * 5;
            double Y5 = Y3 - (Yp / d) * 5;

            Line line = new Line
            {
                StrokeThickness = 1.5,
                Stroke = brush,
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2
            };
            lines.Add(line);

            line = new Line
            {
                StrokeThickness = 2.5,
                Stroke = line.Stroke,
                X1 = x2,
                Y1 = y2,
                X2 = X4,
                Y2 = Y4
            };
            lines.Add(line);

            line = new Line
            {
                StrokeThickness = 2.5,
                Stroke = line.Stroke,
                X1 = x2,
                Y1 = y2,
                X2 = X5,
                Y2 = Y5
            };
            lines.Add(line);
        }
    }
}
