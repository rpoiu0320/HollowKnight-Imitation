using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Data/Tower")] // ��Ŭ���Ͽ� ������ �� �Ŵ��� �߰��Ǵ°� �� �� ����
public class TowerData : ScriptableObject//, ISerializationCallbackReceiver
{                     // ������ ����ҷ� ���

    //public void OnAfterDeserialize()
    //{
    //    //tower = Towers;
    //}

    //public void OnBeforeSerialize()
    //{

    //}

    [SerializeField] TowerInfo[] towers;
    public TowerInfo[] Towers { get { return towers; } }

    [Serializable]
    public class TowerInfo
    {
        public Tower tower;
        public string name;
        public string description;

        public float buildTime;
        public float buildCost;
        public float sellCost;
        public float range;
    }

    //[SerializeField]
    //public class TowerInfo
    //{
    //    public Tower tower;
    //    public float a;
    //    public int b;
    //}
}
