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
    
    public class IModuleAddInAdapter
    {
        internal static OriginMenu.Views.IModule ContractToViewAdapter(Start9.Api.Contracts.IModuleContract contract)
        {
            if ((contract == null))
            {
                return null;
            }
            if (((System.Runtime.Remoting.RemotingServices.IsObjectOutOfAppDomain(contract) != true) 
                        && contract.GetType().Equals(typeof(IModuleViewToContractAddInAdapter))))
            {
                return ((IModuleViewToContractAddInAdapter)(contract)).GetSourceView();
            }
            else
            {
                return new IModuleContractToViewAddInAdapter(contract);
            }
        }
        internal static Start9.Api.Contracts.IModuleContract ViewToContractAdapter(OriginMenu.Views.IModule view)
        {
            if ((view == null))
            {
                return null;
            }
            if (view.GetType().Equals(typeof(IModuleContractToViewAddInAdapter)))
            {
                return ((IModuleContractToViewAddInAdapter)(view)).GetSourceContract();
            }
            else
            {
                return new IModuleViewToContractAddInAdapter(view);
            }
        }
    }
}
