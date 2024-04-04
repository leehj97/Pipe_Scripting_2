using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class FacilityJsonConverter : MonoBehaviour
{
    private readonly Vector3 facilityOffset = new Vector3(237066, 40, 455461);
    public const string FACILITY_ASSET_PATH = "Assets/AssetBundles/manhole";
    public const string FACILITY_JSON_PATH = "Assets/Resources/Facility.json";

    [SerializeField]
    private GameObject facilityPrefab;
    [SerializeField]
    private GameObject facilitiesParent;
    [SerializeField]
    private GameObject[] facilityAssets;
    private void Start()
    {
        PoolingFacilityAssets(FACILITY_ASSET_PATH);
        ConvertFacilityJsonInfo();
    }

    public void PoolingFacilityAssets(string path)
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(FACILITY_ASSET_PATH));

        AssetBundle bundle = request.assetBundle;
        facilityAssets = bundle.LoadAllAssets<GameObject>();
    }

    private void ConvertFacilityJsonInfo()
    {
        string jsonContent = File.ReadAllText(FACILITY_JSON_PATH);
        FacilityDataList facilityDataList = JsonUtility.FromJson<FacilityDataList>(jsonContent);

        foreach (FacilityData facility in facilityDataList.facility)
        {
            CreateFacilityFromJsonInfo(facility);
        }
    }

    public void CreateFacilityFromJsonInfo(FacilityData facilityData)
    {
        Vector3 startPosition = new Vector3(facilityData.xpos, facilityData.zpos, facilityData.ypos);
        Vector3 position = startPosition - facilityOffset;
        Vector3 scale = new Vector3(facilityData.xscale, facilityData.yscale, facilityData.zscale);

        foreach (var asset in facilityAssets)
        {
            if (asset.name == facilityData.modelFname)
            {
                facilityPrefab = asset;
                GameObject facility = Instantiate(facilityPrefab, position, Quaternion.identity, facilitiesParent.transform);
                facility.transform.localScale = scale;

                FacilityInfo facilityInfo = facility.GetComponent<FacilityInfo>();
                if (facilityInfo != null)
                {
                    facilityInfo.obstName = facilityData.obstName;
                    facilityInfo.pointId = facilityData.pointId;
                }
            }
        }
    }
}
