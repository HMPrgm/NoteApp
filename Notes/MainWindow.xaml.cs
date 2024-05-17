using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Notes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int score = 0;
        private Note curNote;
        
        public MainWindow()
        {
            InitializeComponent();
            Note.InitializeNotes();
            RefreshApp();
        }

        private void RefreshApp(Note? note = null)
        {
            if (note == null) { note = Note.GetFirst(); }
            RefreshNoteList();
            UpdateCurrentNote(note);
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void OnClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveCurrentNote();
            Note.SaveNotes();
        }



        private void noteList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Code that gets item clicked (I don't really know how it works)
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is ListViewItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null){ return; }
            TextBlock item = (TextBlock)noteList.ItemContainerGenerator.ItemFromContainer(dep);
            UpdateCurrentNote(Note.FindByID((int)item.Tag));
        }

        private void RefreshNoteList()
        {
            noteList.Items.Clear();
            foreach (Note note in Note.Get())
            {
                TextBlock text = new TextBlock();
                text.Text = note.getTitle() == "" ? "Untitled Note" : note.getTitle();
                text.Name = "Note_" + note.getId().ToString();
                text.Tag = note.getId();
                noteList.Items.Add(text);
            }
        }

        private void UpdateCurrentNote(Note? note)
        {
            if (note == null) { return; }
            //Saves old note
            if (curNote != null) { SaveCurrentNote(); }
            //Sets New Note
            curNote = note;
            noteTitle.Text = note.getTitle();
            noteBox.Text = note.getText();
        }

        private void SaveCurrentNote()
        {
            if (curNote == null) { return; }
            curNote.setText(noteBox.Text);
            curNote.setTitle(noteTitle.Text);
            RefreshNoteList();
        }

        private void NoteChanged(object sender, TextChangedEventArgs e)
        {
            mainWindow.Title = noteTitle.Text.Length > 0 ? noteTitle.Text : "Untitled Note";
            
        }

        private void AddNewNote(object sender, RoutedEventArgs e) 
        {
            Note note = AddNewNote();
            RefreshApp(note);
        }
        private Note AddNewNote(string title = "", string text = "")
        {
            Note note = new Note(title: title, text: text);
            Note.Add(note);
            return note;
        }

        private void DeleteCurrentNote(object sender, RoutedEventArgs e)
        {
            if (curNote != null)
            { 
                Note.Remove(curNote); 
            }
            if (Note.GetFirst() == null)
            {
                AddNewNote();
            }
            RefreshApp();
        }

        
    }
}
