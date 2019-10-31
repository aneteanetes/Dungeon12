namespace Dungeon12.Classes
{
    using Dungeon.Items;

    public abstract partial class Character
    {
        public Result<string> Buy(Item item, object merchant)
        {
            if (this.Gold - item.Cost >= 0)
            {
                this.Gold -= item.Cost;
                this.Backpack.Add(item);

                return Result<string>
					.Success;
            }

            return Result<string>
				.Failure
                .Set("Недостаточно золота для покупки");
        }

        public Result<string> Sell(Item item, object merchant)
        {
            this.Gold += item.Cost;
            this.Backpack.Remove(item);

            return Result<string>.Success;
        }
    }
}