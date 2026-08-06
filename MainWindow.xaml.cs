using Microsoft.Identity.Client.Extensions.Msal;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VoidPulse;

public partial class MainWindow : Window
    {
    private StorageHelper _storage;
    private PlayerHelper _player;
    private readonly Random _rand = new Random();
    private bool _isPlaying = false;

    public MainWindow()
        {
        InitializeComponent();
        _storage = new StorageHelper();
        _player = new PlayerHelper(_storage);

        Loaded += MainWindow_Loaded;
        Loaded += (s, e) => StartVisualizer();
        }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
        await _storage.LoadTracksAsync();

        string track = _storage.GetRandomTrack();
        var stream = await _storage.DownloadTrackAsync(track);
        _player.LoadTrack(stream);
        _player.SetCurrentTrack(track);


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
                    Fill = new SolidColorBrush(Color.FromRgb(0x2A, 0x7F, 0x6A)),
                    Opacity = 0.85
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
        var btn = sender as Button;

        if (!_isPlaying)
            {
            _isPlaying = true;
            btn.Content = "⏸";

            if (_player.IsStopped)
                _player.ReloadTrack();

            _player.Play();
            }
        else
            {
            _isPlaying = false;
            btn.Content = "⏵";
            _player.Pause();
            }
        }

    private async void Fwd_Button_Click(object sender, RoutedEventArgs e)
        {
        await _player.NextTrack(_isPlaying);
        }

    private async void Prev_Button_Click(object sender, RoutedEventArgs e)
        {
        await _player.PrevTrack(_isPlaying);
        }

    private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
        _player.KillEvent();
        _player.KillReader();

        _isPlaying = false;
        PlayButton.Content = "⏵";
        }
    }
