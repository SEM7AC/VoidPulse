# VoidPulse

A minimal WPF music player that streams tracks directly from Azure Blob Storage. Built with a dark, compact UI and an animated visualizer.

---

## Overview

VoidPulse connects to a private Azure Blob Storage container via SAS URL, enumerates the available tracks on load, selects one at random, downloads it as a stream, and plays it through the local audio device — all without requiring a backend server or local file library.

---

## Features

- **Azure Blob streaming** — tracks are loaded and downloaded on demand from Azure Blob Storage via SAS-authenticated `BlobContainerClient`
- **Random track selection** — on launch, a random track is picked from the full blob listing
- **Animated visualizer** — 40-bar canvas animation driven by `CompositionTarget.Rendering`
- **Compact dark UI** — 300×160px fixed-size window, `#0F1A2B` background, Showcard Gothic title font, track and artist data bindings
- **Playback controls** — play, forward, and rewind buttons (in progress)

---

## Architecture

```
VoidPulse/
├── App.xaml                  # Application entry point
├── MainWindow.xaml           # UI layout — visualizer canvas, controls, track bindings
├── MainWindow.xaml.cs        # Startup logic, visualizer rendering loop, control handlers
├── StorageHelper.cs          # Azure Blob Storage integration — load, select, download
├── PlayerHelper.cs           # Audio playback (in progress)
└── VoidPulse.csproj
```

**Startup flow:**

```
App launch
  → LoadTracksAsync()       # Enumerate blobs from container via SAS URL
  → GetRandomTrack()        # Pick random track name from list
  → DownloadTrackAsync()    # Download blob to MemoryStream
  → player.Play(stream)     # Stream to audio output
  + StartVisualizer()       # Begin CompositionTarget.Rendering loop
```

---

## Tech Stack

| Layer        | Technology                        |
|--------------|-----------------------------------|
| Framework    | WPF (.NET)                        |
| UI           | XAML — fixed 300×160px window     |
| Storage      | Azure Blob Storage (SAS URL)      |
| Blob client  | Azure.Storage.Blobs               |
| Visualizer   | WPF Canvas + CompositionTarget    |
| Auth (planned) | Microsoft.Identity.Client (MSAL)|

---

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- An Azure Blob Storage account with a container of audio tracks
- A valid SAS URL with `read` and `list` permissions on the container

### 1. Clone the repository

```bash
git clone https://github.com/SEM7AC/voidPulse.git
cd voidPulse
```

### 2. Configure your SAS URL

In `StorageHelper.cs`, replace the `sasUrl` field value with your own SAS URL:

```csharp
private string sasUrl = @"https://<your-account>.blob.core.windows.net/<container>?<sas-token>";
```

### 3. Run

```bash
dotnet run
```

---

## Status

| Component          | Status        |
|--------------------|---------------|
| Blob enumeration   | Complete      |
| Random track select| Complete      |
| Stream download    | Complete      |
| Visualizer         | Complete      |
| Playback controls  | In progress   |
| MSAL auth          | Planned       |
