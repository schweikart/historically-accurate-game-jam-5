using UnityEngine;
using UnityEngine.AI;

public class MoveToRomanState : BaseState
{

    private static float dist = 2;
    private Roman roman;
    NavMeshAgent agent;

    public override void StartState()
    {
        agent = owner.GetComponent<NavMeshAgent>();
        roman = FindNearestRomanAndGo();
    }

    private Roman FindNearestRomanAndGo()
    {
        Roman nearest = null;
        float dis = float.MaxValue;
        foreach (Roman roman in GlobalRomanManager.Instance.Romans)
        {
            float distance = Vector3.Distance(owner.transform.position, roman.transform.position);
            if (distance < dis)
            {
                dis = distance;
                nearest = roman;
            }
        }
        if (nearest != null)
        {
            agent.destination = nearest.transform.position;
        }

        return nearest;
    }

    private static float timenewComputation = 0.05f;
    private float time;

    public override void UpdateState()
    {
        if (roman == null) return;

        time += Time.deltaTime;
        if (time >= timenewComputation)
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(roman.transform.position, path);
            time -= timenewComputation;
            agent.path = path;
        }

        if(Vector3.Distance(owner.transform.position, roman.transform.position) <= dist)
        {
            owner.ChangeState(new AttackRomanState(roman));
        }
    }

    float PathLength(NavMeshPath path)
    {
        if (path.corners.Length < 2)
            return 0;

        Vector3 previousCorner = path.corners[0];
        float lengthSoFar = 0.0F;
        int i = 1;
        while (i < path.corners.Length)
        {
            Vector3 currentCorner = path.corners[i];
            lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
            previousCorner = currentCorner;
            i++;
        }
        return lengthSoFar;
    }
}
