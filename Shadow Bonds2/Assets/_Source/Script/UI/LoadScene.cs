using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Source.Script.UI
{
   public class LoadScene : MonoBehaviour
   {
      [SerializeField] private int sceneNumber;
      public void LoadScenes()
      {
         SceneManager.LoadScene(sceneNumber);
      }
   }
}
