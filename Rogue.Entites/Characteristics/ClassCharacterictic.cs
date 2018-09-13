using Rogue.Entites.Enums;
using Rogue.Entites.Structural;

namespace Rogue.Entites.Characteristics
{
    public class ClassCharacterictic : Characteristic<Class>
    {
        public ClassCharacterictic(Class @class) : base(@class.ToDisplay()) { }
        
        protected override void CallApply(dynamic obj)
        {
            this.Apply(obj);
        }

        protected override void CallDiscard(dynamic obj)
        {
            this.Discard(obj);
        }
    }
}
