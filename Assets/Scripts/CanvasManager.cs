using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private GameObject backGround;
    [SerializeField]
    private TextMeshProUGUI pipeMaterialText;
    [SerializeField]
    private TextMeshProUGUI pipeYearText;
    [SerializeField]
    private TextMeshProUGUI linkIdText;
    [SerializeField]
    private TextMeshProUGUI obstNameText;

    public void SetInfo(string pipeMaterial, string pipeYear, int linkId, string obstName)
    {
        pipeMaterialText.text = $"재질 : {pipeMaterial}";
        pipeYearText.text = $"연식 : {pipeYear}";
        linkIdText.text = $"관리번호 : {linkId}";
        obstNameText.text = $"관종 : {obstName}";
    }
    public void OpenInfo()
    {
        backGround.SetActive(true);
    }
    public void CloseInfo()
    {
        backGround.SetActive(false);
    }
}