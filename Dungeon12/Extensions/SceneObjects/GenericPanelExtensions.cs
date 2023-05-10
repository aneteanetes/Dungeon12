using Dungeon;
using Dungeon.Drawing;
using Dungeon.View.Interfaces;
using Dungeon12.Entities.Plates;
using System.ComponentModel;

namespace Dungeon12.Extensions.SceneObjects
{
    internal static class GenericPanelExtensions
    {
        public static IDrawText Title(this GenericData data)
        {
            return data.Title.Default().InSize(24).InBold().Calibri();
        }

        public static IDrawText Rank(this GenericData data)
        {
            if (data.Rank==default)
                return null;

            return data.Rank.Default().InColor(DrawColor.Gray).InSize(22);
        }

        public static IDrawText Subtype(this GenericData data)
        {
            if (data.Subtype==default)
                return null;

            return data.Subtype.Default().InColor(DrawColor.DarkGray);
        }

        public static IDrawText Resources(this GenericData data)
        {
            if(data.Resources.IsEmpty())
                return null;

            var text = "".Default();
            foreach (var res in data.Resources)
            {
                if (!text.IsEmptyInside)
                    text.Append(Default(" / "));

                text.Append(Default(res.ToString(), res.Color));
            }

            return text;
        }

        public static IDrawText Radius(this GenericData data)
        {
            if (data.Radius==default)
                return null;

            return $"{Global.Strings["Radius"]}: {data.Radius}".Default();
        }

        public static IDrawText Duration(this GenericData data)
        {
            if (data.Duration==default)
                return null;

            return data.Duration.ToString().Default();
        }

        public static IDrawText Cooldown(this GenericData data)
        {
            if (data.Cooldown==default)
                return null;

            var text = "".Default();
            text.Append((Global.Strings["Cooldown"]+": ").Default());
            if(data.Cooldown.Type != Entities.Cooldowns.CooldownType.Battle)
            text.Append(data.Cooldown.Value.ToString().Default());
            text.Append(Global.Strings[data.Cooldown.Type].Default().InSize(10));

            return text;
        }

        public static IDrawText Charges(this GenericData data)
        {
            if (data.Charges==default)
                return null;

            return $"{data.Charges} {Global.Strings["Charges"]}".Default();
        }

        public static IDrawText Requires(this GenericData data)
        {
            if (data.Requires.IsEmpty())
                return null;

            var text = "".Default();
            foreach (var req in data.Requires)
            {
                text.Append(req.Text.Default(req.Color));
                text.AppendNewLine();
            }

            return text;
        }

        public static IDrawText RequireLevel(this GenericData data)
        {
            if(data.RequiresLevel==default)
                return null;

            return $"{Global.Strings["Requires"]} {data.RequiresLevel}-{Global.Strings["nth"]} {Global.Strings["level"]}".Default();
        }

        public static IDrawText Fraction(this GenericData data)
        {
            if (data.Fraction== Entities.Enums.Fraction.Neutral)
                return null;

            return $"{Global.Strings["Fraction"]}: {Global.Strings[data.Fraction]}".Default();
        }

        public static IDrawText Description(this GenericData data)
        {
            var txt = data.Text.Default();
            txt.LineSpacing = -10;

            return txt;
        }

        public static IDrawText Rune(this GenericData data)
        {
            if (data.Rune==default)
                return null;

            return $"{data.Rune.Name} ({data.Rune.SetName})".Default().InColor(DrawColor.LawnGreen);
        }

        private static DrawText Default(this string txt, DrawColor color=null) => txt.AsDrawText().InSize(20).InColor(color ?? Global.CommonColorLight).Calibri().As<DrawText>();
    }
}
