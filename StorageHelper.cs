using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VoidPulse;

internal class StorageHelper
    {

    private BlobContainerClient _container;
    private List<string> _tracks;
    private Random _rand;
    



    public StorageHelper() 
        {
        _tracks = new List<string>();
        _rand = new Random();
       

        var builder = new ConfigurationBuilder()
        .AddUserSecrets<StorageHelper>()
        .Build();

        string sasUrl = builder["Blob:SasUrl"] ?? throw new InvalidOperationException("Missing Blob:SasUrl");
        
        _container = new BlobContainerClient(new Uri(sasUrl));


        }

    public async Task LoadTracksAsync()
        {
        _tracks.Clear();
        
        await foreach (BlobItem blob in _container.GetBlobsAsync())
            {
            if (blob.Name.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                {
                _tracks.Add(blob.Name);
                }
            }
        }

    public string GetRandomTrack()
        {
        if (_tracks.Count == 0)
            throw new InvalidOperationException("No tracks loaded.");
        else
            return _tracks[_rand.Next(_tracks.Count)];
        }
    public async Task<Stream> DownloadTrackAsync(string trackName)
        {
        BlobClient _client = _container.GetBlobClient(trackName);
        MemoryStream _stream = new MemoryStream();
        await _client.DownloadToAsync(_stream);
        _stream.Position = 0;

        return _stream;
        }


    }

