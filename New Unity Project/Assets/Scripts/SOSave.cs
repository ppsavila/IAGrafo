using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

[System.Serializable]
[CreateAssetMenu(menuName="Save",fileName="SaveObject")]
public class SOSave : ScriptableObject
{
    [SerializeField]
    public List<Vector3> SaveList = new List<Vector3>();


    //Teste de save *nao usado atualmente*
    public Vector3 worldBottomS;
    public int gridS;
    public Vector3 worldPointS;
    //
}
