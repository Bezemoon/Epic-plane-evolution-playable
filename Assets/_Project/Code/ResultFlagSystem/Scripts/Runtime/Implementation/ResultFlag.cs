using UnityEngine;

namespace ResultFlagSystem
{
    public class ResultFlag : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        
        public void Place(Vector3 position)
        {
            _transform.position = new Vector3(position.x, _transform.position.y, position.z);
            _transform.gameObject.SetActive(true);
        }
 
        public void Hide()
        {
            _transform.gameObject.SetActive(false);
        }
    }
}
