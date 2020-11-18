using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    //public variables==================================================================
    public Sprite[] diceSides;
    public Image Dice_Image;
    public GameObject diceSound;
    public Button P1_Skip_Button, P2_Skip_Button , P1_Play_Button, P2_Play_Button;
    public int finalSide  , P1_skipside , P2_skipside;
    public bool player , rollDice;
    public static Dice instance;

    //private variables================================================================
    int randomDiceSide , P1_D6_Counter , P2_D6_Counter ;
    //int dice_Counter = 1;
   
    private void Awake()
    {
        instance = this; 
    }

    private void Start () {
        P1_Skip_Button.interactable = false;
        P2_Skip_Button.interactable = false;
        Reset_Dice();
    }

    //reset the all properties and variables
    public void Reset_Dice()
    {
        finalSide = 0;
        P1_skipside = P2_skipside = 0;
        player = false;
        rollDice = true;
        Dice_Image.gameObject.GetComponent<Button>().interactable = true;
        randomDiceSide = 0;
        P1_D6_Counter = 1;
        P2_D6_Counter = 1;
        diceSound.SetActive(false);
        P1_Play_Button.interactable = false;
        P2_Play_Button.interactable = false;
        
    }

    //action when dice is activated
    public void OnDiceClick()
    {
        if(rollDice == true)
        {
            Dice_Image.gameObject.GetComponent<Button>().interactable = false;
            //BoardManager.instance.Change_Board_Map();
            StartCoroutine("RollTheDice");
        } 
    }

    // Coroutine that rolls the dice
    IEnumerator RollTheDice()
    {
        for (int i = 0; i <= 10; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            Dice_Image.sprite = diceSides[randomDiceSide];
            diceSound.SetActive(true);
            yield return new WaitForSeconds(0.02f);
        }

        diceSound.SetActive(false);
        rollDice = false;
        
        finalSide = randomDiceSide + 1;
        Debug.Log(finalSide);

        if(finalSide == 6)
        {
            BoardManager.instance.Change_Board_Map();
        }

        P1_Play_Button.interactable = true;
        P2_Play_Button.interactable = true;
        //PlayerChance();
    }

    //method which decides the turn of each player
    public void PlayerChance()
    {
        //only use for two players game play script
        if (player == false)
        {
            BoardManager.instance.move_Player1();

            if(finalSide == 6)
            {
                
                if (P1_D6_Counter == 1)
                {
                    player = false;
                    P1_D6_Counter++;
                } 
                else if(P1_D6_Counter == 2)
                {
                    player = true;
                    P1_Skip_Button.interactable = true;
                    BoardManager.instance.p1_Indicator.SetActive(false);
                    BoardManager.instance.p2_Indicator.SetActive(true);
                    P1_D6_Counter = 1;
                }
            }
            else
            {
                P1_Skip_Button.interactable = true;
                BoardManager.instance.p1_Indicator.SetActive(false);
                BoardManager.instance.p2_Indicator.SetActive(true);
                player = true;
            }

            P1_skipside = 0;
        }
        else if (player == true)
        {
            BoardManager.instance.move_Player2();

            if(finalSide == 6)
            {
                if (P2_D6_Counter == 1)
                {
                    player = true;
                    P2_D6_Counter++;
                }
                else if (P2_D6_Counter == 2)
                {
                    player = false;
                    P2_Skip_Button.interactable = true;
                    BoardManager.instance.p1_Indicator.SetActive(true);
                    BoardManager.instance.p2_Indicator.SetActive(false);
                    P2_D6_Counter = 1;
                }
            }
            else
            {
                P2_Skip_Button.interactable = true;
                BoardManager.instance.p1_Indicator.SetActive(true);
                BoardManager.instance.p2_Indicator.SetActive(false);
                player = false;
            }
            P2_skipside = 0;
        }

        rollDice = true;
        //Dice_Image.gameObject.GetComponent<Button>().interactable = true;

        P1_Play_Button.interactable = false;
        P2_Play_Button.interactable = false;
        //end of two player script 
    }

    //method for skip button
    public void Skip_Chance()
    {
        if (player == false)
        {
            P1_skipside = finalSide;
            P1_Skip_Button.interactable = false;
            BoardManager.instance.p1_Indicator.SetActive(false);
            BoardManager.instance.p2_Indicator.SetActive(true);
            player = true;
        }
        else if(player == true)
        {
            P2_skipside = finalSide;
            P2_Skip_Button.interactable = false;
            BoardManager.instance.p1_Indicator.SetActive(true);
            BoardManager.instance.p2_Indicator.SetActive(false);
            player = false;
        }

        rollDice = true;
        Dice_Image.gameObject.GetComponent<Button>().interactable = true;

        P1_Play_Button.interactable = false;
        P2_Play_Button.interactable = false;
    } 
    //End of skip button method
}
