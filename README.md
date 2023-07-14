# Subtitler
 A simple subtitle tool for Unity3D inspired by Source Engine Closed Captions 

![Capture1](https://github.com/GasimoCodes/Subtitler/assets/22917863/65229daa-4547-4c9e-8277-c8cde53e8d0f)
![Capture2](https://github.com/GasimoCodes/Subtitler/assets/22917863/d1607038-3afd-45d1-968a-6507ebf08b3d)


## Features

 - [x] Stop subtitles when player goes far away from audioSource
 - [x] Define dialogue entries of variable timing and display length
 - [x] API to display subtitles from custom scripts (Not documented)
 - [x] Assign AudioClip to each dialogue entry or use one big clip for whole dialogue  
 - [x] Support for markdown (uses TextMeshPro) 
 - [x] Overrides (Alignment, background panel visibility, Font size)
 - [X] Play events when a specific subtitle line gets trigerred
 - [X] Exit the display loop of one of the playing subtitle files based on generated session id. 
 - [ ] Fancy Editor UI 

# Dependencies
**Cysharp UniTask** for proper Async support when exiting play mode.
**Demigiant DoTween** for performant tweening animations.
**TextMeshPro** for text display.
**Addressables** for loading subtitle files.
