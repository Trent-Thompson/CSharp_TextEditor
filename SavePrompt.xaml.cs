// 
// File:		SavePrompt.xaml.cs
// Programmer:	Trent Thompson
// Date:		September 27, 2019
// Description:	This file contains the code implementation for the SavePrompt window.
//



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

namespace WMP_2 {

    public partial class SavePrompt : Window {
        // Events
        public event RoutedEventHandler SaveWork;
        public event RoutedEventHandler DontSave;



        //
        // Method:      SavePrompt - Constructor
        // Description: Constructor to initialize the SavePrompt window
        // Parameters:  none
        // Returns:     none
        //
        public SavePrompt() {
            InitializeComponent();
        }



        //
        // Method:      YesBtn_Click
        // Description: Allows the user to save their work upon a yes button press
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        //
        private void YesBtn_Click(object sender, RoutedEventArgs e) {
            // Close the save prompt
            this.Close();

            // Trigger the SaveWork event
            if (this.SaveWork != null) { 
                this.SaveWork(this, new RoutedEventArgs());
            }
        }



        //
        // Method:      NoBtn_Click
        // Description: Simply closes the SavePrompt box
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        //
        private void NoBtn_Click(object sender, RoutedEventArgs e) {
            // If the user selects that they don't want to save, just close the prompt
            this.Close();

            // Trigger the DontSave event
            if (this.DontSave != null) { 
                this.DontSave(this, new RoutedEventArgs());
            }
        }
    }
}
