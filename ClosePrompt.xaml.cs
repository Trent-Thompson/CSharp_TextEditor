// 
// File:		ClosePrompt.xaml.cs
// Programmer:	Trent Thompson
// Date:		September 27, 2019
// Description:	This file contains the code implementation for the ClosePrompt window       
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

    public partial class ClosePrompt : Window {
        // Events
        public event RoutedEventHandler SaveWork;
        public event RoutedEventHandler Quit;

        //
        // Method:      ClosePrompt - Constructor
        // Description: Constructor for the ClosePrompt window
        // Parameters:  none
        // Returns:     none
        //
        public ClosePrompt() {
            InitializeComponent();
        }



        //
        // Method:      SaveAndCloseBtn_Click
        // Description: Closes the ClosePrompt window and triggers the SaveWork event
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        //
        private void SaveAndCloseBtn_Click(object sender, RoutedEventArgs e) {
            // Close the window
            this.Close();

            // Trigger the SaveWork event event
            if (this.SaveWork != null) { 
                this.SaveWork(this, new RoutedEventArgs());
            }
        }



        //
        // Method:      CloseWithoutSavingBtn_Click
        // Description: Closes the ClosePrompt window and triggers the Quit event
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        //
        private void CloseWithoutSavingBtn_Click(object sender, RoutedEventArgs e) {
            // Close the window
            this.Close();

            // Trigger the Quit event
            if (this.Quit != null) { 
                this.Quit(this, new RoutedEventArgs());
            }
        }



        //
        // Method:      CancelBtn_Click
        // Description: Simply closes the ClosePrompt
        // Parameters:  object sender, RoutedEventArgs e
        // Returns:     void
        //
        private void CancelBtn_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
