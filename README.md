# XAudio2 Glitch Test
Audio playback glitching test of different XAudio2 wrappers for .NET. 

WARNING: Test programs allocate almost 5GB of memory. To run these tests, a PC with at least 8GB of RAM is recommended. 

##### XAudio wrappers tested:

- [SharpDX](https://github.com/sharpdx/SharpDX)
- [Vortice](https://github.com/amerkoleci/Vortice.Windows)
- Custom C style wrapper

In SharpDX and Vortice tests garbage collection is blocking XAudio2 thread and causing audio playback glitches. Both are using [SharpGenTools](https://github.com/SharpGenTools/SharpGenTools) to automate wrapper creation, so this may be or may not be the source of the problem. Although, a background GC is blocking a lot less than the other GC methods, user threads are still being suspended under certain circumstances, which is enough to cause audio playback glitches. See [here](https://stackoverflow.com/a/2584658/500861), [here](https://mattwarren.org/2017/01/13/Analysing-Pause-times-in-the-.NET-GC/) or [here](https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals) for more details on background GC.

With custom C style wrapper audio playback is not blocked by GC. Note, that this wrapper is is just a quick test too see, if it is possible to have .NET XAudio2 wrapper that is not affected by GC. I'm not a C++ programmer, so it is probably full of bugs. 

Make sure to run tests in a 'Release' mode and without VS debugger attached.
