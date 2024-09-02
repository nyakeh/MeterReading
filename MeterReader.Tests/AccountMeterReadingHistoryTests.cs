using MeterReader.Data;
using MeterReader.Services;
using MeterReader.Models;
using Moq;

namespace MeterReader.Tests
{
    public class GivenAnAccountWithAMeterReadingHistory
    {
        private Mock<ISmartMeterRepository> _mockSmartMeterRepository;
        private List<MeterReadingDto> _result;

        [OneTimeSetUp]
        public void WhenRetrievingAllPreviousMeterReadings()
        {
            _mockSmartMeterRepository = new Mock<ISmartMeterRepository>();
            var johnsAccount = new Account { Id = 2346, FirstName = "John", LastName = "Doe" };
            var meterReadings = new List<MeterReading>
            {
                new MeterReading
                {
                    Account = johnsAccount,
                    MeterReadingDateTime = new DateTime(2024, 6, 29, 8, 0, 0),
                    MeterReadValue = 7235
                },
                new MeterReading
                {
                    Account = johnsAccount,
                    MeterReadingDateTime = new DateTime(2024, 7,28, 10, 30, 0),
                    MeterReadValue = 7850
                },                
                new MeterReading
                {
                    Account = johnsAccount,
                    MeterReadingDateTime = new DateTime(2024, 8, 30, 12, 15, 0),
                    MeterReadValue = 8766
                }                
            };
            _mockSmartMeterRepository.Setup(m => m.GetMeterReadingsByAccount(2346)).Returns(meterReadings);

            var subject = new MeterReadingService(_mockSmartMeterRepository.Object);
            _result = subject.MeterReadingsByAccount(2346);
        }

        [Test]
        public void ThenTheRepositoryIsCalled()
        {
            _mockSmartMeterRepository.Verify(m => m.GetMeterReadingsByAccount(2346), Times.Once);
        }

        [Test]
        public void Result_ShouldNotBeNull()
        {
            Assert.IsNotNull(_result);
        }

        [Test]
        public void ResultShouldHaveThreeReadings()
        {
            Assert.That(_result.Count, Is.EqualTo(3));
        }

        [Test]
        public void FirstReading_HasTheCorrectAccountId()
        {
            Assert.That(_result[0].AccountId, Is.EqualTo(2346));
        }

        [Test]
        public void FirstReading_ShouldHaveTheExpectedDateTime()
        {
            Assert.That(_result[0].MeterReadingDateTime, Is.EqualTo(new DateTime(2024, 6, 29, 8, 0, 0)));
        }

        [Test]
        public void FirstReading_HasTheCorrectMeterReadValue()
        {
            Assert.That(_result[0].MeterReadValue, Is.EqualTo(7235));
        }

        [Test]
        public void SecondReading_ShouldHaveCorrectAccountId()
        {
            Assert.That(_result[1].AccountId, Is.EqualTo(2346));
        }

        [Test]
        public void SecondReading_HasTheCorrectDateTime()
        {
            Assert.That(_result[1].MeterReadingDateTime, Is.EqualTo(new DateTime(2024, 7, 28, 10, 30, 0)));
        }

        [Test]
        public void SecondReading_HasTheCorrectMeterReading()
        {
            Assert.That(_result[1].MeterReadValue, Is.EqualTo(7850));
        }

        [Test]
        public void ThirdReading_HasTheCorrectAccountId()
        {
            Assert.That(_result[2].AccountId, Is.EqualTo(2346));
        }

        [Test]
        public void ThirdReading_HasTheCorrectDate()
        {
            Assert.That(_result[2].MeterReadingDateTime, Is.EqualTo(new DateTime(2024, 8, 30, 12, 15, 0)));
        }

        [Test]
        public void ThirdReading_HasTheCorrectMeterReadValue()
        {
            Assert.That(_result[2].MeterReadValue, Is.EqualTo(8766));
        }
    }

    public class GivenAnAccountWithNoMeterReadingHistory
    {
        private Mock<ISmartMeterRepository> _mockSmartMeterRepository;
        private List<MeterReadingDto> _result;

        [OneTimeSetUp]
        public void WhenRetrievingAllPreviousMeterReadings()
        {
            _mockSmartMeterRepository = new Mock<ISmartMeterRepository>();
            _mockSmartMeterRepository.Setup(m => m.GetMeterReadingsByAccount(2344)).Returns(new List<MeterReading>());
            var subject = new MeterReadingService(_mockSmartMeterRepository.Object);
            _result = subject.MeterReadingsByAccount(2344);
        }

        [Test]
        public void ThenTheResultShouldNotBeNull()
        {
            Assert.IsNotNull(_result);
        }

        [Test]
        public void ThenNoMeterReadingsAreReturned()
        {
            Assert.That(_result.Count, Is.EqualTo(0));
        }

        [Test]
        public void ThenTheRepositoryIsCalled()
        {
            _mockSmartMeterRepository.Verify(m => m.GetMeterReadingsByAccount(2344), Times.Once);
        }
    }

    public class GivenAnAccountThatDoesNotExist
    {
        private Mock<ISmartMeterRepository> _mockSmartMeterRepository;
        private List<MeterReadingDto> _result;

        [OneTimeSetUp]
        public void WhenRetrievingAllPreviousMeterReadings()
        {
            _mockSmartMeterRepository = new Mock<ISmartMeterRepository>();
            _mockSmartMeterRepository.Setup(m => m.GetMeterReadingsByAccount(9999)).Returns(new List<MeterReading>());
            var subject = new MeterReadingService(_mockSmartMeterRepository.Object);
            _result = subject.MeterReadingsByAccount(9999);
        }

        [Test]
        public void ThenTheResultShouldNotBeNull()
        {
            Assert.IsNotNull(_result);
        }

        [Test]
        public void ThenNoMeterReadingsAreReturned()
        {
            Assert.That(_result.Count, Is.EqualTo(0));
        }

        [Test]
        public void ThenTheRepositoryIsCalled()
        {
            _mockSmartMeterRepository.Verify(m => m.GetMeterReadingsByAccount(9999), Times.Once);
        }
    }
}
