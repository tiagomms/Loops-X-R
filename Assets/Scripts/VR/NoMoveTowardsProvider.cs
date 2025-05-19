using Oculus.Interaction;
using UnityEngine;

public class NoMoveTowardsProvider : MonoBehaviour, IMovementProvider
{
    public IMovement CreateMovement()
    {
        return null;
    }
}
