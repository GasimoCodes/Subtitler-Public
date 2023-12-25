# Subtitler
 A simple to use Subtitling/Closed-Captions solution for Unity3D inspired by Source Engine Closed Captions. Currently under active development. 

![Capture1](https://github.com/GasimoCodes/Subtitler/assets/22917863/65229daa-4547-4c9e-8277-c8cde53e8d0f)
![Capture2](https://github.com/GasimoCodes/Subtitler/assets/22917863/d1607038-3afd-45d1-968a-6507ebf08b3d)


## Features

- Cull subtitles based on Player Distance / Audio Volume
- Define dialogue entries with variable timing offsets and display lengths
- Assign AudioClip to each dialogue entry, or use one big clip for whole dialogue instead (useful with videos or heavily processed sounds which cant be split by lines)
- Rich Text Support (uses TextMeshPro)
- UI Options (Alignment, background panel visibility, Font size)
- Trigger events (ScriptableEvent) when a certain line gets played
- Editor Subtitle Authoring tool (>2023.3)
- API to hook your custom subtitles scripts or logic
- API to stop currently playing subtitles using their runtime ID.
- Timeline Integration
## [Quick Start](https://gasimocodes.github.io/Subtitler/manual/gettingstarted.html?tabs=newer)


## Dependencies
**Cysharp UniTask** for proper Async support when exiting play mode.
**Demigiant DoTween** for performant tweening animations.
**TextMeshPro** for text display.
**Addressables** for loading subtitle files.
