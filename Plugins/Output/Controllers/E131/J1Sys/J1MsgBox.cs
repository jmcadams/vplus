//=============================================================================
//
//	J1MsgBox - J1Sys's MessageBox Replacement
//
//		allows the message box to be aligned base on several options
//
//		includes an optional scrollable text area
//
//		version 1.0.0.0 - 1 june 2010
//
//=============================================================================

//=====================================================================
//
// Copyright (c) 2010 Joshua 1 Systems Inc. All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are
// permitted provided that the following conditions are met:
//
//    1. Redistributions of source code must retain the above copyright notice, this list of
//       conditions and the following disclaimer.
//
//    2. Redistributions in binary form must reproduce the above copyright notice, this list
//       of conditions and the following disclaimer in the documentation and/or other materials
//       provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY JOSHUA 1 SYSTEMS INC. "AS IS" AND ANY EXPRESS OR IMPLIED
// WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
// ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
// ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
// The views and conclusions contained in the software and documentation are those of the
// authors and should not be interpreted as representing official policies, either expressed
// or implied, of Joshua 1 Systems Inc.
//
//=====================================================================

using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Controllers.E131.J1Sys
{
	public class J1MsgBox : Form
	{
		static private	Icon	_messageBoxIconExclamation;
		static private	Icon	_messageBoxIconInformation;
		static private	Icon	_messageBoxIconQuestion;
		static private	Icon	_messageBoxIconStop;

		Icon		_msgIcon;
		Point		_msgIconPoint;
		string		_msgText;
		RectangleF	_msgTextRectF;
		TextBox		_msgScrollTextBox;


		public J1MsgBox()
		{
			InitBox();
		}

		private void InitBox()
		{
			Icon				= null;
			ControlBox			= false;
			MaximizeBox			= false;
			MinimizeBox			= false;
			StartPosition		= FormStartPosition.CenterScreen;

			Controls.Clear();

			_msgIcon				= null;
			_msgIconPoint		= new Point();
			_msgText				= "";
			_msgTextRectF		= new RectangleF();
			_msgScrollTextBox	= null;

			DialogResult		= DialogResult.None;
		}

		public static DialogResult ShowMsg(string text)
		{
			return ShowMsg(text, null, "", MessageBoxButtons.OK, MessageBoxIcon.None);
		}

		public static DialogResult ShowMsg(string text, string caption)
		{
			return ShowMsg(text, null, caption, MessageBoxButtons.OK, MessageBoxIcon.None);
		}

		public static DialogResult ShowMsg(string text, string caption, MessageBoxButtons mBB)
		{
			return ShowMsg(text, null, caption, mBB, MessageBoxIcon.None);
		}

		public static DialogResult ShowMsg(string text, string caption, MessageBoxButtons mBB, MessageBoxIcon mBI)
		{
			return ShowMsg(text, null, caption, mBB, mBI);
		}

		public static DialogResult ShowErrMsg(string text, string caption)
		{
			MessageBeepClass.MessageBeep(MessageBeepClass.BeepType.IconAsterisk);
			return ShowMsg(text, null, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static DialogResult ShowMsg(string text, string scrollText, string caption, MessageBoxButtons mBB, MessageBoxIcon mBI)
		{
			var	j1Mb = new J1MsgBox();

			return j1Mb.Show(text, scrollText, caption, mBB, mBI);
		}

		public DialogResult Show(string text)
		{
			return Show(text, null, "", MessageBoxButtons.OK, MessageBoxIcon.None);
		}

		public DialogResult Show(string text, string caption)
		{
			return Show(text, null, caption, MessageBoxButtons.OK, MessageBoxIcon.None);
		}

		public DialogResult Show(string text, string caption, MessageBoxButtons mBB)
		{
			return Show(text, null, caption, mBB, MessageBoxIcon.None);
		}

		public DialogResult Show(string text, string caption, MessageBoxButtons mBB, MessageBoxIcon mBI)
		{
			return Show(text, null, caption, mBB, mBI);
		}

		public DialogResult Show(string text, string scrollText, string caption, MessageBoxButtons mBB, MessageBoxIcon mBI)
		{
			Make(text, scrollText, caption, mBB, mBI);
			return ShowDialog();
		}

		public void Make(string text, string scrollText, string caption, MessageBoxButtons mBB, MessageBoxIcon mBI)
		{
			Graphics	grfx = CreateGraphics();
			SizeF		captionSizeF, textSizeF, scrollTextSizeF;
			Size		btnsSize;
			int			btns;
			Point		textPoint, btnsPoint;
			int			clientWidth, clientHeight, clientMaxWidth, clientMaxHeight, clientWhiteSpace;
			int			width, height;
			bool		hasIcon;
			Font		captionFont;


			SuspendLayout();
			
			// first re-init box to the defaults (ours)
			
			InitBox();

			// calculate the raw size of the caption
			
			captionFont  = new Font(SystemInformation.MenuFont, FontStyle.Bold);
			captionSizeF = grfx.MeasureString(caption, captionFont);

			// calculate the raw size of the text

			textSizeF = text.Length != 0 ? grfx.MeasureString(text, Font) : new SizeF(0,0);

			// set the msgIcon
			
			if (mBI != MessageBoxIcon.None) _msgIcon = LoadMessageBoxIcon(mBI);
			hasIcon = (_msgIcon != null);

			// if needed, create the textbox and add it to the form

			if (scrollText != null)
			{
				scrollTextSizeF = grfx.MeasureString(scrollText, Font);
				_msgScrollTextBox = new TextBox();
				_msgScrollTextBox.Text = scrollText;
				_msgScrollTextBox.Multiline = true;
				_msgScrollTextBox.ReadOnly = true;
				_msgScrollTextBox.Select(0,0);
				_msgScrollTextBox.ScrollBars = ScrollBars.Both;
				Controls.Add(_msgScrollTextBox);
			}

			else scrollTextSizeF = new SizeF(0,0);

			// create the button controls and add them to form

			switch (mBB)
			{
				case MessageBoxButtons.AbortRetryIgnore:
					AddBtn("Abort");
					AddBtn("Retry");
					AddBtn("Ignore");
					break;

				case MessageBoxButtons.OK:
					AddBtn("OK");
					break;

				case MessageBoxButtons.OKCancel:
					AddBtn("OK");
					AddBtn("Cancel");
					break;

				case MessageBoxButtons.RetryCancel:
					AddBtn("Retry");
					AddBtn("Cancel");
					break;

				case MessageBoxButtons.YesNo:
					AddBtn("Yes");
					AddBtn("No");
					break;

				case MessageBoxButtons.YesNoCancel:
					AddBtn("Yes");
					AddBtn("No");
					AddBtn("Cancel");
					break;
			}

			// calculate the buttons total size

			btnsSize = new Size();
			btns     = 0;
			
			foreach (Control control in Controls)
			{
				if (control.GetType() == typeof(Button))
				{
					btnsSize.Width += control.Size.Width;
					btnsSize.Height = control.Size.Height;
					btns++;
				}
			}

			if (btns > 0) btnsSize.Width += 10 * (btns - 1);

			// we've got all the sizes and made the buttons,
			// now figure clientsize, and our points of reference

			clientWidth			=   0;
			clientMaxWidth		= 600;
			clientMaxHeight		= 400;
			clientWhiteSpace	=  15;

			// first check the caption

			width = (int) captionSizeF.Width + 2 * clientWhiteSpace;
			if (width > clientWidth) clientWidth = width;

			// next check the icon/text combination

			width  = 0;
			if (hasIcon) width = _msgIcon.Size.Width + clientWhiteSpace;
			width += (int) textSizeF.Width + 1;
			if (width > clientWidth) clientWidth = width;

			// next check the scrollText

			width = (int) scrollTextSizeF.Width + 2 * clientWhiteSpace;
			if (width > clientWidth) clientWidth = width;

			// finally check the buttons

			width = btnsSize.Width;
			if (width > clientWidth) clientWidth = width;

			// check for max width
			if (clientWidth > clientMaxWidth) clientWidth = clientMaxWidth;

			width = clientWidth;
			if (hasIcon) width -= _msgIcon.Size.Width + clientWhiteSpace;
			if (width < textSizeF.Width) textSizeF = grfx.MeasureString(text, Font, width);

			// now calculate the height of client

			clientHeight = 0;
			if (hasIcon) clientHeight = _msgIcon.Size.Height;
			if (textSizeF.Height > clientHeight) clientHeight = (int) textSizeF.Height + 1;
			if (scrollText != null)
			{
				if (clientHeight > 0) clientHeight += clientWhiteSpace;
				clientHeight += (int) scrollTextSizeF.Height + clientWhiteSpace;
			}
			if (btns > 0)
			{
				if (clientHeight > 0) clientHeight += clientWhiteSpace;
				clientHeight += btnsSize.Height;
			}

			// check for max height
			if (clientHeight > clientMaxHeight) clientHeight = clientMaxHeight;

			ClientSize = new Size(clientWidth + 2 * clientWhiteSpace, clientHeight + 2 * clientWhiteSpace);
			
			_msgIconPoint = new Point(clientWhiteSpace, clientWhiteSpace);
			textPoint    = new Point(clientWhiteSpace, clientWhiteSpace);
			if (hasIcon)
			{
				if (_msgIcon.Size.Height > textSizeF.Height) textPoint.Y += (_msgIcon.Size.Height - (int) textSizeF.Height) / 2;
			
				textPoint.X += _msgIcon.Size.Width + clientWhiteSpace;
			}

			if (scrollText != null)
			{
				height = 0;
				if (hasIcon) height = _msgIcon.Size.Height;
				if ((int) textSizeF.Height > height) height = (int) textSizeF.Height;
				if (height > 0) height += clientWhiteSpace;
				height += clientWhiteSpace;

				_msgScrollTextBox.Location	= new Point(clientWhiteSpace, height);
				_msgScrollTextBox.Width		= ClientSize.Width - 2 * clientWhiteSpace;
				_msgScrollTextBox.Height		= ClientSize.Height - 2 * clientWhiteSpace - btnsSize.Height - height;
			}

			btnsPoint = new Point((ClientSize.Width - btnsSize.Width) / 2, ClientSize.Height - (btnsSize.Height + clientWhiteSpace));

			// set the results for the Form class and our OnPaint
			
			Text = caption;

			_msgText      = text;
			_msgTextRectF = _msgText.Length > 0 ? new RectangleF(textPoint, textSizeF) : new RectangleF(0,0,0,0);

			foreach (Control control in Controls.Cast<Control>().Where(control => control.GetType() == typeof(Button))) {
			    control.Location = btnsPoint;
			    btnsPoint.X += control.Size.Width + 10;
			}

			ResumeLayout();

			grfx.Dispose();
		}

		void AddBtn(string title)
		{
			var	btn = new Button {Text = @"&" + title, Name = title + "Btn"};

		    btn.Click += BtnClick;
			Controls.Add(btn);
		}

		void BtnClick(object sender, EventArgs e)
		{
			switch (((Button) sender).Name)
			{
				case "AbortBtn":
					DialogResult = DialogResult.Abort;
					break;

				case "CancelBtn":
					DialogResult = DialogResult.Cancel;
					break;

				case "IgnoreBtn":
					DialogResult = DialogResult.Ignore;
					break;

				case "NoBtn":
					DialogResult = DialogResult.No;
					break;

				case "OKBtn":
					DialogResult = DialogResult.OK;
					break;

				case "RetryBtn":
					DialogResult = DialogResult.Retry;
					break;

				case "YesBtn":
					DialogResult = DialogResult.Yes;
					break;
			}

			Close();
		}

		protected override void OnPaint(PaintEventArgs pEA)
		{
			base.OnPaint(pEA);

			if (_msgIcon != null)
			{
				pEA.Graphics.DrawIconUnstretched(_msgIcon, new Rectangle(_msgIconPoint, _msgIcon.Size));
			}

			if (_msgText.Length > 0)
			{
				pEA.Graphics.DrawString(_msgText, Font, Brushes.Black, _msgTextRectF);
			}
		}

		//---------------------------------------------------------------------
		//---------------------------------------------------------------------

		protected Icon LoadMessageBoxIcon(MessageBoxIcon mBI)
		{
			Assembly	assembly = Assembly.GetAssembly(typeof(J1MsgBox));
			Icon		icon     = null;

			string[]	decoratedNames = assembly.GetManifestResourceNames();
			
			switch (mBI)
			{
				case MessageBoxIcon.Exclamation:
					if (_messageBoxIconExclamation == null)
					{
						foreach (string decoratedName in decoratedNames)
						{
							if (decoratedName.EndsWith(".MessageBoxIcons.exclamation.ico"))
							{
								_messageBoxIconExclamation = new Icon(assembly.GetManifestResourceStream(decoratedName));
								break;
							}
						}
					}
					icon = _messageBoxIconExclamation;
					break;

				case MessageBoxIcon.Information:
					if (_messageBoxIconInformation == null)
					{
						foreach (string decoratedName in decoratedNames)
						{
							if (decoratedName.EndsWith(".MessageBoxIcons.information.ico"))
							{
								_messageBoxIconInformation = new Icon(assembly.GetManifestResourceStream(decoratedName));
								break;
							}
						}
					}
					icon = _messageBoxIconInformation;
					break;

				case MessageBoxIcon.Question:
					if (_messageBoxIconQuestion    == null)
					{
						foreach (string decoratedName in decoratedNames)
						{
							if (decoratedName.EndsWith(".MessageBoxIcons.question.ico"))
							{
								_messageBoxIconQuestion = new Icon(assembly.GetManifestResourceStream(decoratedName));
								break;
							}
						}
					}
					icon = _messageBoxIconQuestion;
					break;

				case MessageBoxIcon.Stop:
					if (_messageBoxIconStop        == null)
					{
						foreach (string decoratedName in decoratedNames)
						{
							if (decoratedName.EndsWith(".MessageBoxIcons.stop.ico"))
							{
								_messageBoxIconStop = new Icon(assembly.GetManifestResourceStream(decoratedName));
								break;
							}
						}
					}
					icon = _messageBoxIconStop;
					break;
			}

			return icon;
		}
	}
}
