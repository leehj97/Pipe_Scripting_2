using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private GameObject info;
    [SerializeField]
    private GameObject legend;
    [SerializeField]
    private TextMeshProUGUI pipeMaterialText;
    [SerializeField]
    private TextMeshProUGUI pipeYearText;
    [SerializeField]
    private TextMeshProUGUI linkIdText;
    [SerializeField]
    private TextMeshProUGUI obstNameText;

    private void Start()
    {
        info = transform.Find("Info").gameObject;
        legend = transform.Find("Legend").gameObject;
    }
    public void SetInfo(string pipeMaterial, string pipeYear, int linkId, string obstName)
    {
        pipeMaterialText.text = $"재질 : {pipeMaterial}";
        pipeYearText.text = $"연식 : {pipeYear}";
        linkIdText.text = $"관리번호 : {linkId}";
        obstNameText.text = $"관종 : {obstName.Split('&')[0]}";
    }
    public void SetInfo(string obstName, int pointId)
    {
        linkIdText.text = $"ID : {pointId}";
        obstNameText.text = $"시설물 종류 : {obstName}";
    }
    public void OpenInfo()
    {
        info.SetActive(true);
    }
    public void CloseInfo()
    {
        info.SetActive(false);
    }
    public void OpenLegend()
    {
        legend.SetActive(true);
    }
    public void CloseLegend()
    {
        legend.SetActive(false);
    }
}