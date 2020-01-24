#include <xaudio2.h>
#include <windows.h>

extern "C" __declspec(dllexport) int __cdecl CreateMasteringVoice(IXAudio2* pXAudio2Instance, IXAudio2MasteringVoice** outpMasterVoice);
extern "C" __declspec(dllexport) HRESULT __cdecl CreateSourceVoice(IXAudio2* pXaudio2, byte* audioData, int audioBytes, WORD channels, int sampleRate, IXAudio2SourceVoice** outpSourceVoice);
extern "C" __declspec(dllexport) HRESULT __cdecl PlaySourceVoice(IXAudio2SourceVoice* pSourceVoice);
extern "C" __declspec(dllexport) void __cdecl ShutDown(IXAudio2* pXAudio2Instance, IXAudio2MasteringVoice* pMasterVoiceInstance, IXAudio2SourceVoice* pSourceVoiceInstance);
extern "C" __declspec(dllexport) int __cdecl GetGlitchesCount(IXAudio2* pXAudio2);


