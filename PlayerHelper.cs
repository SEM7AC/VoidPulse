using NAudio.Wave;
using System.IO;

namespace VoidPulse;

internal class PlayerHelper
    {
    private WaveOutEvent? _event;
    private Mp3FileReader? _reader;
    private StorageHelper _storage;
    private MemoryStream? _currentStream;

    private readonly Stack<string> _history = new();
    private string? _currentTrack;

    public bool IsStopped => _event == null || _reader == null;
    
    public PlayerHelper(StorageHelper storage)
        {
        _storage = storage;
        }

    public void SetCurrentTrack(string track)
        {
        _currentTrack = track;
        }

    // Load a track into a rewindable MemoryStream
    public void LoadTrack(Stream trackstream)
        {
        KillEvent();
        KillReader();

        _currentStream = new MemoryStream();
        trackstream.CopyTo(_currentStream);
        _currentStream.Position = 0;

        _reader = new Mp3FileReader(_currentStream);
        _event = new WaveOutEvent();
        _event.Init(_reader);
        }

    // Reload the same track after STOP
    public void ReloadTrack()
        {
        if (_currentStream == null)
            return;

        KillEvent();
        KillReader();

        _currentStream.Position = 0;

        _reader = new Mp3FileReader(_currentStream);
        _event = new WaveOutEvent();
        _event.Init(_reader);
        }

    // Next track with autoplay support
    public async Task NextTrack(bool autoPlay)
        {
        KillEvent();
        KillReader();

        if (_currentTrack != null)
            {
            _history.Push(_currentTrack);
            }

        var trackName = _storage.GetRandomTrack();
        var stream = await _storage.DownloadTrackAsync(trackName);

        _currentTrack = trackName;
        LoadTrack(stream);

        if (autoPlay)
            Play();
        }

    public async Task PrevTrack(bool autoPlay)
        {
        KillEvent();
        KillReader();

        if (_history.Count == 0)
            return;

        var trackName = _history.Pop();
        var stream = await _storage.DownloadTrackAsync(trackName);

        _currentTrack = trackName;
        LoadTrack(stream);

        if (autoPlay)
            Play();
        }


    public void Play() => _event?.Play();
    public void Pause() => _event?.Pause();

    // Kill Methods for Event, Reader, and Stream and one for everything
    public void KillEvent()
        {
        if (_event != null)
            {
            _event.Stop();
            _event.Dispose();
            _event = null;
            }
        }
    public void KillReader()
        {

        if (_reader != null)
            {
            _reader.Dispose();
            _reader = null;
            }
        }
    private void KillStream()
        {
        if (_currentStream != null)
            {
            _currentStream.Dispose();
            _currentStream = null;
            }
        }
    private void KillAll()
        {
        KillEvent();
        KillReader();
        KillStream();
        }
        
    }
