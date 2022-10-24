using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Dungeon.Monogame
{
    public partial class GameClient
    {
        private void LoadPenumbra()
        {
            if (_settings.Add2DLighting)
            {
                var penumbraShaders = new Dictionary<string, Effect>();
                var penumbraShaderPaths = new (string path, string key)[]
                {
                ( $"Dungeon.Monogame.Resources.Shaders.PenumbraHull.xnb" ,"PenumbraHull"),
                ( $"Dungeon.Monogame.Resources.Shaders.PenumbraLight.xnb" ,"PenumbraLight"),
                ( $"Dungeon.Monogame.Resources.Shaders.PenumbraShadow.xnb" ,"PenumbraShadow"),
                ( $"Dungeon.Monogame.Resources.Shaders.PenumbraTexture.xnb" ,"PenumbraTexture")
                };

                var asm = Assembly.GetExecutingAssembly();

                foreach (var (path, key) in penumbraShaderPaths)
                {
                    using (Stream stream = asm.GetManifestResourceStream(path))
                    {
                        if (stream.CanSeek)
                        {
                            stream.Seek(0, SeekOrigin.Begin);
                        }
                        penumbraShaders.Add(key, Content.Load<Effect>(path, stream));
                    }
                }
                penumbra = new PenumbraComponent(this, penumbraShaders);
                penumbra.Initialize();
                if (_settings.Add2DLighting)
                {
                    if (_settings.Add2DLighting != default)
                    {
                        penumbra.AmbientColor = _settings.AmbientColor2DLight;
                    }
                    Components.Add(penumbra);
                }


                //penumbra.Lights.Add(SunLight);
            }
        }

        readonly Light SunLight = new PointLight
        {
            Scale = new Vector2(3700f),
            ShadowType = ShadowType.Illuminated, // Will not lit hulls themselves,
            Rotation = 0.8707998f,
            Position = new Vector2(-2660, -500),
            //ConeDecay = 5f,
            Radius = 10000,
        };

        Light light = new PointLight()
        {
            Scale = new Vector2(300),
            ShadowType = ShadowType.Occluded,
            Radius = 100
        };

        private const float Seconds = 1320;
        private float RotationUnit = (1.5708f - 0.8707998f) / Seconds * 2;
        private float BaseIlluminationUnit = (12350 - 3700) / Seconds;
        private float IllumnationUnit
        {
            get
            {
                float illum = BaseIlluminationUnit;

                if (DungeonGlobal.Time.Hours >= 6 && DungeonGlobal.Time.Hours < 18)
                {
                    illum = BaseIlluminationUnit * 2;
                }

                if (DungeonGlobal.Time.Hours >= 18)
                {

                    illum = BaseIlluminationUnit * 4;
                }

                return illum;
            }
        }

        private readonly float PositionUnit = 6600 / Seconds;

        private void CalculateSunlight()
        {
            var time = DungeonGlobal.Time;
            AddSunLight(1, time);
        }

        private void WhenTimeSetted(Dungeon.Time was, Dungeon.Time now)
        {
            var wasTime = new DateTime(1, 1, 1, was.Hours, was.Minutes, 0);
            var nowTime = new DateTime(1, 1, 1, now.Hours, now.Minutes, 0);

            var minutes = (wasTime - nowTime).TotalMinutes;
            if (minutes > 0)
            {
                //throw new Exception("Двигаем время назад, да?");

                wasTime = new DateTime(1, 1, 1, was.Hours, was.Minutes, 0);
                nowTime = new DateTime(1, 1, 2, now.Hours, now.Minutes, 0);

                minutes = (wasTime - nowTime).TotalMinutes;
            }

            AddSunLight((int)Math.Abs(minutes), was);
        }

        private void AddSunLight(int minutes, Time was)
        {
            for (int i = 0; i < minutes; i++)
            {
                was.AddMinute();
                if (was.Hours >= 4 && was.Hours < 22)
                {
                    this.SunLight.Rotation += RotationUnit;
                    this.SunLight.Position += new Vector2(PositionUnit, 0);

                    if (was.Hours >= 13)
                    {
                        this.SunLight.Scale -= new Vector2(IllumnationUnit);
                    }
                    else
                    {
                        this.SunLight.Scale += new Vector2(IllumnationUnit);
                    }
                }

                if (was.Hours >= 22)
                {
                    SunLight.Rotation = 0.8707998f;
                    SunLight.Scale = new Vector2(3700f);
                    SunLight.Position = new Vector2(-2660, -500);
                }
            }
        }
    }
}
