using UnityEngine;
using UnityEngine.Serialization;

namespace Src.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private WordInputView textBox;
        [SerializeField] private MiniMapView miniMap;
        [SerializeField] private ResultScreen resultScreen;

        public void ShowTextBox()
        {
            textBox.Show();
            EmptyTextBox();
        }

        public void HideTextBox()
        {
            textBox.Hide();
        }

        public void EmptyTextBox()
        {
            textBox.inputField.text = "";
        }

        public void HideMap()
        {
            miniMap.Hide();
        }

        public void ShowMap()
        {
            miniMap.Show();
        }

        public void ShowResults(Sprite mapSprite, Vector2 marker1Norm, Vector2 marker2Norm)
        {
            resultScreen.Show(mapSprite, marker1Norm, marker2Norm);
        }

        public void HideResults()
        {
            resultScreen.Hide();
        }
    }
}