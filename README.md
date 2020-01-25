# XAudio2 Glitch Test
Audio playback glitching test of different XAudio2 wrappers for .NET. 

WARNING: Test programs allocate ~5GB of memory!

##### XAudio wrappers tested:

- [SharpDX](https://github.com/sharpdx/SharpDX)
- [Vortice](https://github.com/amerkoleci/Vortice.Windows)
- Custom C style wrapper with C# interop

XAudio2 engine creates its own native thread. When using SharpDX or Vortice wrappers garbage collection is blocking XAudio2 thread and causing audio playback glitches. This might be due to the way they register for callbacks by default.

Custom C style wrapper with C# interop is is just a small .NET XAudio2 wrapper made for test purposes. Test using this wrapper is not affected by GC. Again, this may be because in this case we are not registering for callbacks. Note, that I don't have any background in C++, so this wrapper is probably full of bugs. 

Make sure to run tests in a 'Release' mode and without VS debugger attached. All 3 tests are playing a basic sine wave. The easiest way to test, if XAudio thread is being blocked, is to press H to run full blocking garbage collection and hear if sound output being blocked. 
