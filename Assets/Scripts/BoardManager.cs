using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    //public variables  =========================================================================================================================================================================
    public GameObject[] grids;
    public GameObject[] players;
    public GameObject[] shortcut_Arrow, pitfall_Arrow;
    public GameObject p1_Indicator, p2_Indicator;
    public GameObject start_Screen , gameOver_Screen , instruction_Screen , splash_Screen , game_Screen , exit_Screen , resume_Screen;
    public GameObject winPlayer_Text , Path_Grid;
    public float lerpValue = 1.0f , speed = 1.0f;
    public int player1_Counter, player2_Counter;
    public int[] s = new int[2] ;
    public int[] p = new int[2] ;

    //private variables  ========================================================================================================================================================================
    GameObject[] path_list = new GameObject[64];
    int[] grids_LeftElements_Number = { 0, 15, 16, 31, 32, 47, 48, 63 };
    int x, y, z , p1_Start_Point , p1_End_Point , p2_Start_Point, p2_End_Point;
    bool P1_Move, P2_Move;
    Vector3 current_Grid_P1, destination_Grid_P1 , current_Grid_P2, destination_Grid_P2;

    public static BoardManager instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        start_Screen.SetActive(false);
        splash_Screen.SetActive(true);
        Reset_BoardManager();
        StartCoroutine(After_SplashScreen());
    }

    void Update()
    {
        if(P1_Move == true)
        {
            P1_Lerp();
        }

        if(P2_Move == true)
        {
            P2_Lerp();
        }
    }

    IEnumerator After_SplashScreen()
    {
        yield return new WaitForSeconds(7.0f);
        splash_Screen.SetActive(false);
        start_Screen.SetActive(true);
    }

    //public methods  ===========================================================================================================================================================================

    //use reset the values of game 
    public void Reset_BoardManager()
    {
        instruction_Screen.SetActive(false);
        gameOver_Screen.SetActive(false);
        game_Screen.SetActive(false);
        exit_Screen.SetActive(false);
        resume_Screen.SetActive(false);
        //s1 = s2 = p1 = p2 = 0;
        player1_Counter = 0;
        player2_Counter = 0;
        p1_Indicator.SetActive(true);
        p2_Indicator.SetActive(true);
        Reset_Grids();
        Instantiate_Path();
        Reset_Path();
    }

    //use to randomly generate shortcut / pitfalls. call each time when players click on dice.
    public void Change_Board_Map()
    {
        Reset_Grids();
        Random_Shortcut_Number();
        Random_Pitfall_Number();
        Shortcut_Creator();
        Pitfall_Creator();
    }

    public void move_Player1()
    {
        if(player1_Counter < 64)
        {
            p1_Start_Point = player1_Counter;
            current_Grid_P1 = grids[player1_Counter].GetComponent<RectTransform>().transform.position;

            player1_Counter = player1_Counter + Dice.instance.finalSide + Dice.instance.P1_skipside;
            
            if (player1_Counter >= 64)
            {
                player1_Counter = player1_Counter - Dice.instance.finalSide; 
            }

            //shortcuts
            if (player1_Counter == s[0])
            {
                x = s[0] / 8;
                if( (x + 1) % 2 == 0){ y = 7; }
                else if((x + 1) % 2 != 0 ) { y = s[0] % 8; }
                Calculate_Move(x + 1);
                player1_Counter = z;
            }

            if (player1_Counter == s[1]) { player1_Counter = s[1] + 16; }

            //pitfalls
            if (player1_Counter == p[0])
            {
                x = p[0] / 8;
                if ((x - 1) % 2 == 0) { y = 7; }
                else if ((x - 1) % 2 != 0) { y = p[0] % 8; }
                Calculate_Move(x - 1);
                player1_Counter = z;
            }

            if (player1_Counter == p[1]) { player1_Counter = p[1] - 16; }

            destination_Grid_P1 = grids[player1_Counter].GetComponent<RectTransform>().transform.position;
            p1_End_Point = player1_Counter;

            Show_Path(p1_Start_Point, p1_End_Point);

            P1_Move = true;
            //players[0].GetComponent<RectTransform>().transform.position = Vector3.Lerp(current_Grid, destination_Grid, lerpValue);
        }
    }

    void P1_Lerp()
    {
        //Debug.Log("////");
        float step = speed * Time.deltaTime; // calculate distance to move
        players[0].GetComponent<RectTransform>().transform.position = Vector3.Lerp(players[0].GetComponent<RectTransform>().transform.position, destination_Grid_P1, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(players[0].GetComponent<RectTransform>().transform.position, destination_Grid_P1) < 0.1f)
        {
            Reset_Path();
            Dice.instance.Dice_Image.gameObject.GetComponent<Button>().interactable = true;

            if (player1_Counter == 63)
            {
                winPlayer_Text.GetComponent<Text>().text = "Player 1 Win !!!";
                gameOver_Screen.SetActive(true);
            }

            P1_Move = false;
        }
    }

    public void move_Player2()
    {

        if (player2_Counter < 64)
        {
            p2_Start_Point = player2_Counter;
            current_Grid_P2 = grids[player2_Counter].GetComponent<RectTransform>().transform.position;

            player2_Counter = player2_Counter + Dice.instance.finalSide + Dice.instance.P2_skipside;
            //Debug.Log("Final Move is :" + player2_Counter);

            if (player2_Counter >= 64)
            {
                player2_Counter = player2_Counter - Dice.instance.finalSide;
            }

            //shortcuts
            if (player2_Counter == s[0])
            {
                x = s[0] / 8;
                if ((x + 1) % 2 == 0) { y = 7; }
                else if ((x + 1) % 2 != 0) { y = s[0] % 8; }
                Calculate_Move(x + 1);
                player2_Counter = z;
            }

            if (player2_Counter == s[1]) { player2_Counter = s[1] + 16; }

            //pitfalls
            if (player2_Counter == p[0])
            {
                x = p[0] / 8;
                if ((x - 1) % 2 == 0) { y = 7; }
                else if ((x - 1) % 2 != 0) { y = p[0] % 8; }
                Calculate_Move(x - 1);
                player2_Counter = z;
            }

            if (player2_Counter == p[1]) { player2_Counter = p[1] - 16; }

            destination_Grid_P2 = grids[player2_Counter].GetComponent<RectTransform>().transform.position;

            p2_End_Point = player2_Counter;

            Show_Path(p2_Start_Point, p2_End_Point);

            P2_Move = true;
            //players[1].GetComponent<RectTransform>().transform.position = Vector3.Lerp(current_Grid, destination_Grid, lerpValue);
  
        }
    }

    void P2_Lerp()
    {
        //Debug.Log("////");
        float step = speed * Time.deltaTime; // calculate distance to move
        players[1].GetComponent<RectTransform>().transform.position = Vector3.Lerp(players[1].GetComponent<RectTransform>().transform.position, destination_Grid_P2, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(players[1].GetComponent<RectTransform>().transform.position, destination_Grid_P2) < 0.1f)
        {
            Reset_Path();
            Dice.instance.Dice_Image.gameObject.GetComponent<Button>().interactable = true;

            if (player2_Counter == 63)
            {
                winPlayer_Text.GetComponent<Text>().text = "Player 2 Win !!!";
                gameOver_Screen.SetActive(true);
            }

            P2_Move = false;
        }
    }

    //use to calculate the position of move fromShortcut / Pitfall
    int Calculate_Move(int id)
    {
        if (grids_LeftElements_Number[id] % 8 == 0)
        {
            z = grids_LeftElements_Number[id] + y;
        }
        else if (grids_LeftElements_Number[id] % 8 != 0)
        {
            z = grids_LeftElements_Number[id] - y;
        }

        Debug.Log("Value of Z is :" + z);
        return z;
    }

    public void onStart_Screen()
    {
        game_Screen.SetActive(true);
        start_Screen.SetActive(false);
        reset_Player_Position();
        p1_Indicator.SetActive(true);
        p2_Indicator.SetActive(false);
        BoardManager.instance.Change_Board_Map();
    }

    //use to Play again or Restart the game
    public void PlayAgain()
    {
        Dice.instance.Reset_Dice();
        Reset_BoardManager();
        onStart_Screen();
    }

    public void On_Home_Button()
    {
        Dice.instance.Reset_Dice();
        Reset_BoardManager();
        start_Screen.SetActive(true);
    }

    //use to open user manual ( information screen )
    public void Open_User_Guide()
    {
        instruction_Screen.SetActive(true);
    }

    public void Close_User_Guide()
    {
        instruction_Screen.SetActive(false);
    }

    //exit from application
    public void Quite_Application()
    {
        exit_Screen.SetActive(true);
    }

    public void on_Yes()
    {
        Application.Quit();
    }

    public void on_No()
    {
        exit_Screen.SetActive(false);
    }

    public void on_Pause()
    {
        resume_Screen.SetActive(true);
    }

    public void on_Resume()
    {
        resume_Screen.SetActive(false);
    }

    //private methods  ==========================================================================================================================================================================

    //use to reset the positions of each player
    private void reset_Player_Position()
    {
        for(int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<RectTransform>().transform.position = grids[0].GetComponent<RectTransform>().transform.position;
        }
    }

    //reset color of all Grids
    private void Reset_Grids()
    {
        for (int i = 0; i < grids.Length; i++)
        {
            grids[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }

    //instantiate path object in each grid ( suare blocks or tiles )
    private void Instantiate_Path()
    {
        for (int i = 0; i < path_list.Length; i++)
        {
            GameObject myPath = Instantiate(Path_Grid, grids[i].transform);
            myPath.GetComponent<RectTransform>().transform.position = grids[i].GetComponent<RectTransform>().transform.position;
            path_list[i] = myPath;
        }
    }

    //use reset the colour component of given objects
    private void Reset_Path()
    {
        for (int i = 0; i < path_list.Length; i++)
        {
            path_list[i].GetComponent<Image>().color = new Color(0, 0, 0, 0); ;
        }
    }

    //use to highlight the path on which user travel
    private void Show_Path(int startPoint , int endPoint)
    {
        Reset_Path();
        for (int i = startPoint; i <= endPoint; i++)
        {
            path_list[i].GetComponent<Image>().color = new Color(0.1918833f, 0.1918833f, 00.8301887f, 0.24f); ;
        }
    }

    private void Shortcut_Creator()
    {
        shortcut_Arrow[0].GetComponent<RectTransform>().position = grids[s[0]].GetComponent<RectTransform>().position  + new Vector3(0, 68.0f, 0) ;
        shortcut_Arrow[1].GetComponent<RectTransform>().position = grids[s[1]].GetComponent<RectTransform>().position  + new Vector3(0, 209.5f , 0);

        for (int i = 0; i < s.Length; i++)
        {
            grids[s[i]].GetComponent<Image>().color = new Color(0.4477572f, 0.7488796f, 0.8113208f, 1.0f);  
        }
    }

    private void Random_Shortcut_Number()
    {
        s[0] = Random.Range(1, 56);
        s[1] = Random.Range(1, 47);

        while (s[0] == s[1] || s[0] == p[0] || s[0] == p[1] || s[1] == p[0] || s[1] == p[1] || s[0] == 48)
        {
            Random_Shortcut_Number();
        }
    }

    private void Pitfall_Creator()
    {
        pitfall_Arrow[0].GetComponent<RectTransform>().position = grids[p[0]].GetComponent<RectTransform>().position  - new Vector3(0, 68.0f, 0);
        pitfall_Arrow[1].GetComponent<RectTransform>().position = grids[p[1]].GetComponent<RectTransform>().position  - new Vector3(0, 209.5f, 0);

        for (int i = 0; i < p.Length; i++)
        {
            grids[p[i]].GetComponent<Image>().color = new Color(0.8773585f, 0.6166341f, 0.6166341f, 1.0f);
        } 
    }

    private void Random_Pitfall_Number()
    {
        p[0] = Random.Range(8, 63);
        p[1] = Random.Range(17, 63);

        while (p[0] == p[1] || p[0] == s[0] || p[0] == s[1] || p[1] == s[0] || p[1] == s[1] || p[0] == 15)
        {
            Random_Pitfall_Number();
        }
    }
}
