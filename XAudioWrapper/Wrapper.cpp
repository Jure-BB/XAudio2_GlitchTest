#include <windows.h>
#include "Wrapper.h"
#include <xaudio2.h>
#include <winerror.h>
#include <memory>
#include <string>
#include <conio.h>
#include "VoiceCallback.h"

XAUDIO2_BUFFER buffer = { 0 };
IXAudio2* pXAudio2;
IXAudio2MasteringVoice* pMasterVoice;
IXAudio2SourceVoice* pSourceVoice;
VoiceCallback* voiceCallback;


int CreateMasteringVoice(IXAudio2* pXAudio2, IXAudio2MasteringVoice** outpMasterVoice)
{
	int hr = 0;

	IXAudio2MasteringVoice* pMasterVoice = NULL;
	if (FAILED(hr = pXAudio2->CreateMasteringVoice(&pMasterVoice)))
		return hr;

	*outpMasterVoice = pMasterVoice;

	return hr;
}

HRESULT CreateSourceVoice(IXAudio2* pXaudio2, byte* audioData, int audioBytes, WORD channels, int sampleRate, IXAudio2SourceVoice** outpSourceVoice)
{
	int hr = 0;

	// Create XAUDIO2_BUFFER
	buffer = { 0 };
	buffer.pAudioData = audioData;
	buffer.Flags = XAUDIO2_END_OF_STREAM;  // tell the source voice not to expect any data after this buffer
	buffer.AudioBytes = audioBytes;

	// Create source voice
	WAVEFORMATEX waveFormat;
	waveFormat.wFormatTag = WAVE_FORMAT_PCM;
	waveFormat.nChannels = channels;
	waveFormat.nSamplesPerSec = sampleRate;
	waveFormat.nBlockAlign = (short)(channels * (16 / 8));;
	waveFormat.nAvgBytesPerSec = sampleRate * waveFormat.nBlockAlign;
	waveFormat.wBitsPerSample = 16;
	waveFormat.cbSize = 0;

	// Create VoiceCallback instance
	voiceCallback = new VoiceCallback{};
	
	IXAudio2SourceVoice* pSourceVoice;
	//if (FAILED(hr = pXaudio2->CreateSourceVoice(&pSourceVoice, &waveFormat)))
	if (FAILED(hr = pXaudio2->CreateSourceVoice(&pSourceVoice, &waveFormat, 0, XAUDIO2_DEFAULT_FREQ_RATIO, voiceCallback, NULL, NULL )))
	{
		return hr;
	}

	*outpSourceVoice = pSourceVoice;

	return hr;
}
HRESULT PlaySourceVoice(IXAudio2SourceVoice* pSourceVoice)
{
	int hr = 0;

	// Submit an XAUDIO2_BUFFER to the source voice
	if (FAILED(hr = pSourceVoice->SubmitSourceBuffer(&buffer)))
	{
		pSourceVoice->DestroyVoice();
		return hr;
	}

	// start playing
	if (FAILED(hr = pSourceVoice->Start(0)))
		return hr;

	//WaitForSingleObjectEx( voiceCallback->hBufferEndEvent, INFINITE, TRUE );

	return hr;
}
void ShutDown(IXAudio2* pXAudio2, IXAudio2MasteringVoice* pMasteringVoice, IXAudio2SourceVoice* pSourceVoice)
{
	pSourceVoice->DestroyVoice();
	pMasteringVoice->DestroyVoice();
	pXAudio2->StopEngine();
	pXAudio2->Release();
}

int GetGlitchesCount(IXAudio2* pXAudio2)
{
	XAUDIO2_PERFORMANCE_DATA perf = {};
	pXAudio2->GetPerformanceData(&perf);
	return perf.GlitchesSinceEngineStarted;
}


