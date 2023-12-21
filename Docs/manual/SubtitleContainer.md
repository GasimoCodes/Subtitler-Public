---
_layout: landing
---

# Welcome to **Subtitler** docs!

Subtitler is a simple to use Subtitling/Closed-Captions solution for Unity3D. Its currently under active development but is completely usable. Report all bugs or feature requests [here](https://github.com/GasimoCodes/Subtitler/issues/new/choose).


## Quick Start:
Recommended Unity version (as of now) is 2023.3 due to a [bug in UIToolkit](https://forum.unity.com/threads/cant-bind-multicolumnlistview-to-property.1425945/), which is yet to be backported. Minimum supported version is 2022, but it doesnt feature custom editor for easier subtitle editing. 

### UPM:

1. Add `https://github.com/GasimoCodes/Subtitler.git?path=com.gasimo.subtitler` into **Package Manager** 
(*Package Manager/+/Install package from GIT URL*)

3. If you dont have [DOTween](https://dotween.demigiant.com/download.php) in your project, import it.

3. If you are on 2023.1 or below, add TextMeshPro package in your Package Manager. For 2023.2 onwards, Unity merged TextMeshPro with Unity Core, nothing needs to be done. 

4. To see **samples**, open Package Manager, navigate to Subtitler. Press Samples and select import. Sample will automatically import into *Assets/Samples/Subtitler*

  
  

## Features

- Cull subtitles based on Player Distance / Audio Volume
- Define dialogue entries with variable timing offsets and display lengths
- Assign AudioClip to each dialogue entry, or use one big clip for whole dialogue instead
- Rich Text Support (uses TextMeshPro)
- UI Options (Alignment, background panel visibility, Font size)
- Trigger events (ScriptableEvent) when a certain line gets played
- Editor Subtitle Authoring tool
- API to hook your custom subtitles scripts or logic
- API to stop currently playing subtitles using their runtime ID.
  

## Planned Features

- Optional Localization Package Support

- Optional Addressables for audioClips support

- Timeline support