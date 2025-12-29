using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Work.Combat;

namespace Work.Utils.Datas
{
    public abstract class DataSO : ScriptableObject
    {
        [field: SerializeField] public DataParams Params { get; private set; }
    }
}
