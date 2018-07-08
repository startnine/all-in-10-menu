//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OriginMenu.Adapters
{
    
    public class IReceiverEntryViewToContractAddInAdapter : System.AddIn.Pipeline.ContractBase, Start9.Api.Contracts.IReceiverEntryContract
    {
        private OriginMenu.Views.IReceiverEntry _view;
        private System.Collections.Generic.Dictionary<Start9.Api.Contracts.IMessageEventHandlerContract, System.EventHandler<OriginMenu.Views.MessageReceivedEventArgs>> MessageReceived_handlers;
        public IReceiverEntryViewToContractAddInAdapter(OriginMenu.Views.IReceiverEntry view)
        {
            _view = view;
            MessageReceived_handlers = new System.Collections.Generic.Dictionary<Start9.Api.Contracts.IMessageEventHandlerContract, System.EventHandler<OriginMenu.Views.MessageReceivedEventArgs>>();
        }
        public System.Type Type
        {
            get
            {
                return _view.Type;
            }
        }
        public string FriendlyName
        {
            get
            {
                return _view.FriendlyName;
            }
        }
        public virtual void SendMessage(Start9.Api.Contracts.IMessageContract message)
        {
            _view.SendMessage(OriginMenu.Adapters.IMessageAddInAdapter.ContractToViewAdapter(message));
        }
        public virtual void MessageReceivedEventAdd(Start9.Api.Contracts.IMessageEventHandlerContract handler)
        {
            System.EventHandler<OriginMenu.Views.MessageReceivedEventArgs> adaptedHandler = new System.EventHandler<OriginMenu.Views.MessageReceivedEventArgs>(new OriginMenu.Adapters.IMessageEventHandlerContractToViewAddInAdapter(handler).Handler);
            _view.MessageReceived += adaptedHandler;
            MessageReceived_handlers[handler] = adaptedHandler;
        }
        public virtual void MessageReceivedEventRemove(Start9.Api.Contracts.IMessageEventHandlerContract handler)
        {
            System.EventHandler<OriginMenu.Views.MessageReceivedEventArgs> adaptedHandler;
            if (MessageReceived_handlers.TryGetValue(handler, out adaptedHandler))
            {
                MessageReceived_handlers.Remove(handler);
                _view.MessageReceived -= adaptedHandler;
            }
        }
        internal OriginMenu.Views.IReceiverEntry GetSourceView()
        {
            return _view;
        }
    }
}

