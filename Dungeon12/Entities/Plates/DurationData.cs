namespace Dungeon12.Entities.Plates
{
    internal class DurationData
    {
        public Duration Duration { get; set; }

        public int Value { get; set; }

        public override string ToString()
        {
            var dur = Global.Strings[Duration];

            if (Duration== Duration.Turns)
                dur = $"{Value} {dur}";

            return dur;
        }

        public static implicit operator DurationData(Duration duration)=>new DurationData() {  Duration = duration };   
    }
}