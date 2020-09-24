using Dungeon.Types;

namespace InTheWood.Entities.MapScreen
{
    /// <summary>
    /// Пиздец замудрёная штука, но хоть как-то должна работать
    /// </summary>
    public class SectorConnection
    {
        public Sector To { get; }

        public Sector What { get; }

        public SectorConnection(Sector toAttach, Sector whatAttach)
        {
            To = toAttach;
            What = whatAttach;
        }

        public SimpleDirection ConnectDirection { get; set; }

        /// <summary>
        /// 0..2
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Левый или правый тайл в сегменте который надо добавить либо сверху либо снизу 0 лево, 1 право 
        /// <para>Для право-лево указывает оффсет, 0 занчит без, 1 значит надо ниже, -1 выше</para>
        /// </summary>
        public int Offset { get; set; }
    }
}