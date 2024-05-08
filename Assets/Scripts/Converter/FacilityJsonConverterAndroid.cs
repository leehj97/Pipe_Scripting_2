using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FacilityJsonConverterAndroid : MonoBehaviour
{
    private readonly Vector3 FACILITYOFFSET = new Vector3(237066, 40, 455461);
    private readonly string FACILITY_ASSET_PATH = "manholeandroid";
    private readonly string FACILITY_JSON_PATH = "Facility";

    [SerializeField]
    private GameObject facilitiesParent;
    [SerializeField]
    private GameObject[] facilityAssets;

    private NameClassifier nameClassifier;
    private void Start()
    {
        nameClassifier = facilitiesParent.GetComponent<NameClassifier>();
        PoolingFacilityAssets();
        CreateFacilityWithJson();
    }
    private void PoolingFacilityAssets()
    {
        var bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, FACILITY_ASSET_PATH));
        facilityAssets = bundle.LoadAllAssets<GameObject>();
    }

    private void CreateFacilityWithJson()
    {
        var jsonContent = Resources.Load<TextAsset>(FACILITY_JSON_PATH);
        FacilityDataList facilityDataList = JsonUtility.FromJson<FacilityDataList>(jsonContent.text);

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
