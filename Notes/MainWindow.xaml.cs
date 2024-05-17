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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            Note.Add(noteBox.Text, noteTitle.Text);
            UpdateNoteList();
            noteBox.Clear(); noteTitle.Clear();
        }

        private void UpdateNoteList()
        {
            noteList.Items.Clear();
            foreach (Note note in Note.Get())
            {
                TextBlock text = new TextBlock();
                text.Text = note.getTitle();
                text.Name = "Note_" + note.getId().ToString();
                text.Tag = note.getId();
                noteList.Items.Add(text);
            }
        }

        private void noteList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is ListViewItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            TextBlock item = (TextBlock)noteList.ItemContainerGenerator.ItemFromContainer(dep);
            ShowNote noteView = new ShowNote((int)item.Tag);
            noteView.Show();
        }
    }
}
