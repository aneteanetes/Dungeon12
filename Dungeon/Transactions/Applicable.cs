using Dungeon.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Transactions
{
    /// <summary>
    /// Realize generic Apply(T object) methods and CallApply
    /// For typed runtime  and runtime method bindings
    /// <para>
    /// ПИЗДЕЦ ТЫ АНГЛИЧАНИН НАХУЙ, надо реализовать Apply(T) и Discard(T) методы, а в оверрайдах вызывать this.Apply/Disacard
    /// </para>
    /// <para>
    /// Карочи, applicable слишком жирная абстракция, поэтому пока что сюда переедет `Image`
    /// </para>
    /// </summary>
    public abstract class Applicable : IApplicable
    {
        /// <summary>
        /// Флаг указывающий что этот объект обрабатывает любые события шины через диспатч
        /// </summary>
        public virtual bool Events => false;

        public Applicable()
        {
            if (Events)
            {
                Global.Events.Subscribe(@event =>
                {
                    this.Dispatch((so, arg) => so.OnEvent(arg), @event);
                });
            }
        }

        public virtual void OnEvent(object @object)
        {
            CallOnEvent(@object as dynamic);
        }
        protected virtual void CallOnEvent(dynamic obj) => this.OnEvent(obj);

        public virtual string Image { get; set; }

        private bool InApply { get; set; }

        public virtual void Apply(object @object)
        {
            if (!InApply)
            {
                InApply = true;
                this.CallApply(@object as dynamic);
            }
            InApply = false;
        }

        /// <summary>
        /// Method must call this.Apply(obj); for runtime dynamic binding 
        /// </summary>
        /// <param name="obj"></param>
        protected abstract void CallApply(dynamic obj);

        private bool InDiscard { get; set; }

        public virtual void Discard(object @object)
        {
            if (!InDiscard)
            {
                InDiscard = true;
                this.CallDiscard(@object as dynamic);
            }
            InDiscard = false;
        }

        /// <summary>
        /// Method must call this.Discard(obj); for runtime dynamic binding 
        /// </summary>
        /// <param name="obj"></param>
        protected abstract void CallDiscard(dynamic obj);
    }

    public interface IApplicable
    {
        void Apply(object @object);

        void Discard(object @object);
    }
}