using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CommonTools;

namespace Xaudio2Test
{
    class Program_NativeWrapper
    {
        const int XAUDIO2_DEBUG_ENGINE = 0x0001;
       
        static void Main(string[] args)
        {
            Tools.ShowHelp("Native Wrapper");

            // Create XAudio Engine
            var pXAudio2 = XAudio29.XAudio2Create(XAUDIO2_DEBUG_ENGINE, XAUDIO2_PROCESSOR.XAUDIO2_DEFAULT_PROCESSOR);

            // Create mastering voice
            var pMasteringVoice = XAudio29.CreateMasteringVoice(pXAudio2);

            // Generate audio data
            var sampleRate = 48000;
            ushort channels = 2;
            var sineWave = AudioGen.CreateByteArraySineWave(TimeSpan.FromMinutes(3), sampleRate, channels, 220, 0.25);

            // Set callback
            XAudio29.SourceVoiceCallbackDelegate callback = CallbackTest;
            XAudio29.SetSourceVoiceCallbackDelegate(callback);

            // Create source voice
            var pSourceVoice = XAudio29.CreateSourceVoice(pXAudio2, sineWave, channels, sampleRate);

            // Start playing audio
            XAudio29.PlaySourceVoice(pSourceVoice);
            Debug.WriteLine("Playing audio...");
            Debug.WriteLine("Playing audio...");
            Console.WriteLine("Playing audio...");

            // Set GC latency mode and initialize some garbage
            Tools.SetGCLatencyMode();
            Tools.CreateGarbage();

            // Output glitches count
            Func<int> getGlichesCount = () => XAudio29.GetGlitchesCount(pXAudio2);
            Console.WriteLine($"Glitches since engine started: {getGlichesCount()}");

            // Start loop
            Tools.StartLoop(getGlichesCount);

            XAudio29.Destroy(pXAudio2, pMasteringVoice, pSourceVoice);
        }

        static void CallbackTest()
        {
            Console.WriteLine();
            Console.WriteLine("Callback successful!");
            Console.WriteLine();
        }
    }
}
