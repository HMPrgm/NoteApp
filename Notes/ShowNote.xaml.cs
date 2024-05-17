using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Notes
{
    /// <summary>
    /// Interaction logic for ShowNote.xaml
    /// </summary>
    public partial class ShowNote : Window
    {
        Note note;
        public ShowNote(Note note)
        {
            InitializeComponent();
            this.note = note;
            DisplayNote();
        }

        public ShowNote(int id)
        {
            InitializeComponent();
            Note? note = Note.FindByID(id);
            if (note == null) 
            {
                throw new Exception("Null Note trying to be shown. id: " + id);
            }
            this.note = note;
            DisplayNote();
        }

        private void DisplayNote()
        {
            title.Text = note.getTitle();
            body.Text = note.getText();
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
