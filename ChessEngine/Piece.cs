public static class Piece
{
    public const int None = 0b00000;
    public const int Pawn = 0b00001;
    public const int Knight = 0b00010;
    public const int Bishop = 0b00011;
    public const int Rook = 0b00100;
    public const int Queen = 0b00101;
    public const int King = 0b00110;
    public const int White = 0b01000;
    public const int Black = 0b10000;
    
    private static readonly char[] PieceChars = ['.', 'P', 'N', 'B', 'R', 'Q', 'K'];
    
    private static int[] BuildCharToType()
    {
        int[] map = new int[128];
        map['P'] = Pawn;
        map['N'] = Knight;
        map['B'] = Bishop;
        map['R'] = Rook;
        map['Q'] = Queen;
        map['K'] = King;
        return map;
    }
    private static readonly int[] CharToType = BuildCharToType();
    
    public static int Type(int piece)
    {
        return piece & 0b00111;
    }
    
    public static int Color(int piece)
    {
        return piece & 0b11000;
    }
    
    public static bool IsWhite(int piece)
    {
        return Color(piece) == White;
    }
    
    public static bool IsBlack(int piece)
    {
        return Color(piece) == Black;
    }
    
    public static char ToChar(int piece)
    {
        if (Type(piece) == None)
            return '.';
            
        char letra = PieceChars[Type(piece)];
        
        if (IsBlack(piece))
            return char.ToLower(letra);
            
        return letra;
    }
    
    public static int FromChar(char c)
    {
        if (c == '.')
            return None;
            
        if (c>=128)
            return -1;
            
        int numero = CharToType[char.ToUpperInvariant(c)];
        
        if (tipo == None)
            return -1;
        
        if (char.IsUpper(c))
            return White | numero;
        return Black | numero;
    }
}