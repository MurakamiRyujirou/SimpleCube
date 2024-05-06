using UnityEngine;

/// �L���[�u�N���X.
/// 27(=3x3x3)�T�C�Y�̔z��ɃL���[�r�[������.
public class Cube : MonoBehaviour
{
    /// �L���[�r�[�̃v���n�u.
    public GameObject cubiePrefab;

    /// �L���[�r�[��ێ�����z��.3x3x3��27�ێ�����.
    public Cubie[] Cubies;

    /// �L���[�u�̉�]��S������N���X�������Ȃ����̂�CubeRotator�Ƃ��ĊO�o������.
    private CubeRotator rotator;

    /// �L���[�u�������̏���.
    void Start()
    {
        Cubies = new Cubie[3 * 3 * 3];
        for (int x = 0; x < 3; x++)
            for (int y = 0; y < 3; y++)
                for (int z = 0; z < 3; z++)
                {
                    // �L���[�r�[�𐶐�����.
                    GameObject cubie = Instantiate(cubiePrefab, transform);

                    // �L���[�r�[�̍��W.
                    cubie.transform.position = new(x - 1, y - 1, z - 1);

                    // �L���[�r�[���g�Ɏ����̌��݂̍��W���o�������Ă���.
                    Cubie cubieView = cubie.GetComponent<Cubie>();
                    cubieView.X = x;
                    cubieView.Y = y;
                    cubieView.Z = z;
                    Cubies[x * 3 * 3 + y * 3 + z] = cubieView;
                }

        // �L���[�u�̉�]��S������N���X��c�����Ă���.
        rotator = GetComponent<CubeRotator>();

        // ��]���삪1��I������Ƃ���Œʒm�����炤�悤�ɂ��Ă���.
        // �������Ă������ƂŁA��]����I���������ɃL���[�r�[�̍��W�����������ł���.
        rotator.completeRotate += RotateDone;
    }

    /// ����I�ɌĂяo����郁�\�b�h. 
    void Update()
    {
        // �L���[�u�̉�]��S������N���X�ɁA����]�r���ł���΂P�t���[����]���s���悤�Ɏw��.
        if (rotator.IsRotating) rotator.OnUpdate();
    }

    /// ��]����.
    /// ��]����Ώۂ̃L���[�r�[���Z�b�g������A��]�p�x�Ɖ�]�������Z�b�g�����肷��.
    /// �{�^���������ꂽ�Ƃ���GameManager����Ăяo�����.
    public bool Rotate(Operations oper)
    {
        // ���݉�]�r���̏ꍇ�A�V������]�����͍s�킸�{�����͏I������.
        if (rotator.IsRotating) return false;

        // ��]���삩��񂷂ׂ��L���[�r�[���擾����.
        Transform[] rotateCubies = GetCubies(oper);

       // �񂷂ׂ��L���[�r�[�Ɖ�]����������ɉ�]�������s��.
        return rotator.Rotate(rotateCubies, oper);
    }


    /// ��]����ɑ΂��āA�������ׂ��L���[�r�[���擾����(3x3��9��).
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

    /// ��]����ɑ΂��āA�������ׂ��L���[�r�[1���擾����.
    private Cubie GetCubie(int x, int y, int z)
    {
        foreach (Cubie cubie in Cubies)
        {
            if (cubie.X == x && cubie.Y == y && cubie.Z == z)
                return cubie;
        }
        return null;
    }

    /// �L���[�u�̉�]�����S������N���X��90�x���傤�ǉ�]���I�������Ăяo�����.
    /// �L���[�r�[�������Ă��錻�݂̍��W�����������Ă��.
    /// �����͂���������ƃX�}�[�g�ɏ��������������悢����...
    private void RotateDone(Operations oper, bool isOperationDone)
    {
        if (isOperationDone)
        {
            if (oper == Operations.R)
            {
                // �R�[�i�[�p�[�c4�̍��W������������.
                Cubie RDF = GetCubie(2, 0, 0);
                Cubie RDB = GetCubie(2, 0, 2);
                Cubie RUB = GetCubie(2, 2, 2);
                Cubie RUF = GetCubie(2, 2, 0);
                RDF.Y = 2;
                RDB.Z = 0;
                RUB.Y = 0;
                RUF.Z = 2;
                // �G�b�W�p�[�c4�̍��W������������.
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
