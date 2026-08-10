namespace VoidPulse.Visualizer;

public class VisualizerEngine
    {
    private readonly Random _rand = new();

    public double[] GenerateBars(int count, double maxHeight)
        {
        var bars = new double[count];

        for (int i = 0; i < count; i++)
            bars[i] = _rand.NextDouble() * maxHeight;

        return bars;
        }
    }
