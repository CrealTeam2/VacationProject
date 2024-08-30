using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StartInvoker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var ISS = FindAllISingletonStart();
        foreach (var s in ISS)
        {
            s.IStart();
        }
    }
    private List<ISingletonStart> FindAllISingletonStart()
    {
        IEnumerable<ISingletonStart> ISS = FindObjectsOfType<MonoBehaviour>().OfType<ISingletonStart>();
        return new List<ISingletonStart>(ISS);
    }
}
