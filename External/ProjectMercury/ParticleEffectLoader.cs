namespace ProjectMercury
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using ProjectMercury.Controllers;
    using ProjectMercury.Emitters;
    using ProjectMercury.Modifiers;
    using YAXLib;

    public class ParticleEffectLoader
    {
        private ParticleEffect _particleEffect = new ParticleEffect();
        private XDocument xDocument;

        public ParticleEffectLoader(Stream stream) => xDocument = XDocument.Load(stream);

        public ParticleEffect Load()
        {
            foreach (var element in xDocument.Root.Elements())
            {
                Visit(element);
            }

            return _particleEffect;
        }

        private void Visit(XElement element)
        {
            foreach (var attribute in element.Attributes())
            {
                VisitNodeAttribute(attribute);
            }

            string visitMethodName = "Visit";
            visitMethodName += !string.IsNullOrEmpty(CurrentNamespace)
                ? CurrentNamespace
                : element.Name.ToString();

            var visitMethod = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(x => x.Name == visitMethodName);

            if (visitMethod != null)
            {
                visitMethod.Invoke(this, new object[] { element });
            }

            CurrentNamespace = string.Empty;
            CurrentType = string.Empty;

            foreach (var child in element.Elements())
            {
                Visit(child);
            }
        }

        private string CurrentType = string.Empty;
        private string CurrentNamespace = string.Empty;

        private void VisitNodeAttribute(XAttribute xAttribute)
        {
            if (xAttribute.Name == "Type" && xAttribute.Value.Contains(":"))
            {
                var delimiterPos = xAttribute.Value.IndexOf(":");
                CurrentNamespace = xAttribute.Value.Substring(0, delimiterPos);
                CurrentType = xAttribute.Value.Substring(delimiterPos + 1);
            }
        }

        private Emitter emitter;

        private void VisitEmitters(XElement element)
        {
            emitter = (Emitter)Activator.CreateInstance(Type.GetType(CurrentType));
            _particleEffect.Add(emitter);
        }

        private void VisitName(XElement element)
        {
            emitter.Name = element.Value;
        }

        private void VisitBudget(XElement element)
        {
            emitter.Budget = int.Parse(element.Value);
        }

        private void VisitTerm(XElement element)
        {
            emitter.Term = float.Parse(element.Value);
        }

        private void VisitReleaseQuantity(XElement element)
        {
            emitter.ReleaseQuantity = int.Parse(element.Value);
        }

        private void VisitReleaseSpeed(XElement element)
        {
            emitter.ReleaseSpeed = new VariableFloat()
            {
                Value = float.Parse(element.Element("Value").Value),
                Variation = float.Parse(element.Element("Variation").Value)
            };
        }

        private void VisitReleaseColour(XElement element)
        {
            emitter.ReleaseColour = new VariableFloat3()
            {
                Value = ParseVector3(element.Element("Value")),
                Variation = ParseVector3(element.Element("Variation"))
            };
        }

        private void VisitReleaseOpacity(XElement element)
        {
            emitter.ReleaseOpacity = new VariableFloat()
            {
                Value = float.Parse(element.Element("Value").Value),
                Variation = float.Parse(element.Element("Variation").Value)
            };
        }

        private void VisitReleaseScale(XElement element)
        {
            emitter.ReleaseScale = new VariableFloat()
            {
                Value = float.Parse(element.Element("Value").Value),
                Variation = float.Parse(element.Element("Variation").Value)
            };
        }

        private void VisitReleaseRotation(XElement element)
        {
            emitter.ReleaseRotation = new VariableFloat()
            {
                Value = float.Parse(element.Element("Value").Value),
                Variation = float.Parse(element.Element("Variation").Value)
            };
        }

        private void VisitReleaseImpulse(XElement element)
        {
            emitter.ReleaseImpulse = ParseVector2(element);
        }

        private void VisitParticleTextureAssetName(XElement element)
        {
            emitter.ParticleTextureAssetName = element.Value;
        }

        private void VisitBlendMode(XElement element)
        {
            emitter.BlendMode = Enum.Parse<EmitterBlendMode>(element.Value);
        }

        private void VisitTriggerOffset(XElement element)
        {
            emitter.TriggerOffset = ParseVector2(element);
        }

        private void VisitMinimumTriggerPeriod(XElement element)
        {
            emitter.MinimumTriggerPeriod = float.Parse(element.Value);
        }

        private void VisitRadius(XElement element)
        {
            if (emitter is CircleEmitter circleEmitter)
            {
                circleEmitter.Radius = float.Parse(element.Value);
            }
        }

        private void VisitRing(XElement element)
        {
            if (emitter is CircleEmitter circleEmitter)
            {
                circleEmitter.Ring = bool.Parse(element.Value);
            }
        }

        private void VisitRadiate(XElement element)
        {
            if (emitter is CircleEmitter circleEmitter)
            {
                circleEmitter.Radiate = bool.Parse(element.Value);
            }
        }

        Modifier modifier;

        private void VisitModifiers(XElement element)
        {
            if (emitter.Modifiers == null)
            {
                emitter.Modifiers = new Modifiers.ModifierCollection();
            }
            else
            {
                modifier = (Modifier)Activator.CreateInstance(Type.GetType(CurrentType));
            }
        }

        private void VisitInitial(XElement element)
        {
            if (modifier is OpacityModifier opacityModifier)
            {
                opacityModifier.Initial = float.Parse(element.Value);
            }
        }

        private void VisitUltimate(XElement element)
        {
            if (modifier is OpacityModifier opacityModifier)
            {
                opacityModifier.Ultimate = float.Parse(element.Value);
            }
        }

        private void VisitInitialScale(XElement element)
        {
            if (modifier is ScaleModifier scaleModifier)
            {
                scaleModifier.InitialScale = float.Parse(element.Value);
            }
        }

        private void VisitUltimateScale(XElement element)
        {
            if (modifier is ScaleModifier scaleModifier)
            {
                scaleModifier.UltimateScale = float.Parse(element.Value);
            }
        }

        private void VisitDampingCoefficient(XElement element)
        {
            if (modifier is DampingModifier dampingModifier)
            {
                dampingModifier.DampingCoefficient = float.Parse(element.Value);
            }
        }

        private void VisitInitialRate(XElement element)
        {
            if (modifier is RotationRateModifier rotationRateModifier)
            {
                rotationRateModifier.InitialRate = float.Parse(element.Value);
            }
        }

        private void VisitFinalRate(XElement element)
        {
            if (modifier is RotationRateModifier rotationRateModifier)
            {
                rotationRateModifier.FinalRate = float.Parse(element.Value);
            }
        }

        private void VisitRotationRate(XElement element)
        {
            if (modifier is RotationModifier rotationModifier)
            {
                rotationModifier.RotationRate = float.Parse(element.Value);
            }
        }

        private Vector3 ParseVector3(XElement element)
        {
            var values = element.Value
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            Vector3 v = new Vector3();
            v.X = float.Parse(values[0]);
            v.Y = float.Parse(values[1]);
            v.Z = float.Parse(values[2]);

            return v;
        }

        private Vector2 ParseVector2(XElement element)
        {
            var values = element.Value
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            Vector2 v = new Vector2();
            v.X = float.Parse(values[0]);
            v.Y = float.Parse(values[1]);

            return v;
        }
    }
}