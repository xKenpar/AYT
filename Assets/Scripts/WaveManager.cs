using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] List<GameObject> Enemies;
    [SerializeField] List<int> Chances;

    enum EnemyType {
        Bug,
        DDOS,
        MiddleMan,
        BackDoor,
        Chain,
        Worm,
    }
    [System.Serializable] class WaveEnemy { 
        public EnemyType Type; public float Time; 
        public void Deconstruct(out EnemyType type, out float time) { type = Type;time = Time; }    
    }
    [System.Serializable] class Wave { public List<WaveEnemy> Enemies; }

    [SerializeField] List<LineRenderer> Paths;

    [SerializeField] List<Wave> Waves;

    public IEnumerator SpawnWave(int wave) {
        if(wave >= Waves.Count){
            int totalChance = 0;
            foreach(int chance in Chances){
                totalChance += chance;
            }

            for(int i = 0;i < wave;i++){
                int enemyType = 0;
                int enemyRandom = Random.Range(0,totalChance)+1;
                foreach(int chance in Chances){
                    enemyRandom -= chance;
                    if(enemyRandom <= 0)
                        break;
                    enemyType++;
                }

                int path = Random.Range(0, Paths.Count);
                Instantiate(Enemies[(int)enemyType],Paths[path].GetPosition(0), Quaternion.identity).GetComponent<Enemy>().Init(Paths[path]);

                yield return new WaitForSeconds(Random.Range(1,4));
            }
        } else {
            foreach((EnemyType type, float time) in Waves[wave].Enemies){
                int path = Random.Range(0, Paths.Count);
                Instantiate(Enemies[(int)type],Paths[path].GetPosition(0), Quaternion.identity).GetComponent<Enemy>().Init(Paths[path]);

                yield return new WaitForSeconds(time);
            }
        }
    }
}
