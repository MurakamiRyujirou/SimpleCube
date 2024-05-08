using UnityEngine;

/// キューブクラス.
/// 27(=3x3x3)サイズの配列にキュービーを持つ.
public class Cube : MonoBehaviour
{
    /// キュービーのプレハブ.
    public GameObject cubiePrefab;

    /// キュービーを保持する配列.3x3x3の27個保持する.
    public Cubie[] Cubies;

    /// キューブの回転を担当するクラスが長くなったのでCubeRotatorとして外出しした.
    private CubeRotator rotator;

    /// キューブ生成時の処理.
    void Start()
    {
        Cubies = new Cubie[3 * 3 * 3];
        for (int x = 0; x < 3; x++)
            for (int y = 0; y < 3; y++)
                for (int z = 0; z < 3; z++)
                {
                    // キュービーを生成する.
                    GameObject cubie = Instantiate(cubiePrefab, transform);

                    // キュービーの座標.
                    cubie.transform.position = new(x - 1, y - 1, z - 1);

                    // キュービー自身に自分の現在の座標を覚えさせておく.
                    Cubie cubieView = cubie.GetComponent<Cubie>();
                    cubieView.X = x;
                    cubieView.Y = y;
                    cubieView.Z = z;
                    Cubies[x * 3 * 3 + y * 3 + z] = cubieView;
                }

        // キューブの回転を担当するクラスを把握しておく.
        rotator = GetComponent<CubeRotator>();

        // 回転操作が1回終わったところで通知をもらうようにしておく.
        // こうしておくことで、回転操作終わった直後にキュービーの座標を書き換えできる.
        rotator.completeRotate += RotateDone;
    }

    /// 定期的に呼び出されるメソッド. 
    void Update()
    {
        // キューブの回転を担当するクラスに、今回転途中であれば１フレーム回転を行うように指示.
        if (rotator.IsRotating) rotator.OnUpdate();
    }

    /// 回転処理.
    /// 回転する対象のキュービーをセットしたり、回転角度と回転方向をセットしたりする.
    /// ボタンを押されたときにGameManagerから呼び出される.
    public bool Rotate(Operations oper)
    {
        // 現在回転途中の場合、新しい回転処理は行わず本処理は終了する.
        if (rotator.IsRotating) return false;

        // 回転操作から回すべきキュービーを取得する.
        Transform[] rotateCubies = GetCubies(oper);

       // 回すべきキュービーと回転操作を引数に回転処理を行う.
        return rotator.Rotate(rotateCubies, oper);
    }


    /// 回転操作に対して、動かすべきキュービーを取得する(3x3の9個).
    private Transform[] GetCubies(Operations oper)
    {
        Transform[] ret = new Transform[3 * 3];
        int x, y, z;
        if (oper == Operations.R)
        {
            x = 2;
            for (y = 0; y < 3; y++)
                for (z = 0; z < 3; z++)
                    ret [y * 3 + z] = GetCubie(x, y, z).gameObject.transform;
        }
        else if (oper == Operations.L)
        {
            x = 0;
            for (y = 0; y < 3; y++)
                for (z = 0; z < 3; z++)
                    ret[y * 3 + z] = GetCubie(x, y, z).gameObject.transform;
        }
        else if (oper == Operations.U)
        {
            y = 2;
            for (x = 0; x < 3; x++)
                for (z = 0; z < 3; z++)
                    ret[x * 3 + z] = GetCubie(x, y, z).gameObject.transform;
        }
        else if (oper == Operations.D)
        {
            y = 0;
            for (x = 0; x < 3; x++)
                for (z = 0; z < 3; z++)
                    ret[x * 3 + z] = GetCubie(x, y, z).gameObject.transform;
        }
        else if (oper == Operations.B)
        {
            z = 2;
            for (x = 0; x < 3; x++)
                for (y = 0; y < 3; y++)
                    ret[x * 3 + y] = GetCubie(x, y, z).gameObject.transform;
        }
        else if (oper == Operations.F)
        {
            z = 0;
            for (x = 0; x < 3; x++)
                for (y = 0; y < 3; y++)
                    ret[x * 3 + y] = GetCubie(x, y, z).gameObject.transform;
        }
        return ret;
    }

    /// 回転操作に対して、動かすべきキュービー1個を取得する.
    private Cubie GetCubie(int x, int y, int z)
    {
        foreach (Cubie cubie in Cubies)
        {
            if (cubie.X == x && cubie.Y == y && cubie.Z == z)
                return cubie;
        }
        return null;
    }

    /// キューブの回転操作を担当するクラスが90度ちょうど回転が終わったら呼び出される.
    /// キュービーが持っている現在の座標を書き換えてやる.
    /// ここはもうちょっとスマートに書き換えた方がよいかも...
    private void RotateDone(Operations oper, bool isOperationDone)
    {
        if (isOperationDone)
        {
            if (oper == Operations.R)
            {
                // コーナーパーツ4個の座標を書き換える.
                Cubie RDF = GetCubie(2, 0, 0);
                Cubie RDB = GetCubie(2, 0, 2);
                Cubie RUB = GetCubie(2, 2, 2);
                Cubie RUF = GetCubie(2, 2, 0);
                RDF.Y = 2;
                RDB.Z = 0;
                RUB.Y = 0;
                RUF.Z = 2;
                // エッジパーツ4個の座標を書き換える.
                Cubie RD = GetCubie(2, 0, 1);
                Cubie RB = GetCubie(2, 1, 2);
                Cubie RU = GetCubie(2, 2, 1);
                Cubie RF = GetCubie(2, 1, 0);
                RD.Y = 1; RD.Z = 0;
                RB.Z = 1; RB.Y = 0; 
                RU.Y = 1; RU.Z = 2;
                RF.Z = 1; RF.Y = 2;
            }
            if (oper == Operations.L)
            {
                Cubie LDF = GetCubie(0, 0, 0);
                Cubie LUF = GetCubie(0, 2, 0);
                Cubie LUB = GetCubie(0, 2, 2);
                Cubie LDB = GetCubie(0, 0, 2);
                LDF.Z = 2;
                LUF.Y = 0;
                LUB.Z = 0;
                LDB.Y = 2;
                Cubie LD = GetCubie(0, 0, 1);
                Cubie LB = GetCubie(0, 1, 2);
                Cubie LU = GetCubie(0, 2, 1);
                Cubie LF = GetCubie(0, 1, 0);
                LD.Y = 1; LD.Z = 2;
                LB.Z = 1; LB.Y = 2;
                LU.Y = 1; LU.Z = 0;
                LF.Z = 1; LF.Y = 0;
            }
            if (oper == Operations.U)
            {
                Cubie ULF = GetCubie(0, 2, 0);
                Cubie ULB = GetCubie(0, 2, 2);
                Cubie URB = GetCubie(2, 2, 2);
                Cubie URF = GetCubie(2, 2, 0);
                ULF.Z = 2;
                ULB.X = 2;
                URB.Z = 0;
                URF.X = 0;
                Cubie UB = GetCubie(1, 2, 2);
                Cubie UR = GetCubie(2, 2, 1);
                Cubie UF = GetCubie(1, 2, 0);
                Cubie UL = GetCubie(0, 2, 1);
                UB.X = 2; UB.Z = 1;
                UR.X = 1; UR.Z = 0;
                UF.X = 0; UF.Z = 1;
                UL.X = 1; UL.Z = 2;
            }
            if (oper == Operations.D)
            {
                Cubie DLF = GetCubie(0, 0, 0);
                Cubie DLB = GetCubie(0, 0, 2);
                Cubie DRB = GetCubie(2, 0, 2);
                Cubie DRF = GetCubie(2, 0, 0);
                DLF.X = 2;
                DLB.Z = 0;
                DRB.X = 0;
                DRF.Z = 2;
                Cubie DB = GetCubie(1, 0, 2);
                Cubie DR = GetCubie(2, 0, 1);
                Cubie DF = GetCubie(1, 0, 0);
                Cubie DL = GetCubie(0, 0, 1);
                DB.X = 0; DB.Z = 1;
                DR.X = 1; DR.Z = 2;
                DF.X = 2; DF.Z = 1;
                DL.X = 1; DL.Z = 0;
            }
            if (oper == Operations.B)
            {
                Cubie BLD = GetCubie(0, 0, 2);
                Cubie BLU = GetCubie(0, 2, 2);
                Cubie BRU = GetCubie(2, 2, 2);
                Cubie BRD = GetCubie(2, 0, 2);
                BLD.X = 2;
                BLU.Y = 0;
                BRU.X = 0;
                BRD.Y = 2;
                Cubie BU = GetCubie(1, 2, 2);
                Cubie BR = GetCubie(2, 1, 2);
                Cubie BD = GetCubie(1, 0, 2);
                Cubie BL = GetCubie(0, 1, 2);
                BU.X = 0; BU.Y = 1;
                BR.X = 1; BR.Y = 2;
                BD.X = 2; BD.Y = 1;
                BL.X = 1; BL.Y = 0;
            }
            if (oper == Operations.F)
            {
                Cubie FLD = GetCubie(0, 0, 0);
                Cubie FLU = GetCubie(0, 2, 0);
                Cubie FRU = GetCubie(2, 2, 0);
                Cubie FRD = GetCubie(2, 0, 0);
                FLD.Y = 2;
                FLU.X = 2;
                FRU.Y = 0;
                FRD.X = 0;
                Cubie FU = GetCubie(1, 2, 0);
                Cubie FR = GetCubie(2, 1, 0);
                Cubie FD = GetCubie(1, 0, 0);
                Cubie FL = GetCubie(0, 1, 0);
                FU.X = 2; FU.Y = 1;
                FR.X = 1; FR.Y = 0;
                FD.X = 0; FD.Y = 1;
                FL.X = 1; FL.Y = 2;
            }
        }
    }
}
