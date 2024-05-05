using DjTool.ViewModels;

namespace DjTool.Views
{
    public class TrackSpeedSelectItem
    {
        public TrackSpeed Value { get; set; }
        public string Description { get; set; }

        public override string? ToString()
        {
            return Description;
        }
    }

}
