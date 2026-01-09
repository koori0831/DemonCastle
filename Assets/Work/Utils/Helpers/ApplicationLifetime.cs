
#if UNITY_EDITOR

using UnityEditor;

#endif

using UnityEngine;

namespace Work.Utils.Helpers
{
    public class ApplicationLifetime : MonoBehaviour
    {
        private bool disposed;

        private void OnApplicationQuit()
        {
            DisposeOnce();
        }

#if UNITY_EDITOR
        private void OnDestroy()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            DisposeOnce();
        }
#endif

        private void DisposeOnce()
        {
            if (disposed) return;
            disposed = true;

            GameBootstrap.HelperManager?.Dispose();
        }
    }
}