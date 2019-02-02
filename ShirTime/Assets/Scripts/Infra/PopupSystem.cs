namespace ShirTime.Infra
{
    using System;
    using ShirTime.Services;
    using ShirTime.UI;
    using UniRx;
    using Zenject;

    internal class PopupSystem : IInitializable
    {
        private IMainUICallbacks uiCallbacks;

        public PopupSystem(IMainUICallbacks uiCallbacks)
        {
            this.uiCallbacks = uiCallbacks;
        }

        public void Initialize()
        {
            uiCallbacks.ErrorStream.Subscribe(Observer.Create<OperationResult>(ShowError));
        }

        private void ShowError(OperationResult error)
        {
            if (error == OperationResult.OK) return;

            throw new NotImplementedException(error.ToString());
        }
    }

}