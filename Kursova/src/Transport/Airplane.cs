namespace Transport
{
    public class AirPlane
    {
        public string Model { get; set; } = string.Empty;
        public int Rows { get; set; }
        public int Columns { get; set; }

        public int TotalSeats => Rows * Columns;


        public string GetSeatName(int row, int col)
        {
            if (row < 0 || row >= Rows || col < 0 || col >= Columns)
            {
                return "Invalid";
            }
            char columnLetter = (char)('A' + col);
            return $"{row + 1}{columnLetter}";
        }

        public (int row, int col)? GetSeatIndices(string seatName)
        {
            if (string.IsNullOrEmpty(seatName) || seatName.Length < 2) return null;

            char colLetter = char.ToUpper(seatName[seatName.Length - 1]);
            if (!int.TryParse(seatName.Substring(0, seatName.Length - 1), out int rowNum)) return null;
            int row = rowNum - 1;
            int col = colLetter - 'A';

            if (row >= 0 && row < Rows && col >= 0 && col < Columns) return (row, col);

            return null;

        }
    }
}