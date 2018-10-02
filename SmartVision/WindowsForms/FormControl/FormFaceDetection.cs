﻿using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WindowsForms.FaceAnalysis;
using WindowsForms.Persons;

namespace WindowsForms.FormControl
{
    public partial class FormFaceDetection : Form
    {
        private bool cameraEnabled = false;
        public static FormFaceDetection Current { get; private set; }

        public FormFaceDetection()
        {
            InitializeComponent();
            Current = this;
        }

        private void FormFaceDetection_Load(object sender, EventArgs e)
        {
            homePanel.BringToFront();
        }

        /// <summary>
        /// Main buttons - switching through panels, exiting application
        /// Author: Tomas Drasutis
        /// </summary>
        public void homeButton_Click(object sender, EventArgs e)
        {
            if (cameraEnabled)
            {
                WebcamInput.DisableWebcam();
                cameraEnabled = false;
            }

            homePanel.BringToFront();
        }

        private void scanButton_Click(object sender, EventArgs e)
        {
            if (!cameraEnabled && WebcamInput.EnableWebcam())
            {
                scanPanel.BringToFront();
                cameraEnabled = true;
            }

            else
            {
                homePanel.BringToFront();
            }
        }

        private void addPersonButton_Click(object sender, EventArgs e)
        {
            if (cameraEnabled)
            {
                WebcamInput.DisableWebcam();
                cameraEnabled = false;
            }

            addPersonPanel.BringToFront();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Gets information about the missing person
        /// Author: Tomas Drasutis
        /// </summary>
        private void addMissingPersonButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Verify that properties are valid
                if(!Regex.IsMatch(firstNameBox.Text,@"^[a-zA-Z]+$") || firstNameBox.Text.Equals(""))
                {
                    MessageBox.Show("Missing person First name should contain only letters and cannot be empty!");
                    return;
                }

                if(!Regex.IsMatch(lastNameBox.Text,@"^[a-zA-Z]+$") || lastNameBox.Text.Equals(""))
                {
                    MessageBox.Show("Missing person Last name should contain only letters and cannot be empty!");
                    return;
                }

                if(!Regex.IsMatch(contactFirstNameBox.Text,@"^[a-zA-Z]+$") || contactFirstNameBox.Text.Equals(""))
                {
                    MessageBox.Show("Contact person First name should contain only letters and cannot be empty!");
                    return;
                }

                if(!Regex.IsMatch(contactLastNameBox.Text,@"^[a-zA-Z]+$") || contactLastNameBox.Text.Equals(""))
                {
                    MessageBox.Show("Contact person Last name should contain only letters and cannot be empty!");
                    return;
                }

                if(!Regex.IsMatch(contactPhoneNumberBox.Text,@"^[+][0-9]+$") || contactPhoneNumberBox.Text.Equals(""))
                {
                    MessageBox.Show("Contact person Phone number should be in a valid format(+[Country code]00...00) and cannot be empty!");
                    return;
                }

                if(!Regex.IsMatch(contactEmailAddressBox.Text,@"^([\w\.]+)@([\w]+)((\.(\w){2,3})+)$") || contactEmailAddressBox.Text.Equals(""))
                {
                    MessageBox.Show("Contact person Email address should be in a valid format (foo@bar.baz) and cannot be empty!");
                    return;
                }

                Bitmap missingPersonImage = new Bitmap(missingPersonPictureBox.Image);
                MissingPerson missingPerson = new MissingPerson(firstNameBox.Text, lastNameBox.Text, additionalInfoBox.Text, locationBox.Text, dateOfBirthPicker.Value, lastSeenOnPicker.Value, missingPersonImage);
                ContactPerson contactPerson = new ContactPerson(contactFirstNameBox.Text, contactLastNameBox.Text, contactPhoneNumberBox.Text, contactEmailAddressBox.Text);

                if (FaceDetection.FaceDetectionFromPicture(missingPersonImage))
                {
                    //to put to database
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

        }

        /// <summary>
        /// Uploads the picture
        /// Author: Tomas Drasutis
        /// </summary>
        private void uploadButton_Click(object sender, EventArgs e)
        {
            ImageUpload.UploadImage();
        }

        /// <summary>
        /// Dispose the input of the webcam when form is closed
        /// Author: Deividas Brazenas
        /// </summary>
        private void FormFaceDetection_FormClosing(object sender, FormClosedEventArgs e)
        {
            try
            {
                WebcamInput.DisableWebcam();
                FaceDetection.DisableFaceDetection();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}