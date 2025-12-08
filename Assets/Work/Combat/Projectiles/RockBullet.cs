using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Work.Combat.Projectiles
{
    public class RockBullet : Projectile
    {
        protected override void OnCollisionAfter(Collision collision)
        {
            base.OnCollisionAfter(collision);
        }
    }
}
