using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class MetricBmiCalculatorTestsData : IEnumerable<object[]>
    {
        private const string JSON_PATH = "Data/MetricBmiCalculator.json";

        public IEnumerator<object[]> GetEnumerator()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var jsonFullPath = Path.GetRelativePath(currentDir, JSON_PATH);

            if (!File.Exists(jsonFullPath))
            {
                throw new ArgumentException($"Couldn't fint file: {jsonFullPath}");
            }

            var jsonData = File.ReadAllText(jsonFullPath);
            var allCases = JsonConvert.DeserializeObject<IEnumerable<object[]>>(jsonData);

            return allCases.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
