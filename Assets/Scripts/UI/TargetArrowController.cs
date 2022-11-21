using System.Collections;
using UnityEngine;

public class TargetArrowController : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    private float _interval = 0.1f;
    private Vector3 _startPos;
    private Vector3 _endPos;

    void Update()
    {
        DrawProjection();
    }

    public void ShowArrow(Vector3 startPos, Vector3 endPos)
    {
        _startPos = startPos;
        _endPos = endPos;
    }

    private void DrawProjection()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 1;
        int i = 0;
        var point = _startPos;
        var normalized = (_endPos - _startPos).normalized;
        lineRenderer.SetPosition(i, point);
        for (float time = 0; Vector3.Distance(_startPos, point) < Vector3.Distance(_startPos,_endPos); time += _interval)
        {
            i++;
            point = _startPos + normalized * time;
            lineRenderer.positionCount = i + 1;
            lineRenderer.SetPosition(i, point);
        }
    }
}
