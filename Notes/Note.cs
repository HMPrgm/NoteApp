using System.IO;
using System.Text.Json;
using System.Windows.Controls;

namespace Notes
{
    public class Note
    {
        public string text { get; set; }
        public string title { get; set; }
        public int id { get; }
        public TextBlock textBlock { get; set; }

        private static int CURR_ID = 1;

        public Note(string text, string title)
        {
            this.text = text;
            this.title = title;

            this.id = CURR_ID;
            CURR_ID++;
        }

        #region Accessors/Mutators
        

       
        public string ToFile()
        {
            return $"{this.title}\n{this.text}";
        }
        public override string ToString()
        {
            return $"{this.id}: \n\tName:{title}\n\t{text}";
        }
        #endregion
        /********** NOTE STORAGE **********
         * Notes are stored in a json file
         * The NoteStcut class helps convert data to and from json
         */
        private static List<Note> notes = new List<Note>();
        private static string filePath = "notes.json";
      
        struct NoteStruct
        {
            public string title { get; set; }
            public string text { get; set; }
        }

        public static void InitializeNotes()
        {
            try
            {
                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    notes = new List<Note>();
                    List<NoteStruct>? noteStructs = JsonSerializer.Deserialize<List<NoteStruct>>(json);
                    if (noteStructs == null) { return; }
                    foreach (NoteStruct noteStruct in noteStructs)
                    {
                        notes.Add(new Note(title: noteStruct.title, text: noteStruct.text));
                    } 
                }
            }
            catch (FileNotFoundException)
            {
                File.Create(filePath);
                notes = new List<Note>();
            }
            //try
            //{
            //    string[] filenames = Directory.GetFiles(fileDirectory);
            //    foreach (string filename in filenames)
            //    {
            //        if (!fileDirectory.EndsWith(fileExtenstion)) { continue; }
            //        string contents = File.ReadAllText(filename);
            //        string title = contents.Substring(0, contents.IndexOf("\n"));
            //        string text = contents.Substring(contents.IndexOf("\n") + 1);
            //        notes.Add(new Note(title: title, text: text));
            //    }
            //}
            //catch (DirectoryNotFoundException)
            //{
            //    Directory.CreateDirectory(fileDirectory);
            //}
        }
        
        public static void SaveNotes()
        {
            //Line 67 Describes process


            List<NoteStruct> noteStruct = new List<NoteStruct>();
            foreach (Note note in notes)
            {
                noteStruct.Add(new()
                {
                    text = note.text,
                    title = note.title
                });
                
            }
            string jsonString = JsonSerializer.Serialize(noteStruct, new JsonSerializerOptions { WriteIndented = true });
            using (StreamWriter outPut = new StreamWriter(filePath))
            {
                outPut.WriteLine(jsonString);
            }
        }
        public static bool Remove(Note note)
        {
            return notes.Remove(note);
        }

        public static void Add(Note note)
        {
            notes.Add(note);
        }

        public static void Add(string text, string title)
        {
            Add(new Note(text, title));
        }

        public static List<Note> Get()
        {
            return notes;
        }
        public static Note? GetFirst()
        {
            return notes.Count() > 0? notes[0]:null;
        }
        public static Note? FindByID(int id)
        {
            foreach (Note note in notes)
            {
                if (note.id == id) return note;
            }
            return null;
        }

        public static bool Update(int id, string title = "", string text = "")
        {
            Note? note = FindByID(id);
            if (note == null) return false;
            if (title != "") { note.title = title; }
            if (text != "") { note.text = text; }

            return true;
        }

    }
}
