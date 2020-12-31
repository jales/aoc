using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using BenchmarkDotNet.Validators;
using Perfolizer.Horology;
using static AoC.Infrastructure.Puzzles.PuzzleRegistrar;

namespace AoC.Infrastructure.Puzzles
{
    public static class PuzzleBenchmarkRunner
    {
        public static async Task<Summary> Benchmark(PuzzleRegistration registration)
        {
            registration.ConfigurePuzzle();

            var benchmarkType = IsSequential(registration) ? typeof(SequentialPuzzleBenchmark<>) : typeof(PuzzleBenchmark<>);
            benchmarkType = benchmarkType.MakeGenericType(registration.PuzzleType);

            return await Task.Run(() => BenchmarkRunner.Run(benchmarkType, BuildBenchmarkConfig()));
        }

        private static ManualConfig BuildBenchmarkConfig()
        {
            var config = ManualConfig.CreateEmpty()
               .AddColumnProvider(
                    DefaultColumnProviders.Instance)
               .AddColumn(
                    StatisticColumn.Min,
                    StatisticColumn.Max,
                    StatisticColumn.P95)
               .AddDiagnoser(
                    MemoryDiagnoser.Default)
               .AddAnalyser(
                    EnvironmentAnalyser.Default,
                    OutliersAnalyser.Default,
                    MinIterationTimeAnalyser.Default,
                    RuntimeErrorAnalyser.Default,
                    ZeroMeasurementAnalyser.Default,
                    BaselineCustomAnalyzer.Default)
               .AddValidator(
                    BaselineValidator.FailOnError,
                    SetupCleanupValidator.FailOnError,
                    JitOptimizationsValidator.DontFailOnError,
                    RunModeValidator.FailOnError,
                    GenericBenchmarksValidator.DontFailOnError,
                    DeferredExecutionValidator.FailOnError,
                    ParamsAllValuesValidator.FailOnError)
               .AddLogger(
                    NullLogger.Instance)
               .AddExporter(
                    NullExporter.Instance)
               .WithUnionRule(
                    ConfigUnionRule.Union)
               .WithSummaryStyle(
                    SummaryStyle.Default
                       .WithSizeUnit(SizeUnit.B)
                       .WithTimeUnit(TimeUnit.Nanosecond)
                       .WithCultureInfo(CultureInfo.InvariantCulture))
               .WithOptions(
                    ConfigOptions.Default)
               .WithOption(
                    ConfigOptions.KeepBenchmarkFiles, false)
               .WithOption(
                    ConfigOptions.DisableLogFile, true)
               .AddJob(
                    Job.MediumRun
                       .WithIterationCount(20)
                       .WithRuntime(CoreRuntime.Core50)
                       .WithPlatform(Platform.X64)
                       .WithToolchain(InProcessNoEmitToolchain.DontLogOutput));

            return config;
        }

        private class NullExporter : IExporter
        {
            public static IExporter Instance { get; } = new NullExporter();

            public string Name => nameof(NullExporter);

            public void ExportToLog(Summary summary, ILogger logger) { }

            public IEnumerable<string> ExportToFiles(Summary summary, ILogger consoleLogger)
            {
                yield break;
            }
        }

        public class PuzzleBenchmark<TPuzzle>
        {
            private readonly Func<string, Puzzle> _factory;
            private readonly string _input;
            private Puzzle _instance = null!;

            public PuzzleBenchmark()
            {
                _factory = GetPuzzleFactory(typeof(TPuzzle));
                _input = GetOfficialRunForPuzzle(typeof(TPuzzle)).Input;
            }

            [Benchmark]
            public void Parse()
            {
                Consume(_factory(_input));
            }

            [IterationSetup(Targets = new[] {nameof(Part1), nameof(Part2)})]
            public void Setup()
            {
                _instance = _factory(_input);
            }

            [Benchmark]
            public void Part1()
            {
                Consume(_instance.BenchmarkPart1());
            }

            [Benchmark]
            public void Part2()
            {
                Consume(_instance.BenchmarkPart2());
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void Consume(object input) { }
        }

        public class SequentialPuzzleBenchmark<TPuzzle>
        {
            private readonly Func<string, Puzzle> _factory;
            private readonly string _input;
            private Puzzle _instance = null!;

            public SequentialPuzzleBenchmark()
            {
                _factory = GetPuzzleFactory(typeof(TPuzzle));
                _input = GetOfficialRunForPuzzle(typeof(TPuzzle)).Input;
            }

            [Benchmark]
            public void Parse()
            {
                Consume(_factory(_input));
            }

            [IterationSetup(Target = nameof(Part1))]
            public void Part1Setup()
            {
                _instance = _factory(_input);
            }

            [Benchmark]
            public void Part1()
            {
                Consume(_instance.BenchmarkPart1());
            }

            [IterationSetup(Target = nameof(Part2))]
            public void Part2Setup()
            {
                _instance = _factory(_input);
                _instance.BenchmarkPart1();
            }

            [Benchmark]
            public void Part2()
            {
                Consume(_instance.BenchmarkPart2());
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void Consume(object input) { }
        }
    }
}