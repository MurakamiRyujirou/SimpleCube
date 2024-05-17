using System.Collections.Generic;
using UnityEngine;

/// キューブの回転操作を担当するクラス.
/// 「これを回しなさい」と指示されるので、それに従ってRotateAroundをするだけ.
public class CubeRotator : MonoBehaviour
{
    // 回転完了時に通知をする機能.
    public delegate void OnCompleteRotate(Operations oper, bool isOperationDone);
    public OnCompleteRotate completeRotate;

    /// 現在回転しているキュービーのリスト.
    private List<Transform> rotateCubies = new();

    /// 現在行なっている回転操作.
    public Operations CurrentOperation { get; private set; }

    /// 現在回転操作中か否かの状態を保持するブール値.
    public bool IsRotating { get; private set; } = false;

    /// 現在回転中のキュービーの回転量.0fから90f or 180fとなる.
    /// 向きを_dirで保持するので、-90fなどマイナスの値は持たないこととする.
    public float Angle { get; private set; } = 0f;

    /// 現在行なっている回転の回転スピード.
    public float RotateSpeed = 10f;

    /// 現在の回転操作での回転量の最大90f or 180fとなる.
    private float angleMax = 90f;

    /// 現在行なっている回転の、回転向き(+1,-1のいずれか).
    private float dir = 0f;

    /// 回転操作を行うための設定をする.
    /// <param name="cubies">回転するキュービーのリスト.</param>
    /// <param name="oper">回転操作.</param>
    public bool Rotate(Transform[] cubies, Operations oper)
    {
        // 回転中には次の回転は行わない.
        if (IsRotating) return false;

        // 新たに行う回転用に回転対象のTransformを格納するリストを初期化.
        rotateCubies = new List<Transform>();
        foreach (Transform cubie in cubies)
        {
            rotateCubies.Add(cubie);
        }
        CurrentOperation = oper;   // これから行う回転操作を記録.
        IsRotating = true;         // 回転中のステータスに変更.
        Angle = 0f;                // 現在の回転量をリセット.
        angleMax = 90f;            // 回転の最大値をセット.
        dir = Direction(oper);     // 回転向きを取得.
        return true;
    }

    /// 更新処理.
    public void OnUpdate()
    {
        if (IsRotating)
            RotateAround();
    }

    /// 回転操作のキューブが動く処理部分.
    private void RotateAround()
    {
        // 今回のフレームで到達する回転角度.
        float target;

        // 今回のフレームで回転が終了するか(90/180度回ったか)判断する.
        bool rotateDone = Angle + RotateSpeed >= angleMax;
        if (rotateDone)
        {
            target = angleMax - Angle;
            Angle = 0f;
        }
        else
        {
            target = RotateSpeed;
            Angle += RotateSpeed;
        }

        // 各キュービーを回転させる.
        foreach (Transform cubie in rotateCubies)
        {
            // 標準機能のRotateAroundを呼び出す.
            // GetAxisで回転軸を取得するが、プラスマイナス考慮が面倒なのでX,Y,Zのいずれか３種とした.speedは回転量.
            // dirは回転の向き（正負の符号）であり回転操作開始時にDirectionで計算する.
            // [注意]GetAxisはキューブ全体の向きを変更する動きが行われたときに軸を計算し直す必要があるので都度計算とする.
            Vector3 axis = GetAxis(CurrentOperation);
            cubie.RotateAround(transform.position, axis, target * dir);
        }

        // 今回のフレームで回転終了であればisRotatingの状態を変化させる.
        if (rotateDone)
        {
            IsRotating = false;
            completeRotate(CurrentOperation, true);
        }
    }

    /// 回転軸となるVector3の値を取得する.Cubeごと回転させても正しく取得できるようにtransformから取得している.
    private Vector3 GetAxis(Operations currentOperation)
    {
        if (currentOperation == Operations.R || currentOperation == Operations.L || currentOperation == Operations.M) return transform.right;
        if (currentOperation == Operations.U || currentOperation == Operations.D) return transform.up;
        if (currentOperation == Operations.B || currentOperation == Operations.F) return transform.forward;
        return Vector3.zero;
    }

    /// 回転軸に対して正負どちらの方向に回転させるかを取得する.
    private float Direction(Operations currentOperation)
    {
        if (currentOperation == Operations.R) return 1f;
        if (currentOperation == Operations.L) return -1f;
        if (currentOperation == Operations.U) return 1f;
        if (currentOperation == Operations.D) return -1f;
        if (currentOperation == Operations.B) return 1f;
        if (currentOperation == Operations.F) return -1f;
        if (currentOperation == Operations.M) return -1f;
        return 0f;
    }
}
