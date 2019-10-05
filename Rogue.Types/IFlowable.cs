using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue
{
    /// <summary>
    /// Интерфейс который указывает что возможно использовать последовательный вызов
    /// </summary>
    public interface IFlowable
    {
        T GetFlowProperty<T>(string property);

        bool SetFlowProperty<T>(string property, T value);

        void SetFlowContext(object context);

        object GetFlowContext();

        void SetParentFlow(IFlowable parent);

        IFlowable GetParentFlow();
    }

    /// <summary>
    /// Определяет что метод используется <see cref="IFlowable"/>
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class FlowMethodAttribute : Attribute
    {
        public Type ContextType { get; }

        public FlowMethodAttribute() { }

        public FlowMethodAttribute(Type contextType) => ContextType = contextType;
    }
}
