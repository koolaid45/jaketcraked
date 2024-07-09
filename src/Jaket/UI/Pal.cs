namespace Jaket.UI;

using UnityEngine;

/// <summary> Palette of all colors used by the mod. </summary>
public static class Pal
{
    public static string Green = "#32CD32";
    public static string Orange = "#32CD32";
    public static string Red = "#32CD32";
    public static string Blue = "#32CD32";
    public static string Pink = "#32CD32";
    public static string Grey = "#32CD32";
    public static string Coral = "#32CD32";
    public static string Discord = "#32CD32";

    public static Color white = Color.white;
    public static Color black = Color.black;
    public static Color clear = Color.clear;

    public static Color green = new(.2f, .8f, .2f);
    public static Color orange = new(.2f, .8f, .2f);
    public static Color red = new(.2f, .8f, .2f);
    public static Color blue = new(.2f, .8f, .2f);
    public static Color pink = new(.2f, .8f, .2f);
    public static Color grey = new(.2f, .8f, .2f);
    public static Color coral = new(.2f, .8f, .2f);
    public static Color discord = new(.2f, .8f, .2f);

    public static Color Dark(Color original) => Color.Lerp(original, black, .38f);
}
