using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour 
{
	public GameObject GameManagerGO;
	public GameObject Playerbulletobject;
	public GameObject Bulletpos1;
	public GameObject Bulletpos2;
	public GameObject ExplosionGO;
	public Text LivesUIText;
	const int MaxLives = 3;
	int lives;
	public float speed=5f;
	public float timer = 5f;

	public void Init()
    {
		lives = MaxLives;
		LivesUIText.text = lives.ToString();

		gameObject.SetActive(true);

	}

	// Use this for initialization
	void Start () 
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		timer -= Time.deltaTime * 25;
		while(timer<=0){
			GameObject bullet01 = (GameObject)Instantiate(Playerbulletobject);
			bullet01.transform.position = Bulletpos1.transform.position;

			GameObject bullet02 = (GameObject)Instantiate(Playerbulletobject);
			bullet02.transform.position = Bulletpos2.transform.position;
			timer = 5f;
		}

		float x = Input.GetAxisRaw ("Horizontal");//the value will be -1, 0 or 1 (for left, no input, and right)
		float y = Input.GetAxisRaw ("Vertical");//the value will be -1, 0 or 1 (for down, no input, and up)

		//now based on the input we compute a direction vector, and we normalize it to get a unit vector
		Vector2 direction = new Vector2 (x, y).normalized;

		//noe we call the function that computes and sets the player's position
		Move (direction);
	}

	void Move(Vector2 direction)
	{
		//find the screen limits to the player's movement (left, right, top and bottom edges of the screen)
		Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0)); //this is the bottom-left point (corner) of the screen
		Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1)); //this is the top-right point (corner) of the screen

		max.x = max.x - 0.225f; //subtract the player sprite half width
		min.x = min.x + 0.225f; //add the player sprite half width

		max.y = max.y - 0.285f; //subtract the player sprite half height
		min.y = min.y + 0.285f; //add the player sprite half height

		//Get the player's current position
		Vector2 pos = transform.position;

		//Calculate the new position
		pos += direction * speed * Time.deltaTime;

		//Make sure the new position is outside the screen
		pos.x = Mathf.Clamp (pos.x, min.x, max.x);
		pos.y = Mathf.Clamp (pos.y, min.y, max.y);

		//Update the player's position
		transform.position = pos;

		
        }
	void OnTriggerEnter2D(Collider2D col)
	{
		if ((col.tag == "EnemyShipTag") || (col.tag == "EnemyBulletTag"))
			{
			PlayExplosion();
			lives--;
			LivesUIText.text = lives.ToString();
            if (lives == 0)
            {
				GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);
				gameObject.SetActive(false);
			}
			
		} 
	}
	void PlayExplosion()
    {
		GameObject explosion = (GameObject)Instantiate(ExplosionGO);
		 
		explosion.transform.position = transform.position;
    }
}