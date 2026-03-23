using System;
using UnityEngine;

[Serializable]
public class Tower
{
    public string name;
    public int cost;
    public GameObject prefab;
    public Sprite sprite;

    public Tower(string _name, int _cost, GameObject _prefab, Sprite _sprite)
    {
        name = _name;
        cost = _cost;
        prefab = _prefab;
        sprite = _sprite;
    }
}