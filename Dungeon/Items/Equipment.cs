namespace Dungeon.Items
{
    using Dungeon.Transactions;
    using Dungeon.View.Interfaces;

    /// <summary>
    /// <para>
    /// Реализация:
    /// </para>
    /// <para>
    /// public void Apply/Discard(TTarget obj)
    /// </para>
    /// <para>
    /// protected override void CallApply(dynamic obj) => this.Apply(obj);
    /// </para>
    /// protected override void CallDiscard(dynamic obj) => this.Discard(obj);
    /// <para>
    /// </para>
    /// </summary>
    public abstract class Equipment : Applicable
    {
        public string Identify { get; set; }

        public virtual string Title { get; set; }

        public IDrawColor Color { get; set; }
    }
}