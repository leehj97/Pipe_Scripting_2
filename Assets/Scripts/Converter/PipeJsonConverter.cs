using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

public class PipeJsonConverter : MonoBehaviour
{
    private readonly Vector3 PIPEOFFSET = new Vector3(237066, 40, 455461);
    private readonly string PIPE_JSON_PATH = "Assets/Resources/Pipe2.json";

    [SerializeField]
    private GameObject pipePrefab;
    [SerializeField]
    private GameObject pipesParent;
    [SerializeField]
    private GameObject focus;

    private IPipeVector3Value iPipeVector3Value;
    private INameClassifier iNameClassifier;

    [Inject]
    public void Init(IPipeVector3Value _iPipeVector3Value)//, INameClassifier _iNameClassifier)
    {
        iPipeVector3Value = _iPipeVector3Value;
        // iNameClassifier = _iNameClassifier;
    }
    
    private List<float> xList = new List<float>();
    private List<float> yList = new List<float>();
    private List<float> zList = new List<float>();
    public List<float> XList
    { get { return xList; } }
    public List<float> YList
    { get { return yList; } }
    public List<float> ZList
    { get { return zList; } }
    private void Start()
    {
        iNameClassifier = pipesParent.GetComponent<PipeNameClassifier>();
        CreatePipeWithJson();
    }
    private void CreatePipeWithJson()
    {
        string jsonContent = File.ReadAllText(PIPE_JSON_PATH);
        PipeDataList pipeDataList = JsonUtility.FromJson<PipeDataList>(jsonContent);

        foreach (PipeData pipe in pipeDataList.pipeline)
        {
            CreatePipe(pipe);
        }
    }
    private void CreatePipe(PipeData pipeData)
    {
        ((StringToVector3Converter)iPipeVector3Value).SetPoint(pipeData.frNodePoint);
        Vector3 startPoint = iPipeVector3Value.GetVector3();
        
        ((StringToVector3Converter)iPipeVector3Value).SetPoint(pipeData.toNodePoint);
        Vector3 endPoint = iPipeVector3Value.GetVector3();

        Vector3 offset = endPoint - startPoint;
        Vector3 position = startPoint + offset * 0.5f - PIPEOFFSET;
        Vector3 scale = new Vector3(pipeData.pipeDia * 0.001f, offset.magnitude * 0.5f, pipeData.pipeDia * 0.001f);

        ColorUtility.TryParseHtmlString($"{pipeData.obstColor}", out Color obstcolor);

        GameObject pipe = Instantiate(pipePrefab, position, Quaternion.identity, pipesParent.transform);

        iNameClassifier.ClassifyWithName(pipe, pipeData.obstName.Split('&')[0]);

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