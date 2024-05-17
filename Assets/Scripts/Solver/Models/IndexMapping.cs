public class IndexMapping
{
    public static int PermutationToIndex(int[] prm)
    {
        int index = 0;
        for (int i = 0; i < prm.Length - 1; i++)
        {
            index *= prm.Length - i;
            for (int j = i + 1; j < prm.Length; j++)
            {
                if (prm[i] > prm[j])
                {
                    index++;
                }
            }
        }
        return index;
    }

    public static int EvenPermutationToIndex(int[] prm)
    {
        int index = 0;
        for (int i = 0; i < prm.Length - 2; i++)
        {
            index *= prm.Length - i;
            for (int j = i + 1; j < prm.Length; j++)
            {
                if (prm[i] > prm[j])
                {
                    index++;
                }
            }
        }
        return index;
    }

    public static int[] IndexToPermutation(int index, int length)
    {
        int[] permutation = new int[length];
        permutation[length - 1] = 0;
        for (int i = length - 2; i >= 0; i--)
        {
            permutation[i] = index % (length - i);
            index /= length - i;
            for (int j = i + 1; j < length; j++)
            {
                if (permutation[j] >= permutation[i])
                {
                    permutation[j]++;
                }
            }
        }
        return permutation;
    }

    public static int[] IndexToEvenPermutation(int index, int length)
    {
        int sum = 0;
        int[] permutation = new int[length];
        permutation[length - 1] = 1;
        permutation[length - 2] = 0;
        for (int i = length - 3; i >= 0; i--)
        {
            permutation[i] = index % (length - i);
            sum += permutation[i];
            index /= length - i;
            for (int j = i + 1; j < length; j++)
            {
                if (permutation[j] >= permutation[i])
                {
                    permutation[j]++;
                }
            }
        }
        if (sum % 2 != 0)
        {
            int temp = permutation[permutation.Length - 1];
            permutation[permutation.Length - 1] = permutation[permutation.Length - 2];
            permutation[permutation.Length - 2] = temp;
        }
        return permutation;
    }

    // -------- ORIENTATION --------

    public static int OrientationToIndex(int[] orientation, int nValues)
    {
        int index = 0;
        for (int i = 0; i < orientation.Length; i++)
            index = nValues * index + orientation[i];
        return index;
    }

    public static int ZeroSumOrientationToIndex(int[] orientation, int nValues)
    {
        int index = 0;
        for (int i = 0; i < orientation.Length - 1; i++)
        {
            index = nValues * index + orientation[i];
        }
        return index;
    }

    public static int[] IndexToOrientation(int index, int nValues, int length)
    {
        int[] orientation = new int[length];
        for (int i = length - 1; i >= 0; i--)
        {
            orientation[i] = index % nValues;
            index /= nValues;
        }
        return orientation;
    }

    public static int[] IndexToZeroSumOrientation(int index, int nValues, int length)
    {
        int[] orientation = new int[length];
        orientation[length - 1] = 0;
        for (int i = length - 2; i >= 0; i--)
        {
            orientation[i] = index % nValues;
            index /= nValues;

            orientation[length - 1] += orientation[i];
        }
        orientation[length - 1] = (nValues - orientation[length - 1] % nValues) % nValues;
        return orientation;
    }

    public static int CombinationToIndex(bool[] combination, int k)
    {
        int index = 0;
        for (int i = combination.Length - 1; i >= 0 && k > 0; i--)
            if (combination[i])
                index += NChooseK(i, k--);
        return index;
    }

    public static bool[] IndexToCombination(int index, int k, int length)
    {
        bool[] combination = new bool[length];
        for (int i = length - 1; i >= 0 && k >= 0; i--)
        {
            if (index >= NChooseK(i, k))
            {
                combination[i] = true;
                index -= NChooseK(i, k--);
            }
        }
        return combination;
    }

    // -------- PRIVATE --------

    private static int NChooseK(int n, int k)
    {
        int value = 1;
        for (int i = 0; i < k; i++) value *= n - i;
        for (int i = 0; i < k; i++) value /= k - i;
        return value;
    }

}

