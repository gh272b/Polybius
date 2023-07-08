using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class TopicManager : MonoBehaviour //�� �������� ��� �� ������� �� ButtonManager, ThemeManager � SliderManager, �� �����
{
    [SerializeField] private Slider slider;
    private bool canChangeTopic;
    
    [SerializeField] private GameObject FirstButton;
    [SerializeField] private GameObject SecondButton;
    [SerializeField] private GameObject ThirdButton;
    [SerializeField] private GameObject CurrentTheme;
    [SerializeField] private GameObject SliderManager;
    [SerializeField] private GameObject HeaderManager;
    [SerializeField] private GameObject NewsManager;
    [SerializeField] private GameObject PictureManager;


    public List<Sprite> picturesList;
    private List<string> topics = new List<string> {"anime :(", "blogger!", "wow,kitty!<3", "city news",
    "games!!", "top 10 lifehacks", "political news", "wow look i'm rich!!", "social problems", "do you like hurt other people?"};
    private List<float> mood_updates = new List<float> {-2f, 2f, 4f, -1f, 3.5f, 1.2f, -3.1f, -1.4f, -2.5f, -4f};
    private List<float> er_update = new List<float> { -1f, 3f, 4f, -2f, 2f, -3f, -3.2f, 2.1f, 1.2f, -4f };
    private List<TopicClass> TopicList = new List<TopicClass>();
    

    private List<GameObject> Buttons = new List<GameObject>();
    private Unity.Mathematics.Random RandomGenerator = new Unity.Mathematics.Random(3232);
    void Start()
    {
        canChangeTopic = true;
        
        Buttons.Add(FirstButton);
        Buttons.Add(SecondButton);
        Buttons.Add(ThirdButton);
        UpdateButtons();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator Cooldown()
    {
        canChangeTopic = false;
        foreach (GameObject btn in Buttons)
        {
            btn.GetComponent<TopicButtonClass>().canChangeTopic = false;
        }
        for (int i = 0; i <= 5; i++)
        {
            slider.value = 5 - i;
            yield return new WaitForSeconds(1);
        }

        canChangeTopic = true;
        foreach (GameObject btn in Buttons)
        {
            btn.GetComponent<TopicButtonClass>().canChangeTopic = true;
        }
        UpdateButtons();
    }
    public void UpdateByUser(string choice, float mood_upd, float er_upd, Sprite pic)
    {
        CurrentTheme.GetComponent<TMP_Text>().text=choice;
        
        SliderManager.GetComponent<SliderManager>().ChangeSliders(mood_upd, er_upd);
        PictureManager.GetComponent<PicturesManager>().UpdatePictures(pic);
        for (int i=0; i<3; i++)
        {
            GameObject btn = Buttons[i];
            btn.GetComponent<TMP_Text>().text = "wait!";
        }
        StartCoroutine(Cooldown());
        UpdateButtons();
        HeaderManager.GetComponent<HeaderManager>().UpdateHeaders(choice);
        NewsManager.GetComponent<NewsManager>().UpdateNews();
        
    }
    
    void UpdateButtons()
    {
        if (!canChangeTopic)
            return;
        
        List<int> used = new List<int>();
        while (used.Count != 3)
        {
            int index = RandomGenerator.NextInt(0, 10);
            if (!(used.Contains(index)) && used.Count != 3)
            {
                used.Add(index);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject btn = Buttons[i];
            string topic_name = topics[used[i]];
            Sprite icon = picturesList[used[i]];
            float mood_update = mood_updates[used[i]];
            float engagement_update = er_update[used[i]];
            btn.GetComponent<TopicButtonClass>().DataUpdate(new TopicClass(topic_name, icon, mood_update, engagement_update));
            btn.GetComponent<TMP_Text>().text = btn.GetComponent<TopicButtonClass>().ToString();
        }
    }
}
