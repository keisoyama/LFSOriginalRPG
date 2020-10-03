
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ItemDictionary))]
[CustomPropertyDrawer(typeof(LevelUpDictionary))]
public class SerializableDictionary : SerializableDictionaryPropertyDrawer
{

}