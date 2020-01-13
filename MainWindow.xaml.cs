// 
// File:		MainWindow.xaml.cs
// Programmer:	Trent Thompson
// Date:		September 27, 2019
// Description:	This file contains the code implementation for the MainWindow window.
//              This file also contains event handlers for the events used in 
//              various windows within the entire program.              
//



using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WMP_2 {

    public partial class MainWindow : Window {
        // Data members 
        private bool _UnsavedWork;

        // Interface for the _UnsavedWork private data member
        public bool UnsavedWork { 
            get{ 
                return _UnsavedWork;
            } 
            set { 
                _UnsavedWork = value;
            }
        }

        

        //
        // Method:      MainWindow - Constructor
        // Description: Constructor for the MainWindow
        // Parameters:  none
        // Returns:     none
        //
        public MainWindow() {
            // Start the program saying all work is saved
            UnsavedWork = false;

            InitializeComponent();
        }

        

        //
        // Method:      SaveFile
        // Description: Opens a SaveFile Dialog and sets the SaveStatus to saved
        //              upon saving a file. 
        // Parameters:  none
        // Returns:     none
        // Source       https://www.wpf-tutorial.com/dialogs/the-savefiledialog/
        //
        private void SaveFile() { 
            SaveFileDialog saveFile = new SaveFileDialog();
            // Only allow user to save as a text file
            saveFile.Filter = "Text File (*.txt) | *.txt";

            // Open the new save file dialog and check if a file was actually saved
            if (saveFile.ShowDialog() == true) {
                File.WriteAllText(saveFile.FileName, GetRichText(TypingArea));

                // Set the unsaved work status to false
                UnsavedWorkStatus(false);
            }
        }



        //
        // Method       GetText
        // Description: Returns the text that is entered in a Rich Text Box
        // Parameters:  RichTextBox TextBox
        // Returns:     string: The string found in the RichTextBox
        // Source:      https://docs.microsoft.com/en-us/dotnet/framework/wpf/controls/how-to-extract-the-text-content-from-a-richtextbox
        //
        public string GetRichText(RichTextBox TextBox) { 
            string ReturnText = "";  

            // Take everything inside of the text box
            var RichText = new TextRange(TextBox.Document.ContentStart, TextBox.Document.ContentEnd);
                         
            // Set the string we are returning to the Text within the text box
            ReturnText = RichText.Text;
            
            // Remove the \n and \r added to the string
            ReturnText = ReturnText.Replace("\n", string.Empty);
            ReturnText = ReturnText.Replace("\r", string.Empty);

            return ReturnText;
        }



        //
        // Method:      UnsavedWorkStatus
        // Description: Changes the save status of the file being edited
        // Parameters:  bool NewStatus
        // Returns:     void
        //
        public void UnsavedWorkStatus(bool NewStatus) { 
            this.UnsavedWork = NewStatus;

            // Set the savestatus text based on the UnsavedWork bool
            if (UnsavedWork == true) { 
                this.SaveStatus.Text = "Unsaved";
            }
            else { 
                this.SaveStatus.Text = "Saved";
            }
        }



        //
        // Method:      CloseProgram
        // Description: Closes the MainWindow
        // Parameters:  void
        // Returns:     void
        //
        private void CloseProgram() {
            this.Close();
        }



        //
        // Method:      StartNewFile
        // Description: Clears the TextBox and sets the UnsavedWork status to false
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        //
        private void StartNewFile() { 
            // Clear the text box
            this.TypingArea.Document.Blocks.Clear();

            // Change the save status to saved
            this.UnsavedWorkStatus(false);
        }



        //
        // Method:      OpenFile
        // Description: Clears the text box and imports a new file to the TextBox
        // Parameters:  void
        // Returns:     void
        //
        private void OpenFile() { 
            // Open file dialog
            OpenFileDialog OpenFile = new OpenFileDialog();

            // Display the open file dialog
            OpenFile.ShowDialog();

            // Safe guard in case the user wants to cancel instead of open the file
            if (OpenFile.FileName != "") { 
                // Clear the TypingArea
                TypingArea.Document.Blocks.Clear();

                // Read all the text to the typing area
                TypingArea.AppendText(File.ReadAllText(OpenFile.FileName));

                // Change the status of the UnsavedWork
                UnsavedWorkStatus(false);
            }
        }

        

        //
        // Method       TypingArea_TextChanged
        // Description: Calculates the amount of characters in the text box and displays 
        //              that number to the CharCount on the status bar.
        // Parameters:  object sender, TextChangedEventArgs e
        // Returns:     void
        //
        private void TypingArea_TextChanged(object sender, TextChangedEventArgs e) {
            int StringLength = 0;
            string Text = "";  

            // Set the unsaved work status to true, since something has been changed in the text area
            UnsavedWorkStatus(true);

            // Take the string from the text box
            Text = GetRichText(TypingArea);

            // Find the length of the string
            StringLength = Text.Length;
            
            // Display the character count to the CharCount on the status bar
            CharCount.Text = StringLength.ToString();
        }

        
        
        //
        // Method:      About_Click
        // Description: Draws the modal "AboutWindow" to the screen when "About" is clicked
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        // Source:      https://stackoverflow.com/questions/499294/how-to-make-modal-dialog-in-wpf
        // 
        private void About_Click(object sender, RoutedEventArgs e) {
            // Create a new AboutWindow object
            AboutWindow Window = new AboutWindow();

            // Draw the new AboutWindow object to the Screen
            Window.ShowDialog();
        }



        //
        // Method:      SaveAs_Click   
        // Description: Calls the SaveFile method whenever SaveAs is clicked
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void     
        //
        private void SaveAs_Click(object sender, RoutedEventArgs e) {
            // Call the save file method
            this.SaveFile();
        }


        
        //
        // Method:      New_Click
        // Description: Allows the user to start a new file when clicking New
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        //
        private void New_Click(object sender, RoutedEventArgs e) {
           // Check if there is unsaved work in the text box
           if (this.UnsavedWork == true) { 
                // Create a new SavePrompt Dialog
                SavePrompt Save = new SavePrompt();

                // Link the Events
                Save.SaveWork += New_SaveWork_Handler;
                Save.DontSave += New_DontSave_Handler;

                // Show the dialog
                Save.ShowDialog();
           }
           // Otherwise, just invoke the start new file method
           else { 
                StartNewFile();
           }
        }



        //
        // Method:      New_SaveWork_Handler
        // Description: Handles the SaveWork event of the SavePrompt class when New_Click 
        //              has been invoked and the option to Save has been selected
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        //
        private void New_SaveWork_Handler(object sender, RoutedEventArgs e) { 
            // Call the SaveAs_Click method
            this.SaveFile();

            // Check if the work has been saved or not (ie the user presses cancel on the save file dialog)
            if (this.UnsavedWork == false) { 
                // If the file was indeed saved, then go ahead and start a new file
                StartNewFile();

            }
        }



        //
        // Method:      New_DontSave_Handler
        // Description: Handles the DontSave event of the SavePrompt class when New_Click 
        //              has been invoked and the option not to Save has been selected
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        //
        private void New_DontSave_Handler(object sender, RoutedEventArgs e) { 
            StartNewFile();
        }



        //
        // Method:      Open_Click
        // Description: Allows the user to open a file for editing.
        //              Also checks for unsaved work and gives user 
        //              a chance to save their work
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        //
        private void Open_Click(object sender, RoutedEventArgs e) {
            // Check if there is unsaved work in the text box
           if (this.UnsavedWork == true) { 
                // Create a new SavePrompt Dialog
                SavePrompt Save = new SavePrompt();

                // Link the Events
                Save.SaveWork += Open_SaveWork_Handler;
                Save.DontSave += Open_DontSave_Handler;

                // Show the box
                Save.ShowDialog();
           }
           else { 
                OpenFile();     
           }
        }



        //
        // Method:      Open_SaveWork_Handler
        // Description: Handles the SaveWork event of the SavePrompt class when Open_Click 
        //              has been invoked and the option to Save has been selected
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        //
        private void Open_SaveWork_Handler(object sender, RoutedEventArgs e) { 
            // Call the SaveAs_Click method
            this.SaveFile();

            // Check if the work has been saved or not (ie the user presses cancel on the save file dialog)
            if (this.UnsavedWork == false) { 
                // If the file was indeed saved, then go ahead and open a file
                OpenFile();

            }
        }



        //
        // Method:      Open_DontSave_Handler
        // Description: Handles the DontSave event of the SavePrompt class when New_Click 
        //              has been invoked and the option not to Save has been selected
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        //
        private void Open_DontSave_Handler(object sender, RoutedEventArgs e) { 
            OpenFile();
        }



        //  
        // Method:      Close_Click
        // Description: Allows the user to close the program. Also checks
        //              for unsaved work and allows the user to save before closing.    
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        //
        private void Close_Click(object sender, RoutedEventArgs e) {
            // Check if there is any unsaved work or not
            if (this.UnsavedWork == true) {
                // Otherwise, we have to draw the close prompt to see if the user would like to save
                ClosePrompt Close = new ClosePrompt();

                // Link the events to their handlers
                Close.SaveWork += Close_SaveWork_Handler;
                Close.Quit += Close_Quit_Handler;

                // Show the dialog
                Close.ShowDialog();
            }
            // Otherwise just quit the program
            else { 
                this.Close();
            }        
        }



        //
        // Method:      Close_SaveWork_Handler
        // Description: Handles the DontSave event of the ClosePrompt class when Close_Click 
        //              has been invoked and the option to Save has been selected
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        //
        private void Close_SaveWork_Handler(object sender, RoutedEventArgs e) { 
            // Call the method to Save the file
            SaveFile();

            // If the work has been saved, quit the program
            if (this.UnsavedWork == false) { 
                this.Close();
            }
        }



        //
        // Method:      Close_Quit_Handler
        // Description: Handles the Quit event of the ClosePrompt class when Close_Click 
        //              has been invoked and the option not to Save has been selected
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        //
        private void Close_Quit_Handler(object sender, RoutedEventArgs e) { 
            // Simply quit the program
            this.Close();
        }



        //
        // Method:      BoldSelection_Click
        // Description: Changes the selected text to bold
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        // Source:      https://stackoverflow.com/questions/11874800/change-style-of-selected-text-in-richtextbox
        //
        private void BoldSelection_Click(object sender, RoutedEventArgs e) {
            TextSelection Selection = TypingArea.Selection;

            // Change the selected area to Bold
            TypingArea.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        }



        //
        // Method:      UnBoldSelection_Click
        // Description: Unbolds the selected text
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        // Source:      https://stackoverflow.com/questions/11874800/change-style-of-selected-text-in-richtextbox
        //
        private void UnboldSelection_Click(object sender, RoutedEventArgs e) {
            TextSelection Selection = TypingArea.Selection;
        
            // Unbold the selected area
            TypingArea.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Regular);
        }



        //
        // Method:      ItalicizeSelection_Click
        // Description: Changes the selected text to italics
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        // Source:      https://stackoverflow.com/questions/11874800/change-style-of-selected-text-in-richtextbox
        //
        private void ItalicizeSelection_Click(object sender, RoutedEventArgs e) { 
            TextSelection Selection = TypingArea.Selection;
        
            // Change the selected area to Italics
            TypingArea.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
        }
        


        //
        // Method:      UnItalicizeSelection_Click
        // Description: Unitalicize the selected text
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        // Source:      https://stackoverflow.com/questions/11874800/change-style-of-selected-text-in-richtextbox
        //
        private void UnitalicizeSelection_Click(object sender, RoutedEventArgs e) { 
            TextSelection Selection = TypingArea.Selection;

            // Unitalicize the selected area
            TypingArea.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
        }



        //
        // Method:      Arial_Click
        // Description: Changes the TextArea to Arial font
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        // Source:      https://stackoverflow.com/questions/674384/wpf-richtextbox-fontface-fontsize
        //
        private void Arial_Click(object sender, RoutedEventArgs e) { 
            // Take everything inside of the text box
            var RichText = new TextRange(TypingArea.Document.ContentStart, TypingArea.Document.ContentEnd);

            // Change the font to Arial
            RichText.ApplyPropertyValue(TextElement.FontFamilyProperty, "Arial");
        }



        //
        // Method:      Calibri_Click
        // Description: Changes the TextArea to Calibri font
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        // Source:      https://stackoverflow.com/questions/674384/wpf-richtextbox-fontface-fontsize
        //
        private void Calibri_Click(object sender, RoutedEventArgs e) { 
            // Take everything inside of the text box
            var RichText = new TextRange(TypingArea.Document.ContentStart, TypingArea.Document.ContentEnd);

            // Change the font to Calibri
            RichText.ApplyPropertyValue(TextElement.FontFamilyProperty, "Calibri");
        }



        //
        // Method:      TimesNewRoman_Click
        // Description: Changes the TextArea to Times New Roman font
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        // Source:      https://stackoverflow.com/questions/674384/wpf-richtextbox-fontface-fontsize
        //
        private void TimesNewRoman_Click(object sender, RoutedEventArgs e) { 
            // Take everything inside of the text box
            var RichText = new TextRange(TypingArea.Document.ContentStart, TypingArea.Document.ContentEnd);

            // Change the font to Times New Roman
            RichText.ApplyPropertyValue(TextElement.FontFamilyProperty, "Times New Roman");
        }



        //
        // Method:      Wingdings_Click
        // Description: Changes the TextArea to Wingdings font
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        // Source:      https://stackoverflow.com/questions/674384/wpf-richtextbox-fontface-fontsize
        //
        private void Wingdings_Click(object sender, RoutedEventArgs e) { 
            // Take everything inside of the text box
            var RichText = new TextRange(TypingArea.Document.ContentStart, TypingArea.Document.ContentEnd);

            // Change the font to Wingdings
            RichText.ApplyPropertyValue(TextElement.FontFamilyProperty, "Wingdings");
        }
    }
}