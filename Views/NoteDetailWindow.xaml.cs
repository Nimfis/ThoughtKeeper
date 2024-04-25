using System.Windows;
using ThoughtKeeper.DTO;

namespace ThoughtKeeper
{
    public partial class NoteDetailWindow : Window
    {
        public NoteDetailWindow(NoteDTO note)
        {
            InitializeComponent();
            DisplayNoteDetails(note);
        }

        private void DisplayNoteDetails(NoteDTO note)
        {
            titleTextBlock.Text = note.Title;
            contentTextBlock.Text = note.Content;
            dateCreatedTextBlock.Text = note.DateCreated.ToString("g");
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}