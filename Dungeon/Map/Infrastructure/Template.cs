namespace Dungeon.Map.Infrastructure
{
    using System;

    /// <summary>
    /// Маппинг символа в шаблоне к типу объекта
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TemplateAttribute : Attribute
    {
        public TemplateAttribute(string template)
        {
            this.Template = template;
        }

        public string Template { get; }
    }
}
