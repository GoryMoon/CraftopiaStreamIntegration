using Oc;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CraftopiaStreamIntegration
{
    public class Utils
    {
        public static bool InGame => SingletonMonoBehaviour<OcGameMng>.Inst && SingletonMonoBehaviour<OcGameMng>.Inst.IsInGame;
        
        public static bool InvertControls { get; set; }
        
        public static bool InvertMouse { get; set; }
        
        public static Vector3 RandomVector3(float range) => new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
        
        public static Vector3 GetBoundedRandVector(float min, float max)
        {
            var x = Random.value > 0.5 ? Random.Range(-max, -min) : Random.Range(min, max);
            var y = Random.value > 0.5 ? Random.Range(-max, -min) : Random.Range(min, max);
            var z = Random.value > 0.5 ? Random.Range(-max, -min) : Random.Range(min, max);
            return new Vector3(x, y, z);
        }

        public static Vector3 GetRandomVector3()
        {
            return GetBoundedRandVector(0, 1).normalized;
        }
    }
}