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
Console.WriteLine(Piece.FromChar('x'));
Console.WriteLine(Piece.FromChar('é'));
Console.WriteLine(Piece.FromChar('k'));