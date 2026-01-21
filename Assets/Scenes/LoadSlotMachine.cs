using Sirenix.OdinInspector;
using UnityEngine;

public class LoadSlotMachine : MonoBehaviour
{
    [SerializeField] private GameObject slotMachinePrefab;
    [SerializeField] private Transform spawnPoint;

    private GameObject currentMachine;

    [Button("Spawn Machine")]
    public void SpawnMachine()
    {
        if (currentMachine != null)
        {
            Destroy(currentMachine);
        }

        currentMachine = Instantiate(
            slotMachinePrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );
    }
}
