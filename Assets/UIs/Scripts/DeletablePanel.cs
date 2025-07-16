public class DeletablePanel : RefreshablePanel
{
    public void Delete()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);

        RefreshParentLayout();
    }
}