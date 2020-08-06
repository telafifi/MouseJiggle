using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MouseJiggle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);
        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        public MainWindow()
        {
            InitializeComponent();
            startJiggle = false;
            timer = new Timer();
            timer.Interval = (int)(60 * 1000); //run this every minute
            timer.Elapsed += Timer_Elapsed;
            flip = 5;
        }

        Timer timer;
        bool startJiggle;
        int flip;

        /// <summary>
        /// Move the cursor by the flip amount specified every time the timer completes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Random random = new Random();
            //SetCursor(random.Next(-200, 200), random.Next(-200, 200));
            SetCursor(flip, flip);
            flip = -1 * flip;
        }

        /// <summary>
        /// Jiggle the mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJiggle_Click(object sender, RoutedEventArgs e)
        {
            if (startJiggle)
            {
                timer.Stop();
                startJiggle = false;
                btnJiggle.Content = "Start Moving Mouse";
            }
            else
            {
                timer.Start();
                startJiggle = true;
                btnJiggle.Content = "Stop Moving Mouse";
            }
            
        }

        /// <summary>
        /// Get the mouse position and move ita specific distance
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SetCursor(int x, int y)
        {
            Dispatcher.Invoke(() =>
            {
                Point mousePos = GetMousePosition();

                SetCursorPos(x + (int)mousePos.X, y + (int)mousePos.Y);
            });
        }

        /// <summary>
        /// Get the current mouse position
        /// </summary>
        /// <returns></returns>
        public static Point GetMousePosition()
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }
    }
}
