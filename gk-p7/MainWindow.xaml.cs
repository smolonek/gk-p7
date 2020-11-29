using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gk_p7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PointCollection points = new PointCollection();
        private List<Ellipse> ellipses = new List<Ellipse>();
        private bool drawing = false;
        private bool addingPoints = false;
        private bool mMove = false;
        private bool mRotate = false;
        private bool mScale = false;
        private List<Point> latMousePoints = new List<Point>();
        private Point firstPoint, lastPoint;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddingPoints_Click(object sender, RoutedEventArgs e)
        {
            SetAllToFalse();
            addingPoints = true;
        }

        private void DrawShape_Click(object sender, RoutedEventArgs e)
        {
            SetAllToFalse();
            drawing = true;
            DrawShape();
            ClearPoints();
        }

        private void ClearCanvas_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvas();
        }
        private void ClearCanvas()
        {
            canvas.Children.Clear();
            points.Clear();
            ellipses.Clear();
        }
        private void SetAllToFalse()
        {
            drawing = false;
            addingPoints = false;
            mMove = false;
            mRotate = false;
            mScale = false;
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (addingPoints)
            {
                double x = e.GetPosition(this).X, y = e.GetPosition(this).Y;
                points.Add(new Point(x, y));
                Ellipse ellipse = new Ellipse()
                {
                    Fill = Brushes.LightGray,
                    StrokeThickness = 1,
                    Stroke = Brushes.Gray,
                    Width = 12,
                    Height = 12
                };
                Canvas.SetLeft(ellipse, x - ellipse.Width / 2);
                Canvas.SetTop(ellipse, y - ellipse.Height / 2);
                canvas.Children.Add(ellipse);
                ellipses.Add(ellipse);
            }
            else if (mMove)
            {
                FirstPoint.Content = FirstPoint.Content = latMousePoints[0].X.ToString() + ", " + latMousePoints[0].Y.ToString();
                firstPoint = latMousePoints.First();
                MoveShape(e.GetPosition(this).X - firstPoint.X, e.GetPosition(this).Y - firstPoint.Y);
                latMousePoints.Clear();
            }
            else if (mRotate)
            {
                //p1 p2 p3
                // p1 - punkt pligonu, p2 - tam ghdzie klika myuszka, p3 - tam gdzie puszcza myszka
                FirstPoint.Content = FirstPoint.Content = latMousePoints[0].X.ToString() + ", " + latMousePoints[0].Y.ToString();
                firstPoint = latMousePoints.First();
                double alfa = Math.Acos((Math.Pow(GetDistance(points[0], firstPoint), 2) + Math.Pow(GetDistance(points[0], e.GetPosition(this)), 2) - Math.Pow(GetDistance(firstPoint, e.GetPosition(this)), 2)) / (2 * GetDistance(points[0], firstPoint) * GetDistance(points[0], e.GetPosition(this))));
                RotateShape(e.GetPosition(this).X - firstPoint.X, e.GetPosition(this).Y - firstPoint.Y, alfa);
                latMousePoints.Clear();
            }
        }
        private double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
        private void DrawShape()
        {
            Polygon polygon = new Polygon();
            polygon.Fill = Brushes.Blue;
            polygon.Stroke = Brushes.Red;
            polygon.StrokeThickness = 2;
            polygon.Points = points;
            polygon.MouseMove += ShapeMouseMove;
            canvas.Children.Add(polygon);
            Canvas.SetLeft(polygon, 10);
            Canvas.SetTop(polygon, 10);
            FirstPoint.Content = points.First().X.ToString() + ", " + points.First().Y.ToString();
        }
        private void ClearPoints()
        {
            List<Ellipse> tmp = new List<Ellipse>();
            for (int i = canvas.Children.Count - 1; i >= 0; i--)
            {
                if(canvas.Children[i].GetType() == typeof(Ellipse))
                    tmp.Add((Ellipse)canvas.Children[i]);
            }
            foreach (var item in tmp)
            {
                canvas.Children.Remove(item);
            }
        }
        private void ShapeMouseMove(object sender, MouseEventArgs e)
        {
            var polygon = (Polygon)sender;
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                //FirstPoint.Content = latMousePoints[0].X.ToString() + ", " + latMousePoints[0].Y.ToString();
                //points[0] = latMousePoints[0];
                //MoveShape(Math.Abs(latMousePoints[0].X - points[0].X), Math.Abs(latMousePoints[0].Y - points[0].Y));
                //polygon.

                //firstPoint = latMousePoints.First();
                //MoveShape(e.GetPosition(this).X - firstPoint.X, e.GetPosition(this).Y - firstPoint.Y);
                //latMousePoints.Clear();
            }
        }
        private int GetPolygonIndex()
        {
            foreach(var item in canvas.Children)
            {
                if (item.GetType() == typeof(Polygon))
                    return canvas.Children.IndexOf((UIElement)item);
            }
            return -1;
        }
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            MoveShape(Convert.ToDouble(XTextBox.Text), Convert.ToDouble(YTextBox.Text));            
        }
        private void MoveShape(double x, double y)
        {
            for (int i = 0; i < points.Count; i++)
            {
                points[i] = new Point(points[i].X + x, points[i].Y + y);
            }
        }
        private void RotateShape(double x, double y, double alfa)
        {
            for (int i = 0; i < points.Count; i++)
            {
                points[i] = new Point(x + (points[i].X - x) * Math.Cos(alfa) - (points[i].Y - y) * Math.Sin(alfa), y + (points[i].X - x) * Math.Sin(alfa) - (points[i].Y - y) * Math.Cos(alfa));
            }
        }
        private void RotateButton_Click(object sender, RoutedEventArgs e)
        {
            double x = Convert.ToDouble(XTextBox.Text), y = Convert.ToDouble(YTextBox.Text), alfa = (Convert.ToInt32(AlfaTextBox.Text) * Math.PI) / 180;
            RotateShape(x, y, alfa);
        }

        private void ScaleButton_Click(object sender, RoutedEventArgs e)
        {
            double x = Convert.ToDouble(XTextBox.Text), y = Convert.ToDouble(YTextBox.Text), k = Convert.ToDouble(KTextBox.Text);
            for (int i = 0; i < points.Count; i++)
            {
                points[i] = new Point(points[i].X * k + ((1 - k) * x), points[i].Y * k + ((1 - k) * y));
            }
        }

        private void MMove_Click(object sender, RoutedEventArgs e)
        {
            SetAllToFalse();
            mMove = true;
        }

        private void MRotate_Click(object sender, RoutedEventArgs e)
        {
            SetAllToFalse();
            mRotate = true;
        }

        private void MScale_Click(object sender, RoutedEventArgs e)
        {
            SetAllToFalse();
            mScale = true;
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            latMousePoints.Add(e.GetPosition(this));
            FirstPoint.Content = latMousePoints[0].X.ToString() + ", " + latMousePoints[0].Y.ToString();
        }
    }
}
