using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

public class PipeJsonConverterAndroid : MonoBehaviour
{
    private readonly Vector3 PIPEOFFSET = new Vector3(237066, 40, 455461);
    private string PIPE_JSON_PATH = "Pipe2";

    [SerializeField]
    private GameObject pipePrefab;
    [SerializeField]
    private GameObject pipesParent;
    [SerializeField]
    private GameObject focus;
    
    private NameClassifier nameClassifier;

    private IPipeVector3Value iPipeVector3Value;
    private PipesPositionGetter pipesPositionGetter;
    [Inject]
    public void Init(IPipeVector3Value iPipeVector3Value, PipesPositionGetter pipesPositionGetter)
    {
        this.iPipeVector3Value = iPipeVector3Value;
        this.pipesPositionGetter = pipesPositionGetter;
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
        nameClassifier = pipesParent.GetComponent<NameClassifier>();
        CreatePipeWithJson();
    }
    private void CreatePipeWithJson()
    {
#if !UNITY_ANDROID
        var jsonContent = Resources.Load<TextAsset>(PIPE_JSON_PATH);
        PipeDataList pipeDataList = JsonUtility.FromJson<PipeDataList>(jsonContent.text);
#endif
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

        nameClassifier.ClassifyWithName(pipe, pipeData.obstName.Split('&')[0]);

        pipe.transform.up = offset;
        pipe.transform.localScale = scale;
        pipe.GetComponent<MeshRenderer>().material.color = obstcolor;

        pipesPositionGetter.GetPipePos(position.x, position.y, position.z);

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