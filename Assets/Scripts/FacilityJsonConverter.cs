using System;
using System.IO;
using UniRx.Triggers;
using UnityEditor.Rendering.Utilities;
using UnityEditor.VersionControl;
using UnityEngine;

public class FacilityJsonConverter : MonoBehaviour
{
    private readonly Vector3 facilityOffset = new Vector3(237066, 40, 455461);
    private readonly string FACILITY_ASSET_PATH = "Assets/AssetBundles/manhole";
    private readonly string FACILITY_JSON_PATH = "Assets/Resources/Facility.json";

    [SerializeField]
    private GameObject facilitiesParent;
    [SerializeField]
    private GameObject[] facilityAssets;
    private void Start()
    {
        PoolingFacilityAssets(FACILITY_ASSET_PATH);
        CreateFacilityWithJson();
    }

    public void PoolingFacilityAssets(string path)
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(FACILITY_ASSET_PATH));

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

    public void CreateFacility(FacilityData facilityData)
    {
        Vector3 startPosition = new Vector3(facilityData.xpos, facilityData.zpos, facilityData.ypos);
        Vector3 position = startPosition - facilityOffset;
        Vector3 scale = new Vector3(facilityData.xscale, facilityData.yscale, facilityData.zscale);
        Vector3 lookDirection = new Vector3(facilityData.xxpos - facilityData.xpos, facilityData.zzpos - facilityData.zpos, facilityData.yypos - facilityData.ypos);

        var facilityPrefab = Array.Find(facilityAssets, asset => asset.name == facilityData.modelFname);
        GameObject facility = Instantiate(facilityPrefab, position, Quaternion.identity, facilitiesParent.transform);
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
