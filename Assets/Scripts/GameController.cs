using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;
using System.Runtime.CompilerServices;

public class GameController : MonoBehaviour
{
    public Image promptImg;
    public Sprite[] characterSprites;
    private Sprite tonightGroom;
    private List<Sprite> remainingGrooms;

    public TMP_Text scoreText;
    public TMP_Text titleText;
    private bool gameEnded = false;
    private int score = 0;

    public AudioSource kissSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   Random.InitState(System.Environment.TickCount);
        remainingGrooms = new List<Sprite>(characterSprites);
        ShowGroom();
        scoreText.text = "Grooms I've married tonight: " + score;
        
    }
    void ShowGroom()
    {
        int randomIndex = Random.Range(0, remainingGrooms.Count);
        tonightGroom = remainingGrooms[randomIndex];
        promptImg.sprite = tonightGroom;
        remainingGrooms.RemoveAt(randomIndex);
    }
    // Update is called once per frame
    void Update()
    {
        if (gameEnded) return;
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new UnityEngine.Vector3(mousePosition.x, mousePosition.y, 10f));
            Collider2D clickedObject = Physics2D.OverlapPoint(worldPosition);

            if (clickedObject != null && clickedObject.CompareTag("characters"))
            {
                Sprite clickedSprite = clickedObject.GetComponent<SpriteRenderer>().sprite;
                if(clickedSprite == tonightGroom)
                {
                    Debug.Log("Marry me!");
                    score++;
                    kissSound.Play();
                    scoreText.text = "[tonight groom counts: " + score + "]";

                    if (remainingGrooms.Count == 0)
                    {
                        Debug.Log("Night is over!");
                        titleText.text = "Good night";
                        scoreText.gameObject.SetActive(false);
                        gameEnded = true;
                    }else
                    {
                        ShowGroom();
                    }
                }
            }

        }
    }
}
