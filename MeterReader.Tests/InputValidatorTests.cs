using MeterReader.Services;

namespace MeterReader.Tests
{
    public class GivenAnInvalidInputRow
    {
        [Test]
        public void WhenTheInputRowIsEmpty()
        {
            var result = InputValidator.TryParse(string.Empty, out var smartMeterInput);
            Assert.IsFalse(result);
        }

        [Test]
        public void WhenTheThreeRequiredInputsArentPresent()
        {
            var result = InputValidator.TryParse("2344,22/04/2019 09:24", out var smartMeterInput);
            Assert.IsFalse(result);
        }

        [Test]
        public void GivenAnInputRowUsingAnAlternativeDataSeparator()
        {
            var result = InputValidator.TryParse("2344|22/04/2019 09:24|1002|", out var smartMeterInput);
            Assert.IsFalse(result);
        }

        [Test]
        public void WhenTheAccountIdProvidedIsAGuid()
        {
            var result = InputValidator.TryParse("f69a22fb-09e7-4b7f-9b33-38420c6b651e,22/04/2019 09:24,1002,", out var smartMeterInput);
            Assert.IsFalse(result);
        }

        [Test]
        public void WhenTheAccountIdProvidedIsString()
        {
            var result = InputValidator.TryParse("twentythree,22/04/2019 09:24,1002,", out var smartMeterInput);
            Assert.IsFalse(result);
        }

        [Test]
        public void WhenTheAccountIdProvidedIsAlphaNumeric()
        {
            var result = InputValidator.TryParse("2thirty4,22/04/2019 09:24,1002,", out var smartMeterInput);
            Assert.IsFalse(result);
        }

        [Test]
        public void WhenTheAccountIdIsMissing()
        {
            var result = InputValidator.TryParse(",22/04/2019 09:24,1002,", out var smartMeterInput);
            Assert.IsFalse(result);
        }

        [Test]
        public void WhenTheMeterReadDateProvidedIsMissingTheTime()
        {
            var result = InputValidator.TryParse("2345,22/04/2019,45522,", out var smartMeterInput);
            Assert.IsFalse(result);
        }

        [Test]
        public void WhenTheMeterReadDateProvidedIsNotInTheExpectedFormat()
        {
            var result = InputValidator.TryParse("2345,2019-04-22T12:25:00,45522,", out var smartMeterInput);
            Assert.IsFalse(result);
        }

        [Test]
        public void WhenTheMeterReadDateProvidedIsNotInARecognisedFormat()
        {
            var result = InputValidator.TryParse("2345,22nd/Apr/19 half 12,45522,", out var smartMeterInput);
            Assert.IsFalse(result);
        }

        [Test]
        public void WhenTheMeterReadValueProvidedIsNotANumber()
        {
            var result = InputValidator.TryParse("2346,22/04/2019 12:25,Large,", out var smartMeterInput);
            Assert.IsFalse(result);
        }

        [Test]
        public void WhenTheMeterReadValueProvidedIsADecimal()
        {
            var result = InputValidator.TryParse("2346,22/04/2019 12:25,9999.99,", out var smartMeterInput);
            Assert.IsFalse(result);
        }

        [Test]
        public void WhenTheMeterReadValueProvidedIsAlphanumeric()
        {
            var result = InputValidator.TryParse("2346,22/04/2019 12:25,9ninety9,", out var smartMeterInput);
            Assert.IsFalse(result);
        }
    }

    public class GivenAnValidInputRow
    {
        [Test]
        public void WhenUsingTheProvidedSampleInput()
        {
            var result = InputValidator.TryParse("2344,22/04/2019 09:24,1002,", out var smartMeterInput);
            Assert.IsTrue(result);
        }

        [Test]
        public void WhenTheRowsTrailingCommaIsRemoved()
        {
            var result = InputValidator.TryParse("2233,22/04/2019 12:25,323", out var smartMeterInput);
            Assert.IsTrue(result);
        }

        [Test]
        public void WhenTheRowsContainsAdditionalColumns()
        {
            var result = InputValidator.TryParse("8766,22/04/2019 12:25,3440,additional,columns,007", out var smartMeterInput);
            Assert.IsTrue(result);
        }
    }
}