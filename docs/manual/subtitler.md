---
_layout: landing
---

# Subtitler
The subtitling system works using three major components:

### Subtitler
This is the brains of the subtitling empire. All subtitling display and playing logic runs through it. 

### SubtitleContainer (Player)
This is a component representing a single Subtitle source in the world. It contains link to subtitle entries data and (either automatically or thru APIs) plays them using Subtitler. If you wish to play subtitles from custom scripts, you can use this script as API example for the Subtitler.  

### Data
This is a data file containing a single sequence of closed-captions. If you have 2023.3 or newer, a fancy custom editor table is exposed for you to use while editing them. 


# Subtitler Prefab
In order to use Subtitler, you need to drag the Subtitler Prefab from the package into the scene. Alternatively, you can copy the prefab from the provided Samples.

The Subtitler component exposes some commonly changed properties such as font-size, background visibility and text alignment. If you wish to change any other visual property, you can do so by modifying the underlying objects in the canvas.

