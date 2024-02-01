# Subtitler
 A simple to use Subtitling/Closed-Captions solution for Unity3D inspired by Source Engine Closed Captions. Currently under active development. 

#### [Docs](https://gasimo.dev/Subtitler/manual/gettingstarted.html?tabs=newer)

![Capture1](https://github.com/GasimoCodes/Subtitler/assets/22917863/65229daa-4547-4c9e-8277-c8cde53e8d0f)
![Capture2](https://github.com/GasimoCodes/Subtitler/assets/22917863/d1607038-3afd-45d1-968a-6507ebf08b3d)


## Features

- Cull subtitles based on Player Distance / Audio Volume
- Define dialogue entries with variable timing offsets and display lengths
- Assign AudioClip to each dialogue entry, or use one big clip for whole dialogue instead (useful for videos or heavily processed sounds which can't be split by lines)
- Rich Text Support, multiline support
- UI Options (Alignment, background panel visibility, Font size)
- Trigger events (ScriptableEvent) when a certain line gets played
- Fancy Editor Subtitle Authoring tool (>=2023.3, fallbacks to normal Inspector)
- API to hook your custom subtitles scripts or logic
- API to stop currently playing subtitles using their runtime ID
- Timeline Integration


## [Quick Start](https://gasimocodes.github.io/Subtitler/manual/gettingstarted.html?tabs=newer)

1. Import [Cysharp UniTask]((https://github.com/Cysharp/UniTask)) by adding `https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask` into **Package Manager** 
(*Package Manager/+/Install package from GIT URL*)

2. Import Subtitler `https://github.com/GasimoCodes/Subtitler.git?path=com.gasimo.subtitler` into **Package Manager** 
  
3. To see **samples**, open Package Manager, navigate to Subtitler. Press Samples and select import. Sample will automatically import into *Assets/Samples/Subtitler*

For usage see [Docs](https://gasimo.dev/Subtitler/manual/gettingstarted.html?tabs=newer)


## Dependencies
- **Cysharp UniTask** for proper Async support when exiting play mode.
- **Unity 2022 or Newer** (Lower versions werent tested, should work on any version with proper UIToolkit support)
