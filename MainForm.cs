using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using DropShadow;

using File_Installer.Properties;

namespace File_Installer
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			DropShadowCS shadow = new DropShadowCS();
			shadow.CreateWNDParams(CreateParams);
			shadow.CreateDropShadow(this);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			Invalidate();
		}
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			base.OnResize(e);
		}

		#region Pre-Made Window Handling Code | DO NOT TOUCH/MODIFY
		[DllImport("user32")]
		private static extern bool ReleaseCapture();

		[DllImport("user32")]
		private static extern int SendMessage(IntPtr handle, int msg, int wp, int lp);

		private bool isHovering = false;
		private FormWindowState prevState;

		private void ChangeImage(PictureBox picBox, Image newImage)
		{
			picBox.Image = newImage;
		}
		private void ResizeWindow(int htborder)
		{
			ReleaseCapture();
			SendMessage(Handle, 161, htborder, 0);
		}
		private void close_Click(object sender, EventArgs e)
		{
			Timer t = new Timer { Interval = 1 };
			t.Tick += delegate (object sender_, EventArgs e_)
			{
				Opacity -= .025d;
				if (Opacity <= 0)
				{
					t.Enabled = false;
					Close();
				}
			};
			t.Start();
		}
		private void maxres_Click(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Maximized)
			{
				Timer t = new Timer { Interval = 1 };
				t.Tick += delegate (object sender_, EventArgs e_)
				{
					Opacity -= .025d;
					if (Opacity <= 0)
					{
						t.Enabled = false;
						WindowState = FormWindowState.Normal;
						Timer t_ = new Timer { Interval = 1 };
						t_.Tick += delegate (object sender__, EventArgs e__)
						{
							Opacity += .025d;
							if (Opacity >= 1)
							{
								t_.Enabled = false;
								Opacity = 1;
								prevState = WindowState;
							}
						};
						t_.Start();
					}
				};
				t.Start();
			}
			else if (WindowState == FormWindowState.Normal)
			{
				Timer t = new Timer { Interval = 1 };
				t.Tick += delegate (object sender_, EventArgs e_)
				{
					Opacity -= .025d;
					if (Opacity <= 0)
					{
						t.Enabled = false;
						WindowState = FormWindowState.Maximized;
						Timer t_ = new Timer { Interval = 1 };
						t_.Tick += delegate (object sender__, EventArgs e__)
						{
							Opacity += .025d;
							if (Opacity >= 1)
							{
								t_.Enabled = false;
								Opacity = 1;
								prevState = WindowState;
							}
						};
						t_.Start();
					}
				};
				t.Start();
			}
		}
		private void minimize_Click(object sender, EventArgs e)
		{
			Timer t = new Timer { Interval = 1 };
			t.Tick += delegate (object sender_, EventArgs e_)
			{
				Opacity -= .025d;
				if (Opacity <= 0)
				{
					t.Enabled = false;
					WindowState = FormWindowState.Minimized;
				}
			};
			t.Start();
		}
		private void close_MouseEnter(object sender, EventArgs e)
		{
			ChangeImage(close, Resources.close_active);
		}
		private void maxres_MouseEnter(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Maximized)
			{
				ChangeImage(maxres, Resources.maximize_active);
				isHovering = true;
			}
			else if (WindowState == FormWindowState.Normal)
			{
				ChangeImage(maxres, Resources.maximize_active);
				isHovering = true;
			}
		}
		private void minimize_MouseEnter(object sender, EventArgs e)
		{
			ChangeImage(minimize, Resources.minimize_active);
		}
		private void close_MouseLeave(object sender, EventArgs e)
		{
			ChangeImage(close, Resources.close_inactive);
		}
		private void maxres_MouseLeave(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Maximized)
			{
				ChangeImage(maxres, Resources.maximize_inactive);
				isHovering = false;
			}
			else if (WindowState == FormWindowState.Normal)
			{
				ChangeImage(maxres, Resources.maximize_inactive);
				isHovering = false;
			}
		}
		private void minimize_MouseLeave(object sender, EventArgs e)
		{
			ChangeImage(minimize, Resources.minimize_inactive);
		}
		private void Form1_Activated(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized) Opacity = 0;
			Timer t = new Timer { Interval = 1 };
			t.Tick += delegate (object sender_, EventArgs e_)
			{
				Opacity += .025d;
				WindowState = prevState;
				if (Opacity >= 1)
				{
					t.Enabled = false;
					Opacity = 1;
				}
			};
			t.Start();
		}
		private void Form1_Shown(object sender, EventArgs e)
		{
			Opacity = 0;
			Timer t = new Timer { Interval = 1 };
			t.Tick += delegate (object sender_, EventArgs e_)
			{
				Opacity += .025d;
				if (Opacity >= 1)
				{
					t.Enabled = false;
					Opacity = 1;
				}
			};
			t.Start();
		}
		protected sealed override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			switch (WindowState)
			{
				case FormWindowState.Maximized:
					if (isHovering == true) maxres.Image = Resources.maximize_active;
					if (isHovering == false) maxres.Image = Resources.maximize_inactive;
					break;
				case FormWindowState.Normal:
					if (isHovering == true) maxres.Image = Resources.maximize_active;
					if (isHovering == false) maxres.Image = Resources.maximize_inactive;
					break;
			}
		}

		private void ResizeLeft(object sender, MouseEventArgs e)
		{
			ResizeWindow(10);
		}
		private void ResizeRight(object sender, MouseEventArgs e)
		{
			ResizeWindow(11);
		}
		private void ResizeBottom(object sender, MouseEventArgs e)
		{
			ResizeWindow(15);
		}
		private void ResizeTop(object sender, MouseEventArgs e)
		{
			ResizeWindow(12);
		}
		private void ResizeTopLeft(object sender, MouseEventArgs e)
		{
			ResizeWindow(13);
		}
		private void ResizeTopRight(object sender, MouseEventArgs e)
		{
			ResizeWindow(14);
		}
		private void ResizeBottomLeft(object sender, MouseEventArgs e)
		{
			ResizeWindow(16);
		}
		private void ResizeBottomRight(object sender, MouseEventArgs e)
		{
			ResizeWindow(17);
		}
		private void DragForm(object sender, MouseEventArgs e)
		{
			ReleaseCapture();
			SendMessage(Handle, 161, 2, 0);
		}
		#endregion

		#region File Installer
		private void fileinst_sourceselect_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog fbd = new FolderBrowserDialog
			{
				Description = "Please select the folder containing your files...",
				ShowNewFolderButton = false,
			})
			{
				if (fbd.ShowDialog() == DialogResult.OK)
				{
					fileinst_source.Text = fbd.SelectedPath;
				}
			}
		}
		private void fileinst_destinationselect_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog fbd = new FolderBrowserDialog
			{
				Description = "Please select the folder where your files will be installed...",
				ShowNewFolderButton = false,
			})
			{
				if (fbd.ShowDialog() == DialogResult.OK)
				{
					fileinst_destination.Text = fbd.SelectedPath;
				}
			}
		}
		private void fileinst_Confirm(object sender, EventArgs e)
		{
			Timer t1 = new Timer { Interval = 32 };
			Timer t2 = new Timer { Interval = 1000 };
			Timer t3 = new Timer { Interval = 1000 };

			t1.Tick += delegate (object sender_, EventArgs e_)
			{
				progress.Value += 1;
				if (progress.Value >= 100)
				{
					t1.Enabled = false;
					progress.Value = 0;
					progressStatus.Text = "Waiting for confirmation...";
				}
			};
			t2.Tick += delegate (object sender_, EventArgs e_)
			{
				progressStatus.Text = "Searching for files...";

				// get each Path.GetFileName(file) and folder in the source
				string[] files = Directory.GetFiles(fileinst_source.Text);
				string[] folders = Directory.GetDirectories(fileinst_source.Text);

				t2.Enabled = false;
				t3.Tick += delegate (object sender__, EventArgs e__)
				{
					progressStatus.Text = "Transferring files...";

					// transfer (copy & paste) each Path.GetFileName(file) and folder to the destination folder/directory
					foreach (string file in files)
					{
						if (deletefiles.Checked)
						{
							if (putinfolder.Checked)
							{
								Directory.CreateDirectory($"{fileinst_destination.Text}\\Installed Content");
								File.Copy(file, $"{fileinst_destination.Text}\\Installed Content{"\\"}{Path.GetFileName(file)}");
								File.Delete(file);
							}
							else
							{
								File.Copy(file, $"{fileinst_destination.Text}\\{Path.GetFileName(file)}");
								File.Delete(file);
							}
						}
						else
						{
							if (putinfolder.Checked)
							{
								Directory.CreateDirectory($"{fileinst_destination.Text}\\Installed Content");
								File.Copy(file, $"{fileinst_destination.Text}\\Installed Content{"\\"}{Path.GetFileName(file)}");
							}
							else
							{
								File.Copy(file, $"{fileinst_destination.Text}\\{Path.GetFileName(file)}");
							}
						}
					}
					foreach (string folder in folders)
					{
						if (deletefiles.Checked)
						{
							// get each Path.GetFileName(file) and folder in the folders
							string[] files_ = Directory.GetFiles(folder);
							string[] folders_ = Directory.GetDirectories(folder);

							// create a new folder in the destination folder
							if (putinfolder.Checked) Directory.CreateDirectory($"{fileinst_destination.Text}\\Installed Content");

							// transfer the content into a folder named after the original folder that will be deleted
							foreach (string file_ in files_)
							{
								if (putinfolder.Checked)
								{
									File.Copy(file_, $"{fileinst_destination.Text}\\Installed Content{"\\"}{Path.GetFileName(file_)}");
									File.Delete(file_);
								}
								else
								{
									File.Copy(file_, $"{fileinst_destination.Text}\\{Path.GetFileName(file_)}");
									File.Delete(file_);
								}
							}
							foreach (string folder_ in folders_)
							{
								if (putinfolder.Checked)
								{
									Directory.Move(folder_, $"{fileinst_destination.Text}\\Installed Content{"\\"}{folder_}");
								}
								else
								{
									Directory.Move(folder_, $"{fileinst_destination.Text}\\{folder_}");
								}
							}

							Directory.Delete(folder);
						}
						else
						{
							// get each Path.GetFileName(file) and folder in the folders
							string[] files_ = Directory.GetFiles(folder);
							string[] folders_ = Directory.GetDirectories(folder);

							// create a new folder in the destination folder
							if (putinfolder.Checked) Directory.CreateDirectory($"{fileinst_destination.Text}\\Installed Content");

							// transfer the content into a folder named after the original folder that will be deleted
							foreach (string file_ in files_)
							{
								if (putinfolder.Checked)
								{
									File.Copy(file_, $"{fileinst_destination.Text}\\Installed Content{"\\"}{Path.GetFileName(file_)}");
								}
								else
								{
									File.Copy(file_, $"{fileinst_destination.Text}\\{Path.GetFileName(file_)}");
								}
							}
							foreach (string folder_ in folders_)
							{
								if (putinfolder.Checked)
								{
									Directory.Move(folder_, $"{fileinst_destination.Text}\\Installed Content{"\\"}{folder_}");
								}
								else
								{
									Directory.Move(folder_, $"{fileinst_destination.Text}\\{folder_}");
								}
							}
						}
					}
					t3.Enabled = false;
				};
				t3.Start();
			};
			t1.Start();
			t2.Start();
		}
		#endregion
		#region ZIP Installer
		private void confirm_Click(object sender, EventArgs e)
		{
			Timer t1 = new Timer { Interval = 32 };
			Timer t2 = new Timer { Interval = 1000 };
			Timer t3 = new Timer { Interval = 1000 };

			t1.Tick += delegate (object sender_, EventArgs e_)
			{
				zip_progress.Value += 1;
				if (zip_progress.Value >= 100)
				{
					t1.Enabled = false;
					zip_progress.Value = 0;
					progStat.Text = "Waiting for confirmation...";
				}
			};
			t2.Tick += delegate (object sender_, EventArgs e_)
			{
				progStat.Text = "Getting files...";

				// get the path of the archive
				string archive = archivesource.Text;

				// get the installation path
				string installPath = archivedestination.Text;

				t2.Enabled = false;
				t3.Tick += delegate (object sender__, EventArgs e__)
				{
					progStat.Text = "Transferring files...";

					// extract and install the archive contents
					if (deletearchive.Checked)
					{
						if (putarchivecontentsinfolder.Checked)
						{
							// create a folder for the contents
							Directory.CreateDirectory($"{archive}\\{Path.GetFileNameWithoutExtension(archive)}");
							string newPath = $"{archive}\\{Path.GetFileNameWithoutExtension(archive)}";

							ZipFile.ExtractToDirectory(archive, $"{installPath}\\{newPath}");
						}
						else
						{
							ZipFile.ExtractToDirectory(archive, installPath);
						}
					}
					else
					{
						if (putarchivecontentsinfolder.Checked)
						{
							// create a folder for the contents
							Directory.CreateDirectory($"{archive}\\{Path.GetFileNameWithoutExtension(archive)}");
							string newPath = $"{archive}\\{Path.GetFileNameWithoutExtension(archive)}";

							ZipFile.ExtractToDirectory(archive, $"{installPath}\\{newPath}");

							File.Delete(archive);
						}
						else
						{
							ZipFile.ExtractToDirectory(archive, installPath);

							File.Delete(archive);
						}
					}
					t3.Enabled = false;
				};
			};
		}
		private void more1_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog ofd = new OpenFileDialog
			{
				Title = "Please select the archive to extract...",
				Filter = "ZIP Archive|*.zip;",
				FilterIndex = 0,
			})
			{
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					archivesource.Text = ofd.FileName;
				}
			}
		}
		private void more2_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog fbd = new FolderBrowserDialog
			{
				Description = "Please select the folder in which you will install your content...",
				ShowNewFolderButton = false,
			})
			{
				if (fbd.ShowDialog() == DialogResult.OK)
				{
					archivedestination.Text = fbd.SelectedPath;
				}
			}
		}
		#endregion
		#region UI Selection
		private void materialCard1_Click(object sender, EventArgs e)
		{
			fileinstaller_ui.BringToFront();
			zipinstaller_ui.SendToBack();
			materialLabel1.Text = "File Installer - UI Selected: File Installer";
		}
		private void materialCard2_Click(object sender, EventArgs e)
		{
			zipinstaller_ui.BringToFront();
			fileinstaller_ui.SendToBack();
			materialLabel1.Text = "File Installer - UI Selected: ZIP Installer";
		}
		#endregion
	}
}
