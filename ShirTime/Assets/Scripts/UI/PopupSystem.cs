using UniRx;
using System;

internal class PopupSystem
{
    private IMainUICallbacks uiCallbacks;

    public PopupSystem(IMainUICallbacks uiCallbacks)
    {
        this.uiCallbacks = uiCallbacks;
        uiCallbacks.ErrorStream.Subscribe(Observer.Create<OperationResult>(ShowError));
    }

    private void ShowError(OperationResult error)
    {
        throw new NotImplementedException(error.ToString());
    }
}
