using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime;
using System.Diagnostics;
using Vortice;
using Vortice.Multimedia;
using Vortice.XAudio2;
using CommonTools;

namespace VorticeTest
{
    class Program_Vortice
    {
        static void Main(string[] args)
        {
            Tools.ShowHelp("Vortice");

            // Create XAudio Engine
            var xAudio = new IXAudio2(XAudio2Flags.DebugEngine, ProcessorSpecifier.DefaultProcessor);
            // Create mastering voice
            var masteringVoice = xAudio.CreateMasteringVoice();

            // Generate audio data
            var sampleRate = 48000;
            var channels = 2;
            var sineWave = AudioGen.CreateByteArraySineWave(TimeSpan.FromMinutes(3), sampleRate, channels, 220, 0.25);
            var buffer = new AudioBuffer(sineWave);

            // Create source voice
            var waveFormat = new WaveFormat(sampleRate, 16, channels);
            var sourceVoice = xAudio.CreateSourceVoice(waveFormat);

            // Start playing audio
            sourceVoice.SubmitSourceBuffer(buffer);
            sourceVoice.Start();
            Debug.WriteLine("Playing audio...");
            Console.WriteLine("Playing audio...");

            // Set GC latency mode and initialize some garbage
            Tools.SetGCLatencyMode();
            Tools.CreateGarbage();

            // Output glitches count
            Func<int> getGlichesCount = () => { 
                var perf = xAudio.PerformanceData;
                return perf.GlitchesSinceEngineStarted;
                };
            Console.WriteLine($"Glitches since engine started: {getGlichesCount()}");

            // Start loop
            Tools.StartLoop(getGlichesCount);
        }

        
        
    }
}
