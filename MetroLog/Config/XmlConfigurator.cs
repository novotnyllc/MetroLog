using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

using MetroLog.Internal;
using MetroLog.Targets;

namespace MetroLog.Config
{
    public class XmlConfigurator
    {
        private const string CONFIGURATION_TAG = "configuration";
        private const string METROLOG_TAG = "metrolog";
        private const string TARGET_TAG = "target";
        private const string TARGET_REF_TAG = "target-ref";
        private const string ROOT_TAG = "root";

        private const string NAME_ATTR = "name";
        private const string TYPE_ATTR = "type";
        private const string VALUE_ATTR = "value";
        private const string REF_ATTR = "ref";
        private const string TARGET_LOGLEVELMIN_TAG = "logLevelMin";
        private const string TARGET_LOGLEVELMAX_TAG = "logLevelMax";

        private readonly Dictionary<string, Target> targets = new Dictionary<string, Target>();

        private IAssemblyService callingAssembly;

        public LoggingConfiguration Configure(Stream stream)
        {
            this.callingAssembly = PlatformAdapter.Resolve<IAssemblyService>();

            XDocument doc = XDocument.Load(stream);
            var configurationSection = doc.Element(CONFIGURATION_TAG);
            if (configurationSection != null)
            {
                var loggerConfigXElement = configurationSection.Element(METROLOG_TAG);
                return this.Configure(loggerConfigXElement);
            }

            InternalLogger.Current.Error("XmlConfigurator: Could not find configuration tag.");
            return null;
        }

        public LoggingConfiguration Configure(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentException("element");
            }

            if (element.Name.LocalName != METROLOG_TAG)
            {
                string errorMessage = string.Format("Xml element does not contain a <{0}> element.", METROLOG_TAG);
                InternalLogger.Current.Error(errorMessage);
                throw new Exception(errorMessage);
            }

            foreach (var currentElement in element.Nodes().Where(n => n.NodeType == XmlNodeType.Element).OfType<XElement>())
            {
                if (currentElement.Name.LocalName == ROOT_TAG)
                {
                    return this.ParseRoot(currentElement);
                }
            }

            InternalLogger.Current.Error("XmlConfigurator: Unable to configure from xml.");
            return null;
        }

        private LoggingConfiguration ParseRoot(XElement rootElement)
        {
            var loggingConfiguration = new LoggingConfiguration();

            foreach (var currentElement in rootElement.Nodes().Where(n => n.NodeType == XmlNodeType.Element).OfType<XElement>())
            {
                if (currentElement.Name.LocalName == TARGET_REF_TAG)
                {
                    string targetName = GetAttributeValue(currentElement, REF_ATTR);

                    var targetXElement = this.FindTargetXElementInReferenceXElement(currentElement, targetName);
                    var target = this.FindTargetByReference(targetXElement, targetName);
                    if (target != null)
                    {
                        var targetConfig = this.GetTargetConfiguration(targetXElement);
                        InternalLogger.Current.Debug(string.Format("Adding target named [{0}] with logLevelMin={1} and logLevelMax={2}", targetName, targetConfig.LogLevelMin, targetConfig.LogLevelMax));
                        loggingConfiguration.AddTarget(targetConfig.LogLevelMin, targetConfig.LogLevelMax, target);
                    }
                    else
                    {
                        InternalLogger.Current.Error("Target named [" + targetName + "] not found.");
                    }
                }
            }

            return loggingConfiguration;
        }

        private TargetConfig GetTargetConfiguration(XElement targetXElement)
        {
            var targetConfig = TargetConfig.Default;
            foreach (var currentElement in targetXElement.Nodes().Where(n => n.NodeType == XmlNodeType.Element).OfType<XElement>())
            {
                if (currentElement.Name.LocalName == TARGET_LOGLEVELMIN_TAG)
                {
                    
                    var logLevelMinValue = GetAttributeValue(currentElement, VALUE_ATTR);
                    targetConfig.LogLevelMin = (LogLevel)ConverterRegistry.ConvertTo(typeof(LogLevel), logLevelMinValue);
                }
                else if (currentElement.Name.LocalName == TARGET_LOGLEVELMAX_TAG)
                {
                    var logLevelMaxValue = GetAttributeValue(currentElement, VALUE_ATTR);
                    targetConfig.LogLevelMax = (LogLevel)ConverterRegistry.ConvertTo(typeof(LogLevel), logLevelMaxValue);
                }
            }

            return targetConfig;
        }

        private XElement FindTargetXElementInReferenceXElement(XElement targetRef, string targetName)
        {
            XElement element = null;

            if (!string.IsNullOrEmpty(targetName))
            {
                foreach (var currentTargetElement in targetRef.Parent.Parent.Elements(TARGET_TAG))
                {
                    if (GetAttributeValue(currentTargetElement, NAME_ATTR) == targetName)
                    {
                        element = currentTargetElement;
                        break;
                    }
                }
            }

            if (element == null)
            {
                InternalLogger.Current.Error("XmlConfigurator: No target named [" + targetName + "] could be found.");
                return null;
            }

            return element;
        }

        private Target FindTargetByReference(XElement element, string targetName)
        {
            if (this.targets.ContainsKey(targetName))
            {
                return this.targets[targetName];
            }

            var target = this.CreateObjectFromXml(element) as Target;
            ////var target = this.ParseTarget(element);
            if (target != null && !this.targets.ContainsKey(targetName))
            {
                this.targets[targetName] = target;
            }
            return target;
        }

        ////private Target ParseTarget(XElement targetElement)
        ////{
        ////    string targetName = GetAttributeValue(targetElement, NAME_ATTR);
        ////    string targetTypeName = GetAttributeValue(targetElement, TYPE_ATTR);

        ////    InternalLogger.Current.Debug("Loading target [" + targetName + "] type: [" + targetTypeName + "]");
        ////    try
        ////    {
        ////        Type targetType = GetTypeFromString(this.callingAssembly, targetTypeName, false);
        ////        int commaIndex = targetTypeName.IndexOf(",", StringComparison.Ordinal);
        ////        if (targetType == null && commaIndex > 0) // Try to find type again without specifying the assembly
        ////        {
        ////            targetTypeName = targetTypeName.Substring(0, commaIndex);
        ////            targetType = GetTypeFromString(this.callingAssembly, targetTypeName, false);
        ////        }

        ////        if (targetType == null)
        ////        {
        ////            InternalLogger.Current.Error("XmlConfigurator: Error loading target [" + targetName + "]. Could not find type [" + targetTypeName + "].");
        ////            return null;
        ////        }

        ////        Target target = (Target)Activator.CreateInstance(targetType, new object[] { });
        ////        InternalLogger.Current.Debug("Created target [" + targetName + "] with default constructor.");

        ////        var propertiesOfTarget = targetElement.Nodes()
        ////            .Where(n => n.NodeType == XmlNodeType.Element)
        ////            .OfType<XElement>()
        ////            .ToList();

        ////        Layout layout = null;
        ////        foreach (var propertyOfTarget in propertiesOfTarget)
        ////        {
        ////            if (propertyOfTarget.Name.LocalName == TARGET_LOGLEVELMIN_TAG || propertyOfTarget.Name.LocalName == TARGET_LOGLEVELMAX_TAG)
        ////            {
        ////                // these two values are ignored
        ////                continue;
        ////            }

        ////            if (propertyOfTarget.Name.LocalName == TARGET_LAYOUT_TAG)
        ////            {
        ////                if (layout == null)
        ////                {
        ////                    layout = this.ParseLayout(propertyOfTarget, targetName);
        ////                    layout = this.CreateObjectFromXml(propertyOfTarget) as Layout;
        ////                    SetParameter(target, TARGET_LAYOUT_TAG, layout);
        ////                }
        ////                else
        ////                {
        ////                    InternalLogger.Current.Warn("Target [" + targetName + "] cannot have more than one layouts.");
        ////                }
        ////            }
        ////            else
        ////            {
        ////                this.SetParameter(propertyOfTarget, target);
        ////            }
        ////        }

        ////        ////var targetLayoutElements = targetElement.Nodes()
        ////        ////    .Where(n => n.NodeType == XmlNodeType.Element)
        ////        ////    .OfType<XElement>()
        ////        ////    .Where(xe => xe.Name.LocalName == TARGET_LAYOUT_TAG)
        ////        ////    .ToList();

        ////        ////if (targetLayoutElements.Count > 1)
        ////        ////{
        ////        ////    InternalLogger.Current.Warn("Target [" + targetName + "] cannot have more than one layouts.");
        ////        ////}


        ////        ////var layoutXElement = targetLayoutElements.FirstOrDefault();
        ////        ////Layout layout = ParseLayout(layoutXElement);

        ////        ////Target target = null;
        ////        ////if (layout != null)
        ////        ////{
        ////        ////    target = (Target)Activator.CreateInstance(targetType, layout);
        ////        ////    InternalLogger.Current.Debug("Created target [" + targetName + "] with layout [" + layout.GetType() + "].");
        ////        ////}
        ////        ////else
        ////        ////{

        ////        ////    target = (Target)Activator.CreateInstance(targetType, new object[]{});
        ////        ////    InternalLogger.Current.Debug("Created target [" + targetName + "] with default layout.");
        ////        ////}

        ////        return target;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        InternalLogger.Current.Error("XmlConfigurator: Could not create target [" + targetName + "] of type [" + targetTypeName + "]. Reported error follows.", ex);
        ////        return null;
        ////    }
        ////}

        ////private Layout ParseLayout(XElement layoutXElement, string targetName)
        ////{
        ////    Layout layout = null;
        ////    if (layoutXElement != null)
        ////    {
        ////        string targetLayoutTypeName = GetAttributeValue(layoutXElement, TYPE_ATTR);
        ////        string targetLayoutParameter = GetAttributeValue(layoutXElement, VALUE_ATTR);

        ////        try
        ////        {
        ////            Type targetLayoutType = GetTypeFromString(this.callingAssembly, targetLayoutTypeName, false);

        ////            if (!string.IsNullOrEmpty(targetLayoutParameter))
        ////            {
        ////                InternalLogger.Current.Debug("Creating layout of type [" + targetLayoutTypeName + "] with parameter [" + targetLayoutParameter + "] for target [" + targetName + "]");
        ////                layout = (Layout)Activator.CreateInstance(targetLayoutType, targetLayoutParameter); // TODO GATH: Create new interface IXMLConfigurable to inject configuration parameters into Layout
        ////            }
        ////            else
        ////            {
        ////                InternalLogger.Current.Debug("Creating layout of type [" + targetLayoutTypeName + "] for target [" + targetName + "]");
        ////                layout = (Layout)Activator.CreateInstance(targetLayoutType, new object[] { });
        ////            }

        ////        }
        ////        catch (Exception ex)
        ////        {
        ////            InternalLogger.Current.Error("XmlConfigurator: Could not create layout of type [" + targetLayoutTypeName + "] for target [" + targetName + "]. Reported error follows.", ex);
        ////        }
        ////    }
        ////    else
        ////    {
        ////        InternalLogger.Current.Debug("Target [" + targetName + "] has no layouts configured. Default layout is applied.");
        ////    }

        ////    return layout;
        ////}

        private void SetParameter(XElement element, object target)
        {
            // Get the property name
            string propertyName = GetAttributeValue(element, NAME_ATTR);

            // If the name attribute does not exist then use the name of the element
            if (string.IsNullOrEmpty(propertyName))
            {
                propertyName = element.Name.LocalName;
            }

            // Ignore logLevelMin and logLevelMax tags. They are handled in a different place.
            if (propertyName == TARGET_LOGLEVELMIN_TAG || propertyName == TARGET_LOGLEVELMAX_TAG)
            {
                return;
            }

            // Look for the property on the target object
            Type targetType = target.GetType();

            // Try to find a writable property
            var propInfo = targetType.GetRuntimeProperties().FirstOrDefault(p => string.Compare(p.Name, propertyName, StringComparison.OrdinalIgnoreCase) == 0);
            if (propInfo != null && propInfo.CanWrite)
            {
                if (propInfo.CanWrite)
                {
                    object propertyValue = null;

                    var attributeValue = GetAttributeValue(element, VALUE_ATTR);
                    if (attributeValue != null)
                    {
                        propertyValue = ConverterRegistry.ConvertTo(propInfo.PropertyType, attributeValue);
                    }
                    else
                    {
                        propertyValue = this.CreateObjectFromXml(element);
                    }

                    // Finally, write the propertyValue to taget's property
                    if (propertyValue == null)
                    {
                        InternalLogger.Current.Warn(string.Format("Setting 'null' to property [{0}] in type [{1}] may indicate a conversion error.", propertyName, targetType.Name));
                    }

                    propInfo.SetValue(target, propertyValue);
                }
                else
                {
                    InternalLogger.Current.Warn(string.Format("Write access to property [{0}] in type [{1}] is not allowed. Please make setter public.", propertyName, targetType.Name));
                }
            }
            else
            {
                InternalLogger.Current.Warn(string.Format("Could not find property with name [{0}] in type [{1}]", propertyName, targetType.Name));
            }
        }

        private object CreateObjectFromXml(XElement element)
        {
            Type objectType = null;

            // Get the object type
            string objectTypeString = GetAttributeValue(element, TYPE_ATTR);
            if (string.IsNullOrEmpty(objectTypeString))
            {
                InternalLogger.Current.Error("Attribute [" + TYPE_ATTR + "] not specified for [ " + element.Name.LocalName + "].");
            }
            else
            {
                // Read the explicit object type
                try
                {
                    objectType = GetTypeFromString(this.callingAssembly, objectTypeString, true);
                }
                catch (Exception ex)
                {
                    InternalLogger.Current.Error("Failed to find type [" + objectTypeString + "]", ex);
                    return null;
                }
            }

            // Create using the default constructor
            object createdObject = null;
            try
            {
                createdObject = Activator.CreateInstance(objectType);
            }
            catch (Exception ex)
            {
                InternalLogger.Current.Error("XmlHierarchyConfigurator: Failed to construct object of type [" + objectType.FullName + "]. Exception: " + ex.ToString());
            }

            // Set any params on object
            foreach (var currentNode in element.Nodes().Where(n => n.NodeType == XmlNodeType.Element).OfType<XElement>())
            {
                this.SetParameter(currentNode, createdObject);
            }

            return createdObject;
        }

        private static string GetAttributeValue(XElement element, string attributeName)
        {
            var typeAttribute = element.Attribute(attributeName);
            if (typeAttribute != null)
            {
                return typeAttribute.Value;
            }

            return null;
        }

        private static Type GetTypeFromString(IAssemblyService assemblyService, string typeName, bool throwOnError)
        {
            // Check if the type name specifies the assembly name
            var isExplicitTypeName = typeName.IndexOf(',') == -1;
            if (isExplicitTypeName)
            {
                // Attempt to lookup the type from the assemblyService
                var executingAssembly = assemblyService.GetExecutingAssembly();
                Type type = executingAssembly.GetType(typeName);
                if (type != null)
                {
                    // Found type in relative assembly
                    InternalLogger.Current.Error("XmlConfigurator: Loaded type [" + typeName + "] from assembly [" + executingAssembly.FullName + "]");
                    return type;
                }

                IEnumerable<Assembly> loadedAssemblies = null;
                try
                {
                    loadedAssemblies = assemblyService.GetAssemblies();
                }
                catch (System.Security.SecurityException)
                {
                    // Insufficient permissions to get the list of loaded assemblies
                }

                if (loadedAssemblies != null)
                {
                    // Search the loaded assemblies for the type
                    foreach (Assembly assembly in loadedAssemblies)
                    {
                        type = assembly.GetType(typeName);
                        if (type != null)
                        {
                            // Found type in loaded assembly
                            InternalLogger.Current.Debug("Loaded type [" + typeName + "] from assembly [" + assembly.FullName + "] by searching loaded assemblies.");
                            return type;
                        }
                    }
                }

                if (throwOnError)
                {
                    throw new TypeLoadException("Could not load type [" + typeName + "]. Tried assembly [" + executingAssembly.FullName + "] and all loaded assemblies");
                }
                return null;
            }
            else
            {
                // Includes explicit assembly name
                return Type.GetType(typeName, throwOnError);
            }
        }

        private struct TargetConfig
        {
            private TargetConfig(LogLevel logLevelMin, LogLevel logLevelMax)
                : this()
            {
                this.LogLevelMin = logLevelMin;
                this.LogLevelMax = logLevelMax;
            }

            public LogLevel LogLevelMin { get; set; }

            public LogLevel LogLevelMax { get; set; }

            public static readonly TargetConfig Default = new TargetConfig(LogLevel.Trace, LogLevel.Fatal);
        }
    }
}