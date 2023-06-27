using System.Collections.Generic;
using System.Drawing;
using GeometryTasks;

namespace GeometryPainting
{
    public static class SegmentExtensions
    {
        private static readonly Dictionary<Segment, Color> StoredColors = new Dictionary<Segment, Color>();

        public static void SetColor(this Segment segment, Color color)
        {
            StoredColors[segment] = color;
        }

        public static Color GetColor(this Segment segment)
        {
            return StoredColors.ContainsKey(segment) ? StoredColors[segment] : Color.Black;
        }
    }
}