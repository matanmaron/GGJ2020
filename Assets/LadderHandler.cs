using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderHandler : MonoBehaviour
{
    public void HandleLadders(Transform me, Collider2D other,float speed, Ladder ladder)
    {
        var num = other.name[1];
        var dir = other.name[2];
        if ((ladder == Ladder.Down && dir == 'D') || (ladder == Ladder.Up && dir == 'U'))
        {
            return;
        }
        var newdir = dir == 'U' ? 'D' : 'U';
        var newlad = "L" + num + newdir;
        var newpos = GameObject.Find(newlad);
        if (GameManager.Instance.IsDebug && newpos == null) { Debug.Log("teleporting problem..."); }

        StartCoroutine(MoveToPosition(me, newpos.gameObject.transform.position,speed));
    }

    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float speed)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / speed;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }
}
