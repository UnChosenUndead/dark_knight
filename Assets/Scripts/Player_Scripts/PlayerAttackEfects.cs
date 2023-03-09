using UnityEngine;

public class PlayerAttackEfects : MonoBehaviour
{
    
    //Spawn
    public GameObject groundImpactSpawn;

    public GameObject kickSpawn;

    public GameObject fireTorandoSpawn;
    
    public GameObject fireShieldSpawn;

    //Prefab
    public GameObject groundImpactPrefab;

    public GameObject kickPrefab;

    public GameObject fireTorandoPrefab;
    
    public GameObject fireShieldPrefab;

    public GameObject healPrefab;
    
    public GameObject thunderAttackPrefab;

    // Update is called once per frame
    void GroundImpact()
    {
        Instantiate(groundImpactPrefab, groundImpactSpawn.transform.position, Quaternion.identity);
    }

    void Kick()
    {
        Instantiate(kickPrefab, kickSpawn.transform.position, Quaternion.identity);
    }

    void FireTornado()
    {
        Instantiate(fireTorandoPrefab, fireTorandoSpawn.transform.position, Quaternion.identity); 
    }
    
    void FireShield()
    {
       GameObject fireObject = Instantiate(fireShieldPrefab, fireShieldSpawn.transform.position, Quaternion.identity);
       fireObject.transform.SetParent(transform);
    }

    void Heal()
    {
        Vector3 temp = transform.position;
        temp.y += 2f;
        GameObject healObj = Instantiate(healPrefab, temp, Quaternion.identity); 
        healObj.transform.SetParent(transform);
    }

    void ThunderAttack()
    {
        for (int i = 0; i < 8; i++)
        {
            Vector3 pos = Vector3.zero;

            switch (i)
            {
                case 0:
                    pos = new Vector3(transform.position.x - 4f, transform.position.y + 2f, transform.position.z);
                    break;
                case 1: 
                    pos = new Vector3(transform.position.x + 4f, transform.position.y + 2f, transform.position.z);
                    break;
                case 2:
                    pos = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z - 4f);
                    break;
                case 3: 
                    pos = new Vector3(transform.position.x , transform.position.y + 2f, transform.position.z +4f);   
                    break;
                case 4: 
                    pos = new Vector3(transform.position.x + 2.5f, transform.position.y + 2f, transform.position.z + 2.5f);   
                    break;
                case 5: 
                    pos = new Vector3(transform.position.x - 2.5f, transform.position.y + 2f, transform.position.z + 2.5f);   
                    break;
                case 6: 
                    pos = new Vector3(transform.position.x - 2.5f, transform.position.y + 2f, transform.position.z - 2.5f);   
                    break;
                case 7: 
                    pos = new Vector3(transform.position.x + 2.5f, transform.position.y + 2f, transform.position.z + 2.5f);   
                    break;
            }

            Instantiate(thunderAttackPrefab, pos, Quaternion.identity);
        }
    }
}
