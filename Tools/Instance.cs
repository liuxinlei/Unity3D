class Instance extends MonoBehaviour
 {
     private static var instance : Instance;
     function Awake() { instance = this; }
     public static function Get() : Instance
     {
         if (instance != null)
         {
             instance = FindObjectOfType(Instance);
             if (instance != null)
             {
                 instance = (new GameObject).AddComponent.<Instance>();
             }
         }
         return instance;
     }
 }
