using System;
using System.IO;
using System.Linq;
using System.Reflection;

using MetroLog.Config;

using Moq;

using Xunit;

namespace MetroLog.Tests
{
    public class XmlConfiguratorTests
    {
        [Fact]
        public void TestXmlConfiguratorUsingConfigurationFileStream()
        {
            // Arrange
            var assemblyServiceMock = new Mock<IAssemblyService>();
            var executingAssembly = Assembly.Load("MetroLog");
            assemblyServiceMock.Setup(a => a.GetExecutingAssembly()).Returns(executingAssembly);

            XmlConfigurator xmlConfigurator = new XmlConfigurator(assemblyServiceMock.Object);

            var assembly = this.GetType().Assembly;
            var stream = GetEmbeddedResourceStream(assembly, ".metrolog.config");

            // Act
            var loggingConfiguration = xmlConfigurator.Configure(stream);

            // Assert
            Assert.NotNull(loggingConfiguration);
        }

        private static Stream GetEmbeddedResourceStream(Assembly assembly, string resourceFileName)
        {
            string[] strArray =
                assembly.GetManifestResourceNames().Where(x => x.EndsWith(resourceFileName, StringComparison.CurrentCultureIgnoreCase)).ToArray();
           
            if (!strArray.Any())
            {
                throw new Exception(string.Format("Resource ending with {0} not found.", resourceFileName));
            }
            if (strArray.Count() > 1)
            {
                throw new Exception(
                    string.Format("Multiple resources ending with {0} found: {1}{2}", resourceFileName, Environment.NewLine, string.Join(Environment.NewLine, strArray)));
            }
            return assembly.GetManifestResourceStream(strArray.Single());
        }
    }
}