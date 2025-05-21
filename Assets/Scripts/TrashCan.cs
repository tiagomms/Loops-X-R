using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    private HashSet<Trashable> nearbyTrashables = new HashSet<Trashable>();

    private void OnTriggerEnter(Collider other)
    {
        Trashable trashable = other.GetComponentInParent<Trashable>();
        if (trashable != null)
        {
            nearbyTrashables.Add(trashable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Trashable trashable = other.GetComponentInParent<Trashable>();
        if (trashable != null)
        {
            nearbyTrashables.Remove(trashable);
        }
    }

    private void Update()
    {
        if (nearbyTrashables.Count == 0) return;
        nearbyTrashables.RemoveWhere(trashable =>
        {
            if (trashable == null) return true;
            
            if (!trashable.WasGrabbed && !trashable.IsTrash)
            {
                trashable.Trash(transform);
                return true;
            }
            return false;
        });
    }
}
