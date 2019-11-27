using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon
{
    /// <summary>
    /// Интерфейс который указывает что возможно использовать последовательный вызов
    /// </summary>
    public interface IFlowable
    {
        T GetFlowProperty<T>(string property, T @default = default);

        bool SetFlowProperty<T>(string property, T value);

        void SetFlowContext(object context);

        object GetFlowContext();

        void SetParentFlow(IFlowable parent);

        IFlowable GetParentFlow();
    }

    /// <summary>
    /// Определяет что метод используется <see cref="IFlowable"/>
    /// </summary>
    [Obsolete("Flow методы более не используются, вместо этого следует использовать GameComponent, Entity")]
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class FlowMethodAttribute : Attribute
    {
        public Type ContextType { get; }

        public FlowMethodAttribute() { }

        public FlowMethodAttribute(Type contextType) => ContextType = contextType;
    }

    /// <summary>
    /// Указывает что метод явно вызывает каскадную логику
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    [Obsolete("Явные Flow методы более не используются, вместо этого есть прямая ссылка на GameComponent или Entity")]
    public sealed class ExcplicitFlowMethodAttribute : Attribute
    {
    }
}
