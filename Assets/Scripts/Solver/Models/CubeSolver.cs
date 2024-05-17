using System;
using System.Collections.Generic;

public class CubeSolver
{
    public MovesTable movesTable;
    public PruneTable pruneTable;

    private const int MAX_SOLUTION_LENGTH = 23;
    private const int MAX_PHASE_2_SOLUTION_LENGTH = 12;

    private CubeModel[] moves1;
    private string[] moveNames1 = new string[]
    {
        "U", "U2", "U'",
        "D", "D2", "D'",
        "L", "L2", "L'",
        "R", "R2", "R'",
        "F", "F2", "F'",
        "B", "B2", "B'",
    };
    private int[] sides1 = new int[]
    {
        0, 0, 0,
        1, 1, 1,
        2, 2, 2,
        3, 3, 3,
        4, 4, 4,
        5, 5, 5,
    };
    private int[] axes1 = new int[]
    {
        0, 0, 0,
        0, 0, 0,
        1, 1, 1,
        1, 1, 1,
        2, 2, 2,
        2, 2, 2,
    };

    private CubeModel[] moves2;
    private string[] moveNames2 = new string[]
    {
        "U", "U2", "U'",
        "D", "D2", "D'",
        "L2",
        "R2",
        "F2",
        "B2",
    };
    private int[] sides2 = new int[]
    {
        0, 0, 0,
        1, 1, 1,
        2,
        3,
        4,
        5,
    };
    private int[] axes2 = new int[]
    {
        0, 0, 0,
        0, 0, 0,
        1,
        1,
        2,
        2,
    };

    private List<int> solution1;
    private List<int> solution2;
    private List<string> currentSolution;
        
    /// <summary>
    /// Constructor.
    /// </summary>
    public CubeSolver()
    {
        moves1 = new CubeModel[moveNames1.Length];
        for (int i = 0; i < moveNames1.Length; i++)
            moves1[i] = Moves.Get(moveNames1[i]);
        moves2 = new CubeModel[moveNames2.Length];
        for (int i = 0; i < moveNames2.Length; i++)
            moves2[i] = Moves.Get(moveNames2[i]);
        movesTable = new MovesTable(moves1, moves2);
        pruneTable = new PruneTable(movesTable, moves1, moves2);
    }

    private CubeModel initialCube;
    public string[] Solution(CubeModel cube)
    {
        initialCube = cube;
        solution1 = null;
        solution2 = null;
        currentSolution = null;
        int co = IndexMapping.ZeroSumOrientationToIndex(cube.CO, 3);
        int eo = IndexMapping.ZeroSumOrientationToIndex(cube.EO, 2);
        int em = IndexMapping.CombinationToIndex(GetIsEEdges(cube), 4);
        int foundDepth = 13;
        for (int depth = 0; depth <= foundDepth; depth++)
        {
            solution1 = new List<int>();
            if (Search1(co, eo, em, depth))
                foundDepth = depth;
        }
        return currentSolution.ToArray();
    }

    public string[] Generate(CubeModel cube)
    {
        string[] solution = Solution(cube);
        Dictionary<string, string> inverseMoveNames = new();
        inverseMoveNames.Add("U",  "U'");
        inverseMoveNames.Add("U2", "U2");
        inverseMoveNames.Add("U'", "U");
        inverseMoveNames.Add("D",  "D'");
        inverseMoveNames.Add("D2", "D2");
        inverseMoveNames.Add("D'", "D");
        inverseMoveNames.Add("L",  "L'");
        inverseMoveNames.Add("L2", "L2");
        inverseMoveNames.Add("L'", "L");
        inverseMoveNames.Add("R",  "R'");
        inverseMoveNames.Add("R2", "R2");
        inverseMoveNames.Add("R'", "R");
        inverseMoveNames.Add("F",  "F'");
        inverseMoveNames.Add("F2", "F2");
        inverseMoveNames.Add("F'", "F");
        inverseMoveNames.Add("B",  "B'");
        inverseMoveNames.Add("B2", "B2");
        inverseMoveNames.Add("B'", "B");
        string[] sequence = new string[solution.Length];
        for (int i = 0; i < solution.Length; i++)
        {
            sequence[i] = inverseMoveNames[solution[solution.Length - i - 1]];
        }
        return sequence;
    }

    // -------- private method. --------

    private bool[] GetIsEEdges(CubeModel cube)
    {
        bool[] isEEdge = new bool[12];
        for (int i = 0; i < isEEdge.Length; i++)
            isEEdge[i] = cube.EP[i] < 4;
        return isEEdge;
    }

    private int[] GetUDEdges(CubeModel cube)
    {
        int[] uDEdges = new int[8];
        for (int i = 0; i < uDEdges.Length; i++)
            uDEdges[i] = cube.EP[i + 4] - 4;
        return uDEdges;
    }

    private int[] GetEEdges(CubeModel cube)
    {
        int[] eEdges = new int[4];
        for (int i = 0; i < eEdges.Length; i++)
            eEdges[i] = cube.EP[i];
        return eEdges;
    }

    /// <summary>
    /// 探索その１.
    /// </summary>
    /// <param name="co">Corner Orientation.</param>
    /// <param name="eo">Edge Orientation.</param>
    /// <param name="em">E Edge Combinations.</param>
    /// <param name="depth">depth of search.</param>
    /// <returns>TRUE:solve.</returns>
    private bool Search1(int co, int eo, int em, int depth)
    {
        if (depth == 0)
        {
            if (co == 0 && eo == 0 && em == 0)
            {
                CubeModel cube = initialCube;
                foreach (int moveIndex in solution1)
                    cube = cube.Rotate(moves1[moveIndex]);
                return Solution2(cube, MAX_SOLUTION_LENGTH - solution1.Count);
            }
            return false;
        }
        bool isSearch1Done = false;
        if (pruneTable.CODistance[co, em] <= depth &&
            pruneTable.EODistance[eo, em] <= depth)
        {
            int[] lastMoves = { -1, -1 };
            for (int i = 0; i < lastMoves.Length && i < solution1.Count; i++)
                lastMoves[i] = solution1[solution1.Count - 1 - i];

            for (int i = 0; i < moves1.Length; i++)
            {
                // same side
                if (lastMoves[0] >= 0 && sides1[i] == sides1[lastMoves[0]]) continue;
                // same axis three times in a row
                if (lastMoves[0] >= 0 && axes1[i] == axes1[lastMoves[0]] &&
                    lastMoves[1] >= 0 && axes1[i] == axes1[lastMoves[1]]) continue;

                solution1.Add(i);
                if (Search1(movesTable.COMove[co, i],
                            movesTable.EOMove[eo, i],
                            movesTable.EMMove[em, i],
                            depth - 1))
                    isSearch1Done = true;
                solution1.RemoveAt(solution1.Count - 1);
            }
        }
        return isSearch1Done;
    }

    /// <summary>
    /// 探索その２の準備.
    /// </summary>
    /// <param name="cube"></param>
    /// <param name="maxDepth"></param>
    /// <returns></returns>
    private bool Solution2(CubeModel cube, int maxDepth)
    {
        if (solution1.Count > 0)
        {
            int lastMove = solution1[solution1.Count - 1];
            for (int i = 0; i < moveNames2.Length; i++)
                if (moveNames1[lastMove].Equals(moveNames2[i]))
                    return false;
        }
        int cp = IndexMapping.PermutationToIndex(cube.CP);
        int ue = IndexMapping.PermutationToIndex(GetUDEdges(cube));
        int ee = IndexMapping.PermutationToIndex(GetEEdges(cube));
        for (int depth = 0; depth < Math.Min(MAX_PHASE_2_SOLUTION_LENGTH, maxDepth); depth++)
        {
            solution2 = new List<int>();
            if (Search2(cp, ue, ee, depth))
                return true;
        }
        return false;
    }

    /// <summary>
    /// 探索その２.
    /// </summary>
    /// <param name="cp">Corner Permutation.</param>
    /// <param name="ue">U-D Edge Permutation.</param>
    /// <param name="ee">E Edge Permutation.</param>
    /// <param name="depth">depth of search.</param>
    /// <returns>TRUE:solve.</returns>
    private bool Search2(int cp, int ue, int ee, int depth)
    {
        if (depth == 0)
        {
            bool isFinish = cp == 0 && ue == 0 && ee == 0;
            if (isFinish)
            {
                if (currentSolution == null || currentSolution.Count > solution1.Count + solution2.Count)
                {
                    currentSolution = new();
                    foreach (int s in solution1) currentSolution.Add(moveNames1[s]);
                    foreach (int s in solution2) currentSolution.Add(moveNames2[s]);
                }
            }
            return isFinish;
        }
        bool isSearch2Done = false;
        if (pruneTable.CPDistance[cp, ee] <= depth &&
            pruneTable.UEDistance[ue, ee] <= depth)
        {
            int lastSide = int.MaxValue;
            if (solution2.Count > 0)
                lastSide = sides2[solution2[solution2.Count - 1]];

            for (int i = 0; i < moves2.Length; i++)
            {
                // avoid superflous moves between phases
                if (solution2.Count == 0)
                {
                    int lastPhase1Axis = int.MaxValue;
                    if (solution1.Count > 0)
                        lastPhase1Axis = axes1[solution1[solution1.Count - 1]];
                    if (axes2[i] == lastPhase1Axis)
                        continue;
                }
                // same side
                if (sides2[i] == lastSide) continue;
                solution2.Add(i);
                if (Search2(movesTable.CPMove[cp, i],
                            movesTable.UEMove[ue, i],
                            movesTable.EEMove[ee, i],
                            depth - 1))
                    isSearch2Done = true;
                solution2.RemoveAt(solution2.Count - 1);
            }
        }
        return isSearch2Done;
    }
}

