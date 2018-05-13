using UnityEngine;
using UnityEditor;
namespace FightTheAbyss
{
    public class PotionSpawn : MonoBehaviour
    {

        // Percentage of probability each potion has to drop
        public float health_potion_chance = 0.7f;
        public float unlimited_stamina_chance = 0.2f;
        public float unlimited_crossbow_chance = 0.1f;

        // Has spawnProbabilty of dropping a random postion in the position and with the rotation specified
        public void DropPotion(float spawnProbability, Vector3 position, Quaternion rotation)
        {
            // If this randomValue is less than spawnPorbability, we don't spawn a potion
            if (Random.value > spawnProbability)
            {
                return;
            }

            GameObject potionToInstantiate;
            // Get random value between 0 and 1, compare with probability to assing potion
            float randomPotion = Random.value;
            if (randomPotion <= health_potion_chance)
            {
                potionToInstantiate = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Potions--Low-Poly Weapons/POTIONS_TO_USE/Prefabs/Health_Potion.prefab", typeof(GameObject));
            }
            else if (randomPotion <= (health_potion_chance + unlimited_stamina_chance))
            {
                potionToInstantiate = potionToInstantiate = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Potions--Low-Poly Weapons/POTIONS_TO_USE/Prefabs/Stamina_Potion.prefab", typeof(GameObject));
            }
            else
            {
                potionToInstantiate = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Potions--Low-Poly Weapons/POTIONS_TO_USE/Prefabs/Crossbow_Potion.prefab", typeof(GameObject));
            }

            // Instantiate said potion
            Instantiate(potionToInstantiate, position, rotation);

        }
    }
}
