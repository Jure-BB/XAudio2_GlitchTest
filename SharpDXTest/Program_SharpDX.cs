using System;
using System.IO;
using SharpDX;
using SharpDX.XAudio2;
using SharpDX.Multimedia;
using System.Collections.Generic;
using System.Runtime;
using System.Diagnostics;
using CommonTools;

namespace SharpDXTest
{
    class Program_SharpDX
    {
        static void Main(string[] args)
        {
            Tools.ShowHelp("SharpDX");

            // Create XAudio Engine
            var xAudio = new XAudio2(XAudio2Flags.DebugEngine, ProcessorSpecifier.DefaultProcessor);
            // Create mastering voice
            var masteringVoice = new MasteringVoice(xAudio);

            // Generate audio data
            var sampleRate = 48000;
            var channels = 2;
            var sineWave = AudioGen.CreateSineWave(TimeSpan.FromMinutes(3), sampleRate, channels, 220, 0.25);
            var stream = DataStream.Create(sineWave, true, true);
            var buffer = new AudioBuffer
            {
                Stream = stream,
                AudioBytes = (int)stream.Length,
                Flags = BufferFlags.EndOfStream
            };

            // Create source voice
            var waveFormat = new WaveFormat(sampleRate, 16, channels);
            var sourceVoice = new SourceVoice(xAudio, waveFormat, true);
            sourceVoice.BufferStart += SourceVoice_BufferStart;

            // Start playing audio
            sourceVoice.SubmitSourceBuffer(buffer, null);
            sourceVoice.Start();
            Debug.WriteLine("Playing audio...");
            Console.WriteLine("Playing audio...");

            // Set GC latency mode and initialize some garbage
            Tools.SetGCLatencyMode();
            Tools.CreateGarbage();

            // Output glitches count
            Func<int> getGlichesCount = () =>
            {
                var perf = xAudio.PerformanceData;
                return perf.GlitchesSinceEngineStarted;
            };
            Console.WriteLine($"Glitches since engine started: {getGlichesCount()}");

            // Start loop
            Tools.StartLoop(getGlichesCount);
        }

        private static void SourceVoice_BufferStart(IntPtr obj)
        {
            Console.WriteLine();
            Console.WriteLine("Callback successful!");
            Console.WriteLine();
        }
    }
}
