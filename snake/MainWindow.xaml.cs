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
        private Ellipse snake_head { get; set; }
        private Ellipse apple { get; set; }
        

        private Timer tick { get; set; }
        private Random rnd;
        private Dictionary<int, Ellipse> food { get; set; }

        private int direction = 1;
        private int speed_multiplier = 1;
        private int time_multiplier = 1;
        private int tick_counter = 0;
        private int time_to_add_more_food = 0;
        private int max_num_of_food = 5;
        private int window_width;
        private int window_height;

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

            this.time_multiplier = 10;
            this.rnd = new Random();
            this.food = new Dictionary<int, Ellipse>();
            this.time_to_add_more_food = this.rnd.Next(10, 1000);
            this.window_height = (Int32)this.Height;
            this.window_width = (Int32)this.Width;
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);

            this.snake_head = new Ellipse();
            this.snake_head.Fill = Brushes.Pink;
            this.snake_head.Width = 16;
            this.snake_head.Height = 16;
            this.snake_head.Stroke = Brushes.Red;
            this.snake_head.StrokeThickness = 1;

            Canvas.SetTop(this.snake_head, 20);
            Canvas.SetLeft(this.snake_head, 20);
            this.paintCanvas.Children.Add(this.snake_head);

            this.tick = new Timer();
            this.tick.Interval = 100 / this.time_multiplier;
            this.tick.Elapsed += tick_Elapsed;
            this.tick.Start();
        }

        private void tick_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.tick.Stop();
            this.tick.Interval = 100 / this.time_multiplier;

            this.Dispatcher.Invoke((Action)(() =>
            {
                double top = Canvas.GetTop(this.snake_head);
                double left = Canvas.GetLeft(this.snake_head);

                switch (direction)
                {
                    case 0:
                        Canvas.SetTop(snake_head, top - 1 * speed_multiplier);
                        break;
                    case 1:
                        Canvas.SetTop(snake_head, top + 1 * speed_multiplier);
                        break;
                    case 2:
                        Canvas.SetLeft(snake_head, left + 1 * speed_multiplier);
                        break;
                    case 3:
                        Canvas.SetLeft(snake_head, left - 1 * speed_multiplier);
                        break;
                    default:
                        Canvas.SetTop(snake_head, top + 1 * speed_multiplier);
                        break;
                }

                this.tick_counter++;

                if ((this.tick_counter == this.time_to_add_more_food && this.food.Count() < this.max_num_of_food) || this.food.Count() < 1)
                    this.GenerateFood();

                if (this.CheckCollision())
                    this.AteAnApple();
                   

                this.tick.Start();
            }));
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
                    case Key.W:
                        direction = 0;
                        break;
                    case Key.S:
                        direction = 1;
                        break;
                    case Key.D:
                        direction = 2;
                        break;
                    case Key.A:
                        direction = 3;
                        break;
                    default:
                        direction = 1;
                        break;
                }
            }
        }

        private void DrawHead() { }

        private void CalculateTailPosition() { }

        private void DrawTail() 
        { 
            
        }

        private bool CheckCollision()
        {
            int i = 0;
            foreach (Ellipse apple in this.food.Values)
            {
                if (MainWindow.CheckCollision(apple, this.snake_head))
                {
                    this.paintCanvas.Children.Remove(apple);
                    this.food.Remove(i);
                    return true;
                }
                i++;
            }
            return false; 
        }

        public static bool CheckCollision(Ellipse e1, Ellipse e2)
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

        private void GenerateFood()
        {
            int x = this.rnd.Next(10, this.window_width-20);
            int y = this.rnd.Next(10, this.window_height-20);
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
