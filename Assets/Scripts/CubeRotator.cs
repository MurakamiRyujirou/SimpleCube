using System.Collections.Generic;
using UnityEngine;

/// �L���[�u�̉�]�����S������N���X.
/// �u������񂵂Ȃ����v�Ǝw�������̂ŁA����ɏ]����RotateAround�����邾��.
public class CubeRotator : MonoBehaviour
{
    // ��]�������ɒʒm������@�\.
    public delegate void OnCompleteRotate(Operations oper, bool isOperationDone);
    public OnCompleteRotate completeRotate;

    /// ���݉�]���Ă���L���[�r�[�̃��X�g.
    private List<Transform> rotateCubies = new();

    /// ���ݍs�Ȃ��Ă����]����.
    public Operations CurrentOperation { get; private set; }

    /// ���݉�]���쒆���ۂ��̏�Ԃ�ێ�����u�[���l.
    public bool IsRotating { get; private set; } = false;

    /// ���݉�]���̃L���[�r�[�̉�]��.0f����90f or 180f�ƂȂ�.
    /// ������_dir�ŕێ�����̂ŁA-90f�Ȃǃ}�C�i�X�̒l�͎����Ȃ����ƂƂ���.
    public float Angle { get; private set; } = 0f;

    /// ���ݍs�Ȃ��Ă����]�̉�]�X�s�[�h.
    public float RotateSpeed = 10f;

    /// ���݂̉�]����ł̉�]�ʂ̍ő�90f or 180f�ƂȂ�.
    private float angleMax = 90f;

    /// ���ݍs�Ȃ��Ă����]�́A��]����(+1,-1�̂����ꂩ).
    private float dir = 0f;

    /// ��]������s�����߂̐ݒ������.
    /// <param name="cubies">��]����L���[�r�[�̃��X�g.</param>
    /// <param name="oper">��]����.</param>
    public bool Rotate(Transform[] cubies, Operations oper)
    {
        // ��]���ɂ͎��̉�]�͍s��Ȃ�.
        if (IsRotating) return false;

        // �V���ɍs����]�p�ɉ�]�Ώۂ�Transform���i�[���郊�X�g��������.
        rotateCubies = new List<Transform>();
        foreach (Transform cubie in cubies)
        {
            rotateCubies.Add(cubie);
        }
        CurrentOperation = oper;   // ���ꂩ��s����]������L�^.
        IsRotating = true;         // ��]���̃X�e�[�^�X�ɕύX.
        Angle = 0f;                // ���݂̉�]�ʂ����Z�b�g.
        angleMax = 90f;            // ��]�̍ő�l���Z�b�g.
        dir = Direction(oper);     // ��]�������擾.
        return true;
    }

    /// �X�V����.
    public void OnUpdate()
    {
        if (IsRotating)
            RotateAround();
    }

    /// ��]����̃L���[�u��������������.
    private void RotateAround()
    {
        // ����̃t���[���œ��B�����]�p�x.
        float target;

        // ����̃t���[���ŉ�]���I�����邩(90/180�x�������)���f����.
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

        // �e�L���[�r�[����]������.
        foreach (Transform cubie in rotateCubies)
        {
            // �W���@�\��RotateAround���Ăяo��.
            // GetAxis�ŉ�]�����擾���邪�A�v���X�}�C�i�X�l�����ʓ|�Ȃ̂�X,Y,Z�̂����ꂩ�R��Ƃ���.speed�͉�]��.
            // dir�͉�]�̌����i�����̕����j�ł����]����J�n����Direction�Ōv�Z����.
            // [����]GetAxis�̓L���[�u�S�̂̌�����ύX���铮�����s��ꂽ�Ƃ��Ɏ����v�Z�������K�v������̂œs�x�v�Z�Ƃ���.
            Vector3 axis = GetAxis(CurrentOperation);
            cubie.RotateAround(transform.position, axis, target * dir);
        }

        // ����̃t���[���ŉ�]�I���ł����isRotating�̏�Ԃ�ω�������.
        if (rotateDone)
        {
            IsRotating = false;
            completeRotate(CurrentOperation, true);
        }
    }

    /// ��]���ƂȂ�Vector3�̒l���擾����.Cube���Ɖ�]�����Ă��������擾�ł���悤��transform����擾���Ă���.
    private Vector3 GetAxis(Operations currentOperation)
    {
        if (currentOperation == Operations.R || currentOperation == Operations.L) return transform.right;
        if (currentOperation == Operations.U || currentOperation == Operations.D) return transform.up;
        if (currentOperation == Operations.B || currentOperation == Operations.F) return transform.forward;
        return Vector3.zero;
    }

    /// ��]���ɑ΂��Đ����ǂ���̕����ɉ�]�����邩���擾����.
    private float Direction(Operations currentOperation)
    {
        if (currentOperation == Operations.R) return 1f;
        if (currentOperation == Operations.L) return -1f;
        if (currentOperation == Operations.U) return 1f;
        if (currentOperation == Operations.D) return -1f;
        if (currentOperation == Operations.B) return 1f;
        if (currentOperation == Operations.F) return -1f;
        return 0f;
    }
}
