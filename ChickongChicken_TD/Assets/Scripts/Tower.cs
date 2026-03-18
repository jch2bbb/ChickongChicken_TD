using System;
using System.ComponentModel.Design.Serialization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;

[Serializable]

public class Tower
{
    public string name;
    public int cost;
    public GameObject prefab;

    PublicKey Tower(StringBuilder _name, int _cost, GameObject _prefab)
    {
        name = _name;
        cost = _cost;
        prefab = _prefab;
    }
}
