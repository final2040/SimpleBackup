using System;
using System.Configuration;
using System.Threading;
using Moq;
using NUnit.Framework;
using SimpleBackupLibrary;
using Configuration = SimpleBackupLibrary.Configuration;

namespace SimpleBackupTests
{
    [TestFixture]
    public class BackupManagerTests
    {
        private Configuration _config;
        private Mock<IBackup> _backupMock;

        [SetUp]
        public void Setup()
        {
            _config = new Configuration()
            {
                DestinationFolder = "C:\\test",
                SourceFolder = "C:\\sourceTest",
                BackupIntervalSeconds = 1,
                StartTimeOfDay = new TimeSpan(08, 00, 00),
                EndTimeOfDay = new TimeSpan(19, 00, 00)
            };
        }

        [Test]
        public void WhenTimeIsProvided_ShouldExecuteBackup()
        {
            // arrange
            _backupMock = new Mock<IBackup>();
            var backup = new BackupManager(_backupMock.Object, _config);
            
            Thread backupThread = new Thread(backup.Start);

            // act
            backupThread.Start();
            Thread.Sleep(3000);
            backupThread.Abort();
            // assert
            _backupMock.Verify(bm => bm.DoBackup(), Times.AtLeast(2));
        }

        [Test]
        public void WhenStartTimeIsNotReached_ShouldNotExecuteBackup()
        {
            // arrange
            _backupMock = new Mock<IBackup>();
            var backup = new BackupManager(_backupMock.Object, _config);
            _config.StartTimeOfDay = DateTime.Now.AddHours(2).TimeOfDay;

            Thread backupThread = new Thread(backup.Start);

            // act
            backupThread.Start();
            Thread.Sleep(3000);
            backupThread.Abort();
            // assert
            _backupMock.Verify(bm => bm.DoBackup(), Times.Never);
        }

        [Test]
        public void WhenEndTimeIsReached_ShouldNotExecuteBackup()
        {
            // arrange
            _backupMock = new Mock<IBackup>();
            var backup = new BackupManager(_backupMock.Object, _config);
            _config.EndTimeOfDay = DateTime.Now.AddHours(-2).TimeOfDay;

            Thread backupThread = new Thread(backup.Start);

            // act
            backupThread.Start();
            Thread.Sleep(3000);
            backupThread.Abort();
            // assert
            _backupMock.Verify(bm => bm.DoBackup(), Times.Never);
        }
    }
}