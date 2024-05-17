public class Moves
{
    public static CubeModel none  = new();
    public static CubeModel moveU = new(new int[] { 3, 0, 1, 2, 4, 5, 6, 7 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 1, 2, 3, 7, 4, 5, 6, 8, 9, 10, 11 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
    public static CubeModel moveD = new(new int[] { 0, 1, 2, 3, 5, 6, 7, 4 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 9, 10, 11, 8 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
    public static CubeModel moveL = new(new int[] { 4, 1, 2, 0, 7, 5, 6, 3 }, new int[] { 2, 0, 0, 1, 1, 0, 0, 2 }, new int[] { 11, 1, 2, 7, 4, 5, 6, 0, 8, 9, 10, 3 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
    public static CubeModel moveR = new(new int[] { 0, 2, 6, 3, 4, 1, 5, 7 }, new int[] { 0, 1, 2, 0, 0, 2, 1, 0 }, new int[] { 0, 5, 9, 3, 4, 2, 6, 7, 8, 1, 10, 11 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
    public static CubeModel moveF = new(new int[] { 0, 1, 3, 7, 4, 5, 2, 6 }, new int[] { 0, 0, 1, 2, 0, 0, 2, 1 }, new int[] { 0, 1, 6, 10, 4, 5, 3, 7, 8, 9, 2, 11 }, new int[] { 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 0 });
    public static CubeModel moveB = new(new int[] { 1, 5, 2, 3, 0, 4, 6, 7 }, new int[] { 1, 2, 0, 0, 2, 1, 0, 0 }, new int[] { 4, 8, 2, 3, 1, 5, 6, 7, 0, 9, 10, 11 }, new int[] { 1, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 });
    public static CubeModel Get(string move)
    {
        return move switch
        {
            "U"  => moveU,
            "U2" => moveU.Rotate(moveU),
            "U'" => moveU.Rotate(moveU).Rotate(moveU),
            "D"  => moveD,
            "D2" => moveD.Rotate(moveD),
            "D'" => moveD.Rotate(moveD).Rotate(moveD),
            "L"  => moveL,
            "L2" => moveL.Rotate(moveL),
            "L'" => moveL.Rotate(moveL).Rotate(moveL),
            "R"  => moveR,
            "R2" => moveR.Rotate(moveR),
            "R'" => moveR.Rotate(moveR).Rotate(moveR),
            "F"  => moveF,
            "F2" => moveF.Rotate(moveF),
            "F'" => moveF.Rotate(moveF).Rotate(moveF),
            "B"  => moveB,
            "B2" => moveB.Rotate(moveB),
            "B'" => moveB.Rotate(moveB).Rotate(moveB),
            _ => none
        };
    }
}
