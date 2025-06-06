using MyLib.Extensions;

namespace MyLib.UnitTests.Extensions;

[TestClass]
public class NumberFormatExtensionTests
{
    [TestMethod]
    [DataRow(0, "0")]
    [DataRow(123, "123")]
    [DataRow(1234, "1,234")]
    [DataRow(1234567, "1,234,567")]
    [DataRow(-1234, "-1,234")]
    [DataRow(-987654321, "-987,654,321")]
    public void WithThousandsSeparator_ReturnsExpectedFormat(int input, string expected)
    {
        // Act
        var result = input.WithThousandsSeparator();

        // Assert
        Assert.AreEqual(expected, result);
    }
}
