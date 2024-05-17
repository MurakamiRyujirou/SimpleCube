public class PruneTable
{
    public int[,] CODistance;
    public int[,] EODistance;
    public int[,] CPDistance;
    public int[,] UEDistance;

    private const int NUM_CO = 2187;
    private const int NUM_EM = 495;
    private const int NUM_EO = 2048;
    private const int NUM_CP = 40320;
    private const int NUM_EE = 24;
    private const int NUM_UE = 40320;

    public PruneTable(MovesTable movesTable, CubeModel[] moves1, CubeModel[] moves2)
    {
        CODistanceInit(movesTable, moves1);
        EODistanceInit(movesTable, moves1);
        CPDistanceInit(movesTable, moves2);
        UEDistanceInit(movesTable, moves2);
    }

    private void CODistanceInit(MovesTable movesTable, CubeModel[] moves)
    {
        CODistance = new int[NUM_CO, NUM_EM];
        for (int i = 0; i < NUM_CO; i++)
            for (int j = 0; j < NUM_EM; j++)
                CODistance[i, j] = -1;
        CODistance[0, 0] = 0;
        int distance = 0;
        int nVisited = 1;
        while (nVisited < NUM_CO * NUM_EM)
        {
            for (int i = 0; i < NUM_CO; i++)
            {
                for (int j = 0; j < NUM_EM; j++)
                {
                    if (CODistance[i, j] == distance)
                    {
                        for (int k = 0; k < moves.Length; k++)
                        {
                            int nextCO = movesTable.COMove[i, k];
                            int nextEM = movesTable.EMMove[j, k];
                            if (CODistance[nextCO, nextEM] < 0)
                            {
                                CODistance[nextCO, nextEM] = distance + 1;
                                nVisited++;
                            }
                        }
                    }
                }
            }
            distance++;
        }
    }

    private void EODistanceInit(MovesTable movesTable, CubeModel[] moves)
    {
        EODistance = new int[NUM_EO, NUM_EM];
        for (int i = 0; i < NUM_EO; i++)
            for (int j = 0; j < NUM_EM; j++)
                EODistance[i, j] = -1;
        EODistance[0, 0] = 0;
        int distance = 0;
        int nVisited = 1;
        while (nVisited < NUM_EO * NUM_EM)
        {
            for (int i = 0; i < NUM_EO; i++)
            {
                for (int j = 0; j < NUM_EM; j++)
                {
                    if (EODistance[i, j] == distance)
                    {
                        for (int k = 0; k < moves.Length; k++)
                        {
                            int nextEO = movesTable.EOMove[i, k];
                            int nextEM = movesTable.EMMove[j, k];
                            if (EODistance[nextEO, nextEM] < 0)
                            {
                                EODistance[nextEO, nextEM] = distance + 1;
                                nVisited++;
                            }
                        }
                    }
                }
            }
            distance++;
        }
    }

    private void CPDistanceInit(MovesTable movesTable, CubeModel[] moves)
    {
        CPDistance = new int[NUM_CP, NUM_EE];
        for (int i = 0; i < NUM_CP; i++)
            for (int j = 0; j < NUM_EE; j++)
                CPDistance[i, j] = -1;
        CPDistance[0, 0] = 0;
        int distance = 0;
        int nVisited = 1;
        while (nVisited < NUM_CP * NUM_EE)
        {
            for (int i = 0; i < NUM_CP; i++)
            {
                for (int j = 0; j < NUM_EE; j++)
                {
                    if (CPDistance[i, j] == distance)
                    {
                        for (int k = 0; k < moves.Length; k++)
                        {
                            int nextCP = movesTable.CPMove[i, k];
                            int nextEE = movesTable.EEMove[j, k];
                            if (CPDistance[nextCP, nextEE] < 0)
                            {
                                CPDistance[nextCP, nextEE] = distance + 1;
                                nVisited++;
                            }
                        }
                    }
                }
            }
            distance++;
        }
    }

    private void UEDistanceInit(MovesTable movesTable, CubeModel[] moves)
    {
        UEDistance = new int[NUM_UE, NUM_EE];
        for (int i = 0; i < NUM_UE; i++)
            for (int j = 0; j < NUM_EE; j++)
                UEDistance[i, j] = -1;
        UEDistance[0, 0] = 0;
        int distance = 0;
        int nVisited = 1;
        while (nVisited < NUM_UE * NUM_EE)
        {
            for (int i = 0; i < NUM_UE; i++)
            {
                for (int j = 0; j < NUM_EE; j++)
                {
                    if (UEDistance[i, j] == distance)
                    {
                        for (int k = 0; k < moves.Length; k++)
                        {
                            int nextUE = movesTable.UEMove[i, k];
                            int nextEE = movesTable.EEMove[j, k];
                            if (UEDistance[nextUE, nextEE] < 0)
                            {
                                UEDistance[nextUE, nextEE] = distance + 1;
                                nVisited++;
                            }
                        }
                    }
                }
            }
            distance++;
        }
    }
}
