using UnityEngine;

public class LevelScrollButton : MonoBehaviour
{
    Vector2 targetPosition; //target position used for Lerping
    float[] buttonAxises = { -680, -1130, -1580, -2030, -2480 }; //axises of each level button except 1 and 7, which are at the end points
    int axisIndex = 0; //current index used for the buttonAxises array

    public bool CanLerp = false; //determines whether the linear interpolation can be used or not

    public UnityEngine.UI.Button forwardButton; //forward button in the menu
    public UnityEngine.UI.Button backButton; //back button in the menu

    void Awake() { Time.timeScale = 1; }

    private void Update()
    {
        CheckScrollViewEdge();
        if (!DragDetect.IsDrag && CanLerp)
        { //set the local position of the scroll view to the intended position when not dragging
            transform.localPosition = new Vector2(Mathf.Lerp(transform.localPosition.x, targetPosition.x, 5 * Time.deltaTime), transform.localPosition.y); //Linearly interpolate between current position and target position
            if (Mathf.Approximately(transform.localPosition.x, targetPosition.x))
            {
                CanLerp = false; //disable Lerping when target position is reached
            }
        }
        else if (DragDetect.IsDrag && CanLerp)
        {
            CanLerp = false; //disable Lerping when dragging
        }

        //check where the current position is and change the axisIndex accordingly
        if (transform.localPosition.x < buttonAxises[axisIndex] - 300)
        {
            axisIndex = Mathf.Clamp(axisIndex + 1, 0, buttonAxises.Length - 1);
        }
        else if (transform.localPosition.x > buttonAxises[axisIndex] + 300)
        {
            axisIndex = Mathf.Clamp(axisIndex - 1, 0, buttonAxises.Length - 1);
        }
    }
    public void SlideChapters(bool forward) //function used by forward and back buttons to move between chapter buttons
    {
        CanLerp = true; //allow Lerping
        targetPosition = transform.localPosition; //copy current position of the scroll
        if (forward) //change to the index that contains the next or previous button's x coordinates depending on the button 
        {
            axisIndex = Mathf.Clamp(axisIndex + 1, 0, buttonAxises.Length - 1);
        }
        else
        {
            axisIndex = Mathf.Clamp(axisIndex - 1, 0, buttonAxises.Length - 1);
        }
        targetPosition.x = buttonAxises[axisIndex]; //set target position for Lerping
        CheckScrollViewEdge();
    }

    void CheckScrollViewEdge() //set the forward or back button to be uninteractable when on the edges of the menu
    {
        if (transform.localPosition.x >= buttonAxises[0] - 200)
        {
            backButton.interactable = false;
        }
        else if (transform.localPosition.x <= buttonAxises[buttonAxises.Length - 1] + 200)
        {
            forwardButton.interactable = false;
        }
        else
        {
            forwardButton.interactable = true;
            backButton.interactable = true;
        }
    }

}
