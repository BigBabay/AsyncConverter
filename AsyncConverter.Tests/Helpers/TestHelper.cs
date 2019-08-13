using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AsyncConverter.Tests.Helpers
{
    public class TestHelper
    {
        public static TestCaseData[] FileNames(string folder)
        {
            var strings = new []{TestContext.CurrentContext.TestDirectory, @"..\..\..\..\Test\Data", folder}.ToArray();
            var testFileDirectory = Path.Combine(strings);

            return Directory
                .GetFiles(testFileDirectory, "*.cs")
                .Select(x => new TestCaseData(Path.GetFileName(x)))
                .ToArray();
        }
    }
}