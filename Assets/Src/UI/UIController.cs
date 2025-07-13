using UnityEngine;

namespace Src.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private WordInputView TextBox;

        public void ShowTextBox()
        {
            TextBox.Show();
            EmptyTextBox();
        }

        public void HideTextBox()
        {
            TextBox.Hide();
        }

        public void EmptyTextBox()
        {
            TextBox.inputField.text = "";
        }
    }
}