using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LevelScatterSystem.Editor
{
    public sealed class LevelScatterWindow : EditorWindow
    {
        private const string GeneratedParentName = "Generated";
        private const string UndoName = "Scatter Level";
        private const float FullTurn = 360f; // град
        private const float SinkIntoGround = 0.05f; // м
        private const float LakeLeftEdge = -2f; // м
        private const float LakeRightEdge = 16f; // м
        private const float LakeNearEdge = 207f; // м
        private const float LakeFarEdge = 263f; // м
 
        private static readonly float[] Sides = { -1f, 1f };
 
        // Начало и конец зоны по оси Z, м.
        private static readonly LevelZone Runway = new LevelZone("Z000_Runway", 0f, 45f);
        private static readonly LevelZone Forest = new LevelZone("Z045_Forest", 45f, 170f);
        private static readonly LevelZone Crash = new LevelZone("Z170_Crash", 170f, 190f);
        private static readonly LevelZone Clearing = new LevelZone("Z190_Clearing", 190f, 280f);
        private static readonly LevelZone Continuation = new LevelZone("Z280_Continue", 280f, 330f);
 
        // Числа: расстояние от центра уровня и шаг вдоль уровня в метрах, затем множитель масштаба.
        private static readonly ScatterRow[] Rows =
        {
            new ScatterRow(Runway, ItemKind.Tree, 22f, 36f, 10f, 15f, 0.9f, 1.25f),
            new ScatterRow(Runway, ItemKind.Bush, 13f, 20f, 10f, 14f, 0.8f, 1.2f),
            new ScatterRow(Runway, ItemKind.Rock, 14f, 24f, 20f, 28f, 0.8f, 1.5f),
            new ScatterRow(Forest, ItemKind.Tree, 13f, 16f, 8f, 10f, 0.9f, 1.2f),
            new ScatterRow(Forest, ItemKind.Tree, 18f, 24f, 8f, 11f, 0.95f, 1.25f),
            new ScatterRow(Forest, ItemKind.Tree, 26f, 38f, 12f, 15f, 1f, 1.3f),
            new ScatterRow(Forest, ItemKind.Bush, 12.5f, 15f, 18f, 24f, 0.8f, 1.2f),
            new ScatterRow(Forest, ItemKind.Rock, 13f, 18f, 24f, 34f, 0.8f, 1.5f),
            new ScatterRow(Crash, ItemKind.Tree, 20f, 26f, 8f, 10f, 0.95f, 1.25f),
            new ScatterRow(Crash, ItemKind.Tree, 28f, 38f, 10f, 13f, 1f, 1.3f),
            new ScatterRow(Crash, ItemKind.Rock, 12.5f, 16f, 8f, 12f, 0.8f, 1.5f),
            new ScatterRow(Clearing, ItemKind.Tree, 34f, 50f, 12f, 18f, 1f, 1.3f),
            new ScatterRow(Clearing, ItemKind.Rock, 12.5f, 30f, 22f, 30f, 0.8f, 1.5f),
            new ScatterRow(Continuation, ItemKind.Tree, 13f, 16f, 8f, 10f, 0.9f, 1.2f),
            new ScatterRow(Continuation, ItemKind.Tree, 18f, 26f, 8f, 11f, 0.95f, 1.25f),
            new ScatterRow(Continuation, ItemKind.Bush, 12.5f, 16f, 16f, 22f, 0.8f, 1.2f)
        };
 
        [SerializeField] private Transform _zonesParent;
        [SerializeField] private GameObject[] _trees;
        [SerializeField] private GameObject[] _rocks;
        [SerializeField] private GameObject[] _bushes;
        [SerializeField] private int _seed = 12345;
 
        private SerializedObject _serializedWindow;
        private System.Random _random;
        private int _treeCount;
        private int _rockCount;
        private int _bushCount;
 
        [MenuItem("Tools/Level/Scatter")]
        private static void Open()
        {
            GetWindow<LevelScatterWindow>("Level Scatter");
        }
 
        private void OnEnable()
        {
            _serializedWindow = new SerializedObject(this);
        }
 
        private void OnGUI()
        {
            DrawFields();
            EditorGUILayout.Space();
 
            if (GUILayout.Button("Generate"))
            {
                Generate();
            }
 
            if (GUILayout.Button("Clear Generated"))
            {
                ClearGeneratedItems();
                MarkSceneDirty();
            }
        }
 
        private void DrawFields()
        {
            _serializedWindow.Update();
            EditorGUILayout.PropertyField(_serializedWindow.FindProperty(nameof(_zonesParent)));
            EditorGUILayout.PropertyField(_serializedWindow.FindProperty(nameof(_trees)), true);
            EditorGUILayout.PropertyField(_serializedWindow.FindProperty(nameof(_rocks)), true);
            EditorGUILayout.PropertyField(_serializedWindow.FindProperty(nameof(_bushes)), true);
            EditorGUILayout.PropertyField(_serializedWindow.FindProperty(nameof(_seed)));
            _serializedWindow.ApplyModifiedProperties();
        }
 
        private void Generate()
        {
            if (CanGenerate() == false)
            {
                Debug.LogError("Level Scatter: укажи Zones Parent и заполни все списки префабов.");
                return;
            }
 
            Undo.IncrementCurrentGroup();
            int undoGroup = Undo.GetCurrentGroup();
 
            ClearGeneratedItems();
            ResetPlacementState();
 
            foreach (ScatterRow row in Rows)
            {
                PlaceRow(row);
            }
 
            Undo.CollapseUndoOperations(undoGroup);
            MarkSceneDirty();
            LogPlacedItems();
        }
 
        private bool CanGenerate()
        {
            return _zonesParent != null
                && HasPrefabs(_trees)
                && HasPrefabs(_rocks)
                && HasPrefabs(_bushes);
        }
 
        private void ClearGeneratedItems()
        {
            if (_zonesParent == null)
            {
                return;
            }
 
            for (int i = 0; i < _zonesParent.childCount; i++)
            {
                Transform generatedParent = GetChildOrNull(_zonesParent.GetChild(i), GeneratedParentName);
                if (generatedParent != null)
                {
                    Undo.DestroyObjectImmediate(generatedParent.gameObject);
                }
            }
        }
 
        private void ResetPlacementState()
        {
            _random = new System.Random(_seed);
            _treeCount = 0;
            _rockCount = 0;
            _bushCount = 0;
        }
 
        private void PlaceRow(ScatterRow row)
        {
            Transform generatedParent = GetGeneratedParent(row.Zone.Name);
            GameObject[] prefabs = GetPrefabs(row.Kind);
 
            foreach (float side in Sides)
            {
                float distanceAlongLevel = row.Zone.Start + PickBetween(0f, row.MaxStep);
 
                while (distanceAlongLevel < row.Zone.End)
                {
                    float sideOffset = side * PickBetween(row.MinSideDistance, row.MaxSideDistance);
                    Vector3 position = new Vector3(sideOffset, -SinkIntoGround, distanceAlongLevel);
 
                    if (IsInsideLake(position) == false)
                    {
                        float scale = PickBetween(row.MinScale, row.MaxScale);
                        PlaceItem(prefabs, generatedParent, position, scale);
                        RememberPlacedItem(row.Kind);
                    }
 
                    distanceAlongLevel += PickBetween(row.MinStep, row.MaxStep);
                }
            }
        }
 
        private Transform GetGeneratedParent(string zoneName)
        {
            Transform zone = GetOrCreateChild(_zonesParent, zoneName);
            return GetOrCreateChild(zone, GeneratedParentName);
        }
 
        private GameObject[] GetPrefabs(ItemKind kind)
        {
            switch (kind)
            {
                case ItemKind.Rock:
                    return _rocks;
                case ItemKind.Bush:
                    return _bushes;
                default:
                    return _trees;
            }
        }
 
        private void PlaceItem(GameObject[] prefabs, Transform parent, Vector3 position, float scale)
        {
            GameObject prefab = prefabs[_random.Next(prefabs.Length)];
            GameObject item = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
            Undo.RegisterCreatedObjectUndo(item, UndoName);
 
            item.transform.position = position;
            item.transform.rotation = Quaternion.Euler(0f, PickBetween(0f, FullTurn), 0f);
            item.transform.localScale = prefab.transform.localScale * scale;
        }
 
        private void RememberPlacedItem(ItemKind kind)
        {
            switch (kind)
            {
                case ItemKind.Rock:
                    _rockCount++;
                    break;
                case ItemKind.Bush:
                    _bushCount++;
                    break;
                default:
                    _treeCount++;
                    break;
            }
        }
 
        private void LogPlacedItems()
        {
            Debug.Log("Level Scatter: деревья " + _treeCount + ", камни " + _rockCount + ", кусты " + _bushCount);
        }
 
        private float PickBetween(float min, float max)
        {
            return min + (float)_random.NextDouble() * (max - min);
        }
 
        private static bool IsInsideLake(Vector3 position)
        {
            bool isBetweenSides = position.x >= LakeLeftEdge && position.x <= LakeRightEdge;
            bool isBetweenEnds = position.z >= LakeNearEdge && position.z <= LakeFarEdge;
            return isBetweenSides && isBetweenEnds;
        }
 
        private static Transform GetOrCreateChild(Transform parent, string childName)
        {
            Transform child = GetChildOrNull(parent, childName);
            if (child != null)
            {
                return child;
            }
 
            GameObject created = new GameObject(childName);
            created.transform.SetParent(parent, false);
            Undo.RegisterCreatedObjectUndo(created, UndoName);
            return created.transform;
        }
 
        private static Transform GetChildOrNull(Transform parent, string childName)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (child.name == childName)
                {
                    return child;
                }
            }
 
            return null;
        }
 
        private static bool HasPrefabs(GameObject[] prefabs)
        {
            if (prefabs == null || prefabs.Length == 0)
            {
                return false;
            }
 
            foreach (GameObject prefab in prefabs)
            {
                if (prefab == null)
                {
                    return false;
                }
            }
 
            return true;
        }
 
        private static void MarkSceneDirty()
        {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
 
        private enum ItemKind
        {
            Tree,
            Rock,
            Bush
        }
 
        private readonly struct LevelZone
        {
            public LevelZone(string name, float start, float end)
            {
                Name = name;
                Start = start;
                End = end;
            }
 
            public string Name { get; }
 
            public float Start { get; }
 
            public float End { get; }
        }
 
        private readonly struct ScatterRow
        {
            public ScatterRow(LevelZone zone, ItemKind kind, float minSideDistance, float maxSideDistance,
                float minStep, float maxStep, float minScale, float maxScale)
            {
                Zone = zone;
                Kind = kind;
                MinSideDistance = minSideDistance;
                MaxSideDistance = maxSideDistance;
                MinStep = minStep;
                MaxStep = maxStep;
                MinScale = minScale;
                MaxScale = maxScale;
            }
 
            public LevelZone Zone { get; }
 
            public ItemKind Kind { get; }
 
            public float MinSideDistance { get; }
 
            public float MaxSideDistance { get; }
 
            public float MinStep { get; }
 
            public float MaxStep { get; }
 
            public float MinScale { get; }
 
            public float MaxScale { get; }
        }
    }
}