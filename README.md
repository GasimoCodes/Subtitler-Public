<img align="right" src="https://github.com/user-attachments/assets/f5efff1d-d151-42dc-a78f-31ec8d7fbd06" alt="Subtitler img" height="250"/>


### Subtitler
 A simple to use Subtitling/Closed-Captions solution for Unity3D inspired by Source Engine Closed Captions. Currently under active development. 

#### [Docs](https://gasimo.dev/Subtitler/manual/gettingstarted.html?tabs=newer)

<br>
<br>
<br>
<br>


![image](https://github.com/user-attachments/assets/a93e090a-3c04-4c05-a334-c8f998d59b0d)
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
- Unity Localization support
- SRT Files support


## [Quick Start](https://gasimocodes.github.io/Subtitler/manual/gettingstarted.html?tabs=newer)

1. Import Subtitler `https://github.com/GasimoCodes/Subtitler.git?path=com.gasimo.subtitler` into **Package Manager** 
  
2. To see **samples**, open Package Manager, navigate to Subtitler. Press Samples and select import. Sample will automatically import into *Assets/Samples/Subtitler*

For usage see [Docs](https://gasimo.dev/Subtitler/manual/gettingstarted.html?tabs=newer)


## Dependencies
- **Unity 2022 or Newer** (Lower versions werent tested, should work on any version with proper UIToolkit support)
