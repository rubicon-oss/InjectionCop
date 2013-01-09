using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using InjectionCop.Properties;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Config
{
  public class Configuration : IBlacklistManager
  {
    private readonly XNamespace Namespace = "http://injectioncop.codeplex.com/";
    private readonly Dictionary<string, FragmentSignature> _fragmentSignatures = new Dictionary<string, FragmentSignature>();

    public string[] GetFragmentTypes (string qualifiedTypeName, string methodName, IList<string> qualifiedParameterTypes)
    {
      throw new NotImplementedException();
    }

    public FragmentSignature GetFragmentTypes (
        string assemblyName, string qualifiedTypeName, string methodName, IList<string> qualifiedParameterTypes)
    {
      var methodKey = GetMethodKey (assemblyName, qualifiedTypeName, methodName, qualifiedParameterTypes);
      if (_fragmentSignatures.ContainsKey (methodKey))
        return _fragmentSignatures[methodKey];
      return null;
    }

    public string GetMethodKey (string assemblyName, string qualifiedTypeName, string methodName, IEnumerable<string> qualifiedParameterTypes)
    {
      ArgumentUtility.CheckNotNullOrEmpty ("assemblyName", assemblyName);
      ArgumentUtility.CheckNotNullOrEmpty ("qualifiedTypeName", qualifiedTypeName);
      ArgumentUtility.CheckNotNullOrEmpty ("methodName", methodName);
      ArgumentUtility.CheckNotNull ("qualifiedParameterTypes", qualifiedParameterTypes);

      return string.Format ("{0}|{1}|{2}|{3}", assemblyName, qualifiedTypeName, methodName, string.Join ("|", qualifiedParameterTypes));
    }

    public void LoadXml (string xml)
    {
      var xDocument = XDocument.Parse (xml);
      ValidateXml(xDocument);

      foreach (var assemblyElement in xDocument.Elements (Namespace + "Blacklist").Elements (Namespace + "Assembly"))
      {
        var assemblyName = assemblyElement.Attribute ("name").Value;

        LoadTypes (assemblyName, assemblyElement);
      }
    }

    private void ValidateXml (XDocument xDocument)
    {
      var schemas = new XmlSchemaSet();
      schemas.Add (Namespace.NamespaceName, XmlReader.Create (new StringReader (Resources.Configuration)));
      xDocument.Validate (schemas, null);
    }

    private void LoadTypes (string assemblyName, XElement assemblyElement)
    {
      foreach (var typeElement in assemblyElement.Elements (Namespace + "Type"))
      {
        var typeName = typeElement.Attribute ("name").Value;

        LoadMethods (assemblyName, typeName, typeElement);
      }
    }

    private void LoadMethods (string assemblyName, string typeName, XElement typeElement)
    {
      foreach (var methodElement in typeElement.Elements (Namespace + "Method"))
      {
        var methodName = methodElement.Attribute ("name").Value;
        var returnFragmentType = methodElement.Attribute ("returnFragmentType").Value;

        var parameterTypes = methodElement.Elements (Namespace + "Parameter")
                                          .Attributes ("type")
                                          .Select (attribute => attribute.Value)
                                          .ToArray();

        var fragmentTypes = methodElement.Elements (Namespace + "Parameter")
                                         .Attributes ("fragmentType")
                                         .Select (attribute => attribute.Value)
                                         .ToArray();

        var key = GetMethodKey (assemblyName, typeName, methodName, parameterTypes);

        AddFragmentSignature (typeName, key, fragmentTypes, returnFragmentType, methodName, parameterTypes);
      }
    }

    private void AddFragmentSignature (
        string typeName, string key, string[] fragmentTypes, string returnFragmentType, string methodName, string[] parameterTypes)
    {
      if (_fragmentSignatures.ContainsKey (key))
        throw new ArgumentException (
            string.Format (
                "Method {0}.{1}({2}) is defined more than once.",
                typeName,
                methodName,
                string.Join (", ", parameterTypes)));
    
      _fragmentSignatures.Add (key, new FragmentSignature (fragmentTypes, returnFragmentType, false));
    }
  }
}