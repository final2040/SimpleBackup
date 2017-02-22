using System;
using System.Configuration;
using NUnit.Framework;
using Configuration = SimpleBackupLibrary.Configuration;

namespace SimpleBackupTests
{
    [TestFixture]
    public class ConfigurationTests
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            // Your SetUp Here
        }

        [Test]
        public void ConfigurationManager_WhenVaildAppConfig_ShouldReturnConfigurationObject()
        {
            // Arrange

            // Act
            var configuration = Configuration.GetConfig();
            // Assert
            Assert.AreEqual("B:\\archivos", configuration.SourceFolder);
            Assert.AreEqual("B:\\Destination", configuration.DestinationFolder);
            Assert.AreEqual(new TimeSpan(9,0,0), configuration.StartTimeOfDay);
            Assert.AreEqual(new TimeSpan(18, 0, 0), configuration.EndTimeOfDay);
            Assert.AreEqual(10800, configuration.BackupIntervalSeconds);
        }
    }
}
