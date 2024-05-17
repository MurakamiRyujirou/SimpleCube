public class MovesTable
{
    public int[,] COMove; // Corner Orientation Move.
    public int[,] EOMove; // Edge Orientation Move.
    public int[,] EMMove; // Edge CoMbination Move.
    public int[,] CPMove; // Corner Permutation Move.
    public int[,] UEMove; // U-D Edges Permutation Move
    public int[,] EEMove; // E Edges Permutation Move

    private const int NUM_CO = 2187; // 1*2*3*4...
    private const int NUM_EO = 2048; // 2*2*2*2...
    private const int NUM_EM = 495;
    private const int NUM_CP = 40320;
    private const int NUM_UE = 40320;
    private const int NUM_EE = 24;
    private const int NUM_EP = 479001600;

    public MovesTable(CubeModel[] moves1, CubeModel[] moves2)
    {
        // ---- phase1. ----

        // CornerOrientationMove init.
        COMove = new int[NUM_CO, moves1.Length];
        for (int i = 0; i < NUM_CO; i++)
        {
            CubeModel cube = new(new int[8], IndexMapping.IndexToZeroSumOrientation(i, 3, 8), new int[12], new int[12]);
            for (int j = 0; j < moves1.Length; j++)
            {
                CubeModel moved = cube.Rotate(moves1[j]);
                COMove[i, j] = IndexMapping.ZeroSumOrientationToIndex(moved.CO, 3);
            }
        }
        // EdgeOrientationMove init.
        EOMove = new int[NUM_EO, moves1.Length];
        for (int i = 0; i < NUM_EO; i++)
        {
            CubeModel cube = new(new int[8], new int[8], new int[12], IndexMapping.IndexToZeroSumOrientation(i, 2, 12));
            for (int j = 0; j < moves1.Length; j++)
            {
                CubeModel moved = cube.Rotate(moves1[j]);
                EOMove[i, j] = IndexMapping.ZeroSumOrientationToIndex(moved.EO, 2);
            }
        }
        // E Edges CombinationMove init.
        EMMove = new int[NUM_EM, moves1.Length];
        for (int i = 0; i < NUM_EM; i++)
        {
            bool[] combination = IndexMapping.IndexToCombination(i, 4, 12);
            int[] edges = new int[12];
            int nextE = 0;
            int nextUD = 4;
            for (int j = 0; j < edges.Length; j++)
                edges[j] = combination[j] ? nextE++ : nextUD++;
            CubeModel cube = new CubeModel(new int[8], new int[8], edges, new int[12]);
            for (int j = 0; j < moves1.Length; j++)
            {
                CubeModel moved = cube.Rotate(moves1[j]);
                bool[] isEEdge = new bool[12];
                for (int k = 0; k < isEEdge.Length; k++)
                    isEEdge[k] = moved.EP[k] < 4;
                EMMove[i, j] = IndexMapping.CombinationToIndex(isEEdge, 4);
            }
        }

        // ---- phase2. ----

        // CornerPermutationMove init.
        CPMove = new int[NUM_CP, moves2.Length];
        for (int i = 0; i < NUM_CP; i++)
        {
            CubeModel cube = new(IndexMapping.IndexToPermutation(i, 8), new int[8], new int[12], new int[12]);
            for (int j = 0; j < moves2.Length; j++)
            {
                CubeModel moved = cube.Rotate(moves2[j]);
                CPMove[i, j] = IndexMapping.PermutationToIndex(moved.CP);
            }
        }
        // U-D Edges Permutation Move init.
        UEMove = new int[NUM_UE, moves2.Length];
        for (int i = 0; i < NUM_UE; i++)
        {
            int[] permutation = IndexMapping.IndexToPermutation(i, 8);
            int[] edges = new int[12];
            for (int j = 0; j < edges.Length; j++)
                edges[j] = j >= 4 ? permutation[j - 4] : j;
            CubeModel cube = new CubeModel(new int[8], new int[8], edges, new int[12]);
            for (int j = 0; j < moves2.Length; j++)
            {
                CubeModel moved = cube.Rotate(moves2[j]);
                int[] uDEdges = new int[8];
                for (int k = 0; k < uDEdges.Length; k++)
                    uDEdges[k] = moved.EP[k + 4] - 4;
                UEMove[i, j] = IndexMapping.PermutationToIndex(uDEdges);
            }
        }
        // E Edges Permutation Move init.
        EEMove = new int[NUM_EE, moves2.Length];
        for (int i = 0; i < NUM_EE; i++)
        {
            int[] permutation = IndexMapping.IndexToPermutation(i, 4);
            int[] edges = new int[12];
            for (int j = 0; j < edges.Length; j++)
                edges[j] = j >= 4 ? j : permutation[j];
            CubeModel cube = new(new int[8], new int[8], edges, new int[12]);
            for (int j = 0; j < moves2.Length; j++)
            {
                CubeModel moved = cube.Rotate(moves2[j]);
                int[] eEdges = new int[4];
                for (int k = 0; k < eEdges.Length; k++)
                    eEdges[k] = moved.EP[k];
                EEMove[i, j] = IndexMapping.PermutationToIndex(eEdges);
            }
        }
    }
}

