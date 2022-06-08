using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Colors 
{
    public static Color MistplayCyan { get { return new Color32(35, 234, 203, 255); } }
    public static Color MistplayBlue { get { return new Color32(28, 28, 84, 255); } }
    public static Color MistplayRed { get { return new Color32(251,73,85, 255); }}
    public static Color MistplayPurple { get { return new Color32(125,90,255, 255); }}
    public static Color NiceRed { get { return new Color32(255, 81, 118, 255); } }
    public static Color NiceOrange { get { return new Color32(255, 198, 90, 255); } }
    public static Color NiceYellow { get { return new Color32(255, 216, 58, 255); } }
    public static Color NiceCyan { get { return new Color32(71, 224, 179, 255); } }
    public static Color NiceBlue { get { return new Color32(48, 120, 255, 255); } }
    public static Color NicePurple { get { return new Color32(188, 107, 255, 255); } }
    public static Color NicePink { get { return new Color32(248, 75, 150, 255); }}
    public static Color NiceRandom => (new Color[5] { NiceRed, NiceOrange, NiceCyan, NiceBlue, NicePurple })[Random.Range(0, 5)];
    public static Color Grey(float value)
    {
        return new Color(value, value, value, 1f);
    }

    public static Color NiceColorByIndex(int index)
    {
        return (new Color[5] { NiceRed, NiceOrange, NiceCyan, NiceBlue, NicePurple })[index];
    }

    public static Color NiceRandomColor(float saturation = 101f)
    {
        float sat = saturation/255f;
        Color color = Random.ColorHSV(0,1,sat,sat,1,1);
        return color;
    }

    public static Color NiceRandomColor(float hue, float saturation = 101f)
    {
        float sat = saturation/255f;
        Color color = Random.ColorHSV(hue,hue,sat,sat,1,1);
        return color;
    }

    public static Color ModifyCopy(Color color, float saturation)
    {
        float h,s,v;
        Color.RGBToHSV(color, out h, out s, out v);

        s = saturation / 255f;

        Color newColor = Color.HSVToRGB(h,s,v);
        return newColor;
    }

    public static Color AverageHue(Color a, Color b)
    {
        Color newColor = Color.white;

        newColor.r = Mathf.Sqrt(Mathf.Pow(a.r, 2) + Mathf.Pow(b.r, 2)) * .5f;
        newColor.g = Mathf.Sqrt(Mathf.Pow(a.g, 2) + Mathf.Pow(b.g, 2)) * .5f;
        newColor.b = Mathf.Sqrt(Mathf.Pow(a.b, 2) + Mathf.Pow(b.b, 2)) * .5f;

        float h,s,v;
        Color.RGBToHSV(a, out h, out s, out v);

        float nh, ns, nv;
        Color.RGBToHSV(newColor, out nh, out ns, out nv);

        Color finalColor = Color.HSVToRGB(nh, s, v);

        return finalColor;
    }
}

public static class ColorExtensions
{
    public static Color Grayscaled(this Color color, float amount = 1)
    {
        // 0.3R+0.6G+0.11B
        float r = .3f;
        float g = .6f;
        float b = .11f;

        float w = r * color.r + g * color.g + b * color.b;

        return Color.Lerp(color, new Color(w, w, w), amount);
    }

    public static Color Alpha(this Color color, float a)
    {
        return new Color(color.r, color.g, color.b, a);
    }

    public static Color Saturate(this Color color, float amount)
    {
        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v);
        s += amount;
        if(s > 1) s = 1f;
        return Color.HSVToRGB(h, s, v);
    }

    public static Color Shift(this Color color, float shift)
    {
        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v);
        h += shift / 360f;
        if(h > 1) h -= 1f;
        return Color.HSVToRGB(h, s, v);
    }

    public static Color Complementary(this Color color)
    {
        float h,s,v = 0;
        Color.RGBToHSV(color, out h, out s, out v);

        h *= 360f;
        h += 180f;
        if(h > 360f) h -= 360f;
        h /= 360f;

        return Color.HSVToRGB(h,s,v);
    }

    public static Color Analogous(this Color color, float spacing = 25f)
    {
        float h,s,v = 0;
        Color.RGBToHSV(color, out h, out s, out v);

        h *= 360f;
        float side = ((360 - h) < h) ? -1f : 1f;
        h += spacing * side;
        if(h > 360f) h -= 360f;
        h /= 360f;

        return Color.HSVToRGB(h,s,v);
    }
}
