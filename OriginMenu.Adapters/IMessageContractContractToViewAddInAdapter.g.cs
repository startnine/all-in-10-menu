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
    
    public class IMessageContractContractToViewAddInAdapter : OriginMenu.Views.IMessageContract
    {
        private Start9.Api.Contracts.IMessageContractContract _contract;
        private System.AddIn.Pipeline.ContractHandle _handle;
        static IMessageContractContractToViewAddInAdapter()
        {
        }
        public IMessageContractContractToViewAddInAdapter(Start9.Api.Contracts.IMessageContractContract contract)
        {
            _contract = contract;
            _handle = new System.AddIn.Pipeline.ContractHandle(contract);
        }
        public System.Collections.Generic.IList<OriginMenu.Views.IMessageEntry> Entries
        {
            get
            {
                return System.AddIn.Pipeline.CollectionAdapters.ToIList<Start9.Api.Contracts.IMessageEntryContract, OriginMenu.Views.IMessageEntry>(_contract.Entries, OriginMenu.Adapters.IMessageEntryAddInAdapter.ContractToViewAdapter, OriginMenu.Adapters.IMessageEntryAddInAdapter.ViewToContractAdapter);
            }
        }
        internal Start9.Api.Contracts.IMessageContractContract GetSourceContract()
        {
            return _contract;
        }
    }
}

