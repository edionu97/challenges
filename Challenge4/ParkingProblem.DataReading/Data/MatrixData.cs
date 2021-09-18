namespace ParkingProblem.DataReading.Data
{
    public record MatrixData
    {
        public char[][] Data { get; init; }

        public int RowCount { get; init; }
        
        public int ColumnCount { get; init; }

        public (int X, int Y) CarLocation { get; init; }

        public void Deconstruct(
            out char[][] data, 
            out int n, out int m, out (int X, int Y) location)
        {
            data = Data;
            n = RowCount;
            m = ColumnCount;
            location = CarLocation;
        }
    }
}
