using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalRomanManager : GenericSingletonClass<GlobalRomanManager>
{
    private List<Roman> romans = new List<Roman>();

    public List<Roman> Romans { get { return romans; } }
}
