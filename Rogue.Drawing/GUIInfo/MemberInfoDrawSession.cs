namespace Rogue.Drawing.GUIInfo
{
    using Rogue.View.Interfaces;

    public class MemberInfoDrawSession : RightInfoDrawSession
    {
        /// <summary>
        /// мемберы пока что так себе реализованы - нихуя не реализованы
        /// </summary>
        public IDrawable Member { get; set; }

        protected override void Draw()
        {
            int pos = (25 / 2) - (Member.Name.Length / 2);
            this.Write(1, pos, Member.Name, Member.ForegroundColor);

            //pos = (25 / 2) - (Member.Affix.Length / 2);
            //this.Write(2, pos, Member.Affix, ConsoleColor.Yellow);

            //this.WriteAvatar(Member.Icon, Member.ForegroundColor);

            //int forCount = 14;
            //foreach (string s in DrawHelp.TextBlock(Member.Info, 21))
            //{
            //    this.Write(forCount++, 3, s, ConsoleColor.White);
            //}
        }
    }
}