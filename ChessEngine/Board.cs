public class Board
{
    public int SideToMove = Piece.White;
    public int EnPassant = -1;
    public int HalfMoveClock = 0;
    public int FullMoveCount = 1;
    public int CastlingRights = 0;
    
    public const int WhiteKingSide = 0b0001;
    public const int WhiteQueenSide = 0b0010;
    public const int BlackKingSide = 0b0100;
    public const int BlackQueenSide = 0b1000;
    
    public int[] Squares = new int[64];

    public bool LoadFenPosition(string fen)
    {
    if (string.IsNullOrWhiteSpace(fen)) return false;
    
    string[] fields = fen.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    int[] board = new int[64];
    
    if (fields.Length<4)
        return false;
    
    int rank = 7;
    int file = 0;
    int side = Piece.White;
    int castling = 0;
    
    foreach (char c in fields[0])
    {
        if (c == '/')
        {
            if (file != 8 || rank <= 0)
            {
                return false;
            }
            rank--;
            file = 0;
        }
        else if (char.IsDigit(c))
        {
            if (c>'0' && c<='8')
            {
                int emptySquares = c - '0';
                file += emptySquares;
            }
            else
            {
                return false;
            }
            if (file > 8)
            {
                return false;
            }
        }
        else
        {
            int piece = Piece.FromChar(c);
            
            if (piece < 0 || file >= 8) 
            {
                return false;
            }
            board[rank * 8 + file] = piece;
            file++;
        }
    }
    
    if (file!=8 || rank != 0)
        return false;
    
    if (fields[1]=="w")
        side = Piece.White;
    else if (fields[1]=="b")
        side = Piece.Black;
    else
        return false;
        
    if (fields[2]!="-")
    {
        foreach (char c in fields[2])
        {
            if (c=='K')
                castling|= WhiteKingSide;
            else if (c=='Q')
                castling|= WhiteQueenSide;
            else if (c=='k')
                castling|= BlackKingSide;
            else if (c=='q')
                castling|= BlackQueenSide;
            else
                return false;
        }
    }
    Array.Copy(board, Squares, 64);
    
    CastlingRights = castling;
    SideToMove = side;
    
    return true;
    }
}