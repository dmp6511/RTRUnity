using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class APIManager : MonoBehaviour
{
    // API information
    private string APIurl = "https://api.thedogapi.com/v1/images/search?limit=3"; // Fetch 3 images
    private string APIKey = "live_Oiw4kjkkAPGUEozuAe9RPOlce9xUU6z4g3fQwk5qnCurD4MBUrxCRHIGxc4h6ZU9";

    // UI Elements for the 3 panels
    public RawImage[] displayImages; // Array for Dog Images
    public TextMeshProUGUI[] breedTexts; // Array for Breed Names
    public Button fetchButton;

    void Start()
    {
        // Assign button function
        fetchButton.onClick.AddListener(GetNewDogs);
        StartCoroutine(GetData());
    }

    // Fetch 3 dog images from API
    IEnumerator GetData()
    {
        // building API Request
        UnityWebRequest request = UnityWebRequest.Get(APIurl);
        request.SetRequestHeader("x-api-key", APIKey); // headers
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse JSON object
            string json = request.downloadHandler.text;
            DogDataWrapper wrapper = JsonUtility.FromJson<DogDataWrapper>($"{{\"dogs\":{json}}}");

            if (wrapper != null && wrapper.dogs.Length > 0)
            {
                // Loop through the fetched data and update the UI
                for (int i = 0; i < wrapper.dogs.Length && i < displayImages.Length; i++)
                {
                    StartCoroutine(LoadImage(wrapper.dogs[i].url, displayImages[i])); // Load images

                    // display breed
                    breedTexts[i].text = (wrapper.dogs[i].breeds.Length > 0) ? wrapper.dogs[i].breeds[0].name : "Unknown Breed";
                }
            }
            else
            {
                Debug.LogError("API returned empty or invalid data.");
            }
        }
        else
        {
            Debug.LogError("API Request Failed: " + request.error);
        }
    }


    // Load the image into the correct panel
    IEnumerator LoadImage(string url, RawImage targetImage)
    {
        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(url);
        yield return textureRequest.SendWebRequest();

        if (textureRequest.result == UnityWebRequest.Result.Success)
        {
            // set the downloaded texture to UI element
            targetImage.texture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
        }
        else
        {
            Debug.LogError("Failed to load image: " + textureRequest.error);
        }
    }

    // Function triggered by button to fetch new dogs
    public void GetNewDogs()
    {
        Debug.Log("Fetching new dogs...");
        StartCoroutine(GetData());
    }


    // Data models for parsing the API Response

    [System.Serializable]
    public class DogDataWrapper
    {
        public DogData[] dogs;
    }

    [System.Serializable]
    public class DogData
    {
        public string url;
        public Breed[] breeds;
    }

    [System.Serializable]
    public class Breed
    {
        public string name;
    }


}
