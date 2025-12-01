using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using PathfinderTest.Algorithm;
using PathfinderTest.Utils;
using Microsoft.VSDiagnostics;

[SimpleJob(RuntimeMoniker.Net80)]
[CPUUsageDiagnoser]
public class AstarBenchmarks
{
    private bool[, ] grid;
    private string[] lines;
    private int startX, startY;
    private int goalX, goalY;
    private FieldInfo? visualizeField;
    private static readonly BindingFlags FieldFlags = BindingFlags.NonPublic | BindingFlags.Instance;
    [Params(false, true)]
    public bool Visualize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);
        lines = Layout.GetLines(random);
        grid = Layout.GetGrid(lines);
        var startCoord = Node.SelectRandom(random, grid);
        startX = startCoord.Item1;
        startY = startCoord.Item2;
        var goalCoord = startCoord;
        while (goalCoord == startCoord)
        {
            goalCoord = Node.SelectRandom(random, grid);
        }

        goalX = goalCoord.Item1;
        goalY = goalCoord.Item2;
        visualizeField = typeof(Astar).GetField("shouldVisualize", FieldFlags);
    }

    private void RunOnce(bool visualize)
    {
        var astar = new Astar();
        if (visualizeField != null)
        {
            visualizeField.SetValue(astar, visualize);
        }

        var start = new Node(startX, startY);
        var goal = new Node(goalX, goalY);
        astar.Algorithm(grid, start, goal, lines);
    }

    [Benchmark]
    public void Astar_Run() => RunOnce(Visualize);
}