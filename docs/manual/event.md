---
_layout: landing
---
# Events
Subtitler allows you to subscribe simple C# events to subtitle entries. These events are fired when the corresponding entry is displayed.

## ScriptableEvent
A [ScriptableObject](https://docs.unity3d.com/Manual/class-ScriptableObject.html) file. Can be created using the *Gasimo/ScriptableEvent* context menu. 
You can subscribe to the ScriptableEvent by calling 

```csharp
onEventRaised += (your_action);
```

