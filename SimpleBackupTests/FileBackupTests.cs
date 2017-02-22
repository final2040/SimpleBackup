using System;
using System.IO;
using Moq;
using NUnit.Framework;
using SimpleBackupLibrary;

namespace SimpleBackupTests
{
    [TestFixture]
    public class FileBackupTests
    {
        private Mock<FileHelper> _fileHelperMock;
        private Mock<Configuration> _configurationMock;

        [SetUp]
        public void SetUp()
        {
            _fileHelperMock = new Mock<FileHelper>();
            _configurationMock = new Mock<Configuration>();
            _configurationMock.Setup(p => p.StartTimeOfDay).Returns(new TimeSpan(08, 00, 00));
            _configurationMock.Setup(p => p.EndTimeOfDay).Returns(new TimeSpan(18, 00, 00));
            _configurationMock.Setup(p => p.BackupIntervalSeconds).Returns(10);
            _configurationMock.Setup(p => p.SourceFolder).Returns("B:\\Tests");
            _configurationMock.Setup(p => p.DestinationFolder).Returns("B:\\DestinationTest");
        }

        [Test]
        public void ShouldCreateFolderWithDateTimePlusOriginName()
        {
            // arrange
            var expectedDestinationFolder = Path.Combine(_configurationMock.Object.DestinationFolder,
                DateTime.Now.ToString("yyyy-MM-ddThh-mm-ss"));
            _fileHelperMock.Setup(fh => fh.CreateDirectory(expectedDestinationFolder)).Verifiable();
            _fileHelperMock.Setup(fh => fh.DirectoryExists(It.IsAny<string>())).Returns(true).Verifiable();

            var backup = new FileBackup(_configurationMock.Object, _fileHelperMock.Object);
            
            // act
            backup.DoBackup();

            // assert
            _fileHelperMock.Verify();
        }

        [Test]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void WhenSourceFolderDoesntExist_ShouldThrowAnException()
        {
            // arrange
            var expectedDestinationFolder = Path.Combine(_configurationMock.Object.DestinationFolder,
                DateTime.Now.ToString("s"));
            _fileHelperMock.Setup(fh => fh.DirectoryExists(_configurationMock.Object.SourceFolder)).Returns(false).Verifiable();
            var backup = new FileBackup(_configurationMock.Object, _fileHelperMock.Object);

            // act
            backup.DoBackup();

            // assert
            _fileHelperMock.Verify();
        }

        [Test]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void WhenDestinationFolderDoesntExist_ShouldThrowAnException()
        {
            // arrange
            var expectedDestinationFolder = Path.Combine(_configurationMock.Object.DestinationFolder,
                DateTime.Now.ToString("s"));
            _fileHelperMock.Setup(fh => fh.DirectoryExists(_configurationMock.Object.SourceFolder)).Returns(true).Verifiable();
            _fileHelperMock.Setup(fh => fh.DirectoryExists(_configurationMock.Object.DestinationFolder)).Returns(false).Verifiable();
            var backup = new FileBackup(_configurationMock.Object, _fileHelperMock.Object);

            // act
            backup.DoBackup();

            // assert
            _fileHelperMock.Verify();
        }

        [Test]
        public void ShouldCreateSubFolders()
        {
            // arrange
            _fileHelperMock.Setup(fh => fh.DirectoryExists(_configurationMock.Object.SourceFolder)).Returns(true).Verifiable();
            _fileHelperMock.Setup(fh => fh.DirectoryExists(_configurationMock.Object.DestinationFolder)).Returns(true).Verifiable();
            _fileHelperMock.SetupSequence(fm => fm.GetDirectories(It.IsAny<string>()))
                .Returns(new [] { "B:\\Tests\\Directory1", "B:\\Tests\\Directory2", "B:\\Tests\\Directory3", "B:\\Tests\\Directory4", "B:\\Tests\\Directroy5" })
                .Returns(new []{ "B:\\Tests\\Directory1\\SubDirectory1", "B:\\Tests\\Directory1\\SubDirectory2" })
                .Returns(new string[]{});

            var backup = new FileBackup(_configurationMock.Object, _fileHelperMock.Object);

            // act
            backup.DoBackup();

            // assert
            _fileHelperMock.Verify(fh => fh.CreateDirectory(It.IsAny<string>()),Times.AtLeast(7));
        }

        [Test]
        public void ShouldCopyFiles()
        {
            // arrange
            _fileHelperMock.Setup(fh => fh.DirectoryExists(_configurationMock.Object.SourceFolder)).Returns(true).Verifiable();
            _fileHelperMock.Setup(fh => fh.DirectoryExists(_configurationMock.Object.DestinationFolder)).Returns(true).Verifiable();
            _fileHelperMock.SetupSequence(fm => fm.GetFiles(It.IsAny<string>()))
                .Returns(new[]
                {
                    "B:\\Tests\\file1.txt", "B:\\Tests\\file2.txt",
                    "B:\\Tests\\file3.txt", "B:\\Tests\\file4.txt",
                    "B:\\Tests\\file5.txt"
                });

            var backup = new FileBackup(_configurationMock.Object, _fileHelperMock.Object);

            // act
            backup.DoBackup();

            // assert
            _fileHelperMock.Verify(fh => fh.Copy(It.IsAny<string>(), It.IsAny<string>(), true), Times.AtLeast(5));
        }

        [Test]
        public void WhenDestinationFolderIsInSourceFolder_ShouldIgnore()
        {
            // arrange
            _configurationMock.Setup(cm => cm.DestinationFolder).Returns("B:\\Tests\\Directory_2");
            _fileHelperMock.Setup(fh => fh.DirectoryExists(_configurationMock.Object.SourceFolder)).Returns(true).Verifiable();
            _fileHelperMock.Setup(fh => fh.DirectoryExists(_configurationMock.Object.DestinationFolder)).Returns(true).Verifiable();
            _fileHelperMock.SetupSequence(fm => fm.GetDirectories(It.IsAny<string>()))
                .Returns(new[] { "B:\\Tests\\Directory1", "B:\\Tests\\Directory_2", "B:\\Tests\\Directory3", "B:\\Tests\\Directory4", "B:\\Tests\\Directroy5" })
                .Returns(new[] { "B:\\Tests\\Directory1\\SubDirectory1", "B:\\Tests\\Directory1\\SubDirectory2" })
                .Returns(new string[] { });

            var backup = new FileBackup(_configurationMock.Object, _fileHelperMock.Object);

            // act
            backup.DoBackup();

            // assert
            _fileHelperMock.Verify(fh => fh.CreateDirectory(It.Is<string>(s => s.EndsWith("Directory_2"))), Times.Never);
        }
    }
}