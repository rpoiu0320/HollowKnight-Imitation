using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Data/Tower")] // 우클릭하여 생성할 때 매뉴에 추가되는걸 알 수 있음
public class TowerData : ScriptableObject//, ISerializationCallbackReceiver
{                     // 데이터 저장소로 사용

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
