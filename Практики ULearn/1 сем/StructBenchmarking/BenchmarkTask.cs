using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace StructBenchmarking
{
    public class Benchmark : IBenchmark
	{
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            var watch = new Stopwatch();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            task.Run();
            watch.Restart();

            for (var i = 0; i < repetitionCount; i++)
                task.Run();
            
            return watch.Elapsed.TotalMilliseconds / repetitionCount;
		}
	}

    public class StringTest : ITask
    {
        public void Run()
        {
            var str = new string('a', 10000);
        }  
    }

    public class StringBuilderTest : ITask
    {
        public void Run()
        {
            var strBuilder = new StringBuilder();

            for (var i = 0; i <= 10000; i++)
                strBuilder.Append('a');

            strBuilder.ToString();
        }
    }

    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            var benchmark = new Benchmark();

            var stringTest = benchmark.MeasureDurationInMs(new StringTest(), 10000);
            var stringBuilderTest = benchmark.MeasureDurationInMs(new StringBuilderTest(), 10000);

            Assert.Less(stringTest, stringBuilderTest);
        }
    }
}