using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGJ.Player;

namespace GGJ.Projectiles
{
    public class ProjectileManager : MonoBehaviour
    {

        private static ProjectileManager _instance;

        private static List<Bullet> bulletList;

        [SerializeField]
        private Bullet bulletPrefab;
        
        void Awake()
        {
            _instance = this;
            bulletList = new List<Bullet>();
        }

        public static Bullet CreateProjectile(GameObject owner, Vector2 dir, float speed = 2.0f, int damage = 1)
        {
            return _instance.InstantiateProjectile(owner,dir,speed,damage);
        }

        // Fill this out
        private Bullet InstantiateProjectile(GameObject owner, Vector2 dir, float speed = 2.0f, int damage = 1)
        {
            Bullet bulletObj = Instantiate(bulletPrefab, owner.transform.position, Quaternion.identity);
            bulletObj.LaunchBullet(owner, dir, speed, damage);
            bulletList.Add(bulletObj);
            return bulletObj;
        }

        public static void DestroyBullet(Bullet bullet)
        {
            bulletList.Remove(bullet);
            Destroy(bullet.gameObject);
        }

        // For cleaning up a scene
        private static void DespawnAllBullets()
        {
            for(int i=bulletList.Count-1; i>=0; i--)
            {
                Destroy(bulletList[i].gameObject);
            }
            bulletList.Clear();
        }

        // Reflect all projectiles at point + range
        public static void ReflectAllProjectiles(Vector3 position, float range, GameObject newOwner)
        {
            // Get all projectiles in range of position
            for(int i=0;i<bulletList.Count;i++)
            {
                Bullet bullet = bulletList[i];

                if( Vector3.Distance(bullet.transform.position, position) <= range
                    && bullet.GetOwner() != newOwner  )
                {
                    // Reflect! New direction is away from player
                    Vector3 newDirection = bullet.transform.position - position;
                    newDirection.y = 0;
                    bullet.transform.forward = newDirection.normalized;
                    bullet.ChangeOwner(newOwner);
                    bullet.isFriendly = true;
                    
                }
            }
        }

    }

}