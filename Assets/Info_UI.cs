using UnityEngine;

public class Info_UI : MonoBehaviour
{
    [SerializeField] GameObject info_UI;
    [SerializeField] private GameObject[] pages;
    private int currentPage { get { return m_currentPage; } set { if (value > 0 && value < pages.Length -1) { m_currentPage = value; } } }
    [SerializeField] public int m_currentPage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var page in pages)
        {
            page.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void NextPage()
    {
        m_currentPage++;
        ClosrOtherPage();
    }

    public void lastPage()
    {
        m_currentPage--;
        ClosrOtherPage();
    }

    void ClosrOtherPage()
    {
        int lastpage = m_currentPage - 1;
        int nextpage = m_currentPage + 1;

        //Debug.Log(lastpage);
        //Debug.Log(nextpage);

        if(lastpage >= 0) pages[lastpage].SetActive(false);
        if (nextpage < pages.Length) pages[nextpage].SetActive(false);

        pages[m_currentPage].SetActive(true);
    }

    public void OpenBook()
    {
        currentPage = 0;
        pages[0].SetActive(true);
        info_UI.SetActive(true);
    }

    public void CloseBook()
    {
        info_UI.SetActive(false);
    }
}
