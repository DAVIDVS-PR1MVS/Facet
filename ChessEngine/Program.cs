Console.WriteLine("Motor iniciado");

// ---------------------------------------------------------------
// Opções: ligue quando a parte correspondente estiver pronta
// ---------------------------------------------------------------
bool testarEnPassant = false;     // ligue quando o campo 4 do FEN (en passant) estiver implementado
bool rejeitarPontoNoFen = false;  // ligue se decidir que '.' não vale num FEN

// ---------------------------------------------------------------
// Infraestrutura: só imprime o que falhou; no fim, código de saída 1 se algo falhou
// ---------------------------------------------------------------
int total = 0;
int falhas = 0;

void Check(string nome, bool condicao)
{
    total++;
    if (!condicao)
    {
        falhas++;
        Console.WriteLine("FALHOU: " + nome);
    }
}

bool Iguais(int[] a, int[] b)
{
    if (a.Length != b.Length) return false;
    for (int k = 0; k < a.Length; k++)
        if (a[k] != b[k]) return false;
    return true;
}

const string Inicial = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
int roqueTodos = Board.WhiteKingSide | Board.WhiteQueenSide | Board.BlackKingSide | Board.BlackQueenSide;

// ---------------------------------------------------------------
// Piece
// ---------------------------------------------------------------
string letras = "";
foreach (int cor in new[] { Piece.White, Piece.Black })
{
    for (int tipo = Piece.Pawn; tipo <= Piece.King; tipo++)
    {
        int p = cor | tipo;
        char letra = Piece.ToChar(p);
        letras += letra;
        Check("ida e volta " + letra, Piece.FromChar(letra) == p);
    }
}
Check("letras de todas as peças", letras == "PNBRQKpnbrqk");
Check("ToChar(0) = '.'", Piece.ToChar(0) == '.');
Check("ToChar(7) = '?'", Piece.ToChar(7) == '?');
Check("FromChar('x') inválido", Piece.FromChar('x') == Piece.Invalid);
Check("FromChar('é') inválido", Piece.FromChar('é') == Piece.Invalid);
Check("FromChar('k') = rei preto", Piece.FromChar('k') == (Piece.Black | Piece.King));
Check("FromChar('K') = rei branco", Piece.FromChar('K') == (Piece.White | Piece.King));
Check("FromChar('.') = vazio", Piece.FromChar('.') == Piece.None);

// ---------------------------------------------------------------
// Posição inicial
// ---------------------------------------------------------------
var ini = new Board();
Check("FEN inicial carrega", ini.LoadFenPosition(Inicial + " w KQkq - 0 1"));
Check("a1 = torre branca", ini.Squares[0] == (Piece.White | Piece.Rook));
Check("e1 = rei branco", ini.Squares[4] == (Piece.White | Piece.King));
Check("e8 = rei preto", ini.Squares[60] == (Piece.Black | Piece.King));
Check("h8 = torre preta", ini.Squares[63] == (Piece.Black | Piece.Rook));

bool peoesBrancos = true;
for (int i = 8; i < 16; i++)
    if (ini.Squares[i] != (Piece.White | Piece.Pawn)) peoesBrancos = false;
Check("casas 8 a 15 = peões brancos", peoesBrancos);

bool peoesPretos = true;
for (int i = 48; i < 56; i++)
    if (ini.Squares[i] != (Piece.Black | Piece.Pawn)) peoesPretos = false;
Check("casas 48 a 55 = peões pretos", peoesPretos);

bool meioVazio = true;
for (int i = 16; i < 48; i++)
    if (ini.Squares[i] != Piece.None) meioVazio = false;
Check("casas 16 a 47 vazias", meioVazio);

Check("quem joga = brancas", ini.SideToMove == Piece.White);
Check("roque = KQkq", ini.CastlingRights == roqueTodos);

// ---------------------------------------------------------------
// FENs válidos: (fen, quem joga, roque esperado)
// ---------------------------------------------------------------
var validos = new (string fen, int lado, int roque)[]
{
    (Inicial + " w KQkq - 0 1", Piece.White, roqueTodos),
    (Inicial + " b KQkq - 0 1", Piece.Black, roqueTodos),
    (Inicial + " w - - 0 1", Piece.White, 0),
    (Inicial + " w Kq - 0 1", Piece.White, Board.WhiteKingSide | Board.BlackQueenSide),
    (Inicial + " w QK - 0 1", Piece.White, Board.WhiteKingSide | Board.WhiteQueenSide),
    (Inicial + " w KQkq -", Piece.White, roqueTodos),  // só 4 campos
    ("  " + Inicial + "   b   Kq   -   0   1  ", Piece.Black, Board.WhiteKingSide | Board.BlackQueenSide),  // espaços extras
};

foreach (var (fen, lado, roque) in validos)
{
    var tab = new Board();
    bool ok = tab.LoadFenPosition(fen);
    Check("válido: [" + fen + "]", ok && tab.SideToMove == lado && tab.CastlingRights == roque);
}

// ---------------------------------------------------------------
// FENs inválidos: todos devem devolver false
// ---------------------------------------------------------------
string[] invalidos =
{
    "08/8/8/8/8/8/8/8 w - - 0 1",                // dígito 0
    "pppppppp9/8/8/8/8/8/8/8 w - - 0 1",         // dígito 9
    "7/8/8/8/8/8/8/8 w - - 0 1",                 // primeira linha curta
    "8/8/8/8/8/8/8/7 w - - 0 1",                 // última linha curta
    "8/8/8/8/8/8/8 w - - 0 1",                   // 7 linhas
    "8/8/8/8/8/8/8/8/8 w - - 0 1",               // 9 linhas
    "xnbqkbnr/8/8/8/8/8/8/8 w - - 0 1",          // letra inválida
    Inicial,                                     // só as peças
    Inicial + " w KQkq",                         // só 3 campos
    "",
    "   ",
    Inicial + " x KQkq - 0 1",                   // quem joga inválido
    Inicial + " W KQkq - 0 1",                   // maiúsculo
    Inicial + " w KQkx - 0 1",                   // letra inválida no roque
    Inicial + " w -K - 0 1",                     // '-' misturado com letras
    Inicial + " w K- - 0 1",
};

foreach (string fen in invalidos)
{
    var tab = new Board();
    Check("inválido: [" + fen + "]", !tab.LoadFenPosition(fen));
}
Check("inválido: null", !new Board().LoadFenPosition(null!));

// ---------------------------------------------------------------
// Falha deixa o Board intacto (protege o array temporário)
// ---------------------------------------------------------------
string baseFen = "8/8/8/8/8/8/8/4K3 b Kq - 0 1";
string[] falhasApos =
{
    Inicial.Replace("rnbqkbnr", "xnbqkbnr") + " w KQkq - 0 1",  // falha nas peças
    Inicial + " x KQkq - 0 1",                                  // falha em quem joga
    Inicial + " w KQkx - 0 1",                                  // falha no roque
};

foreach (string fen in falhasApos)
{
    var tab = new Board();
    bool baseOk = tab.LoadFenPosition(baseFen) && tab.SideToMove == Piece.Black;
    int[] antes = (int[])tab.Squares.Clone();
    int ladoAntes = tab.SideToMove;
    int roqueAntes = tab.CastlingRights;
    int epAntes = tab.EnPassantSquare;

    bool ok = baseOk
              && !tab.LoadFenPosition(fen)
              && Iguais(antes, tab.Squares)
              && tab.SideToMove == ladoAntes
              && tab.CastlingRights == roqueAntes
              && tab.EnPassantSquare == epAntes;
    Check("falha deixa o Board intacto: [" + fen + "]", ok);
}

// ---------------------------------------------------------------
// Opcional: ponto no FEN
// ---------------------------------------------------------------
if (rejeitarPontoNoFen)
    Check("inválido: '.' no FEN", !new Board().LoadFenPosition("8/8/8/8/8/8/8/........ w - - 0 1"));

// ---------------------------------------------------------------
// Opcional: en passant
// ---------------------------------------------------------------
if (testarEnPassant)
{
    var epValidos = new (string fen, int casa)[]
    {
        (Inicial + " w KQkq - 0 1", -1),
        (Inicial + " b KQkq e3 0 1", 20),
        (Inicial + " w KQkq e6 0 1", 44),
        (Inicial + " b KQkq a3 0 1", 16),
        (Inicial + " w KQkq h6 0 1", 47),
    };

    foreach (var (fen, casa) in epValidos)
    {
        var tab = new Board();
        Check("en passant válido: [" + fen + "]", tab.LoadFenPosition(fen) && tab.EnPassantSquare == casa);
    }

    string[] epInvalidos =
    {
        Inicial + " w KQkq e3 0 1",   // linha 3 com as brancas jogando
        Inicial + " b KQkq e6 0 1",   // linha 6 com as pretas jogando
        Inicial + " b KQkq i3 0 1",   // coluna inválida
        Inicial + " b KQkq e 0 1",    // texto curto
        Inicial + " b KQkq e33 0 1",  // texto longo
        Inicial + " b KQkq E3 0 1",   // maiúsculo
        Inicial + " b KQkq e9 0 1",   // linha inválida
    };

    foreach (string fen in epInvalidos)
    {
        var tab = new Board();
        Check("en passant inválido: [" + fen + "]", !tab.LoadFenPosition(fen));
    }
}

// ---------------------------------------------------------------
// Resultado
// ---------------------------------------------------------------
Console.WriteLine((total - falhas) + "/" + total + " testes passaram");
return falhas == 0 ? 0 : 1;