using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/World",
    fileName = "World")]
public class World : ScriptableObject
{
    [field: SerializeField]
    public Vector2 Size;
}
