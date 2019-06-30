namespace ProjectMercury
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;
    using FastMember;
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
                VisitNodeAttribute(attribute,element);
            }

            string visitMethodName = "Visit";
            visitMethodName += !string.IsNullOrEmpty(CurrentNamespace)
                ? CurrentNamespace
                : "Node";

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

            if (element.Name == "Modifiers")
            {
                target = emitter;
            }
        }

        private string CurrentType = string.Empty;
        private string CurrentNamespace = string.Empty;

        private void VisitNodeAttribute(XAttribute xAttribute, XElement element)
        {
            if (xAttribute.Name == "Type" && xAttribute.Value.Contains(":"))
            {
                var delimiterPos = xAttribute.Value.IndexOf(":");
                CurrentNamespace = xAttribute.Value.Substring(0, delimiterPos);
                CurrentType = xAttribute.Value.Substring(delimiterPos + 1);
            }
            else if (xAttribute.Name == "Type" && element.Name == "Item")
            {
                var value = xAttribute.Value;

                CurrentType = value.Replace("ProjectMercury.Emitters.", "")
                    .Replace("ProjectMercury.Modifiers.", "");

                CurrentNamespace = value.Replace("ProjectMercury.", "")
                    .Replace("." + CurrentType, "");
            }
        }

        private object target;

        private Emitter emitter;

        private void VisitEmitters(XElement element)
        {
            var typeName = $"ProjectMercury.Emitters.{CurrentType}, ProjectMercury";
            emitter = (Emitter)Activator.CreateInstance(Type.GetType(typeName));
            _particleEffect.Add(emitter);

            target = emitter;
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
                var typeName = $"ProjectMercury.Modifiers.{CurrentType}, ProjectMercury";
                modifier = (Modifier)Activator.CreateInstance(Type.GetType(typeName));
                emitter.Modifiers.Add(modifier);

                target = modifier;
            }
        }

        private void VisitNode(XElement element)
        {
            if (element.Name == "Modifiers")
            {
                emitter.Modifiers = new Modifiers.ModifierCollection();
                return;
            }

            if (target == null)
                return;

            var accessor = TypeAccessor.Create(target.GetType());
            var name = element.Name.ToString();

            object value = null;

            var prop = accessor.GetMembers().FirstOrDefault(p => p.Name == name);
            if (prop == null)
                return;


            var propType = prop.Type;
            switch (propType)
            {
                case Type _ when propType == typeof(bool):
                    {
                        value = bool.Parse(element.Value);
                        break;
                    }
                case Type _ when propType == typeof(float):
                    {
                        value = float.Parse(element.Value.Replace(".",","));
                        break;
                    }
                case Type _ when propType == typeof(int):
                    {
                        value = int.Parse(element.Value);
                        break;
                    }
                case Type _ when propType == typeof(Vector2):
                    {
                        value = ParseVector2(element);
                        break;
                    }
                case Type _ when propType == typeof(Vector3):
                    {
                        value = ParseVector3(element);
                        break;
                    }
                case Type _ when propType == typeof(string):
                    {
                        value = element.Value;
                        break;
                    }
                case Type _ when propType == typeof(VariableFloat):
                    {
                        value = ParseVariableFloat(element);
                        break;
                    }
                case Type _ when propType == typeof(VariableFloat3):
                    {
                        value = ParseVariableFloat3(element);
                        break;
                    }
                case Type _ when propType == typeof(EmitterBlendMode):
                    {
                        value = Enum.Parse<EmitterBlendMode>(element.Value);
                        break;
                    }
                default:
                    break;
            }

            accessor[target, name] = value;
        }

        private Vector3 ParseVector3(XElement element)
        {
            var values = element.Value
                .Replace(".",",")
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
                .Replace(".",",")
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            Vector2 v = new Vector2();
            v.X = float.Parse(values[0]);
            v.Y = float.Parse(values[1]);

            return v;
        }
               
        private VariableFloat ParseVariableFloat(XElement element)
        {
            return  new VariableFloat()
            {
                Value = float.Parse(element.Element("Value").Value.Replace(".",",")),
                Variation = float.Parse(element.Element("Variation").Value.Replace(".", ","))
            };
        }

        private VariableFloat3 ParseVariableFloat3(XElement element)
        {
            return new VariableFloat3()
            {
                Value = ParseVector3(element.Element("Value")),
                Variation = ParseVector3(element.Element("Variation"))
            };
        }

        private static string Intersect(string a, string b)
        {
            string same = string.Empty;

            var aEnumerator = a.GetEnumerator();
            var bEnumerator = b.GetEnumerator();

            while (aEnumerator.MoveNext() && bEnumerator.MoveNext())
            {
                if (aEnumerator.Current == bEnumerator.Current)
                {
                    same += aEnumerator.Current;
                }
                else
                {
                    break;
                }
            }

            return same;
        }

    }
}