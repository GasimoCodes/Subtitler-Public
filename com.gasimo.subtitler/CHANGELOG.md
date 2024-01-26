# Changelog
All notable changes to this package will be documented in this file.

## [0.1.4] - 26/01/2024
- Added Icon for ScriptableEvent
- Migrated ScriptableEvent to UnityAction instead of C# Delegates
- Added null check to ScriptableEvent to prevent exceptions
- Fixed build errors with Timeline due to Editor scripts not getting excluded


## [0.1.3] - 25/12/2023

### Timeline Support

- Added optional Timeline support (using #if TIMELINE_PRESENT)
- Separated subtitle sequences from entries

## [0.1.2] - 23/12/2023

### General Polish and Bugfixes

- Polished exit animations for the subtitles, which now smoothly push the background panel even on exit
- Minor cleanup in the code
- Added gizmos
- Added custom editor for the subtitle datas

## [0.1.1] - 12/12/2023

- Begin of changelog
