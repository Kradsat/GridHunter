using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class BossAction : EnemyAction
{
    //�@�^�[���ɉ����A���A�N�V����
    private enum TURN_ACTION
    {
        Attack = 1,
        Predict,
        AOE,
        MAX,
    }

    // �͈͍U���̃^�C�v
    private enum AOE_TYPE
    {
        SQUARE = 0,
        CROSS,
        MAX,
    }

    //�@�{�X�T�C�Y
    private const int BOSS_SIZE = 4;
    // �l�p�͈͍U���̃T�C�Y
    private const int SQUARE_WIDTH = 1;

    private int _turn = 0;
    private int _aoe_type = (int)AOE_TYPE.MAX;

    // �l�p�U���̃G���A
    private static List<GridPosition> _square_area_list = new List<GridPosition>();
    // �\���U���̃G���A
    private static List<GridPosition> _cross_area_list = new List<GridPosition>();

    // �͈͍U���G���A���X�g
    private List<List<GridPosition>> _area = new List<List<GridPosition>>
    {
        _square_area_list, _cross_area_list
    };

    // ������
    public override void Init(UnitStruct unit)
    {
        base.Init(unit);
        var xPos = base.GridPosition.x - 1;
        var zPos = base.GridPosition.z - 1;

        SetSquareArea(xPos, zPos);
        SetCrossArea(xPos, zPos);
    }

    public override void Update()
    {
        base.Update();

        // AOE�^�C�v���Z�b�g�����ہA�\���G���A��\������
        if(_aoe_type != (int)AOE_TYPE.MAX)
        {
            GridSystemVisual.Instance.ShowAoePrediction(_area[_aoe_type]);
        }
    }

    // �U������ۂɌĂ�
    public override void Attack(Action callback = null)
    {
        _turn = TurnSystem.Instance.TurnNumber / 2;
        switch (_turn % (int)TURN_ACTION.MAX)
        {
            case (int)TURN_ACTION.Attack:
                NormalAttack(callback);
                break;
            case (int)TURN_ACTION.Predict:
                NormalAttack();
                PickAoeType();
                break;
            case (int)TURN_ACTION.AOE:
                AreaAttack();
                break;
        }
    }

    /// <summary>
    /// ���ʍU��
    /// </summary>
    private void NormalAttack(Action callback = null)
    {
        Debug.Log("NORMAL ATTACK");
        base.Attack(callback);
    }

    /// <summary>
    /// �͈͍U���^�C�v�������_���őI��
    /// </summary>
    private void PickAoeType()
    {
        _aoe_type = UnityEngine.Random.Range(0, (int)AOE_TYPE.MAX);
    } 

    /// <summary>
    /// �͈͍U��
    /// </summary>
    private void AreaAttack()
    {
        if (_aoe_type == (int)AOE_TYPE.MAX)
        {
            PickAoeType();
        }

        foreach(var gridPos in _area[_aoe_type])
        {
            //attack
        }
        _aoe_type = (int)AOE_TYPE.MAX;
    }

    /// <summary>
    /// �l�p�U������ۂ͈̔̓Z�b�g
    /// </summary>
    private void SetSquareArea(int x, int z)
    {
        for (var zPos = x - SQUARE_WIDTH; zPos < x + SQUARE_WIDTH + BOSS_SIZE; zPos++)
        {
            for (var xPos = z - SQUARE_WIDTH; xPos < z + SQUARE_WIDTH + BOSS_SIZE; xPos++)
            {
                _square_area_list.Add(new GridPosition(zPos, xPos));
            }
        }
    }

    /// <summary>
    /// �\���U������ۂ͈̔̓Z�b�g
    /// </summary>
    private void SetCrossArea(int x, int z){
        var height = LevelGrid.Instance.GetHeight();
        var width = LevelGrid.Instance.GetWidth();

        for (int zPos = z; zPos < z + BOSS_SIZE; zPos++)
        {
            for (int xPos = 0; xPos < width; xPos++)
            {
                _cross_area_list.Add(new GridPosition(xPos, zPos));
            }
        }

        for (int zPos = 0; zPos < height; zPos++)
        {
            for (int xPos = x; xPos < x +BOSS_SIZE; xPos++)
            {
                _cross_area_list.Add(new GridPosition(xPos, zPos));
            }
        }

        _cross_area_list.Distinct().ToList();
    }
}
