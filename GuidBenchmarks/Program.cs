﻿// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using GuidBenchmarks;

Console.WriteLine("Hello, World!");

BenchmarkRunner.Run<GuidBenches>();