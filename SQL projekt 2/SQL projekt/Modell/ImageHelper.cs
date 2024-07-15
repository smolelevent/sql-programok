using System;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Data.Sql;
using SQL_projekt.Class;

namespace SQL_projekt.Modell
{
    internal class ImageHelper
    {
        private dBHelper helper = null;
        private string fileLocation = string.Empty;
        private bool isSucces = false;
        private int maxImageSize = 2097152;

        private string FileLocation
        {
            get { return fileLocation; }
            set
            {
                fileLocation = value;
            }
        }

        public Boolean GetSucces()
        {
            return isSucces;
        }

        private Image LoadImage()
        {
            Image image = null;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = @"C:\\";
            dlg.Title = "Select Image File";
            dlg.Filter = "Image Files (*.jpg ; *.jpeg ; *.png ; *.gif ; *.tiff ; *.nef)| *.jpg; *.jpeg; *.png; *.gif; *.tiff; *.nef";
            dlg.ShowDialog();
            this.FileLocation = dlg.FileName;
            if (fileLocation == null || fileLocation == string.Empty)
                return image;
            if (FileLocation != string.Empty && fileLocation != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                FileInfo info = new FileInfo(FileLocation);
                long fileSize = info.Length;
                maxImageSize = (Int32)fileSize;
                if (File.Exists(FileLocation))
                {
                    using (FileStream stream = File.Open(FileLocation, FileMode.Open))
                    {
                        BinaryReader br = new BinaryReader(stream);
                        byte[] data = br.ReadBytes(maxImageSize);
                        image = new Image(dlg.SafeFileName, data, fileSize);
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            return image;
        }

        public Int32 InsertImage()
        {
            DataRow dataRow = null;
            isSucces = false;
            Image image = LoadImage();
            if (image == null) return 0;
            if (image != null)
            {
                string connectionString = dBFunctions.ConnectionStringSQLite;
                string commandText = "SELECT * FROM ImageStore WHERE 1=0";
                helper = new dBHelper(connectionString);
                {
                    if (helper.Load(commandText, "image_id") == true)
                    {
                        helper.DataSet.Tables[0].Rows.Add(
                        helper.DataSet.Tables[0].NewRow());
                        dataRow = helper.DataSet.Tables[0].Rows[0];
                        dataRow["imageFileName"] = image.FileName;
                        dataRow["imageBlob"] = image.ImageData;
                        dataRow["imageFileSizeBytes"] = image.FileSize;
                        try
                        {
                            if (helper.Save() == true)
                            {
                                isSucces = true;
                            }
                            else
                            {
                                isSucces = false;
                                MessageBox.Show("Error during Insertion");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            return Convert.ToInt32(dataRow[0].ToString());
        }


        public void DeleteImage(Int32 imageID)
        {
            isSucces = false;
            string connectionString = dBFunctions.ConnectionStringSQLite;
            string commandText = "SELECT * FROM ImageStore WHERE image_id=" + imageID;
            helper = new dBHelper(connectionString);
            {
                if (helper.Load(commandText, "image_id") == true)
                {
                    if (helper.DataSet.Tables[0].Rows.Count == 1)
                    {
                        helper.DataSet.Tables[0].Rows[0].Delete();
                        try
                        {
                            if (helper.Save() == true)
                            {
                                isSucces = true;
                            }
                            else
                            {
                                isSucces = false;
                                MessageBox.Show("Delete failed");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }


        public void SaveAsImage(Int32 imageID)
        {
            DataRow dataRow = null;
            Image image = null;
            isSucces = false;
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.InitialDirectory = @"C:\\";
            dlg.Title = "Save Image File";
            dlg.Filter = "Tag Image File Format (*.tiff)|*.tiff";
            dlg.Filter += "|Graphics Interchange Format (*.gif)|*.gif";
            dlg.Filter += "|Portable Network Graphic Format (*.png)|*.png";
            dlg.Filter += "|Joint Photographic Experts Group Format (*.jpg)|*.jpg";
            dlg.Filter += "|Joint Photographic Experts Group Format (*.jpeg)|*.jpeg";
            dlg.Filter += "|Bitmap Image File Format (*.bmp)|*.bmp";
            dlg.Filter += "|Nikon Electronic Format (*.nef)|*.nef";
            dlg.ShowDialog();
            if (dlg.FileName != "")
            {
                Cursor.Current = Cursors.WaitCursor;
                string defaultExt = ".png";
                int pos = -1;
                string[] ext = new string[7] {".tiff", ".gif", ".png", ".jpg", ".jpeg", ".bmp", ".nef"};
                string extFound = string.Empty;
                string filename = dlg.FileName.Trim();
                for (int i = 0; i < ext.Length; i++)
                {
                    pos = filename.IndexOf(ext[i], pos + 1);
                    if (pos > -1)
                    {
                        extFound = ext[i];
                        break;
                    }
                }
                if (extFound == string.Empty) filename = filename + defaultExt;
                string connectionString = dBFunctions.ConnectionStringSQLite;
                string commandText = "SELECT * FROM ImageStore WHERE image_id=" + imageID;
                helper = new dBHelper(connectionString);
                if (helper.Load(commandText, "") == true)
                {
                    dataRow = helper.DataSet.Tables[0].Rows[0];
                    image = new Image((string)dataRow["imageFileName"], (byte[])dataRow["imageBlob"], (long)dataRow["imageFileSizeBytes"]);
                    method.using(FileStream stream = new FileStream(filename, FileMode.Create))
                    {
                        BinaryWriter bw = new BinaryWriter(stream);
                        bw.Write(image.ImageData);
                        isSucces = true;
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            if (isSucces)
            {
                MessageBox.Show("Save succesfull");
            }
            else
            {
                MessageBox.Show("Save failed");
            }
        }



    }
}
