
#include <xaudio2.h>
#include <future>
#include <iostream>
#include <thread>
#pragma once
typedef void (__stdcall * SourceVoiceCallbackDelegate)();
extern "C" __declspec(dllexport) void __cdecl SetSourceVoiceCallbackDelegate(SourceVoiceCallbackDelegate callbackDelegate);

void CallDelegate();

class VoiceCallback : public IXAudio2VoiceCallback
{
public:
    HANDLE hBufferEndEvent;
    HANDLE hBufferStartEvent;
    HANDLE hVoiceProcessingPassStart;
    VoiceCallback()
        : hBufferEndEvent( CreateEvent( NULL, FALSE, FALSE, NULL ) )
        , hBufferStartEvent( CreateEvent( NULL, FALSE, FALSE, NULL ) )
        , hVoiceProcessingPassStart( CreateEvent( NULL, FALSE, FALSE, NULL ) )
    {}
    ~VoiceCallback()
    { 
        CloseHandle( hBufferEndEvent ); 
        CloseHandle( hBufferStartEvent ); 
        CloseHandle( hVoiceProcessingPassStart );
    }

    // Called when the voice has just finished playing a contiguous audio stream.
    void OnStreamEnd() { SetEvent( hBufferEndEvent ); }
    void OnBufferStart(void * pBufferContext) { 
        SetEvent( hBufferStartEvent ); 
    }

    // Called during each processing pass for each voice, just before XAudio2 reads data from the voice's buffer queue.
    void OnVoiceProcessingPassStart(UINT32 SamplesRequired) {  
        SetEvent(hVoiceProcessingPassStart);
    }

    //Unused methods are stubs
    void OnVoiceProcessingPassEnd() { }
    void OnBufferEnd(void * pBufferContext)    { }
    void OnLoopEnd(void * pBufferContext) {    }
    void OnVoiceError(void * pBufferContext, HRESULT Error) { }

    void CallDelegate();
};

