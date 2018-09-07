using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
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

namespace snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Ellipse apple { get; set; }
        

        private Timer tick { get; set; }
        private Random rnd;
        private Dictionary<int, Ellipse> food { get; set; }

        private int direction = 2;
        private int lastdirection = 2;

        List<Rectangle> arr_rec = new List<Rectangle>();

        List<Double> x_tail = new List<Double>();
        List<Double> y_tail = new List<Double>();

        Boolean add_tail = false;

        private int window_width;
        private int window_height;



        private double box;

        public static readonly DependencyProperty numofapplesProperty = DependencyProperty.Register("numofapples", typeof(int), typeof(MainWindow), new UIPropertyMetadata(0));
        public int numofapples
        {
            get { return (int)GetValue(numofapplesProperty); }
            set { SetValue(numofapplesProperty, value); }
        }
        
        public static readonly DependencyProperty snakeLengthProperty = DependencyProperty.Register("snakeLength", typeof(int), typeof(MainWindow), new UIPropertyMetadata(0));
        public int snakeLength
        {
            get { return (int)GetValue(snakeLengthProperty); }
            set { SetValue(snakeLengthProperty, value); }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            this.snakeLength = 5;
            this.rnd = new Random();
            this.food = new Dictionary<int, Ellipse>();
            this.window_width = (Int32)this.Width - 16;
            this.window_height = (Int32)this.Height - 39 - 24;
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);

            box = 12;

            init();

            this.tick = new Timer();
            this.tick.Elapsed += tick_Elapsed;
            this.tick.Interval = 100; //this.time_multiplier
            this.tick.Start();
        }

        private void tick_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.InvokeAsync((Action)(() =>
            {

                switch (direction)
                {
                    case 0:
                        lastdirection = 0;
                        //Canvas.SetTop(snake_head, x_tail[0] - box);
                        if (Canvas.GetTop(arr_rec[arr_rec.Count - 1]) - 24 - box < 0)
                        {
                            DrawTail(Canvas.GetLeft(arr_rec[arr_rec.Count - 1]), window_height + box);
                        }
                        else
                        {
                            DrawTail(Canvas.GetLeft(arr_rec[arr_rec.Count - 1]), Canvas.GetTop(arr_rec[arr_rec.Count - 1]) - box);
                        }
                        break;
                    case 1:
                        lastdirection = 1;
                        //Canvas.SetTop(snake_head, x_tail[0] + box);
                        if (Canvas.GetTop(arr_rec[arr_rec.Count - 1]) > window_height)
                        {
                            DrawTail(Canvas.GetLeft(arr_rec[arr_rec.Count - 1]), 24);
                        }
                        else
                        {
                            DrawTail(Canvas.GetLeft(arr_rec[arr_rec.Count - 1]), Canvas.GetTop(arr_rec[arr_rec.Count - 1]) + box);
                        }
                        break;
                    case 2:
                        lastdirection = 2;
                        //Canvas.SetLeft(snake_head, y_tail[0] + box);
                        if (Canvas.GetLeft(arr_rec[arr_rec.Count - 1]) + 18 > window_width)
                        {
                            DrawTail(0, Canvas.GetTop(arr_rec[arr_rec.Count - 1]));
                        }
                        else
                        {
                            DrawTail(Canvas.GetLeft(arr_rec[arr_rec.Count - 1]) + box, Canvas.GetTop(arr_rec[arr_rec.Count - 1]));
                        }
                        break;
                    case 3:
                        lastdirection = 3;
                        //Canvas.SetLeft(snake_head, y_tail[0] - box);
                        if (Canvas.GetLeft(arr_rec[arr_rec.Count - 1]) - box < 0)
                        {
                            DrawTail(window_width - box, Canvas.GetTop(arr_rec[arr_rec.Count - 1]));
                        }
                        else
                        {
                            DrawTail(Canvas.GetLeft(arr_rec[arr_rec.Count - 1]) - box, Canvas.GetTop(arr_rec[arr_rec.Count - 1]));
                        }
                        break;
                    default:
                        break;
                }

                if (this.food.Count() == 0)
                {
                    this.GenerateFood();
                }

                if (this.CheckCollision())
                {
                    this.AteAnApple();
                }

                if (this.CheckCrush())
                {
                    init();
                }
            }));
        }

        // da rifare

        private void init()
        {
            snakeLength = 5;

            direction = 2;
            lastdirection = 2;

            foreach (Rectangle rec in arr_rec) {
                this.paintCanvas.Children.Remove(rec);
            }

            arr_rec.Clear();

            x_tail.Clear();
            y_tail.Clear();

            double[] x = {48, 60, 72, 84, 96};
            x_tail.AddRange(x);

            double[] y = {48, 48, 48, 48, 48};
            y_tail.AddRange(y);

            for (int i = 0;i<x_tail.Count;i++)
            {
                Rectangle tail;

                tail = new Rectangle();
                if (i == x_tail.Count - 1)
                {
                    tail.Fill = Brushes.Cyan;
                }
                else if (i == 0)
                {
                    tail.Fill = Brushes.DarkBlue;
                }
                else
                {
                    tail.Fill = Brushes.Pink;
                }
                tail.Width = box;
                tail.Height = box;
                tail.Stroke = Brushes.Red;

                tail.StrokeThickness = 1; //1

                Canvas.SetTop(tail, y_tail[i]);
                Canvas.SetLeft(tail, x_tail[i]);

                arr_rec.Add(tail);

                this.paintCanvas.Children.Add(tail);
            }




        }

        private void AteAnApple()
        {
            this.numofapples--;
            this.snakeLength++;
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsDown)
            {
                switch (e.Key)
                {
                    case Key.Up:
                        if (lastdirection != 1)
                        {
                            direction = 0;
                        }
                        break;
                    case Key.Down:
                        if (lastdirection != 0)
                        {
                            direction = 1;
                        }
                        break;
                    case Key.Right:
                        if (lastdirection != 3)
                        {
                            direction = 2;
                        }
                        break;
                    case Key.Left:
                        if (lastdirection != 2)
                        {
                            direction = 3;
                        }
                        break;
                    default:
                        direction = 1;
                        break;
                }
            }
        }

        private void DrawTail(double x, double y) 
        {

            x_tail.Add(x);
            y_tail.Add(y);

            if (add_tail)
            {
                Rectangle tail;

                tail = new Rectangle();
                tail.Fill = Brushes.Cyan;
                tail.Width = box;
                tail.Height = box;
                tail.Stroke = Brushes.Red;

                tail.StrokeThickness = 1; //1

                Canvas.SetTop(tail, y_tail[0]);
                Canvas.SetLeft(tail, x_tail[0]);

                arr_rec.Add(tail);

                this.paintCanvas.Children.Add(tail);

                add_tail = false;

                arr_rec[arr_rec.Count-2].Fill = Brushes.Pink;
            }
            else
            {
                x_tail.RemoveAt(0);
                y_tail.RemoveAt(0);
            }

            for (int i = 0; i < x_tail.Count; i++)
            {
                Canvas.SetTop(arr_rec[i], y_tail[i]);
                Canvas.SetLeft(arr_rec[i], x_tail[i]);
            }
        }

        private bool CheckCollision()
        {
            int i = 0;
            foreach (Ellipse rec in this.food.Values)
            {
                if (MainWindow.CheckCollision(apple, arr_rec[arr_rec.Count-1]))
                {
                    this.paintCanvas.Children.Remove(apple);
                    this.food.Remove(i);
                    add_tail = true;
                    return true;
                }
                i++;
            }
            return false; 
        }

        public static bool CheckCollision(Ellipse e1, Rectangle e2)
        {
            var r1 = e1.ActualWidth / 2;
            var x1 = Canvas.GetLeft(e1) + r1;
            var y1 = Canvas.GetTop(e1) + r1;
            var r2 = e2.ActualWidth / 2;
            var x2 = Canvas.GetLeft(e2) + r2;
            var y2 = Canvas.GetTop(e2) + r2;
            var d = new Vector(x2 - x1, y2 - y1);
            return d.Length <= r1 + r2;
        }

        private bool CheckCrush()
        {
            int i = 0;
            foreach (Rectangle rec in arr_rec)
            {
                if (i != arr_rec.Count - 1) {
                    if (Canvas.GetTop(arr_rec[arr_rec.Count - 1]) == Canvas.GetTop(rec) && Canvas.GetLeft(arr_rec[arr_rec.Count - 1]) == Canvas.GetLeft(rec))
                    {
                        
                        return true;
                    }
                }
                i++;
            }
            return false;
        }

        private void GenerateFood()
        {
            int x = 1 + 12*this.rnd.Next(this.window_width/12);
            int y = 1 + 24 + 12 * this.rnd.Next(this.window_height / 12);//this.rnd.Next(10, this.window_height-20);
            this.apple = new Ellipse();

            this.apple.Height = 10;
            this.apple.Width = 10;
            this.apple.Fill = Brushes.LimeGreen;
            this.apple.Tag = new KeyValuePair<int, int>(x, y);

            this.food.Add(this.food.Count(), this.apple);

            Canvas.SetTop(this.apple, y);
            Canvas.SetLeft(this.apple, x);

            this.paintCanvas.Children.Add(this.apple);
            this.numofapples = this.food.Count();
        }

        private void LevelUp() { }
    }
}
