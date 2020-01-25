#include "VoiceCallback.h"

SourceVoiceCallbackDelegate callbackDelegate;

void SetSourceVoiceCallbackDelegate(SourceVoiceCallbackDelegate callback)
{
	callbackDelegate = callback;
}

void CallDelegate()
{
	callbackDelegate();
}