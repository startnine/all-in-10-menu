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
    
    public class IHostContractToViewAddInAdapter : OriginMenu.Views.IHost
    {
        private Start9.Api.Contracts.IHostContract _contract;
        private System.AddIn.Pipeline.ContractHandle _handle;
        static IHostContractToViewAddInAdapter()
        {
        }
        public IHostContractToViewAddInAdapter(Start9.Api.Contracts.IHostContract contract)
        {
            _contract = contract;
            _handle = new System.AddIn.Pipeline.ContractHandle(contract);
        }
        public void SendMessage(OriginMenu.Views.IMessage message)
        {
            _contract.SendMessage(OriginMenu.Adapters.IMessageAddInAdapter.ViewToContractAdapter(message));
        }
        public void SaveConfiguration(OriginMenu.Views.IModule module)
        {
            _contract.SaveConfiguration(OriginMenu.Adapters.IModuleAddInAdapter.ViewToContractAdapter(module));
        }
        public System.Collections.Generic.IList<OriginMenu.Views.IModule> GetModules()
        {
            return System.AddIn.Pipeline.CollectionAdapters.ToIList<Start9.Api.Contracts.IModuleContract, OriginMenu.Views.IModule>(_contract.GetModules(), OriginMenu.Adapters.IModuleAddInAdapter.ContractToViewAdapter, OriginMenu.Adapters.IModuleAddInAdapter.ViewToContractAdapter);
        }
        public OriginMenu.Views.IConfiguration GetGlobalConfiguration()
        {
            return OriginMenu.Adapters.IConfigurationAddInAdapter.ContractToViewAdapter(_contract.GetGlobalConfiguration());
        }
        internal Start9.Api.Contracts.IHostContract GetSourceContract()
        {
            return _contract;
        }
    }
}
