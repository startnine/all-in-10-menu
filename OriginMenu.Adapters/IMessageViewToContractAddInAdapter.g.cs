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
    
    public class IMessageViewToContractAddInAdapter : System.AddIn.Pipeline.ContractBase, Start9.Api.Contracts.IMessageContract
    {
        private OriginMenu.Views.IMessage _view;
        public IMessageViewToContractAddInAdapter(OriginMenu.Views.IMessage view)
        {
            _view = view;
        }
        public object Object
        {
            get
            {
                return _view.Object;
            }
        }
        public Start9.Api.Contracts.IMessageEntryContract MessageEntry
        {
            get
            {
                return OriginMenu.Adapters.IMessageEntryAddInAdapter.ViewToContractAdapter(_view.MessageEntry);
            }
        }
        internal OriginMenu.Views.IMessage GetSourceView()
        {
            return _view;
        }
    }
}

