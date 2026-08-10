using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VoidPulse.Audio;

namespace VoidPulse;

public partial class MainWindow : Window
    {
    private StorageHelper _storage;
    private PlayerHelper _player;
    private TrackLoader _trackLoader;
    private readonly Random _rand = new Random();

    private readonly Visualizer.VisualizerEngine _engine = new();
    private readonly Visualizer.VisualizerRenderer _renderer = new();


    private bool _isPlaying = false;

    public MainWindow()
        {
        InitializeComponent();
        _storage = new StorageHelper();
        _player = new PlayerHelper(_storage);
        _trackLoader = new TrackLoader(_storage, _player);

        Loaded += MainWindow_Loaded;
        Loaded += (s, e) => StartVisualizer();
        }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
        await _trackLoader.LoadRandomTrackAsync();
        }

    // WINDOW CONTROLS ----------------------------------------------------
    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
        }

    private void Close_Click(object sender, RoutedEventArgs e)
        {
        Close();
        }

    private void Minimize_Click(object sender, RoutedEventArgs e)
        {
        WindowState = WindowState.Minimized;
        }
    //---------------------------------------------------------------------


    private void StartVisualizer()
        {
        CompositionTarget.Rendering += (s, e) =>
        {
            int barCount = 40;
            double height = VisualizerCanvas.ActualHeight;

            var bars = _engine.GenerateBars(barCount, height);
            _renderer.Render(VisualizerCanvas, bars);
        };
        }


    // BUTTON GROUP ------------------------------------------------

    private void Play_Button_Click(object? sender, RoutedEventArgs e)
        {
        if (sender is not Button btn)
            return;

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

    private async void Fwd_Button_Click(object? sender, RoutedEventArgs e)
        {
        await _player.NextTrack(_isPlaying);
        }

    private async void Prev_Button_Click(object? sender, RoutedEventArgs e)
        {
        await _player.PrevTrack(_isPlaying);
        }

    private void Stop_Button_Click(object? sender, RoutedEventArgs e)
        {
        _player.KillEvent();
        _player.KillReader();

        _isPlaying = false;
        PlayButton.Content = "⏵";
        }
    }
