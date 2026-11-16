namespace POS.Models;

public sealed record Grade
{
    public static Grade Zero { get; } = new(0);

    public decimal Value { get; }

    public Grade(decimal value)
    {
        if (value < 0 || value > 100)
            throw new DomainException("Итог должен быть в диапазоне от 0 до 100.");
        Value = value;
    }

    public static Grade FromFraction(decimal fraction) =>
        new(Math.Round(fraction * 100, 2));

    public override string ToString() => Value.ToString();
}
