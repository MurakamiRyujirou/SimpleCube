/// <summary>
/// Cube domain model for Solving.
/// </summary>
public class CubeModel
{
    // Corner Permutation.
    public int[] CP { get; set; }

    // Corner Orientation.
    public int[] CO { get; set; }

    // Edge Permutation.
    public int[] EP { get; set; }

    // Edge Orientation.
    public int[] EO { get; set; }

    /// Default constructor.
    public CubeModel()
    {
        this.CP = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
        this.CO = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        this.EP = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        this.EO = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }

    /// Constructor.
    /// <param name="cp">Corner Permutation.</param>
    /// <param name="co">Corner Orientation.</param>
    /// <param name="ep">Edge Permutation.</param>
    /// <param name="eo">Edge Orientation.</param>
    public CubeModel(int[] cp, int[] co, int[] ep, int[] eo)
    {
        this.CP = cp;
        this.CO = co;
        this.EP = ep;
        this.EO = eo;
    }

    /// Get rotated cube.
    /// <param name="move">rotate operation.</param>
    /// <returns>SolverCube.</returns>
    public CubeModel Rotate(CubeModel move)
    {
        int[] cp = new int[8];
        int[] co = new int[8];
        int[] ep = new int[12];
        int[] eo = new int[12];
        for (int i = 0; i < 8; i++)
        {
            cp[i] = CP[move.CP[i]];
            co[i] = (CO[move.CP[i]] + move.CO[i]) % 3;
        }
        for (int i = 0; i < 12; i++)
        {
            ep[i] = EP[move.EP[i]];
            eo[i] = (EO[move.EP[i]] + move.EO[i]) % 2;
        }
        return new CubeModel(cp, co, ep, eo);
    }

    /// Multi rotate.
    /// <param name="sequence">rotate sequence.</param>
    /// <returns>SolverCube.</returns>
    public CubeModel Sequence(string[] sequence)
    {
        CubeModel cube = this;
        foreach (string moveStr in sequence)
        {
            CubeModel move = Moves.Get(moveStr);
            cube = cube.Rotate(move);
        }
        return cube;
    }

    // -------- OVERRIDE --------
        
    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        if (obj == this) return true;
        if (obj.GetType() != this.GetType()) return false;
        CubeModel target = (CubeModel)obj;
        if (CP.Length != target.CP.Length) return false;
        if (CO.Length != target.CO.Length) return false;
        if (EP.Length != target.EP.Length) return false;
        if (EO.Length != target.EO.Length) return false;
        for (int i = 0; i < CP.Length; i++)
            if (CP[i] != target.CP[i])
                return false;
        for (int i = 0; i < CO.Length; i++)
            if (CO[i] != target.CO[i])
                return false;
        for (int i = 0; i < EP.Length; i++)
            if (EP[i] != target.EP[i])
                return false;
        for (int i = 0; i < EO.Length; i++)
            if (EO[i] != target.EO[i])
                return false;
        return true;
    }

    public override int GetHashCode()
    {
        int cp = IndexMapping.PermutationToIndex(CP);
        int co = IndexMapping.OrientationToIndex(CO, 3);
        int ep = IndexMapping.PermutationToIndex(EP);
        int eo = IndexMapping.OrientationToIndex(EO, 2);
        int ret = cp;
        ret = ret * 31 + co;
        ret = ret * 31 + ep;
        ret = ret * 31 + eo;
        return ret;
    }

    public override string ToString()
    {
        int cp = IndexMapping.PermutationToIndex(CP);
        int co = IndexMapping.OrientationToIndex(CO, 3);
        int ep = IndexMapping.PermutationToIndex(EP);
        int eo = IndexMapping.OrientationToIndex(EO, 2);
        return "CP=" + cp + ",CO=" + co + ",EP=" + ep + ",EO=" + eo;
    }
}
