using UnityEngine;
using System.Collections;

/// <summary>
/// Provides a Coroutine to change a Camera's matrix smoothly
/// </summary>
[RequireComponent(typeof(Camera))]
public class MatrixBlender : MonoBehaviour
{
    /// <summary>
    /// Calculate the next matrix for the camera
    /// </summary>
    /// <param name="from">Current matrix</param>
    /// <param name="to">Goal matrix</param>
    /// <param name="time">Time used in Lerp</param>
    /// <returns>New camera matrix</returns>
    public static Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float time)
    {
        Matrix4x4 ret = new Matrix4x4();
        for (int i = 0; i < 16; i++)
            ret[i] = Mathf.Lerp(from[i], to[i], time);
        return ret;
    }

    private IEnumerator LerpFromTo(Matrix4x4 src, Matrix4x4 dest, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            camera.projectionMatrix = MatrixLerp(src, dest, (Time.time - startTime) / duration);
            yield return 1;
        }
        camera.projectionMatrix = dest;
    }

    /// <summary>
    /// Starts a coroutine changing the camera's matrix to move it to destination
    /// </summary>
    /// <param name="targetMatrix">Matrix desired at the end of the transition</param>
    /// <param name="duration">The transition's duration</param>
    /// <returns>The started coroutine</returns>
    public Coroutine BlendToMatrix(Matrix4x4 targetMatrix, float duration)
    {
        StopAllCoroutines();
        return StartCoroutine(LerpFromTo(camera.projectionMatrix, targetMatrix, duration));
    }
}