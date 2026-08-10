using System.Threading.Tasks;

namespace VoidPulse.Audio;

public class TrackLoader
    {
    private readonly StorageHelper _storage;
    private readonly PlayerHelper _player;

    public TrackLoader(StorageHelper storage, PlayerHelper player)
        {
        _storage = storage;
        _player = player;
        }

    public async Task LoadRandomTrackAsync()
        {

        await _storage.LoadTracksAsync();

        string track = _storage.GetRandomTrack();
        var stream = await _storage.DownloadTrackAsync(track);

        _player.LoadTrack(stream);
        _player.SetCurrentTrack(track);
        }
    }

