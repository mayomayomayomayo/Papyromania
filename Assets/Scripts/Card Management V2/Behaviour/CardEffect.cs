public abstract class CardEffect
{
    public string Name { get; private set; }

    public abstract void Execute(CardUseContext ctx);
}