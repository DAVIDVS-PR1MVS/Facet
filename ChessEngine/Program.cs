Console.WriteLine("Motor iniciado");

foreach (int cor in new[] { Piece.White, Piece.Black })
{
    for (int tipo = Piece.Pawn; tipo <= Piece.King; tipo++)
    {
        int p = cor | tipo;
        Console.Write(Piece.ToChar(p));
        if (Piece.FromChar(Piece.ToChar(p)) != p)
            Console.Write("(ERRO)");
    }
}
Console.WriteLine();
Console.WriteLine(Piece.FromChar('x')); // -1
Console.WriteLine(Piece.FromChar('é')); // -1
Console.WriteLine(Piece.FromChar('k')); // 22

var board = new Board();
Console.WriteLine(board.LoadFenPosition("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")); // True

Console.WriteLine(board.Squares[0] == (Piece.White | Piece.Rook));  // True (a1)

Console.WriteLine(board.Squares[60] == (Piece.Black | Piece.King)); // True (e8)

Console.WriteLine(board.LoadFenPosition("xnbqkbnr/8/8/8/8/8/8/8 w - - 0 1")); // False