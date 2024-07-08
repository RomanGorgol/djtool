using DjTool.ViewModels;
using System.Windows.Media;

namespace DjTool.Tools
{
    class SpeedToColorMapper
    {
        private static Dictionary<TrackSpeed, Brush> speedToColorMap = new Dictionary<TrackSpeed, Brush>()
        {
            { TrackSpeed.S, new SolidColorBrush(Color.FromRgb(4, 186, 77)) },
            { TrackSpeed.SM, Brushes.GreenYellow},
            { TrackSpeed.M, Brushes.Yellow},
            { TrackSpeed.MF, Brushes.Orange }, // new SolidColorBrush(Color.FromRgb(252, 78, 3))},
            { TrackSpeed.F, new SolidColorBrush(Color.FromRgb(235, 64, 52))}
        };

        public static Brush Map(TrackSpeed speed)
        {
            return speedToColorMap.TryGetValue(speed, out var value)
                ? value
                : Brushes.Transparent;

        }
    }
}
