using Microsoft.Identity.Client.Extensions.Msal;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VoidPulse
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        {
        
        private StorageHelper _storage;
        private PlayerHelper _player;
        private readonly Random _rand = new Random();

        public MainWindow()
            {
            InitializeComponent();
            _storage = new StorageHelper();
            _player = new PlayerHelper();

            Loaded += MainWindow_Loaded;
            //Loaded += (s, e) => StartVisualizer();

            }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
            {
            await _storage.LoadTracksAsync();                 // 1. Load blob names
            string track = _storage.GetRandomTrack();         // 2. Pick random
            var stream = await _storage.DownloadTrackAsync(track); // 3. Download MP3
            _player.Play(stream);                             // 4. Play it
            }

        private void StartVisualizer()
            {
            CompositionTarget.Rendering += (s, e) =>
            {
                VisualizerCanvas.Children.Clear();

                int barCount = 40;
                double barWidth = VisualizerCanvas.ActualWidth / barCount;
                double height = VisualizerCanvas.ActualHeight;

                for (int i = 0; i < barCount; i++)
                    {
                    double barHeight = _rand.NextDouble() * height;

                    var rect = new System.Windows.Shapes.Rectangle
                        {
                        Width = barWidth - 2,
                        Height = barHeight,
                        Fill = Brushes.Lime,
                        Opacity = 0.6
                        };

                    Canvas.SetLeft(rect, i * barWidth);
                    Canvas.SetTop(rect, height - barHeight);

                    VisualizerCanvas.Children.Add(rect);
                    }
            };
            }

        // BUTTON GROUP ------------------------------------------------
        
        private void Play_Button_Click(object sender, RoutedEventArgs e)
            {

            }
        private void Fwd_Button_Click(object sender, RoutedEventArgs e)
            {

            }
        private void Rwd_Button_Click(object sender, RoutedEventArgs e)
            {

            }
        }
    }