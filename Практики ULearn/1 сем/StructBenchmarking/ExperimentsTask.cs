using System;
using System.Collections.Generic;


namespace StructBenchmarking
{
    public interface ICreateChartData
    {
        ITask RunExperiment(int size);
    }

    public class ClassArrayCreation : ICreateChartData
    {
        public ITask RunExperiment(int size) => new ClassArrayCreationTask(size);
    }

    public class StructArrayCreation : ICreateChartData
    {
        public ITask RunExperiment(int size) => new StructArrayCreationTask(size);
    }

    public class MethodCallClass : ICreateChartData
    {
        public ITask RunExperiment(int size) => new MethodCallWithClassArgumentTask(size);
    }

    public class MethodCallStruct : ICreateChartData
    {
        public ITask RunExperiment(int size) => new MethodCallWithStructArgumentTask(size);
    }

    public class Experiments
    {
        public static ChartData BuildChartDataForArrayCreation(
            IBenchmark benchmark, int repetitionAmount)
        {
            var classesTime = GetResult(new List<ExperimentResult>(), benchmark,
                new ClassArrayCreation(), repetitionAmount);

            var structuresTime = GetResult(new List<ExperimentResult>(), benchmark,
                new StructArrayCreation(), repetitionAmount);

            return new ChartData
            {
                Title = "Create array",
                ClassPoints = classesTime,
                StructPoints = structuresTime,
            };
        }

        public static ChartData BuildChartDataForMethodCall(
            IBenchmark benchmark, int repetitionAmount)
        {
            var classesTime = GetResult(new List<ExperimentResult>(), benchmark,
                new MethodCallClass(), repetitionAmount);

            var structuresTime = GetResult(new List<ExperimentResult>(), benchmark,
                new MethodCallStruct(), repetitionAmount);

            return new ChartData
            {
                Title = "Call method with argument",
                ClassPoints = classesTime,
                StructPoints = structuresTime,
            };
        }

        public static List<ExperimentResult> GetResult(
            List<ExperimentResult> result,
            IBenchmark benchmark,
            ICreateChartData chartData,
            int repititionAmount)
        {
            for (var i = 4; i <= 9; i++)
                result.Add(new ExperimentResult(
                    (int)Math.Pow(2, i), 
                    benchmark.MeasureDurationInMs(chartData.RunExperiment((int)Math.Pow(2, i)), 
                    repititionAmount)));

            return result;
        }
    }
}