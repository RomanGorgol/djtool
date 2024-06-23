using DjTool.ViewModels;
using System.Windows.Media;

namespace DjTool.Tools
{
    class SpeedToColorMapper
    {
        private static Dictionary<TrackSpeed, Brush> speedToColorMap = new Dictionary<TrackSpeed, Brush>()
        {
            { TrackSpeed.F, Brushes.Green },
            { TrackSpeed.MF, Brushes.GreenYellow},
            { TrackSpeed.M, Brushes.Yellow},
            { TrackSpeed.SM, Brushes.Orange }, // new SolidColorBrush(Color.FromRgb(252, 78, 3))},
            { TrackSpeed.S, Brushes.Red}
        };

        public static Brush Map(TrackSpeed speed)
        {
            return speedToColorMap.TryGetValue(speed, out var value)
                ? value
                : Brushes.Transparent;

        }
    }
}
