namespace Dungeon12.Abilities.Scaling
{
    using Dungeon;
    using Dungeon.View.Interfaces;
    using Dungeon12.Classes;
    using Dungeon12.Entities.Enums;
    using Dungeon12.Items.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class ScaleRate<TClass> where TClass : Character
    {
        private Expression<Func<TClass, double>>[] scalingParams;

        public ScaleRate(params Expression<Func<TClass, double>>[] scaling)
        {
            scalingParams = scaling;
        }

        public ScaleRateBuilded<TClass> Build()
        {
            var infoes = new List<ScaleRateInfo>();

            foreach (var scale in scalingParams)
            {
                var info = new ScaleRateInfo();

                if (scale.Body is BinaryExpression binaryBody)
                {
                    if (binaryBody.Right is ConstantExpression constant)
                    {
                        info.Ratio = (double)constant.Value;
                    }

                    if (binaryBody.Left is UnaryExpression unary)
                    {
                        if (unary.Operand is MemberExpression member)
                        {
                            info.Property = member.Member.Name;
                        }
                    }

                    var classStat = Global.GameState.Equipment.AdditionalEquipments.FirstOrDefault(aq => aq.HostedPropertyName == info.Property);
                    if (classStat != default)
                    {
                        info.Name = classStat.StatName;
                        info.Color = classStat.Color;
                    }
                    else
                    {
                        var baseStat = typeof(Stats).All<Stats>().FirstOrDefault(x => x.ToString() == info.Property);
                        if (baseStat != default)
                        {
                            info.Name = baseStat.ToDisplay();
                            info.Color = baseStat.Color();
                        }
                    }
                }

                infoes.Add(info);
            }

            return new ScaleRateBuilded<TClass>(infoes);
        }
    }

    public class ScaleRateBuilded<TClass> where TClass : Character
    {
        public List<ScaleRateInfo> Scales { get; set; } = new List<ScaleRateInfo>();

        public ScaleRateBuilded(List<ScaleRateInfo> scales)
        {
            Scales = scales;
        }

        public long Scale(TClass @class, long value)
        {
            double result = 0;
            foreach (var scale in Scales)
            {
                result += value * @class.GetPropertyExpr<double>(scale.Property);
            }

            return (long)Math.Ceiling(result);
        }
    }


    public class ScaleRateInfo
    {
        public string Property { get; set; }

        public double Ratio { get; set; }

        public string Name { get; set; }

        public IDrawColor Color { get; set; }
    }
}