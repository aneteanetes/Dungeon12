using Dungeon.Engine.Editable.PropertyTable;
using Dungeon.Engine.Projects;
using Dungeon.Utils.ReflectionExtensions;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon.Engine.Utils
{
    public static class SceneObjectActivator
    {
        private static List<PropertyTableRow> Constructors(List<PropertyTableRow> properties) => properties.Where(x => x.Name.Contains("Constructor")).ToList();

        public static object Activate(SceneObject obj, out string error)
        {
            error = string.Empty;

            try
            {

                var properties = obj.Properties.ToList();
                var ctors = Constructors(properties);

                object instance = Instantiate(obj, properties, ctors, out error);

                if (instance == default)
                {
                    error = "Object cannot be instantiate!";
                    return default;
                }

                var props = properties.Except(ctors);

                ActivateProperties(props, obj, instance, out error);

                obj.Instance = instance;

                return instance;
            }
            catch (Exception ex)
            {
                error = $"Ошибка материализации: {ex}";
            }

            return default;
        }

        private static object Instantiate(SceneObject obj, List<PropertyTableRow> properties, List<PropertyTableRow> ctors, out string error)
        {
            error = string.Empty;

            if (ctors.Count == 0)
            {
                try
                {
                    return obj.ClassType.NewAs<ISceneObject>();
                }
                catch (Exception ex)
                {
                    error = ex.ToString();
                    return default;
                }
            }
            else
            {
                var activeCtor = ctors.FirstOrDefault(row => row.Value.As<bool>() == true);
                if (activeCtor == default)
                {
                    error = "Для текущего объекта не выбран используемый конструктор!";
                    return default;
                }

                //всё что относится к конструктору мы не должны пытаться установить
                //var firstCtorProp = properties.FirstOrDefault(x => x.Name.ToLowerInvariant().Contains("Constructor"));
                //ctors.AddRange(properties.Skip(properties.IndexOf(firstCtorProp) + 1));
                //ниже идёт Except(ctors)

                var activeCtorIndex = int.Parse(activeCtor.Name.Replace("Constructor ", ""));
                var activeCtorInstance = obj.ClassType.GetConstructors().ElementAtOrDefault(activeCtorIndex);

                if (activeCtorInstance != default)
                {
                    var @params = activeCtorInstance.GetParameters()
                        .Select(param => properties.FirstOrDefault(p => p.Name == param.Name).Value)
                        .ToArray();

                    return obj.ClassType.New<object>(activeCtorInstance, @params);
                }
            }

            return default;
        }

        private static void ActivateProperties(IEnumerable<PropertyTableRow> props, SceneObject obj, object instance, out string error)
        {
            error = "";
            foreach (var prop in props)
            {
                try
                {
                    var ctx = new InstantiateContext()
                    {
                        instance = instance,
                        obj = obj,
                        prop = prop
                    };

                    // collections
                    if (obj.NestedCollections.ContainsKey(prop.Name))
                    {
                        InstantiateCollection(ctx, out error);
                    }
                    // nested objects
                    else if (obj.NestedProperties.ContainsKey(prop.Name))
                    {
                        InstantiateNested(ctx, out error);
                    }
                    // primitive props
                    else
                    {
                        try
                        {
                            InstantiatePrimitive(ctx, out error);
                        }
                        catch
                        {
                            //ну кароч, свойства из конструктора пытаются установиться. 
                        }
                    }
                }
                catch (Exception e)
                {
                    error = $"Property {prop.Name} binding error: {e}";
                }
            }
        }

        private static void InstantiateCollection(InstantiateContext ctx, out string error)
        {
            error = "";
            if (!ctx.obj.NestedCollections.TryGetValue(ctx.prop.Name, out var nestedObjects))
                return;

            var collection = ctx.prop.Type.New();

            foreach (var nestedObj in nestedObjects)
            {
                var nestedInstance = Activate(nestedObj, out error);
                if (nestedInstance == default)
                    continue;

                collection.CallGeneric("Add", new Type[] { ctx.prop.Type.ExtractGenericCollectionItem() }, nestedInstance);
            }

            ctx.instance.SetPropertyExprType(ctx.prop.Name, collection, ctx.prop.Type);
        }

        private static void InstantiateNested(InstantiateContext ctx, out string error)
        {
            error = "";
            if (!ctx.obj.NestedProperties.TryGetValue(ctx.prop.Name, out var nestedObject))
                return;

            var nestedInstance = Activate(nestedObject, out error);

            ctx.instance.SetPropertyExprType(ctx.prop.Name, nestedInstance, ctx.prop.Type);
        }

        private static void InstantiatePrimitive(InstantiateContext ctx, out string error)
        {
            error = "";
            var nowValue = ctx.instance.GetPropertyExprRaw(ctx.prop.Name,false);
            if (nowValue != default)
            {
                if (string.IsNullOrWhiteSpace(ctx.prop.Value?.ToString()))
                {
                    ctx.obj.Set(ctx.prop.Name, nowValue, nowValue.GetType());
                }
                return;
            }
            ctx.instance.SetPropertyExprType(ctx.prop.Name, ctx.prop.Value, ctx.prop.Type);
        }

        private class InstantiateContext
        {
            public object instance;

            public PropertyTableRow prop;

            public SceneObject obj;
        }
    }
}
