/// <summary>
///         ------
///        | 0 4 1|
///        | 7 U 5|
///        | 3 6 2|
///  ------ ------ ------ ------
/// |      |      |      |      |
/// |   L  | 3 F 2|   R  | 1 B 0|
/// |      |      |      |      |
///  ------ ------ ------ ------
///        | 710 6|
///        |11 D 9|
///        | 4 8 5|
///         ------
/// </summary>
public static class CubeConverter
{
    // 8 x 3
    private static int[,] ci = {{0, 2, 2}, {2, 2, 2}, {2, 2, 0}, {0, 2, 0}, {0, 0, 2}, {2, 0, 2}, {2, 0, 0}, {0, 0, 0}};
    // 12 x 3
    private static int[,] ei = {{0, 1, 2}, {2, 1, 2}, {2, 1, 0}, {0, 1, 0}, {1, 2, 2}, {2, 2, 1}, {1, 2, 0}, {0, 2, 1}, {1, 0, 2}, {2, 0, 1}, {1, 0, 0}, {0, 0, 1}};
    // センターの色.
    private static PanelColors uColor, fColor, rColor, bColor, lColor, dColor;

    /// Convert cube movel 2 SolverCube state.
    public static CubeModel Convert(Cube cube)
    {
        int[] cp = new int[8];
        int[] co = new int[8];
        int[] ep = new int[12];
        int[] eo = new int[12];

        uColor = cube.GetCubie(1, 2, 1).GetColor(Faces.UP);
        fColor = cube.GetCubie(1, 1, 0).GetColor(Faces.FRONT);
        rColor = cube.GetCubie(2, 1, 1).GetColor(Faces.RIGHT);
        bColor = cube.GetCubie(1, 1, 2).GetColor(Faces.BACK);
        lColor = cube.GetCubie(0, 1, 1).GetColor(Faces.LEFT);
        dColor = cube.GetCubie(1, 0, 1).GetColor(Faces.DOWN);

        for (int i = 0; i < ci.GetLength(0); i++)
        {
            int x = ci[i, 0];
            int y = ci[i, 1];
            int z = ci[i, 2];
            Cubie cubie = cube.GetCubie(x, y, z);
            cp[i] = GetCpAt(cubie, i);
            co[i] = GetCoAt(cubie, i);
        }
        for (int i = 0; i < ei.GetLength(0); i++)
        {
            int x = ei[i, 0];
            int y = ei[i, 1];
            int z = ei[i, 2];
            Cubie cubie = cube.GetCubie(x, y, z);
            ep[i] = GetEpAt(cubie, i);
            eo[i] = GetEoAt(cubie, i, ep[i]);
        }
        CubeModel ret = new(cp, co, ep, eo);
        return ret;
    }

    private static int GetCpAt(Cubie cubie, int i)
    {
        if (i == 0) return GetCpByFace(cubie, Faces.LEFT,  Faces.UP,   Faces.BACK);
        if (i == 1) return GetCpByFace(cubie, Faces.RIGHT, Faces.UP,   Faces.BACK);
        if (i == 2) return GetCpByFace(cubie, Faces.RIGHT, Faces.UP,   Faces.FRONT);
        if (i == 3) return GetCpByFace(cubie, Faces.LEFT,  Faces.UP,   Faces.FRONT);
        if (i == 4) return GetCpByFace(cubie, Faces.LEFT,  Faces.DOWN, Faces.BACK); 
        if (i == 5) return GetCpByFace(cubie, Faces.RIGHT, Faces.DOWN, Faces.BACK); 
        if (i == 6) return GetCpByFace(cubie, Faces.RIGHT, Faces.DOWN, Faces.FRONT);
        if (i == 7) return GetCpByFace(cubie, Faces.LEFT,  Faces.DOWN, Faces.FRONT);
        throw new System.Exception("Invalid argument error.");
    }

    private static byte GetCpByFace(Cubie cubie, Faces fx, Faces fy, Faces fz)
    {
        PanelColors cx = cubie.GetColor(fx);
        PanelColors cy = cubie.GetColor(fy);
        PanelColors cz = cubie.GetColor(fz);
        if (HasColors(cx, cy, cz, lColor, uColor, bColor)) return 0;
        if (HasColors(cx, cy, cz, rColor, uColor, bColor)) return 1;
        if (HasColors(cx, cy, cz, rColor, uColor, fColor)) return 2;
        if (HasColors(cx, cy, cz, lColor, uColor, fColor)) return 3;
        if (HasColors(cx, cy, cz, lColor, dColor, bColor)) return 4;
        if (HasColors(cx, cy, cz, rColor, dColor, bColor)) return 5;
        if (HasColors(cx, cy, cz, rColor, dColor, fColor)) return 6;
        if (HasColors(cx, cy, cz, lColor, dColor, fColor)) return 7;
        throw new System.Exception("Invalid argument error." + 
            "Cubie=" + cubie.ToString() +
            ",(fx,fy,fz)=" + fx + "," + fy + "," + fz +
            ",PanelColor=" + cx.ToString() + "," + cy.ToString() + "," + cz.ToString()) ;
    }

    private static bool HasColors(PanelColors cx, PanelColors cy, PanelColors cz, PanelColors c1, PanelColors c2, PanelColors c3)
    {
        return (cx == c1 && cy == c2 && cz == c3) ||
                (cx == c1 && cy == c3 && cz == c2) ||
                (cx == c2 && cy == c1 && cz == c3) ||
                (cx == c2 && cy == c3 && cz == c1) ||
                (cx == c3 && cy == c1 && cz == c2) ||
                (cx == c3 && cy == c2 && cz == c1);
    }
    private static int GetCoAt(Cubie cubie, int i)
    {
        if (i == 0) return GetCoByFace(cubie, Faces.UP,   Faces.LEFT,  Faces.BACK);
        if (i == 1) return GetCoByFace(cubie, Faces.UP,   Faces.BACK,  Faces.RIGHT);
        if (i == 2) return GetCoByFace(cubie, Faces.UP,   Faces.RIGHT, Faces.FRONT);
        if (i == 3) return GetCoByFace(cubie, Faces.UP,   Faces.FRONT, Faces.LEFT);
        if (i == 4) return GetCoByFace(cubie, Faces.DOWN, Faces.BACK,  Faces.LEFT);
        if (i == 5) return GetCoByFace(cubie, Faces.DOWN, Faces.RIGHT, Faces.BACK);
        if (i == 6) return GetCoByFace(cubie, Faces.DOWN, Faces.FRONT, Faces.RIGHT);
        if (i == 7) return GetCoByFace(cubie, Faces.DOWN, Faces.LEFT,  Faces.FRONT);
        throw new System.Exception("Invalid argument error. i = " + i);
    }
    private static int GetCoByFace(Cubie cubie, Faces f1, Faces f2, Faces f3)
    {
        PanelColors c1 = cubie.GetColor(f1);
        PanelColors c2 = cubie.GetColor(f2);
        PanelColors c3 = cubie.GetColor(f3);
        if (c1 == uColor || c1 == dColor) return 0;
        if (c2 == uColor || c2 == dColor) return 1;
        if (c3 == uColor || c3 == dColor) return 2;
        throw new System.Exception("Invalid argument error.");
    }
    private static byte GetEpAt(Cubie cubie, int i)
    {
        if (i == 0) return GetEpByFace(cubie, Faces.BACK,  Faces.LEFT);
        if (i == 1) return GetEpByFace(cubie, Faces.BACK,  Faces.RIGHT);
        if (i == 2) return GetEpByFace(cubie, Faces.FRONT, Faces.RIGHT);
        if (i == 3) return GetEpByFace(cubie, Faces.FRONT, Faces.LEFT);
        if (i == 4) return GetEpByFace(cubie, Faces.UP,    Faces.BACK);
        if (i == 5) return GetEpByFace(cubie, Faces.UP,    Faces.RIGHT);
        if (i == 6) return GetEpByFace(cubie, Faces.UP,    Faces.FRONT);
        if (i == 7) return GetEpByFace(cubie, Faces.UP,    Faces.LEFT);
        if (i == 8) return GetEpByFace(cubie, Faces.DOWN,  Faces.BACK);
        if (i == 9) return GetEpByFace(cubie, Faces.DOWN,  Faces.RIGHT);
        if (i == 10) return GetEpByFace(cubie, Faces.DOWN, Faces.FRONT);
        if (i == 11) return GetEpByFace(cubie, Faces.DOWN, Faces.LEFT);
        throw new System.Exception("Invalid argument error.");
    }
    private static byte GetEpByFace(Cubie cubie, Faces f1, Faces f2)
    {
        PanelColors c1 = cubie.GetColor(f1);
        PanelColors c2 = cubie.GetColor(f2);
        if ((c1 == bColor && c2 == lColor) || (c2 == bColor && c1 == lColor)) return 0;
        if ((c1 == bColor && c2 == rColor) || (c2 == bColor && c1 == rColor)) return 1;
        if ((c1 == fColor && c2 == rColor) || (c2 == fColor && c1 == rColor)) return 2;
        if ((c1 == fColor && c2 == lColor) || (c2 == fColor && c1 == lColor)) return 3;
        if ((c1 == uColor && c2 == bColor) || (c2 == uColor && c1 == bColor)) return 4;
        if ((c1 == uColor && c2 == rColor) || (c2 == uColor && c1 == rColor)) return 5;
        if ((c1 == uColor && c2 == fColor) || (c2 == uColor && c1 == fColor)) return 6;
        if ((c1 == uColor && c2 == lColor) || (c2 == uColor && c1 == lColor)) return 7;
        if ((c1 == dColor && c2 == bColor) || (c2 == dColor && c1 == bColor)) return 8;
        if ((c1 == dColor && c2 == rColor) || (c2 == dColor && c1 == rColor)) return 9;
        if ((c1 == dColor && c2 == fColor) || (c2 == dColor && c1 == fColor)) return 10;
        if ((c1 == dColor && c2 == lColor) || (c2 == dColor && c1 == lColor)) return 11;
        throw new System.Exception("Invalid argument error.");
    }
    /// 現在のエッジが本来の場所で基準となる色と、現在の場所での基準面にある色が一致するなら0,反転なら1を返す.
    private static int GetEoAt(Cubie cubie, int currentIndex, int naturalIndex)
    {
        return GetBaseColor(naturalIndex) == GetBaseColor(cubie, currentIndex) ? 0 : 1;
    }

    private static PanelColors GetBaseColor(int i)
    {
        if (i == 0) return bColor;
        if (i == 1) return bColor;
        if (i == 2) return fColor;
        if (i == 3) return fColor;
        if (i == 4) return uColor;
        if (i == 5) return uColor;
        if (i == 6) return uColor;
        if (i == 7) return uColor;
        if (i == 8) return dColor;
        if (i == 9) return dColor;
        if (i == 10) return dColor;
        if (i == 11) return dColor;
        return PanelColors.NONE;
    }

    private static PanelColors GetBaseColor(Cubie cubie, int i)
    {
        if (i == 0) return cubie.GetColor(Faces.BACK);
        if (i == 1) return cubie.GetColor(Faces.BACK);
        if (i == 2) return cubie.GetColor(Faces.FRONT);
        if (i == 3) return cubie.GetColor(Faces.FRONT);
        if (i == 4) return cubie.GetColor(Faces.UP);
        if (i == 5) return cubie.GetColor(Faces.UP);
        if (i == 6) return cubie.GetColor(Faces.UP);
        if (i == 7) return cubie.GetColor(Faces.UP);
        if (i == 8) return cubie.GetColor(Faces.DOWN);
        if (i == 9) return cubie.GetColor(Faces.DOWN);
        if (i == 10) return cubie.GetColor(Faces.DOWN);
        if (i == 11) return cubie.GetColor(Faces.DOWN);
        return PanelColors.NONE;
    }
}
