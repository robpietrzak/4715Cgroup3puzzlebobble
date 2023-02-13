using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CannonScript : MonoBehaviour
{
    private SpriteRenderer nextColorLoader;
    private SpriteRenderer currentColorLoader;

    public static List<int> colorList = new List<int>();

    public Sprite[] spriteArray;

    private static int nextColor;
    private static int currentColor;
    public static int level = 1;

    public GameObject RedBubble;
    public GameObject OrangeBubble;
    public GameObject YellowBubble;
    public GameObject GreenBubble;
    public GameObject BlueBubble;
    public GameObject PurpleBubble;
    public GameObject WhiteBubble;
    public GameObject BlackBubble;
    public GameObject DeadBubble;
    public ParticleSystem cannonFire;
    public AudioClip popSound;
    AudioSource audioSource;

    private GameObject projectileObject;

    public TextMeshProUGUI timerText;
    
    Vector2 cannonDirection;
    private float angle;
    private float timer = 0.0f;

    private int shotCounter;
    private static bool shootingEnabled = false;
    private bool lost = false;
    private bool shootAnimationOn = false;
    public GameObject shootaniObject;
    public float lossTimer;
    private int timeInt;
    public int checkerCount;

    void Start()
    {
        makeList();
        nextColor = colorList[Random.Range(0, colorList.Count)];
        changeNext();
        shootingEnabled = true;
        shootaniObject.SetActive(false);
        checkerCount = 0;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (lossTimer > 0)
        {
            lossTimer -= Time.deltaTime;
        }

        timeInt = (int)lossTimer;
        timerText.text = timeInt.ToString();

        Vector3 mousePos = Input.mousePosition;

        Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90;
        if (angle > -80 && angle < 80)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            cannonDirection.Set(mousePos.x, mousePos.y);
            cannonDirection.Normalize();
        }

        if(timer > 7 && shootAnimationOn == false)
        {
            shootAnimationOn = true;
            shootaniObject.SetActive(true);
        }

        if ((Input.GetMouseButtonDown(0) || timer >= 10) && shootingEnabled == true)
        {
            shootingEnabled = false;
            timer = 0;
            if (currentColor == 0)
            {
                projectileObject = Instantiate(RedBubble, transform.position, Quaternion.identity);
                checkerCount++;
                Debug.Log("Checker Count Number: " + checkerCount);
            }
            else if (currentColor == 1)
            {
                projectileObject = Instantiate(OrangeBubble, transform.position, Quaternion.identity);  
                checkerCount++;
                Debug.Log("Checker Count Number: " + checkerCount);                  
            }
            else if (currentColor == 2)
            {
                projectileObject = Instantiate(YellowBubble, transform.position, Quaternion.identity);    
                checkerCount++;  
                Debug.Log("Checker Count Number: " + checkerCount);        
            }
            else if (currentColor == 3)
            {
                projectileObject = Instantiate(GreenBubble, transform.position, Quaternion.identity);     
                checkerCount++;         
                Debug.Log("Checker Count Number: " + checkerCount); 
            }
            else if (currentColor == 4)
            {
                projectileObject = Instantiate(BlueBubble, transform.position, Quaternion.identity);     
                checkerCount++;   
                Debug.Log("Checker Count Number: " + checkerCount);         
            }
            else if (currentColor == 5)
            {
                projectileObject = Instantiate(PurpleBubble, transform.position, Quaternion.identity);     
                checkerCount++;             
                Debug.Log("Checker Count Number: " + checkerCount);  
            }
            else if (currentColor == 6)
            {
                projectileObject = Instantiate(WhiteBubble, transform.position, Quaternion.identity);     
                checkerCount++;       
                Debug.Log("Checker Count Number: " + checkerCount);      
            }
            else if (currentColor == 7)
            {
                projectileObject = Instantiate(BlackBubble, transform.position, Quaternion.identity);      
                checkerCount++;         
                Debug.Log("Checker Count Number: " + checkerCount);    
            }
            activeChecker bubble = projectileObject.GetComponent<activeChecker>();
            currentColorLoader.sprite = null;
            cannonFire.Play();
            audioSource.PlayOneShot(popSound);
            bubble.Launch(cannonDirection, 1250);
            shootaniObject.SetActive(false);
            shootAnimationOn = false;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if(lossTimer <= 0 && shootingEnabled == true)
        {
            Debug.Log("Game lost");
            shootingEnabled = false;
            lost = true;
            Invoke(nameof(teleport), 2f);
        }
    }
    void teleport()
    {
        SceneManager.LoadScene("LoseScreen");
    }

    public void changeNext()
    {
        currentColor = nextColor;
        currentColorLoader = GameObject.FindWithTag("Current Color").GetComponent<SpriteRenderer>();
        currentColorLoader.sprite = spriteArray[currentColor];
        nextColor = colorList[Random.Range(0, colorList.Count)];
        nextColorLoader = GameObject.FindWithTag("Next Color").GetComponent<SpriteRenderer>();
        nextColorLoader.sprite = spriteArray[nextColor];
        if  (lost == false)
        {
        shootingEnabled = true;
        }
    }

    public IEnumerator waitForList()
    {
        yield return new WaitForSeconds(.01f);
        if(colorList.Count > 1)
        {
            makeList();
        }
        else
        {
            changeNext();
        }
    }

    public void makeList()
    {
        colorList.Clear();
        if(GameObject.FindGameObjectsWithTag("RedBubble").Length > 0)
        {
            colorList.Add(0);
        }
        if(GameObject.FindGameObjectsWithTag("OrangeBubble").Length > 0)
        {
            colorList.Add(1);
        }
        if(GameObject.FindGameObjectsWithTag("YellowBubble").Length > 0)
        {
            colorList.Add(2);
        }
        if(GameObject.FindGameObjectsWithTag("GreenBubble").Length > 0)
        {
            colorList.Add(3);
        }
        if(GameObject.FindGameObjectsWithTag("BlueBubble").Length > 0)
        {
            colorList.Add(4);
        }
        if(GameObject.FindGameObjectsWithTag("PurpleBubble").Length > 0)
        {
            colorList.Add(5);
        }
        if(GameObject.FindGameObjectsWithTag("WhiteBubble").Length > 0)
        {
            colorList.Add(6);
        }
        if(GameObject.FindGameObjectsWithTag("BlackBubble").Length > 0)
        {
            colorList.Add(7);
        }
        changeNext();
    }

    public void disableShooting()
    {
        shootingEnabled = false;
        lost = true;
        Invoke(nameof(teleport), 2f);
    }
}
