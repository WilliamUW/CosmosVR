using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using GLTFast;
using TMPro;
using UnityEngine.Networking;

public class MeshColliderAndXRGrabAdder : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public Transform playerTransform;
    public Gemini gemini;

    public TMP_Dropdown walletDropdown;

    const string appName = "Cosmos VR";

    private string allAssetNames = "";
    private string ownedAssetNames = "";
    float rotationSpeed = 30f;
    private Quaternion rotationIncrement;
    public List<GameObject> celestialBodies = new List<GameObject>();


    public static readonly Dictionary<string, string> SpaceAssetNameToDescriptionDict = new Dictionary<string, string>
    {
        {
             "Sun",
            "Hello! I am the Sun, the star at the center of our Solar System. I am the source of all energy, and without me, nothing would exist! Be careful though, I'm pretty hot!"

        },
        {"Mercury",
            "Hi! I'm Mercury, the smallest and closest planet to the Sun. I might be small, but I'm fast - I zoom around the Sun in just 88 days!"
            },
        { "Venus",
            "Greetings! I'm Venus, the second planet from the Sun. I might look like a twin to Earth, but trust me, I'm way hotter and have a toxic atmosphere!"
            },
        {"Earth",
            "Hello there! I'm Earth, the planet you call home. I'm the only place in the Solar System where life exists. Isn't that cool?"
            },
        {"Mars",
            "Hey! I'm Mars, also known as the Red Planet. I'm about half the size of Earth and home to the tallest volcano in the Solar System, Olympus Mons!"
            },
        {"Jupiter",
            "Greetings! I'm Jupiter, the largest planet in the Solar System. I'm so big that over 1,300 Earths could fit inside me! Check out my Great Red Spot - it's a giant storm!"
            },
        {"Saturn",
            "Hello! I'm Saturn, known for my beautiful rings. These rings are made of ice and rock - aren't they stunning?"
            },
        {"Uranus",
            "Hi! I'm Uranus, the planet that spins on its side. I'm an ice giant, and my blue-green color comes from methane in my atmosphere."
            },
        {"Neptune",
            "Hey there! I'm Neptune, the farthest planet from the Sun. I have strong winds that can reach up to 1,200 miles per hour - faster than the speed of sound!"
            },
        {"Moon",
            "Hi! I'm the Moon, Earth's only natural satellite. I'm responsible for the tides and I love showing off my phases!"
            },
        {
            "HD 189733 b",
            "Discovered 2005: This far-off blue planet may look like a friendly haven - but don't be deceived! Weather here is deadly. The planet's cobalt blue color comes from a hazy, blow-torched atmosphere containing clouds laced with glass. Howling winds send the storming glass sideways at 5,400 mph (2km/s), whipping all in a sickening spiral. It's death by a million cuts on this slasher planet!"
        },
        {
            "KOI-55 b",
            "Discovered 2011: Kepler-70b (aka KOI-55) could well be another circle of hell with an average temperature hotter than the Sun's surface. It used to be Jupiter-sized until it spent some time inside its now-dead star…a trip that destroys most planets, but left this one a Freddy Krueger-like burned world smaller than Earth. At about 12,000 degrees F (6,800 C), it is one of the hottest planets discovered. In fact, the planet itself is evaporating, soon to be another victim."
        },
        {
            "55 Cancri e",
            "Discovered 2004: This super hot world is covered in a global ocean of lava and has sparkling skies. Another star that orbits close to its host stars, taking under 18 hours to complete an orbit, 55 Cancri e is also inhospitably hot—reaching temperatures as high as 4,172 degrees F (2,300 degrees C). But what really sets this world apart is its composition, which makes the exoplanet, formally known as Janssen, perhaps the most conventionally valuable object in the universe. The fact that 55 Cancri e is twice the size of Earth, but has almost 9 times the mass, led astronomers to propose that this Super-Earth could be composed of high pressurized carbon in the form of graphite and diamond mixed with some iron and other elements, according to NASA. The estimated value of 55 Cancri e is estimated to be 384 quadrillion times more than Earth's entire Gross Domestic Product (GDP), which was valued at 70 USD in 2011. Some astrophysicists suggest that such diamond worlds could form fairly regularly when protoplanetary dust clouds that contained high ratios of carbon collapse to form planets. The idea that 55 Cancri e is made of diamond has been challenged since the exoplanet was first discovered in 2004, moving in and out of favor, proving diamonds may not be forever. Yet despite all these extreme worlds, the most extraordinary exoplanets may still be out there for us to discover, and they may exist in systems of the likes that we have never encountered before."
        },
        {
            "Wasp J1407b",
            "Wasp J1407b is an exoplanet with a massive ring system that's 200 times larger than Saturn's:  Discovery: Discovered in 2012 by astronomer Eric Mamajek using data from the SuperWASP (Super Wide Angle Survey for Planets) project  Location: Located in the Centaurus constellation, about 434 light years away  Rings: The rings are made up of 30 different rings, each tens of millions of miles wide  Mass: The mass of J1407b is estimated to be between 10 and 40 Jupiter masses  Orbital period: The orbital period of J1407b is roughly a decade  Signs of an exomoon: J1407b shows signs of an exomoon that has cleared a path in the rings  Name: The name J1407b follows the exoplanet naming convention by adding the letter b after the host star's name, V1400 Centauri"
        }
    };




    private void Start()
    {
        rotationIncrement = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);

        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs");
        dropdown.ClearOptions();
        var prefabNames = new System.Collections.Generic.List<string>();
        foreach (GameObject prefab in prefabs)
        {
            prefabNames.Add(prefab.name);
            allAssetNames = allAssetNames + ',' + prefab.name;
        }
        ownedAssetNames = allAssetNames;
        dropdown.AddOptions(prefabNames);
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        walletDropdown.ClearOptions();
        var wallets = new System.Collections.Generic.List<string> { appName, "TGcb8vsyQEaXzZmMnLZCrGK3HJNVgWsmU1", "TRjE1H8dxypKM1NZRdysbs9wo7huR4bdNz" }; walletDropdown.AddOptions(wallets);
        walletDropdown.onValueChanged.AddListener(OnWalletDropdownValueChanged);
    }

    void Update()
    {
        // Iterate through each celestial body in the list
        foreach (GameObject body in celestialBodies)
        {
            if (body != null)
            {
                // Rotate the celestial body around its y-axis at 15 degrees per second
                body.transform.rotation *= rotationIncrement;

            }
        }
    }

    IEnumerator GetOwnedAssets()
    {
        if (walletDropdown.value == 0)
        {
            ownedAssetNames = allAssetNames;
            yield break;
        }
        else
        {
            string url = "https://tronspacevr.onrender.com/owned-assets/" + walletDropdown.options[walletDropdown.value].text;
            Debug.Log(url);
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    string json = www.downloadHandler.text;

                    Debug.Log(json);

                    ownedAssetNames = json;
                }
            }
        }
    }

    private void OnWalletDropdownValueChanged(int index)
    {
        Debug.Log($"Dropdown value changed: {index}");
        StartCoroutine(GetOwnedAssets());
    }

    private void OnDropdownValueChanged(int index)
    {
        Debug.Log($"Dropdown value changed: {index}");
        if (index >= 0)
        {
            string selectedObject = dropdown.options[index].text;
            if (ownedAssetNames.Contains(selectedObject))
            {
                string description = "";
                if (SpaceAssetNameToDescriptionDict.ContainsKey(selectedObject))
                {
                    description = SpaceAssetNameToDescriptionDict[selectedObject];
                }
                InitializeGemini(selectedObject, "Concisely respond in first person as the celestial body " + selectedObject + " in a fun and conversational manner. Additional description: " + description);
            }
            else
            {
                gemini.Speak("You do not own the following space object on TRON!");
            }
        }
    }

    public void SpawnObject()
    {
        string selectedObject = dropdown.options[dropdown.value].text;

        if (!ownedAssetNames.Contains(selectedObject))
        {
            gemini.Speak("You do not own the following space object on TRON!");
            return;
        }

        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + selectedObject);

        if (prefab == null)
        {
            Debug.LogError($"No prefab found for {selectedObject}");
            return;
        }

        // Calculate offset based on object size
        Vector3 offset = CalculateOffsetForObject(prefab);

        // Instantiate the object at the calculated position
        GameObject spawnedObject = Instantiate(prefab, offset, Quaternion.identity);

        // Add the spawned object to the celestialBodies list
        celestialBodies.Add(spawnedObject);

        Debug.Log($"Spawned {selectedObject} at position {offset}");
    }

    private Vector3 CalculateOffsetForObject(GameObject prefab)
    {
        // Get the scale of the prefab
        Vector3 prefabScale = prefab.transform.localScale;

        // Calculate the magnitude of the scale vector
        float scaleMagnitude = prefabScale.magnitude;

        // Return a position that's always offset from the origin
        return new Vector3(0, 2, (float)0.5 * scaleMagnitude + 2f);
    }
    public void InitializeGemini(string title, string description)
    {
        // Implement your logic to initialize Gemini with the selected asset's title and description
        Debug.Log($"Initializing Gemini with Title: {title} and Description: {description}");
        gemini.InitializeGemini("Name: " + title + ". Description: " + description, title);
    }

    private IEnumerator MoveRandomly(GameObject obj)
    {
        Vector3 originalPosition = obj.transform.position;
        float moveDistance = 2f;
        float minWaitTime = 0.5f;
        float maxWaitTime = 2f;
        float moveSpeed = 0.5f; // Adjust to control the movement speed

        while (true)
        {
            if (Vector3.Distance(obj.transform.position, playerTransform.position) <= 4f)
            {
                // Pause movement and face the player
                Vector3 directionToPlayer = (playerTransform.position - obj.transform.position).normalized;

                // Zero out the y-component to only rotate around the y-axis
                directionToPlayer.y = 0;

                // Normalize the direction vector again after modification
                directionToPlayer = directionToPlayer.normalized;

                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, lookRotation, Time.deltaTime * 2f);

                yield return null;
            }
            else
            {
                // Determine random direction and distance within range
                Vector3 randomDirection = new Vector3(
                    Random.Range(-1f, 1f),
                    0,
                    Random.Range(-1f, 1f)
                ).normalized;

                float randomDistance = Random.Range(0.5f, moveDistance);
                Vector3 targetPosition = obj.transform.position + randomDirection * randomDistance;

                // Ensure the target position is within the allowed range from the original position
                if (Vector3.Distance(targetPosition, originalPosition) > moveDistance)
                {
                    targetPosition = originalPosition + (targetPosition - originalPosition).normalized * moveDistance;
                }

                // Rotate to face the target direction
                Quaternion targetRotation = Quaternion.LookRotation(randomDirection);
                while (Quaternion.Angle(obj.transform.rotation, targetRotation) > 0.1f)
                {
                    obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, targetRotation, Time.deltaTime * 2f);
                    yield return null;
                }

                // Move towards the target position
                while (Vector3.Distance(obj.transform.position, targetPosition) > 0.1f)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, moveSpeed * Time.deltaTime);
                    yield return null;
                }

                // Ensure the final position is exactly the target position
                obj.transform.position = targetPosition;

                // Wait for a random duration before the next movement
                float randomStopDuration = Random.Range(minWaitTime, maxWaitTime);
                yield return new WaitForSeconds(randomStopDuration);
            }
        }
    }
}
