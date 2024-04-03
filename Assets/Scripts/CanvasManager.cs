using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _backGround;
    [SerializeField]
    private TextMeshProUGUI _pipeMaterialText;
    [SerializeField]
    private TextMeshProUGUI _pipeYearText;
    [SerializeField]
    private TextMeshProUGUI _linkIdText;
    [SerializeField]
    private TextMeshProUGUI _obstNameText;

    public void SetInfo(string pipeMaterial, string pipeYear, int linkId, string obstName)
    {
        _pipeMaterialText.text = $"재질 : {pipeMaterial}";
        _pipeYearText.text = $"연식 : {pipeYear}";
        _linkIdText.text = $"관리번호 : {linkId}";
        _obstNameText.text = $"관종 : {obstName}";
    }
    public void OpenInfo()
    {
        _backGround.SetActive(true);
    }
    public void CloseInfo()
    {
        _backGround.SetActive(false);
    }
}