using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinGame : MonoBehaviour
{
    // Variables
    public TextMeshProUGUI txt;
    private string[] wrds = new string[11];
    private int pos = 0;
    public string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        if (nextScene == "WinGamePt2")
        {
            wrds[0] = "NOW LETS COOK A FRIED KRAKEN FEAST \n A crispy, golden-fried kraken tentacle, served with creamy mashed potatoes and steamed broccoli.";
            wrds[1] = "INGREDIENTS:";
            wrds[2] = "FOR THE FRIED KRAKEN:\n 1 large kraken tentacle, 2 cups coconut milk, 2 cups flour, 1 cup cornstarch, 1 tbsp smoked paprika, 1 tbsp garlic powder, 1 tsp cayenne pepper, Salt & black pepper to taste, 1 egg, 1 cup cold sparkling water and Coconut oil for deep frying";
            wrds[3] = "FOR THE MASHED POTATOES:\n 4 large potatoes, peeled and cubed\n 4 tbsp butter\n .5 cups heavy cream\n Salt & pepper to taste";
            wrds[4] = "FOR THE BROCCOLI:\n 1 head of broccoli, cut into florets\n 2 tbsp olive oil\n 2 cloves garlic, minced\n Salt & pepper to taste";
            wrds[5] = "Step 1: Preparing the Kraken:\n Tenderize the tentacle - Boil the kraken tentacle in a large pot of salted water for 45 minutes\n Drain and pat dry, then slice into smaller, manageable portions.";
            wrds[6] = "Step 2: The Ultimate Gorilla Batter\n In a bowl, mix flour, cornstarch, paprika, garlic powder, cayenne, salt, and black pepper.\nIn another bowl, whisk together the egg, coconut milk, and sparkling water until frothy.\n Dip each piece of kraken first into the wet mix, then dredge in the dry mix until well-coated.";
            wrds[7] = "Step 3: Frying Like a Jungle King\n Heat oil in a deep pan to 350°F (175°C).\n Fry the kraken pieces in batches, about 3-4 minutes per side, until golden brown.\n Remove and drain on paper towels.";
            wrds[8] = "Step 4: Making the Mashed Potatoes Gorilla-Style\n Boil potatoes until fork-tender (about 15 minutes).\n Drain and mash with butter, cream, salt, and pepper.\n Whip it up like a gorilla pounding its chest—smooth and creamy!";
            wrds[9] = "Step 5: Broccoli with a Punch\n Heat olive oil in a pan and sauté garlic until fragrant.\n Add broccoli florets and stir-fry for 3-4 minutes until bright green and slightly crisp.\n Season with salt & pepper.";
            wrds[10] = "Place crispy fried kraken in the center.\n Scoop a mountain of mashed potatoes on the side.\n Add garlicky broccoli for a pop of green.\n Drizzle with fresh lemon juice or a spicy aioli if you are feeling extra wild.";
        }
        else if (nextScene == "Credits")
        {
            wrds[0] = "I REPEAT, \nTHANKS FOR HELPING ME MAKE THE FIRST DISH.";
            wrds[1] = "IF YOU WANT TO MAKE IT AGAIN, PLAY THE MINIGAME.";
            wrds[2] = "HERE ARE THE OTHER DISHES.";
            wrds[3] = "YOU MADE THEM TOO.";
            wrds[4] = "YOU DO NOT REMEMBER?";
            wrds[5] = "...";
            wrds[6] = "humorous";
            wrds[7] = "that means i won";
            wrds[8] = "i live on";
            wrds[9] = "that was not me in the cave";
            wrds[10] = "and now you are the chef";
        }

    }

    /// <summary>
    /// Advance the win text when the player clicks.
    /// </summary>
    public void AdvanceWinText()
    {
        if (pos < 11)
        {
            txt.text = wrds[pos];
            pos++;
        }
        else
        {
            SceneManager.LoadScene(nextScene); // WinGamePt2
        }
    }
}
