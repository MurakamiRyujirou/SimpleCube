using UnityEngine;

/// キューブを構成するブロック(キュービー)のクラス.
/// 現在の回転向きなども保持できるように拡張すると良い.
public class Cubie : MonoBehaviour
{
    /// ６面に張り付けるパネルのプレハブ.
    public GameObject panelPrefab;

    /// パネルに設定する色設定(マテリアル).
    public Material[] materials;

    /// 現在キュービーが位置する座標.0～2の値をとる.
    public int X, Y, Z;

    /// キュービー生成時の処理.
    void Start()
    {
        // ６面にパネルを生成して色(マテリアル)を設定する.
        for (int i = 0; i < 6; i++)
        {
            Vector3 pos = positions[i]; // 座標.
            Quaternion rot = rotations[i]; // 回転向き.
            GameObject panel = Instantiate(panelPrefab, transform);
            panel.transform.localPosition = pos;
            panel.transform.localRotation = rot;
            panel.GetComponent<MeshRenderer>().material = materials[i];
        }
    }

    /// パネルの位置情報(右->左->上->下->奥->前の順番).
    private readonly Vector3[] positions = new Vector3[]
    {
        new Vector3( 0.51f,  0.00f,  0.00f),
        new Vector3(-0.51f,  0.00f,  0.00f),
        new Vector3( 0.00f,  0.51f,  0.00f),
        new Vector3( 0.00f, -0.51f,  0.00f),
        new Vector3( 0.00f,  0.00f,  0.51f),
        new Vector3( 0.00f,  0.00f, -0.51f)
    };

    /// パネルの向き情報(右->左->上->下->奥->前の順番).
    private readonly Quaternion[] rotations = new Quaternion[]
    {
        Quaternion.Euler(new Vector3(  0f,  0f,-90f)),
        Quaternion.Euler(new Vector3(  0f,  0f, 90f)),
        Quaternion.Euler(new Vector3(  0f,  0f,  0f)),
        Quaternion.Euler(new Vector3(180f,  0f,  0f)),
        Quaternion.Euler(new Vector3( 90f,  0f,  0f)),
        Quaternion.Euler(new Vector3(-90f,  0f,  0f))
    };

}
