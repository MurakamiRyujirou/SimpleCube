using UnityEngine;

/// �L���[�u���\������u���b�N(�L���[�r�[)�̃N���X.
/// ���݂̉�]�����Ȃǂ��ێ��ł���悤�Ɋg������Ɨǂ�.
public class Cubie : MonoBehaviour
{
    /// �U�ʂɒ���t����p�l���̃v���n�u.
    public GameObject panelPrefab;

    /// �p�l���ɐݒ肷��F�ݒ�(�}�e���A��).
    public Material[] materials;

    /// ���݃L���[�r�[���ʒu������W.0�`2�̒l���Ƃ�.
    public int X, Y, Z;

    /// �L���[�r�[�������̏���.
    void Start()
    {
        // �U�ʂɃp�l���𐶐����ĐF(�}�e���A��)��ݒ肷��.
        for (int i = 0; i < 6; i++)
        {
            Vector3 pos = positions[i]; // ���W.
            Quaternion rot = rotations[i]; // ��]����.
            GameObject panel = Instantiate(panelPrefab, transform);
            panel.transform.localPosition = pos;
            panel.transform.localRotation = rot;
            panel.GetComponent<MeshRenderer>().material = materials[i];
        }
    }

    /// �p�l���̈ʒu���(�E->��->��->��->��->�O�̏���).
    private readonly Vector3[] positions = new Vector3[]
    {
        new Vector3( 0.51f,  0.00f,  0.00f),
        new Vector3(-0.51f,  0.00f,  0.00f),
        new Vector3( 0.00f,  0.51f,  0.00f),
        new Vector3( 0.00f, -0.51f,  0.00f),
        new Vector3( 0.00f,  0.00f,  0.51f),
        new Vector3( 0.00f,  0.00f, -0.51f)
    };

    /// �p�l���̌������(�E->��->��->��->��->�O�̏���).
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
