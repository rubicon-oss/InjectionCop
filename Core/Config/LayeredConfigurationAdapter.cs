using System.Collections.Generic;
using InjectionCop.Utilities;

namespace InjectionCop.Config
{
  public class LayeredConfigurationAdapter : IBlacklistManager
  {
    private readonly IBlacklistManager[] _configurations;

    public LayeredConfigurationAdapter (Stack<IBlacklistManager> configurations)
    {
      ArgumentUtility.CheckNotNull ("configurations", configurations);
   
      _configurations = configurations.ToArray();
    }

    public IBlacklistManager[] Configurations
    {
      get { return _configurations; }
    }

    public FragmentSignature GetFragmentTypes (string assemblyName, string qualifiedTypeName, string methodName, IList<string> qualifiedParameterTypes)
    {
      foreach (var configurationLayer in _configurations)
      {
        var fragmentSignature = configurationLayer.GetFragmentTypes (assemblyName, qualifiedTypeName, methodName, qualifiedParameterTypes);
        if (fragmentSignature != null)
          return fragmentSignature; 
      }
      return null;
    }
  }
}