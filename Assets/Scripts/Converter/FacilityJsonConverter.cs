using System;
using System.IO;
using UnityEngine;

public class FacilityJsonConverter : MonoBehaviour
{
    private readonly Vector3 FACILITYOFFSET = new Vector3(237066, 40, 455461);
    private readonly string FACILITY_ASSET_PATH = "Assets/AssetBundles/manhole";
    private readonly string FACILITY_JSON_PATH = "Assets/Resources/Facility.json";

    [SerializeField]
    private GameObject facilitiesParent;
    [SerializeField]
    private GameObject[] facilityAssets;

    private NameClassifier nameClassifier;
    private void Start()
    {
        nameClassifier = facilitiesParent.GetComponent<NameClassifier>();
        PoolingFacilityAssets(FACILITY_ASSET_PATH);
        CreateFacilityWithJson();
    }

    private void PoolingFacilityAssets(string path)
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(FACILITY_ASSET_PATH));
        // LoadFromMemoryAsync 말고 서버에서 에셋받아오는 함수도 찾기, 프로그램에서는 에셋 다 서버에서 받아와서 쓰자나....
        AssetBundle bundle = request.assetBundle;
        facilityAssets = bundle.LoadAllAssets<GameObject>();
    }

    private void CreateFacilityWithJson()
    {
        string jsonContent = File.ReadAllText(FACILITY_JSON_PATH);
        FacilityDataList facilityDataList = JsonUtility.FromJson<FacilityDataList>(jsonContent);

        foreach (FacilityData facility in facilityDataList.facility)
        {
            CreateFacility(facility);
        }
    }

    private void CreateFacility(FacilityData facilityData)
    {
        Vector3 startPosition = new Vector3(facilityData.xpos, facilityData.zpos, facilityData.ypos);
        Vector3 position = startPosition - FACILITYOFFSET;
        Vector3 scale = new Vector3(facilityData.xscale, facilityData.yscale, facilityData.zscale);
        Vector3 lookDirection = new Vector3(facilityData.xxpos - facilityData.xpos, facilityData.zzpos - facilityData.zpos, facilityData.yypos - facilityData.ypos);

        var facilityModel = Array.Find(facilityAssets, asset => asset.name == facilityData.modelFname);
        GameObject facility = Instantiate(facilityModel, position, Quaternion.identity, facilitiesParent.transform);
        nameClassifier.ClassifyWithName(facility, facilityData.obstName.Split('&')[0]);

        facility.transform.localScale = scale;
        facility.transform.rotation = Quaternion.LookRotation(lookDirection);

        FacilityInfo facilityInfo = facility.GetComponent<FacilityInfo>();
        if (facilityInfo != null)
        {
            facilityInfo.obstName = facilityData.obstName;
            facilityInfo.pointId = facilityData.pointId;
        }
    }
}
