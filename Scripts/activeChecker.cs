using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class activeChecker : MonoBehaviour
{
    public CannonScript Cannon;
    Rigidbody2D rigidbody2d;
    public bool active;
    private static GameObject lastFiredObject;
    private bool counted = false;
    private bool countedAgain = false;
    private int ceilingTurn = 0;
    private float ceilingPosition;
    public Sprite lossSprite;
    private bool GameOver = false;
    public string sceneName;
    public ParticleSystem popEffect;
    private float lossTimer = 5.0f;
    public static int level = 1;
    private int ballsLeft = 30;
    private int ballsLeft2 = 42;
    private bool shotOnce = false;
    private int oldCheckerCount;
    
    void Awake()
    {
        lastFiredObject = gameObject;
        rigidbody2d = GetComponent<Rigidbody2D>();
        GameObject ceilingItem = GameObject.FindWithTag("Roof");
        ceilingScript Ceiling = ceilingItem.GetComponent<ceilingScript>();
        ceilingPosition = 5 - ((Ceiling.ceilingMoves - 1) * .875f);
    }

    void Start()
    {
        oldCheckerCount = Cannon.checkerCount;
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        if(sceneName == currentScene.name)
        {
            Debug.Log("The name has been set");
        }
    }
    void Update()
    {
        lossTimer -= Time.deltaTime;
        if(ballsLeft == 0)
        {
            SceneManager.LoadScene("Level2Original");
            level++;
        }
        else if(ballsLeft2 == 0)
        {
            SceneManager.LoadScene("WinnerScreen");
        }
        if((Cannon.checkerCount) > oldCheckerCount)
        {
            if(level == 1)
            {
                ballsLeft++;
                Debug.Log("Balls Left: " + ballsLeft);
            }
            else if(level == 2)
            {
                ballsLeft2++;
            }
            oldCheckerCount = Cannon.checkerCount;
        }
        
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag != "Wall" && active == true)
        {
            rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
            if (other.gameObject.tag == "Roof")
            {
                transform.position = new Vector3((Mathf.Round((transform.position.x)-.5f) + .5f), ceilingPosition, 0);
            }
            else if (other.gameObject.tag != "Lose Line")
            {
                Vector3 col = other.transform.position;
                Vector3 dir = transform.position - other.transform.position;
                float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
                angle = angle + 180;
                if (angle >= 0 && angle < 60 )
                {
                    transform.position = col + new Vector3(-.5f, -.875f, 0);
                    if (transform.position.x < -3.5)
                    {
                        transform.position = col + new Vector3(.5f, -.875f, 0);
                    }
                }
                else if (angle >= 60 && angle <120)
                {
                    transform.position = col + new Vector3(-1, 0, 0);
                    if (transform.position.x < -3.5)
                    {
                        if(angle <= 90)
                        {
                            transform.position = col + new Vector3(-.5f, -.875f, 0);
                        }
                        else if(angle > 90)
                        {
                            transform.position = col + new Vector3(-.5f, .875f, 0);
                        }
                    }
                }
                else if (angle >= 120 && angle <180)
                {
                    transform.position = col + new Vector3(-.5f, .875f, 0);
                    if (transform.position.x > 3.5)
                    {
                        transform.position = col + new Vector3(.5f, .875f, 0);
                    }
                }
                else if (angle >= 180 && angle <240)
                {
                    transform.position = col + new Vector3(.5f, .875f, 0);
                    if (transform.position.x > 3.5)
                    {
                        transform.position = col + new Vector3(-.5f, .875f, 0);
                    }
                }
                else if (angle >= 240 && angle <300)
                {
                    transform.position = col + new Vector3(1, 0, 0);
                    if (transform.position.x > 3.5)
                    {
                        if(angle <= 270)
                        {
                            transform.position = col + new Vector3(.5f, .875f, 0);
                        }
                        else if(angle > 270)
                        {
                            transform.position = col + new Vector3(.5f, -.875f, 0);
                        }
                    }
                }
                else if (angle >= 300 && angle <360)
                {
                    transform.position = col + new Vector3(.5f, -.875f, 0);
                    if (transform.position.x > 3.5)
                    {
                        transform.position = col + new Vector3(-.5f, -.875f, 0);
                    }
                }
            }
            StartCoroutine(waitToCheck());
        }
    }

    private IEnumerator waitToCheck()
    {
        yield return new WaitForSeconds(.1f);
        checkColor(0,0);
        GameObject ceilingItem = GameObject.FindWithTag("Roof");
        ceilingScript Ceiling = ceilingItem.GetComponent<ceilingScript>();
        Ceiling.checkCeiling();
    }

    public void checkColor(int chain, int surrounded)
    {
        if(chain >=3 || surrounded >= 3)
        {
            if (counted == false || countedAgain == false)
            {
                counted = true;
                countedAgain = true;
                Collider2D[] colorChecker = Physics2D.OverlapCircleAll(transform.position, .6f);   //detects all colliders in area
                foreach (var item in colorChecker)  //creates variable for every collider
                {
                    activeChecker colorChain = item.GetComponent<Collider2D>().GetComponent<activeChecker>();   //gets script for all colliders in area
                    if (item.tag == gameObject.tag) //checks if objects have same tag as object that triggered
                    {
                        colorChain.checkColor(4,4);
                    }
                } 
            }
            else
            {
                popEffect.Play();
                if(level == 1)
                        {
                            ballsLeft = ballsLeft - 2;
                            Debug.Log("Balls Left: " + ballsLeft);
                        }
                        else if(level == 2)
                        {
                            ballsLeft2 = ballsLeft2 - 2;
                        }
                Destroy(gameObject);
                Destroy(lastFiredObject);
               
            }
        }
        else if (counted == false && GameOver == false) //checks if counted in chain yet
        {
            surrounded = 0;
            chain = chain + 1; //adds to chain (number of colors in a row)
            counted = true; //marks as counted in chain       
            Collider2D[] colorChecker = Physics2D.OverlapCircleAll(transform.position, .6f);   //detects all colliders in area
            foreach (var item in colorChecker)  //creates variable for every collider
            {
                activeChecker colorChain = item.GetComponent<Collider2D>().GetComponent<activeChecker>();   //gets script for all colliders in area
                if (item.tag == gameObject.tag) //checks if objects have same tag as object that triggered
                {
                    surrounded = surrounded + 1;  //adds 1 to chain value received for every object with same tag next to original (including original)
                    colorChain.checkColor(chain, surrounded); //calls this function on every object bordering with same tag with new chain value. Each will be counted, adding to the chain
                    if (chain >=3 || surrounded >=3)
                    {
                        popEffect.Play();
                        colorChain.checkColor(4,4);
                        if(level == 1)
                        {
                            ballsLeft--;
                            Debug.Log("Balls Left: " + ballsLeft);
                        }
                        else if(level == 2)
                        {
                            ballsLeft2--;
                        }
                        Destroy(gameObject);
                    }
                }
            }
            active = false;
            counted = false;
        }
    }

    public void checkCeilingChain(int turnCount)
    {
        if(ceilingTurn < turnCount)
        {
            ceilingTurn = turnCount;
            Collider2D[] ceilingChecker = Physics2D.OverlapCircleAll(transform.position, .6f);
            foreach (var ceilingBubble in ceilingChecker)
            {
                if(ceilingBubble.tag != "Wall" && ceilingBubble.tag != "Roof" && ceilingBubble.tag != "Lose Line")
                {
                    activeChecker ceilingChain = ceilingBubble.GetComponent<Collider2D>().GetComponent<activeChecker>();
                    ceilingChain.checkCeilingChain(turnCount);
                }
            }

        }
    }

    public void ceilingPopper(int turnCount)
    {
        if(ceilingTurn < turnCount)
        {   
            popEffect.Play();
            if(level == 1)
            {
                ballsLeft--;
                Debug.Log("Balls Left: " + ballsLeft);
            }
            else if(level == 2)
            {
                ballsLeft2--;
            }        
            Destroy(gameObject);
         }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Lose Line" && active == false)
        {
            Cannon.disableShooting();
            GameOver = true;
            Debug.Log("Game Over!");
            other.gameObject.SetActive(false);
            var allBubbles = FindObjectsOfType<activeChecker>();
            foreach(var activeBubbles in allBubbles)
            {
                var allSprite = activeBubbles.GetComponent<SpriteRenderer>();
                allSprite.sprite = lossSprite;
                if(activeBubbles == null)
                {
                    if(level == 1)
                    {
                        SceneManager.LoadScene("Level2Original");
                        level++;
                    }
                    else if(level == 2)
                    {
                        SceneManager.LoadScene("WinnerScreen");
                    }
                    Debug.Log("Win or next level");
                }
            }
        }
        else if(lossTimer <= 0)
        {
            var allBubbles = FindObjectsOfType<activeChecker>();
            foreach(var activeBubbles in allBubbles)
            {
                var allSprite = activeBubbles.GetComponent<SpriteRenderer>();
                allSprite.sprite = lossSprite;
                if(activeBubbles == null)
                {
                    Debug.Log("Win or next level");
                }
            }
        }
        else
        {
            return;
        }

    }
}
