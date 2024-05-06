using UnityEngine;

/// <summary>
/// �Q�[���S�̂��Ǘ�����N���X.
/// �L���[�u�����o���A�{�^���N���b�N�ɉ����ăL���[�u����]������.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// �L���[�u�̃v���n�u.
    public GameObject cubePrefab;

    /// �쐬�����L���[�u.
    private Cube cube;

    void Start()
    {
        // �v���n�u����L���[�u���쐬����.CubeView�N���X�͉�]�̎w���ɗ��p����.
        cube = Instantiate(cubePrefab).GetComponent<Cube>();
    }

    // -------- �{�^���N���b�N���̓���. --------

    public void OnClickRotateR() { if (cube != null) cube.Rotate(Operations.R); }
    public void OnClickRotateL() { if (cube != null) cube.Rotate(Operations.L); }
    public void OnClickRotateU() { if (cube != null) cube.Rotate(Operations.U); }
    public void OnClickRotateD() { if (cube != null) cube.Rotate(Operations.D); }
    public void OnClickRotateB() { if (cube != null) cube.Rotate(Operations.B); }
    public void OnClickRotateF() { if (cube != null) cube.Rotate(Operations.F); }
}
