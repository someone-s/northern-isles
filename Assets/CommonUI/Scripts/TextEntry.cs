using UnityEngine;

public class TextEntry : MonoBehaviour
{
    [TextArea]
    public string text;
    public TextEntry under;

    public void AddToStack(TextStack stack)
    {
        if (under != null)
            stack.InsertLine(under, this);
        else
            stack.AddLine(this);
    }

    public void UpdateAtStack(TextStack stack)
    {
        stack.UpdateLine(this);
    }

    public void RemoveFromStack(TextStack stack)
    {
        stack.Remove(this);
    }

    public void RemoveFromStackInstant(TextStack stack)
    {
        stack.RemoveInstant(this);
    }
}