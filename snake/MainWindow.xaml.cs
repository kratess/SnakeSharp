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

        public MainWindow()
        {
            InitializeComponent();
            this.time_multiplier = 10;
            this.rnd = new Random();
            this.food = new Dictionary<int, Ellipse>();
            this.time_to_add_more_food = this.rnd.Next(10, 1000);
            this.window_height = (Int32)this.Height;
            this.window_width = (Int32)this.Width;
            this.food_label.Content = this.food.Count().ToString();
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);

            this.snake_head = new Ellipse();
            this.snake_head.Fill = Brushes.Pink;
            this.snake_head.Width = 15;
            this.snake_head.Height = 15;
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

                this.tick.Start();
            }));
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

        private void DrawTail() { }

        private void GenerateFood()
        {
            int x = this.rnd.Next(1, this.window_width);
            int y = this.rnd.Next(1, this.window_height);
            this.apple = new Ellipse();

            this.apple.Height = 10;
            this.apple.Width = 10;
            this.apple.Fill = Brushes.LimeGreen;

            this.food.Add(this.food.Count()+1, this.apple);

            Canvas.SetTop(this.apple, y);
            Canvas.SetLeft(this.apple, x);

            this.paintCanvas.Children.Add(this.apple);
            this.food_label.Content = this.food.Count().ToString();
        }

        private void LevelUp() { }
    }
}
