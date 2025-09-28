using BenchmarkDotNet.Running;

// Entry point to run benchmarks when executing the project directly.
BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
