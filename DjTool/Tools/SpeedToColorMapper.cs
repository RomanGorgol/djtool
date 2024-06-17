using DjTool.ViewModels;
using System.Windows.Media;

namespace DjTool.Tools
{
    class SpeedToColorMapper
    {
        private static Dictionary<TrackSpeed, Brush> speedToCOlorMap = new Dictionary<TrackSpeed, Brush>()
        {
            { TrackSpeed.F, Brushes.Green },
            { TrackSpeed.M, Brushes.Orange },
            { TrackSpeed.M, Brushes.Orange },
            { TrackSpeed.S, Brushes.Red}
        };

        public static Brush Map(TrackSpeed speed)
        {
            return speedToCOlorMap.TryGetValue(speed, out var value)
                ? value
                : Brushes.Transparent;

        }
    }
}
