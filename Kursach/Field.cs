using System.Text;

namespace Kursach;

public class Field
{
    public int N { get; set; }
    public int Size { get; set; }

    public Cell[,] Map { get; set; }

    public Cell this[Point point]
    {
        get
        {
            return Map[point.X, point.Y];
        }

        set
        {
            Map[point.X, point.Y] = value;
        }
    }

    public Field(int n)
    {
        N = n;
        Size = 2 * N + 1;
        Map = new Cell[Size, Size];
        FillField();
    }

    public Field(int[,] matrix)
    {
        Size = matrix.GetLength(0);
        Map = new Cell[Size, Size];
        N = (Size - 1) / 2;
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                Map[x, y] = new Cell(matrix[x, y]);
            }
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var y = 0; y < Size; y++)
        {
            for (var x = 0; x < Size; x++)
            {
                sb.Append(" " + Map[x, y] + " ");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    public void FillField()
    {
        var random = new Random();
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                Map[x, y] = new Cell(random.Next(0, 10));
            }
        }
    }
}
