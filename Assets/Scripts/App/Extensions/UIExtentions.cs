using UnityEngine.UI;

namespace Extensions
{
    public static class UIExtentions
    {
        public static T SetAlpha<T>(this T g, float newAlpha) where T : Graphic
        {
            var color = g.color;
            color.a = newAlpha;
            g.color = color;
            return g;
        }
    }
}