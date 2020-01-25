
#include <xaudio2.h>
#pragma once
typedef void (__stdcall * SourceVoiceCallbackDelegate)();
extern "C" __declspec(dllexport) void __cdecl SetSourceVoiceCallbackDelegate(SourceVoiceCallbackDelegate callbackDelegate);

void CallDelegate();

class VoiceCallback : public IXAudio2VoiceCallback
{
public:
    HANDLE hBufferEndEvent;
    HANDLE hBufferStartEvent;
    VoiceCallback()
        : hBufferEndEvent( CreateEvent( NULL, FALSE, FALSE, NULL ) )
        , hBufferStartEvent( CreateEvent( NULL, FALSE, FALSE, NULL ) )
    {}
    ~VoiceCallback()
    { 
        CloseHandle( hBufferEndEvent ); 
        CloseHandle( hBufferStartEvent ); 
    }

    //Called when the voice has just finished playing a contiguous audio stream.
    void OnStreamEnd() { SetEvent( hBufferEndEvent ); }
    void OnBufferStart(void * pBufferContext) { 
        SetEvent( hBufferStartEvent ); 
        CallDelegate();
    }

    //Unused methods are stubs
    void OnVoiceProcessingPassEnd() { }
    void OnVoiceProcessingPassStart(UINT32 SamplesRequired) {    }
    void OnBufferEnd(void * pBufferContext)    { }
    
    void OnLoopEnd(void * pBufferContext) {    }
    void OnVoiceError(void * pBufferContext, HRESULT Error) { }
};

