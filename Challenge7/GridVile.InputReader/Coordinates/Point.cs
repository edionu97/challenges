namespace GridVile.DataModeling.Coordinates
{
    /// <summary>
    /// This models a two dimensional point
    /// </summary>
    public record Point(int X, int Y)
    {
        public override string ToString() => $"({X}, {Y})";
    }
}
