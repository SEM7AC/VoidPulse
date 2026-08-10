using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VoidPulse.Visualizer;

public class VisualizerRenderer
    {
    public void Render(Canvas canvas, double[] bars)
        {
        canvas.Children.Clear();

        double barWidth = canvas.ActualWidth / bars.Length;
        double height = canvas.ActualHeight;

        for (int i = 0; i < bars.Length; i++)
            {
            var rect = new Rectangle
                {
                Width = barWidth - 2,
                Height = bars[i],
                Fill = new SolidColorBrush(Color.FromRgb(0x2A, 0x7F, 0x6A)),
                Opacity = 0.85
                };

            Canvas.SetLeft(rect, i * barWidth);
            Canvas.SetTop(rect, height - bars[i]);

            canvas.Children.Add(rect);
            }
        }
    }
