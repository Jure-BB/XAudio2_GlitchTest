using System;
using System.Threading;
using CommonTools;
using SharpGen.Runtime;
using Vortice.XAudio2;

namespace VorticeTest
{
    public class XAudioVoiceCallback : IXAudio2VoiceCallback
    {
        private ShadowContainer _shadow;

        public ShadowContainer Shadow
        {
            get => Volatile.Read(ref _shadow);
            set
            {
                if (value != null)
                {
                    // Only set the shadow container if it is not already set.
                    if (Interlocked.CompareExchange(ref _shadow, value, null) != null)
                    {
                        value.Dispose();
                    }
                }
                else
                {
                    Interlocked.Exchange(ref _shadow, value)?.Dispose();
                }
            }
        }

        public void Dispose() { }

        public void OnBufferEnd(IntPtr context) => Tools.CallbacksCount++;

        public void OnBufferStart(IntPtr context) => Tools.CallbacksCount++;

        public void OnLoopEnd(IntPtr context) => Tools.CallbacksCount++;

        public void OnStreamEnd() => Tools.CallbacksCount++;

        public void OnVoiceError(IntPtr context, Result error) => Tools.CallbacksCount++;

        public void OnVoiceProcessingPassEnd() => Tools.CallbacksCount++;

        public void OnVoiceProcessingPassStart(int bytesRequired) => Tools.CallbacksCount++;
    }
}
