---
_layout: landing
---

# Components
The subtitling system is divided into three major components:

### Subtitler
This is the brains of the subtitling empire. All subtitling UI and playing logic runs through this component. If you wish to use your own underlying system for playing Subtitles, reference this using the singleton `Subtitler.Instance();`.

### SubtitleContainers
This is a component representing a single Subtitle source in the world. It does not contain any logic, only a reference an Subtitle Sequence/Entry.

### Data
This is a data file containing a single sequence of closed-captions. If you have 2023.3 or newer, a fancy custom editor table is exposed for you to use while editing them.




