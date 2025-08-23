using UnityEngine;
using UnityEngine.Serialization;

namespace Src.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private WordInputView textBox;
        [SerializeField] private MiniMapView miniMap;

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
    }
}