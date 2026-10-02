public class Board
{
    public int[] Squares = new int[64];
    
    public bool LoadFenPosition(string fen)
    {
        Array.Clear(Squares, 0, Squares.Length);
        
        string placement = fen.Split(' ')[0];
        int rank = 7;
        int file = 0;
        
        foreach (char c in placement)
        {
            if (c=='/')
            {
                rank--;
                file = 0;
            }
            else if(char.IsDigit(c))
            {
                file+=c-'0';
            }
            else
            {
                int piece = Piece.FromChar(c);
                
                if (piece < 0 || file > 7 || rank < 0)
                    return false;

                Squares[rank * 8 + file] = piece;
                file++;
            }
        }
        return true;
    }
}