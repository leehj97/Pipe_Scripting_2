using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadFromJson : MonoBehaviour
{
    const string filePath = "Assets/Resources/Pipe2.json";
    private readonly Vector3 _pipeOffset = new Vector3(237066, 40, 455461);
    
    [SerializeField]
    private GameObject _pipePrefab;
    [SerializeField]
    private GameObject _pipesParent;
    [SerializeField]
    private GameObject _focus;
    
    private IGetVector3Value _getPointFromString;
    private List<float> _xList = new List<float>();
    private List<float> _yList = new List<float>();
    private List<float> _zList = new List<float>();
    public List<float> XList
    { get { return _xList; } }
    public List<float> YList
    { get { return _yList; } }
    public List<float> ZList
    { get { return _zList; } }
    void Start()
    {
        _getPointFromString = new GetPointFromString();
        DrawPipeWithData();
    }
    private void DrawPipeWithData()
    {
        string jsonContent = File.ReadAllText(filePath);
        PipeDataList pipeDataList = JsonUtility.FromJson<PipeDataList>(jsonContent);

        foreach (PipeData pipe in pipeDataList.pipeline)
        {
            CreatePipe(pipe);
        }
    }
    private void CreatePipe(PipeData pipeData)
    {
        ((GetPointFromString)_getPointFromString).SetPoint(pipeData.frNodePoint);
        Vector3 startPoint = _getPointFromString.GetVector3();
        
        ((GetPointFromString)_getPointFromString).SetPoint(pipeData.toNodePoint);
        Vector3 endPoint = _getPointFromString.GetVector3();

        Vector3 offset = endPoint - startPoint;
        Vector3 position = startPoint + offset * 0.5f - _pipeOffset;
        Vector3 scale = new Vector3(pipeData.pipeDia * 0.001f, offset.magnitude * 0.5f, pipeData.pipeDia * 0.001f);

        ColorUtility.TryParseHtmlString($"{pipeData.obstColor}", out Color obstcolor);

        GameObject pipe = Instantiate(_pipePrefab, position, Quaternion.identity, _pipesParent.transform);
        pipe.transform.up = offset;
        pipe.transform.localScale = scale;
        pipe.GetComponent<MeshRenderer>().material.color = obstcolor;

        XList.Add(position.x);
        YList.Add(position.y);
        ZList.Add(position.z);

        PipeInfo pipeInfo = pipe.GetComponent<PipeInfo>();
        if (pipeInfo != null)
        {
            pipeInfo.pipeMaterial = pipeData.pipeMaterial;
            pipeInfo.pipeYear = pipeData.pipeYear;
            pipeInfo.linkId = pipeData.linkId;
            pipeInfo.obstName = pipeData.obstName;
        }
    }
}