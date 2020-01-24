using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Xaudio2Test
{
     internal static unsafe class XAudio29
    {
        public static IntPtr XAudio2Create(int flags, XAUDIO2_PROCESSOR processorSpecifier)
        {
            var nativePtr = IntPtr.Zero;
            Result result = XAudio2Create(&nativePtr, flags, (int)processorSpecifier);
            result.CheckError();
            return nativePtr;
        }

        public static IntPtr CreateMasteringVoice(IntPtr xAudio2)
        {
            var nativePtr = IntPtr.Zero;
            Result result = CreateMasteringVoice(xAudio2, &nativePtr);
            result.CheckError();
            return nativePtr;
        }

        public static unsafe IntPtr CreateSourceVoice(IntPtr xAudio2, byte[] audioData, ushort channels, int sampleRate)
        {
            var handle = GCHandle.Alloc(audioData, GCHandleType.Pinned);
            var pData = (byte*)handle.AddrOfPinnedObject();

            var nativePtr = IntPtr.Zero;
            Result result = CreateSourceVoice(xAudio2, pData, audioData.Length, channels, sampleRate, &nativePtr);
            result.CheckError();
            return nativePtr;
        }

        public static void PlaySourceVoice(IntPtr inSourceVoice)
        {
            Result result = playSourceVoice(inSourceVoice);
            result.CheckError();
        }

        public static void Destroy(IntPtr xAudio2, IntPtr masterVoice, IntPtr sourceVoice)
        {
            Result result = ShutDown(xAudio2, masterVoice, sourceVoice);
            result.CheckError();
        }

        [DllImport("xaudio2_9.dll", EntryPoint = "XAudio2Create", CallingConvention = CallingConvention.StdCall)]
        private unsafe static extern int XAudio2Create(void* outIXAudio2, int flags, int processorSpecifier);

        [DllImport("XAudioWrapper.dll", EntryPoint = "CreateMasteringVoice", CallingConvention = CallingConvention.StdCall)]
        private static unsafe extern int CreateMasteringVoice(
            IntPtr inIXAudio2, 
            void* outIXAudio2MasteringVoice);//, 
            //int InputChannels, 
            //int InputSampleRate, 
            //int Flags, 
            //string szDeviceId, 
            //void* pEffectChain, 
            //int /*enum*/ StreamCategory);

        [DllImport("XAudioWrapper.dll", EntryPoint = "CreateSourceVoice", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static unsafe extern int CreateSourceVoice(
            IntPtr inIXAudio2,
            byte* audioData,
            int audioBytes,
            ushort channels,
            int sampleRate,
            void* outpSourceVoice);
        
        [DllImport("XAudioWrapper.dll", EntryPoint = "PlaySourceVoice", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static unsafe extern int playSourceVoice(IntPtr inSourceVoice);

        [DllImport("XAudioWrapper.dll", EntryPoint = "ShutDown", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern int ShutDown(
            IntPtr inIXAudio2,
            IntPtr inMasterVoice,
            IntPtr inSourceVoice);

        [DllImport("XAudioWrapper.dll", EntryPoint = "GetGlitchesCount", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int GetGlitchesCount(IntPtr inIXAudio2);
    }
}
