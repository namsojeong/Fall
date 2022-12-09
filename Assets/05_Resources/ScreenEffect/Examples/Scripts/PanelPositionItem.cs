using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace WaterRippleForScreens {

public class PanelPositionItem : MonoBehaviour {
    public int ID; //Identifier

    public Button buttonRef; //Internal reference to delete button

    public Text positionXText, positionYText;

    public void AddOnClickListener() {
        buttonRef.onClick.AddListener(() => UIManager.instance.OnClickDeleteTargetPosition(ID));
    }

    public void UpdateOnClickListenerID() {
        buttonRef.onClick.RemoveAllListeners(); //Remove onClick events

        buttonRef.onClick.AddListener(() => UIManager.instance.OnClickDeleteTargetPosition(ID)); //Apply new ID
    }

    public void SetPositionText(int posX, int posY) {
        positionXText.text = "X:" + posX.ToString("d0");
        positionYText.text = "Y:" + posY.ToString("d0");
    }
}

}
