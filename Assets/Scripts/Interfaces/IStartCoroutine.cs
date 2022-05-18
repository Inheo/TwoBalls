using System.Collections;
using UnityEngine;

public interface IStartCoroutine
{
    Coroutine StartCoroutine(IEnumerator coroutine);
}
