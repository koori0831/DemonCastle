using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Work.Combat.Projectiles
{
    public class SendBullet : Projectile
    {
        [SerializeField] private ParticleSystem destroyEffect;

        protected override void OnCollisionAfter()
        {
            Instantiate(destroyEffect,transform.position,Quaternion.identity);
            base.OnCollisionAfter();
        }
    }
}
