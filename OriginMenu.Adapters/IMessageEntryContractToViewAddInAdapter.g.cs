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
    
    public class IMessageEntryContractToViewAddInAdapter : OriginMenu.Views.IMessageEntry
    {
        private Start9.Api.Contracts.IMessageEntryContract _contract;
        private System.AddIn.Pipeline.ContractHandle _handle;
        static IMessageEntryContractToViewAddInAdapter()
        {
        }
        public IMessageEntryContractToViewAddInAdapter(Start9.Api.Contracts.IMessageEntryContract contract)
        {
            _contract = contract;
            _handle = new System.AddIn.Pipeline.ContractHandle(contract);
        }
        public System.Type Type
        {
            get
            {
                return _contract.Type;
            }
        }
        public string FriendlyName
        {
            get
            {
                return _contract.FriendlyName;
            }
        }
        internal Start9.Api.Contracts.IMessageEntryContract GetSourceContract()
        {
            return _contract;
        }
    }
}

