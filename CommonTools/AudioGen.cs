using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools
{
    public static class AudioGen
    {

        
        public static short[] CreateSineWave(TimeSpan length, int sampleRate, int channels = 2, int noteFrequency = 220, double amplitude = 0.25)
        {
            if (channels != 1 && channels != 2)
                throw new ArgumentException("Only 1 or 2 channels supported");
            short[] buffer = new short[(uint)(sampleRate * length.TotalSeconds) * channels];
            amplitude *= short.MaxValue;
            for (int i = 0; i < buffer.Length - 1; i += channels)
            {
                var sample = (short)(amplitude * Math.Sin((2 * Math.PI * i / channels * noteFrequency) / sampleRate));
                for (int j = 0; j < channels; j++)
                    buffer[i + j] = sample;
            }
            return buffer;
        }

        public static byte[] CreateByteArraySineWave(TimeSpan length, int sampleRate, int channels = 2, int noteFrequency = 220, double amplitude = 0.25)
        {
            var sineData = CreateSineWave(length, sampleRate, channels, noteFrequency, amplitude);
            byte[] result = new byte[sineData.Length * sizeof(short)];
            Buffer.BlockCopy(sineData, 0, result, 0, result.Length);
            return result;
        }
    }
}
