using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject bestiaireUI;
    private bool bestiaireOn;
    private GameObject actualTopic;

    [Header("Topics")]
    [SerializeField] private GameObject[] topics;
    #endregion


    #region Built-in Methods
    private void Start()
    {
        Time.timeScale = 0;

        inGameUI.SetActive(false);
        menuUI.SetActive(true);
        bestiaireUI.SetActive(false);

        actualTopic = topics[0];
        actualTopic.transform.GetChild(0).gameObject.SetActive(true);
    }
    #endregion


    #region Customs Methods
    public void StartGame()
    {
        Time.timeScale = 1;

        inGameUI.SetActive(true);
        menuUI.SetActive(false);
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        Time.timeScale = 0;

        inGameUI.SetActive(false);
        menuUI.SetActive(true);
    }

    #region Bestiaire UI
    public void Bestiaire()
    {
        if (!bestiaireOn)
        {
            bestiaireOn = true;
            menuUI.SetActive(false);
            bestiaireUI.SetActive(true);
        }
        else if (bestiaireOn)
        {
            bestiaireOn = false;
            menuUI.SetActive(true);
            bestiaireUI.SetActive(false);
        }
    }

    public void PagePrecedente()
    {
        if (actualTopic.transform.childCount > 1)
        {
            actualTopic.transform.GetChild(0).gameObject.SetActive(true);
            actualTopic.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void PageSuivante()
    {
        if (actualTopic.transform.childCount > 1)
        {
            actualTopic.transform.GetChild(1).gameObject.SetActive(true);
            actualTopic.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void CloseTopic()
    {
        if (actualTopic.transform.childCount > 1)
        {
            actualTopic.transform.GetChild(1).gameObject.SetActive(false);
            actualTopic.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            actualTopic.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void SteveIrwin()
    {
        CloseTopic();

        actualTopic = topics[0];
        actualTopic.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void PoissonClown()
    {
        CloseTopic();

        actualTopic = topics[1];
        actualTopic.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void TortueCacouanne()
    {
        CloseTopic();

        actualTopic = topics[2];
        actualTopic.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void RequinMarteau()
    {
        CloseTopic();

        actualTopic = topics[3];
        actualTopic.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void DauphinMaui()
    {
        CloseTopic();

        actualTopic = topics[4];
        actualTopic.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void BaleineBleue()
    {
        CloseTopic();

        actualTopic = topics[5];
        actualTopic.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Plastique()
    {
        CloseTopic();

        actualTopic = topics[6];
        actualTopic.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void TonneauxEssences()
    {
        CloseTopic();

        actualTopic = topics[7];
        actualTopic.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void FiletDePeches()
    {
        CloseTopic();

        actualTopic = topics[8];
        actualTopic.transform.GetChild(0).gameObject.SetActive(true);
    }
    #endregion

    #endregion
}