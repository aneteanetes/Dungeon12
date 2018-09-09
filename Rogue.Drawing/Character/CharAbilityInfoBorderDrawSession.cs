namespace Rogue.Drawing.Character
{
    public class CharAbilityInfoBorderDrawSession : CharMainInfoBorderDrawSession
    {
        public CharAbilityInfoBorderDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                
            };
        }

        protected override string InLoop(string stringBuffer)
        {
            return " " + DrawHelp.Border(true, 3) + stringBuffer.Remove(stringBuffer.Length - 49) + DrawHelp.Border(true, 3) + stringBuffer.Remove(stringBuffer.Length - 50) + DrawHelp.Border(true, 3);
        }
    }
}
