using Xunit;
using Xunit.Abstractions;

namespace SwikiToCsv.Tests
{
    public class SwikiToCsvTests
    {
        private readonly ITestOutputHelper _output;

        public SwikiToCsvTests(
            ITestOutputHelper output)
        {
            _output = output;    
        }

        [Fact]
        public void SwikiToCsv_ReadsSwikiFile()
        {
            var testFile = "dailystats.swiki";
            var sut = new SwikiReader(testFile);
            _output.WriteLine($"file:{testFile} lines:{sut.Lines.Count}");
            Assert.True(sut.Lines.Count > 0);
        }

        [Theory]
        [InlineData("bla bla bla")]
        [InlineData("Niners for Superbowl LIV")]
        public void TestSwikiLineTypeText(string line)
        {
            var sut = new SwikiTable();
            Assert.True(
                sut.LineType(line).Equals(
                    SwikiLineType.Text));
            Assert.False(
                sut.LineType(line).Equals(
                    SwikiLineType.TableHeader));
            Assert.False(
                sut.LineType(line).Equals(
                    SwikiLineType.TableRow));
        }

        [Theory]
        [InlineData("||  **")]
        [InlineData("||  **Date**  ||  **Result**  ||")]
        public void TestSwikiLineTypeTableHeader(string line)
        {
            var sut = new SwikiTable();
            Assert.True(
                sut.LineType(line).Equals(
                    SwikiLineType.TableHeader));
            Assert.False(
                sut.LineType(line).Equals(
                    SwikiLineType.TableRow));
            Assert.False(
                sut.LineType(line).Equals(
                    SwikiLineType.Text));
        }

        [Theory]
        [InlineData("||  ||  ||  ||")]
        [InlineData("||  2020-01-21   ||  Win ||")]
        public void TestSwikiLineTypeTableRow(string line)
        {
            var sut = new SwikiTable();
            Assert.True(
                sut.LineType(line).Equals(
                    SwikiLineType.TableRow));
            Assert.False(
                sut.LineType(line).Equals(
                    SwikiLineType.TableHeader));
            Assert.False(
                sut.LineType(line).Equals(
                    SwikiLineType.Text));
        }
    }
}
