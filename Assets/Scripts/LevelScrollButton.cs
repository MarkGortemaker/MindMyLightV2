using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScrollButton : MonoBehaviour
{
    Vector2 position;
    public void Slide(bool forward) //probably best to make this into three separate predetermined sections instead of adding to the x value each time
    {
        position = transform.localPosition; //copy current position of the scroll
        if (forward) //adjust the intended position to the next or previous section depending on the button 
        {
            position.x -= 900; 
        }
        else 
        {
            position.x += 900;
        }
        transform.localPosition = position; //set the local position of the scroll to the intended position
    }

   /* I tried to use Lerp here for a smooth transition but since it is constantly active in Update it messes with the swipe scroll controls, and I couldn't get any loops to work either, going to look into this
    * private void Update()
    {
        transform.localPosition = new Vector2(Mathf.Lerp(transform.localPosition.x, position.x, 5 * Time.deltaTime), position.y);
    } */

}
