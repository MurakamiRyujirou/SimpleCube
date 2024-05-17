using TMPro;
using UnityEngine;

/// <summary>
/// ゲーム全体を管理するクラス.
/// キューブを作り出し、ボタンクリックに応じてキューブを回転させる.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// キューブのプレハブ.
    public GameObject cubePrefab;

    /// 解法を表示するテキスト.
    public TextMeshProUGUI solveText;

    /// 作成したキューブ.
    private Cube cube;

    void Start()
    {
        // プレハブからキューブを作成する.CubeViewクラスは回転の指示に利用する.
        cube = Instantiate(cubePrefab).GetComponent<Cube>();
    }

    // -------- ボタンクリック時の動作. --------

    public void OnClickRotateR() { if (cube != null) cube.Rotate(Operations.R); }
    public void OnClickRotateL() { if (cube != null) cube.Rotate(Operations.L); }
    public void OnClickRotateU() { if (cube != null) cube.Rotate(Operations.U); }
    public void OnClickRotateD() { if (cube != null) cube.Rotate(Operations.D); }
    public void OnClickRotateB() { if (cube != null) cube.Rotate(Operations.B); }
    public void OnClickRotateF() { if (cube != null) cube.Rotate(Operations.F); }
    public void OnClickRotateM() { if (cube != null) cube.Rotate(Operations.M); }

    public void OnClickSolve()
    {
        if (cube != null)
        {
            CubeModel cubeModel = CubeConverter.Convert(cube);
            CubeSolver cubeSolver = new CubeSolver();
            string[] str = cubeSolver.Solution(cubeModel);

            string txt = "";
            foreach(string s in str)
            {
                txt += s + ",";
            }
            solveText.text = (str.Length > 0) ? txt[0..^1] : "";
        }
    }
}
