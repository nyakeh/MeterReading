using MeterReader.Services;
using MeterReader.Models;
using MeterReader.Data;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace MeterReader.Tests
{
    public class ProcessMeterReadingsTests
    {
        private Mock<ISmartMeterRepository> _mockSmartMeterRepository;
        private MeterReadingService _meterReadingService;
        private UploadMeterReadingsResponse _response;

        [OneTimeSetUp]
        public void ProcessMeterReadings_ShouldProcessReadingsForMultipleAccounts()
        {
            _mockSmartMeterRepository = new Mock<ISmartMeterRepository>();
            _mockSmartMeterRepository.Setup(repo => repo.GetAccountById(It.IsAny<int>()))
                .Returns<int>(id => new Account { Id = id, FirstName = "Tommy", LastName = "Test" });

            _meterReadingService = new MeterReadingService(_mockSmartMeterRepository.Object);
            var data = @"AccountId,MeterReadingDateTime,MeterReadValue,
                         1239,17/05/2019 09:24,45345,
                         1240,18/05/2019 09:24,978,
                         1241,11/04/2019 09:24,436,X
                         1242,20/05/2019 09:24,124,
                         1239,25/05/2019 11:01,45482,
                         1243,21/05/2019 09:24,77,
                         1239,21/05/2019 10:12,45399,";

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
            var meterReadingFile = new FormFile(stream, 0, stream.Length, "file", "Meter_Reading.csv");

            _response = _meterReadingService.ProcessMeterReadings(meterReadingFile);
        }

        [Test]
        public void ThenAllTheDistinctAccountsAreLookedUpOnce()
        {
            _mockSmartMeterRepository.Verify(repo => repo.GetAccountById(1239), Times.Once);
            _mockSmartMeterRepository.Verify(repo => repo.GetAccountById(1240), Times.Once);
            _mockSmartMeterRepository.Verify(repo => repo.GetAccountById(1241), Times.Once);
            _mockSmartMeterRepository.Verify(repo => repo.GetAccountById(1242), Times.Once);
            _mockSmartMeterRepository.Verify(repo => repo.GetAccountById(1243), Times.Once);
        }

        [Test]
        public void ThenBulkAddMeterReadingsIsCalledWithExpectedData()
        {
            _mockSmartMeterRepository.Verify(repo => repo.BulkAddMeterReadings(It.Is<List<MeterReading>>(list =>
                list.Count == 5 &&
                list.Any(mr => mr.Account.Id == 1239 && mr.MeterReadingDateTime == new DateTime(2019, 5, 25, 11, 01, 0) && mr.MeterReadValue == 45482) &&
                list.Any(mr => mr.Account.Id == 1240 && mr.MeterReadingDateTime == new DateTime(2019, 5, 18, 9, 24, 0) && mr.MeterReadValue == 978) &&
                list.Any(mr => mr.Account.Id == 1242 && mr.MeterReadingDateTime == new DateTime(2019, 5, 20, 9, 24, 0) && mr.MeterReadValue == 124) &&
                list.Any(mr => mr.Account.Id == 1241 && mr.MeterReadingDateTime == new DateTime(2019, 4, 11, 9, 24, 0) && mr.MeterReadValue == 436) &&
                list.Any(mr => mr.Account.Id == 1243 && mr.MeterReadingDateTime == new DateTime(2019, 5, 21, 9, 24, 0) && mr.MeterReadValue == 77)
            )), Times.Once);
        }

        [Test]
        public void ThenTheFiveAccountsHaveAMeterReadingStored()
        {
            Assert.That(_response.SuccessfulReadings, Is.EqualTo(5));
        }

        [Test]
        public void ThenTheTwoRedundantReadingsForTheSameAccountAreRejected()
        {
            Assert.That(_response.FailedReadings, Is.EqualTo(2));
        }

        [Test]
        public void ThenTheInvalidMeterReadingsCountIsCorrect()
        {
            Assert.That(_response.InvalidMeterReadings.Count, Is.EqualTo(2));
        }

        [Test]
        public void ThenTheFirstInvalidMeterReadingsIsForTheReplacedOlderMeterReading()
        {
            Assert.That(_response.InvalidMeterReadings[0].RawInput, Is.EqualTo(""));
            Assert.That(_response.InvalidMeterReadings[0].Reason, Is.EqualTo("Row replaced by newer meter read date:'17/05/2019 09:24' for AccountId: '1239'."));
        }

        [Test]
        public void ThenTheSecondInvalidMeterReadingsIsBecauseTheNewReadIsntOlderThanTheExistingRead
()
        {
            Assert.That(_response.InvalidMeterReadings[1].RawInput.TrimStart(), Is.EqualTo("1239,21/05/2019 10:12,45399,"));
            Assert.That(_response.InvalidMeterReadings[1].Reason, Is.EqualTo("Meter reading date is older than existing reading."));
        }
    }
}
