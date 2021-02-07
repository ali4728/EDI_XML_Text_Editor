﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ScintillaNET;
using ScintillaNET.Demo.Utils;

namespace ScintillaNET.Demo {
	public partial class MainForm : Form {
		public MainForm() {
			InitializeComponent();
		}

		ScintillaNET.Scintilla TextArea;

		private void MainForm_Load(object sender, EventArgs e) {

			// CREATE CONTROL
			TextArea = new ScintillaNET.Scintilla();
			TextPanel.Controls.Add(TextArea);

			// BASIC CONFIG
			TextArea.Dock = System.Windows.Forms.DockStyle.Fill;
			TextArea.TextChanged += (this.OnTextChanged);

			// INITIAL VIEW CONFIG
			TextArea.WrapMode = WrapMode.None;
			TextArea.IndentationGuides = IndentView.LookBoth;

			// STYLING
			InitColors();
			//InitSyntaxColoring();

			// NUMBER MARGIN
			InitNumberMargin();
			InitSyntaxColoringPlain();


			// BOOKMARK MARGIN
			InitBookmarkMargin();

			// CODE FOLDING MARGIN
			//InitCodeFolding();

			// DRAG DROP
			InitDragDropFile();

			// DEFAULT FILE
			//LoadDataFromFile("../../MainForm.cs");

			// INIT HOTKEYS
			InitHotkeys();

		}

		private void InitColors() {

			TextArea.SetSelectionBackColor(true, IntToColor(0xE8CAB3));
			//0xE8CAB3 = light solmon  0x114D9C = blue
		}

		private void InitHotkeys() {

			// register the hotkeys with the form
			HotKeyManager.AddHotKey(this, OpenSearch, Keys.F, true);
			HotKeyManager.AddHotKey(this, OpenFindDialog, Keys.F, true, false, true);
			HotKeyManager.AddHotKey(this, OpenReplaceDialog, Keys.R, true);
			HotKeyManager.AddHotKey(this, OpenReplaceDialog, Keys.H, true);
			HotKeyManager.AddHotKey(this, Uppercase, Keys.U, true);
			HotKeyManager.AddHotKey(this, Lowercase, Keys.L, true);
			HotKeyManager.AddHotKey(this, ZoomIn, Keys.Oemplus, true);
			HotKeyManager.AddHotKey(this, ZoomOut, Keys.OemMinus, true);
			HotKeyManager.AddHotKey(this, ZoomDefault, Keys.D0, true);
			HotKeyManager.AddHotKey(this, CloseSearch, Keys.Escape);
			HotKeyManager.AddHotKey(this, UpdateClip, Keys.D9, true);
			HotKeyManager.AddHotKey(this, unWrapXMLShortcut, Keys.D8, true);
			HotKeyManager.AddHotKey(this, buttonRightShortcut, Keys.M, true, false, true);
			HotKeyManager.AddHotKey(this, buttonLeftShortcut, Keys.N, true, false, true);
			
			// remove conflicting hotkeys from scintilla
			TextArea.ClearCmdKey(Keys.Control | Keys.F);
			TextArea.ClearCmdKey(Keys.Control | Keys.R);
			TextArea.ClearCmdKey(Keys.Control | Keys.H);
			TextArea.ClearCmdKey(Keys.Control | Keys.L);
			TextArea.ClearCmdKey(Keys.Control | Keys.U);

		}

		private void InitSyntaxColoringSQL()
		{

			TextArea.StyleResetDefault();
			TextArea.Styles[Style.Default].Font = "Consolas";
			TextArea.Styles[Style.Default].Size = 11;
			TextArea.StyleClearAll();

			// Set the SQL Lexer
			TextArea.Lexer = Lexer.Sql;

			// Show line numbers
			//TextArea.Margins[0].Width = 20;


			//TextArea.SetFoldMarginColor(true, IntToColor(FORE_COLOR));
			//TextArea.SetFoldMarginHighlightColor(true, IntToColor(FORE_COLOR));


			//TextArea.Styles[Style.LineNumber].ForeColor = Color.FromArgb(255, 128, 128, 128);  //Dark Gray
			//TextArea.Styles[Style.LineNumber].BackColor = Color.FromArgb(255, 228, 228, 228);  //Light Gray
			TextArea.Styles[Style.Sql.Comment].ForeColor = Color.Green;
			TextArea.Styles[Style.Sql.CommentLine].ForeColor = Color.Green;
			TextArea.Styles[Style.Sql.CommentLineDoc].ForeColor = Color.Green;
			TextArea.Styles[Style.Sql.Number].ForeColor = Color.Maroon;
			TextArea.Styles[Style.Sql.Word].ForeColor = Color.Blue;
			TextArea.Styles[Style.Sql.Word2].ForeColor = Color.Fuchsia;
			TextArea.Styles[Style.Sql.User1].ForeColor = Color.Gray;
			TextArea.Styles[Style.Sql.User2].ForeColor = Color.FromArgb(255, 00, 128, 192);    //Medium Blue-Green
			TextArea.Styles[Style.Sql.String].ForeColor = Color.Red;
			TextArea.Styles[Style.Sql.Character].ForeColor = Color.Red;
			TextArea.Styles[Style.Sql.Operator].ForeColor = Color.Black;

			// Set keyword lists
			// Word = 0
			TextArea.SetKeywords(0, @"add alter as authorization backup begin bigint binary bit break browse bulk by cascade case catch check checkpoint close clustered column commit compute constraint containstable continue create current cursor cursor database date datetime datetime2 datetimeoffset dbcc deallocate decimal declare default delete deny desc disk distinct distributed double drop dump else end errlvl escape except exec execute exit external fetch file fillfactor float for foreign freetext freetexttable from full function goto grant group having hierarchyid holdlock identity identity_insert identitycol if image index insert int intersect into key kill lineno load merge money national nchar nocheck nocount nolock nonclustered ntext numeric nvarchar of off offsets on open opendatasource openquery openrowset openxml option order over percent plan precision primary print proc procedure public raiserror read readtext real reconfigure references replication restore restrict return revert revoke rollback rowcount rowguidcol rule save schema securityaudit select set setuser shutdown smalldatetime smallint smallmoney sql_variant statistics table table tablesample text textsize then time timestamp tinyint to top tran transaction trigger truncate try union unique uniqueidentifier update updatetext use user values varbinary varchar varying view waitfor when where while with writetext xml go ");
			// Word2 = 1
			TextArea.SetKeywords(1, @"ascii cast char charindex ceiling coalesce collate contains convert current_date current_time current_timestamp current_user floor isnull max min nullif object_id session_user substring system_user tsequal ");
			// User1 = 4
			TextArea.SetKeywords(4, @"all and any between cross exists in inner is join left like not null or outer pivot right some unpivot ( ) * ");
			// User2 = 5
			TextArea.SetKeywords(5, @"sys objects sysobjects ");


		}

		private void InitSyntaxColoringXML()
		{

			// Configure the default style
			TextArea.StyleResetDefault();
			TextArea.Styles[Style.Default].Font = "Consolas";
			TextArea.Styles[Style.Default].Size = 11;
			//TextArea.Styles[Style.Default].BackColor = IntToColor(0x212121);
			//TextArea.Styles[Style.Default].ForeColor = IntToColor(0xFFFFFF);
			TextArea.StyleClearAll();

			
			TextArea.Styles[Style.Xml.Comment].ForeColor = Color.DarkGray;
			TextArea.Styles[Style.Xml.Attribute].ForeColor = Color.Red;
			TextArea.Styles[Style.Xml.Default].ForeColor = Color.Black;
			TextArea.Styles[Style.Xml.XmlStart].ForeColor = Color.Red;
			TextArea.Styles[Style.Xml.XmlEnd].ForeColor = Color.Red;
			TextArea.Styles[Style.Xml.Tag].ForeColor = Color.Blue;
			TextArea.Styles[Style.Xml.TagEnd].ForeColor = Color.Blue;
			TextArea.Styles[Style.Xml.Value].ForeColor = Color.Black;
			TextArea.Styles[Style.Xml.CData].ForeColor = Color.Orange;
			
			TextArea.Lexer = Lexer.Xml;


		}

		private void InitSyntaxColoringPlain()
		{
			TextArea.StyleResetDefault();
			TextArea.Styles[Style.Default].Font = "Consolas";
			TextArea.Styles[Style.Default].Size = 11;
			TextArea.StyleClearAll();
		}


		private void InitSyntaxColoring() {

			// Configure the default style
			TextArea.StyleResetDefault();
			TextArea.Styles[Style.Default].Font = "Consolas";
			TextArea.Styles[Style.Default].Size = 11;
			TextArea.Styles[Style.Default].BackColor = IntToColor(0x212121);
			TextArea.Styles[Style.Default].ForeColor = IntToColor(0xFFFFFF);
			TextArea.StyleClearAll();

			// Configure the CPP (C#) lexer styles
			TextArea.Styles[Style.Cpp.Identifier].ForeColor = IntToColor(0xD0DAE2);
			TextArea.Styles[Style.Cpp.Comment].ForeColor = IntToColor(0xBD758B);
			TextArea.Styles[Style.Cpp.CommentLine].ForeColor = IntToColor(0x40BF57);
			TextArea.Styles[Style.Cpp.CommentDoc].ForeColor = IntToColor(0x2FAE35);
			TextArea.Styles[Style.Cpp.Number].ForeColor = IntToColor(0xFFFF00);
			TextArea.Styles[Style.Cpp.String].ForeColor = IntToColor(0xFFFF00);
			TextArea.Styles[Style.Cpp.Character].ForeColor = IntToColor(0xE95454);
			TextArea.Styles[Style.Cpp.Preprocessor].ForeColor = IntToColor(0x8AAFEE);
			TextArea.Styles[Style.Cpp.Operator].ForeColor = IntToColor(0xE0E0E0);
			TextArea.Styles[Style.Cpp.Regex].ForeColor = IntToColor(0xff00ff);
			TextArea.Styles[Style.Cpp.CommentLineDoc].ForeColor = IntToColor(0x77A7DB);
			TextArea.Styles[Style.Cpp.Word].ForeColor = IntToColor(0x48A8EE);
			TextArea.Styles[Style.Cpp.Word2].ForeColor = IntToColor(0xF98906);
			TextArea.Styles[Style.Cpp.CommentDocKeyword].ForeColor = IntToColor(0xB3D991);
			TextArea.Styles[Style.Cpp.CommentDocKeywordError].ForeColor = IntToColor(0xFF0000);
			TextArea.Styles[Style.Cpp.GlobalClass].ForeColor = IntToColor(0x48A8EE);



			TextArea.Lexer = Lexer.Cpp;



			TextArea.SetKeywords(0, "class extends implements import interface new case do while else if for in switch throw get set function var try catch finally while with default break continue delete return each const namespace package include use is as instanceof typeof author copy default deprecated eventType example exampleText exception haxe inheritDoc internal link mtasc mxmlc param private return see serial serialData serialField since throws usage version langversion playerversion productversion dynamic private public partial static intrinsic internal native override protected AS3 final super this arguments null Infinity NaN undefined true false abstract as base bool break by byte case catch char checked class const continue decimal default delegate do double descending explicit event extern else enum false finally fixed float for foreach from goto group if implicit in int interface internal into is lock long new null namespace object operator out override orderby params private protected public readonly ref return switch struct sbyte sealed short sizeof stackalloc static string select this throw true try typeof uint ulong unchecked unsafe ushort using var virtual volatile void while where yield");
			TextArea.SetKeywords(1, "void Null ArgumentError arguments Array Boolean Class Date DefinitionError Error EvalError Function int Math Namespace Number Object RangeError ReferenceError RegExp SecurityError String SyntaxError TypeError uint XML XMLList Boolean Byte Char DateTime Decimal Double Int16 Int32 Int64 IntPtr SByte Single UInt16 UInt32 UInt64 UIntPtr Void Path File System Windows Forms ScintillaNET");

		}

		private void InitControls()
		{
			FileUtils.CurFileName = "";
			FileUtils.fileSize = 0;
			labelTotalBytes.Text = String.Format("Bytes: {0:n0}", 0);
			labelTotals.Text = "0";
			this.Text = "";

		}

		private void OnTextChanged(object sender, EventArgs e) {

		}
		

		#region Numbers, Bookmarks, Code Folding

		/// <summary>
		/// the background color of the text area
		/// </summary>
		private const int BACK_COLOR = 0x2A211C;

		/// <summary>
		/// default text color of the text area
		/// </summary>
		private const int FORE_COLOR = 0xB7B7B7;

		/// <summary>
		/// change this to whatever margin you want the line numbers to show in
		/// </summary>
		private const int NUMBER_MARGIN = 1;

		/// <summary>
		/// change this to whatever margin you want the bookmarks/breakpoints to show in
		/// </summary>
		private const int BOOKMARK_MARGIN = 2;
		private const int BOOKMARK_MARKER = 2;

		/// <summary>
		/// change this to whatever margin you want the code folding tree (+/-) to show in
		/// </summary>
		private const int FOLDING_MARGIN = 3;

		/// <summary>
		/// set this true to show circular buttons for code folding (the [+] and [-] buttons on the margin)
		/// </summary>
		private const bool CODEFOLDING_CIRCULAR = true;

		private void InitNumberMargin() {

			TextArea.Styles[Style.LineNumber].BackColor = IntToColor(BACK_COLOR);
			TextArea.Styles[Style.LineNumber].ForeColor = IntToColor(FORE_COLOR);
			TextArea.Styles[Style.IndentGuide].ForeColor = IntToColor(FORE_COLOR);
			TextArea.Styles[Style.IndentGuide].BackColor = IntToColor(BACK_COLOR);

			var nums = TextArea.Margins[NUMBER_MARGIN];
			nums.Width = 50;	//30;
			nums.Type = MarginType.Number;
			nums.Sensitive = true;
			nums.Mask = 0;

			TextArea.MarginClick += TextArea_MarginClick;
		}

		private void InitBookmarkMargin() {

			//TextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));

			var margin = TextArea.Margins[BOOKMARK_MARGIN];
			margin.Width = 20;
			margin.Sensitive = true;
			margin.Type = MarginType.Symbol;
			margin.Mask = (1 << BOOKMARK_MARKER);
			//margin.Cursor = MarginCursor.Arrow;

			var marker = TextArea.Markers[BOOKMARK_MARKER];
			marker.Symbol = MarkerSymbol.Circle;
			marker.SetBackColor(IntToColor(0xFF003B));
			marker.SetForeColor(IntToColor(0x000000));
			marker.SetAlpha(100);

		}

		private void InitCodeFolding() {

			TextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));
			TextArea.SetFoldMarginHighlightColor(true, IntToColor(BACK_COLOR));

			// Enable code folding
			TextArea.SetProperty("fold", "1");
			TextArea.SetProperty("fold.compact", "1");

			// Configure a margin to display folding symbols
			TextArea.Margins[FOLDING_MARGIN].Type = MarginType.Symbol;
			TextArea.Margins[FOLDING_MARGIN].Mask = Marker.MaskFolders;
			TextArea.Margins[FOLDING_MARGIN].Sensitive = true;
			TextArea.Margins[FOLDING_MARGIN].Width = 20;

			// Set colors for all folding markers
			for (int i = 25; i <= 31; i++) {
				TextArea.Markers[i].SetForeColor(IntToColor(BACK_COLOR)); // styles for [+] and [-]
				TextArea.Markers[i].SetBackColor(IntToColor(FORE_COLOR)); // styles for [+] and [-]
			}

			// Configure folding markers with respective symbols
			TextArea.Markers[Marker.Folder].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
			TextArea.Markers[Marker.FolderOpen].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
			TextArea.Markers[Marker.FolderEnd].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
			TextArea.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
			TextArea.Markers[Marker.FolderOpenMid].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
			TextArea.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
			TextArea.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

			// Enable automatic folding
			TextArea.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

		}

		private void TextArea_MarginClick(object sender, MarginClickEventArgs e) {
			if (e.Margin == BOOKMARK_MARGIN) {
				// Do we have a marker for this line?
				const uint mask = (1 << BOOKMARK_MARKER);
				var line = TextArea.Lines[TextArea.LineFromPosition(e.Position)];
				if ((line.MarkerGet() & mask) > 0) {
					// Remove existing bookmark
					line.MarkerDelete(BOOKMARK_MARKER);
				} else {
					// Add bookmark
					line.MarkerAdd(BOOKMARK_MARKER);
				}
			}
		}

		#endregion

		#region Drag & Drop File

		public void InitDragDropFile() {

			TextArea.AllowDrop = true;
			TextArea.DragEnter += delegate(object sender, DragEventArgs e) {
				if (e.Data.GetDataPresent(DataFormats.FileDrop))
					e.Effect = DragDropEffects.Copy;
				else
					e.Effect = DragDropEffects.None;
			};
			TextArea.DragDrop += delegate(object sender, DragEventArgs e) {

				// get file drop
				if (e.Data.GetDataPresent(DataFormats.FileDrop)) {

					Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
					if (a != null) {

						string path = a.GetValue(0).ToString();

						LoadDataFromFile(path);

					}
				}
			};

		}

		private int getLimit()
		{
			int limit = 0;
			try
			{
				limit = Int32.Parse(textBoxLimit.Text.Trim());				
			}
			catch (Exception ex)
			{
				limit = 0;
			}
			return limit;
		}
		private void LoadDataFromFile(string path) {
			FileUtils.CurFileName = path;
			if (File.Exists(path)) 
			{
				FileInfo fi = new FileInfo(path);
				long fileSize = fi.Length;
				FileUtils.fileSize = fileSize;
				labelTotalBytes.Text = String.Format("Bytes: {0:n0}", fileSize);
				int totPages = (int)(FileUtils.fileSize / getLimit());
				labelTotals.Text = totPages.ToString(); 
				
				this.Text = path;


				try
                {
					int limit = getLimit();
					
					if (limit > 0)
					{
						if (FileUtils.GCTrigger == 5)
						{
							System.GC.Collect();
							FileUtils.GCTrigger = 0;
						}

						
						TextArea.Text = FileUtils.readNBites(path, limit, 0);
						FileUtils.GCTrigger++;
						return;
					}
				}
				catch (Exception ex )
                {
                    //do nothing
                }

				TextArea.Text = File.ReadAllText(path);

			}
		}

		#endregion

		#region Main Menu Commands

		private void openToolStripMenuItem_Click(object sender, EventArgs e) {
			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				LoadDataFromFile(openFileDialog.FileName);
			}
		}

		private void findToolStripMenuItem_Click(object sender, EventArgs e) {
			OpenSearch();
		}

		private void findDialogToolStripMenuItem_Click(object sender, EventArgs e) {
			OpenFindDialog();
		}

		private void findAndReplaceToolStripMenuItem_Click(object sender, EventArgs e) {
			OpenReplaceDialog();
		}

		private void cutToolStripMenuItem_Click(object sender, EventArgs e) {
			TextArea.Cut();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
			TextArea.Copy();
		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e) {
			TextArea.Paste();
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
			TextArea.SelectAll();
		}

		private void selectLineToolStripMenuItem_Click(object sender, EventArgs e) {
			Line line = TextArea.Lines[TextArea.CurrentLine];
			TextArea.SetSelection(line.Position + line.Length, line.Position);
		}

		private void clearSelectionToolStripMenuItem_Click(object sender, EventArgs e) {
			TextArea.SetEmptySelection(0);
		}

		private void indentSelectionToolStripMenuItem_Click(object sender, EventArgs e) {
			Indent();
		}

		private void outdentSelectionToolStripMenuItem_Click(object sender, EventArgs e) {
			Outdent();
		}

		private void uppercaseSelectionToolStripMenuItem_Click(object sender, EventArgs e) {
			Uppercase();
		}

		private void lowercaseSelectionToolStripMenuItem_Click(object sender, EventArgs e) {
			Lowercase();
		}

		private void wordWrapToolStripMenuItem1_Click(object sender, EventArgs e) {

			// toggle word wrap
			wordWrapItem.Checked = !wordWrapItem.Checked;
			TextArea.WrapMode = wordWrapItem.Checked ? WrapMode.Word : WrapMode.None;
		}
		
		private void indentGuidesToolStripMenuItem_Click(object sender, EventArgs e) {

			// toggle indent guides
			indentGuidesItem.Checked = !indentGuidesItem.Checked;
			TextArea.IndentationGuides = indentGuidesItem.Checked ? IndentView.LookBoth : IndentView.None;
		}

		private void hiddenCharactersToolStripMenuItem_Click(object sender, EventArgs e) {

			// toggle view whitespace
			hiddenCharactersItem.Checked = !hiddenCharactersItem.Checked;
			TextArea.ViewWhitespace = hiddenCharactersItem.Checked ? WhitespaceMode.VisibleAlways : WhitespaceMode.Invisible;
		}

		private void zoomInToolStripMenuItem_Click(object sender, EventArgs e) {
			ZoomIn();
		}

		private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e) {
			ZoomOut();
		}

		private void zoom100ToolStripMenuItem_Click(object sender, EventArgs e) {
			ZoomDefault();
		}

		private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e) {
			TextArea.FoldAll(FoldAction.Contract);
		}

		private void expandAllToolStripMenuItem_Click(object sender, EventArgs e) {
			TextArea.FoldAll(FoldAction.Expand);
		}
		

		#endregion

		#region Uppercase / Lowercase

		private void Lowercase() {

			// save the selection
			int start = TextArea.SelectionStart;
			int end = TextArea.SelectionEnd;

			// modify the selected text
			TextArea.ReplaceSelection(TextArea.GetTextRange(start, end - start).ToLower());

			// preserve the original selection
			TextArea.SetSelection(start, end);
		}

		private void Uppercase() {

			// save the selection
			int start = TextArea.SelectionStart;
			int end = TextArea.SelectionEnd;

			// modify the selected text
			TextArea.ReplaceSelection(TextArea.GetTextRange(start, end - start).ToUpper());

			// preserve the original selection
			TextArea.SetSelection(start, end);
		}

		#endregion

		#region Indent / Outdent

		private void Indent() {
			// we use this hack to send "Shift+Tab" to scintilla, since there is no known API to indent,
			// although the indentation function exists. Pressing TAB with the editor focused confirms this.
			GenerateKeystrokes("{TAB}");
		}

		private void Outdent() {
			// we use this hack to send "Shift+Tab" to scintilla, since there is no known API to outdent,
			// although the indentation function exists. Pressing Shift+Tab with the editor focused confirms this.
			GenerateKeystrokes("+{TAB}");
		}

		private void GenerateKeystrokes(string keys) {
			HotKeyManager.Enable = false;
			TextArea.Focus();
			SendKeys.Send(keys);
			HotKeyManager.Enable = true;
		}

		#endregion

		#region Zoom

		private void ZoomIn() {
			TextArea.ZoomIn();
		}

		private void ZoomOut() {
			TextArea.ZoomOut();
		}

		private void ZoomDefault() {
			TextArea.Zoom = 0;
		}


		#endregion

		#region Quick Search Bar

		bool SearchIsOpen = false;

		private void OpenSearch() {

			SearchManager.SearchBox = TxtSearch;
			SearchManager.TextArea = TextArea;

			if (!SearchIsOpen) {
				SearchIsOpen = true;
				InvokeIfNeeded(delegate() {
					PanelSearch.Visible = true;
					TxtSearch.Text = SearchManager.LastSearch;
					TxtSearch.Focus();
					TxtSearch.SelectAll();
				});
			} else {
				InvokeIfNeeded(delegate() {
					TxtSearch.Focus();
					TxtSearch.SelectAll();
				});
			}
		}
		private void CloseSearch() {
			if (SearchIsOpen) {
				SearchIsOpen = false;
				InvokeIfNeeded(delegate() {
					PanelSearch.Visible = false;
					//CurBrowser.GetBrowser().StopFinding(true);
				});
			}
		}

		private void UpdateClip()
		{
			FileUtils.UpdateClip();
		}
		private void BtnClearSearch_Click(object sender, EventArgs e) {
			CloseSearch();
		}

		private void BtnPrevSearch_Click(object sender, EventArgs e) {
			SearchManager.Find(false, false);
		}
		private void BtnNextSearch_Click(object sender, EventArgs e) {
			SearchManager.Find(true, false);
		}
		private void TxtSearch_TextChanged(object sender, EventArgs e) {
			SearchManager.Find(true, true);
		}

		private void TxtSearch_KeyDown(object sender, KeyEventArgs e) {
			if (HotKeyManager.IsHotkey(e, Keys.Enter)) {
				SearchManager.Find(true, false);
			}
			if (HotKeyManager.IsHotkey(e, Keys.Enter, true) || HotKeyManager.IsHotkey(e, Keys.Enter, false, true)) {
				SearchManager.Find(false, false);
			}
		}

		#endregion

		#region Find & Replace Dialog

		private void OpenFindDialog() {

		}
		private void OpenReplaceDialog() {


		}

		#endregion

		#region Utils

		public static Color IntToColor(int rgb) {
			return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
		}

		public void InvokeIfNeeded(Action action) {
			if (this.InvokeRequired) {
				this.BeginInvoke(action);
			} else {
				action.Invoke();
			}
		}




        #endregion


        #region custom
        private void toolStripMenuItemFileName_Click(object sender, EventArgs e)
        {
			if (FileUtils.CurFileName.Length > 0)
			{
				FileUtils.UpdateClip();
			}
        }

        private void unWrapXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
			InitSyntaxColoringXML();
			TextArea.Text = FileUtils.UnWrapXML(TextArea.Text);
		}

		private void unWrapXMLShortcut()
		{
			InitSyntaxColoringXML();
			TextArea.Text = FileUtils.UnWrapXML(TextArea.Text);
		}

		private void sQLStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
			InitSyntaxColoringSQL();

		}

        private void unWrapEDIToolStripMenuItem_Click(object sender, EventArgs e)
        {

			EDIHelper helper = new EDIHelper();
			if (helper.IsEDIFile(FileUtils.CurFileName))
			{
				TextArea.Text = helper.ParseFile(FileUtils.CurFileName);				 
			}
		}

        private void unWrapEDIPartToolStripMenuItem_Click(object sender, EventArgs e)
        {
			EDIHelper helper = new EDIHelper();
			if (helper.IsEDIFile(FileUtils.CurFileName))
			{
				TextArea.Text = helper.ParseString(TextArea.Text, FileUtils.CurFileName);
				
			}
		}

        private void unWrapFixWidthToolStripMenuItem_Click(object sender, EventArgs e)
        {
			TextArea.Text = FileUtils.getFixWidth(TextArea.Text, 80);
        }

		private void buttonLeftShortcut()
		{
			try
			{
				int page = Int32.Parse(textBoxPage.Text);
				int maxPage = ((int)(FileUtils.fileSize / getLimit()));
				if ((page - 1) >= 0)
				{
					page--;
					int offset = page;

					int limit = getLimit();
					if (limit > 0)
					{
						TextArea.Text = FileUtils.readNBites(FileUtils.CurFileName, limit, offset);
						textBoxPage.Text = page.ToString();
					}

				}
				else
				{
					Console.WriteLine("Page size " + page.ToString() + " you cannot go negative. Max page size of " + maxPage.ToString());
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

		}
		private void buttonLeft_Click(object sender, EventArgs e)
        {
			buttonLeftShortcut();
		}

		private void buttonRightShortcut()
		{
			try
			{
				int page = Int32.Parse(textBoxPage.Text);
				int maxPage = ((int)(FileUtils.fileSize / getLimit()));
				if ((page + 1) <= maxPage)
				{
					page++;
					int offset = page;

					int limit = getLimit();
					if (limit > 0)
					{
						TextArea.Text = FileUtils.readNBites(FileUtils.CurFileName, limit, offset);
						textBoxPage.Text = page.ToString();
					}

				}
				else
				{
					Console.WriteLine("Page size " + page.ToString() + " is => Max page size of " + maxPage.ToString());
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
        private void buttonRight_Click(object sender, EventArgs e)
        {
			buttonRightShortcut();
		}

        private void textBoxPage_DoubleClick(object sender, EventArgs e)
        {

		}


		private void buttonJumpToAnyPage(int page)
		{
			try
			{				
				int maxPage = ((int)(FileUtils.fileSize / getLimit()));
				if (page <= maxPage && page > -1)
				{
					int offset = page;

					int limit = getLimit();
					if (limit > 0)
					{
						TextArea.Text = FileUtils.readNBites(FileUtils.CurFileName, limit, offset);
						textBoxPage.Text = page.ToString();
					}

				}
				else
				{
					Console.WriteLine("Page size " + page.ToString() + " is out of bound Max page size of " + maxPage.ToString());
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
		private void buttonJumpTo_Click(object sender, EventArgs e)
        {
			try
			{
				int page = Int32.Parse(textBoxPage.Text);
				buttonJumpToAnyPage(page);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

        #endregion

        private void syntaxXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
			InitSyntaxColoringXML();
		}

        private void syntaxPlainToolStripMenuItem_Click(object sender, EventArgs e)
        {
			InitSyntaxColoringPlain();
        }

        private void resetObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
			InitControls();
			TextArea.Text = "";			
			TextArea.Dispose();
			System.GC.Collect();
			// CREATE CONTROL
			TextArea = new ScintillaNET.Scintilla();
			TextPanel.Controls.Add(TextArea);

			// BASIC CONFIG
			TextArea.Dock = System.Windows.Forms.DockStyle.Fill;
			TextArea.TextChanged += (this.OnTextChanged);

			// INITIAL VIEW CONFIG
			TextArea.WrapMode = WrapMode.None;
			TextArea.IndentationGuides = IndentView.LookBoth;

			// STYLING
			InitColors();
			//InitSyntaxColoring();

			// NUMBER MARGIN
			InitNumberMargin();
			InitSyntaxColoringPlain();


			// BOOKMARK MARGIN
			InitBookmarkMargin();

			// CODE FOLDING MARGIN
			//InitCodeFolding();

			// DRAG DROP
			InitDragDropFile();

			// DEFAULT FILE
			//LoadDataFromFile("../../MainForm.cs");

			// INIT HOTKEYS
			InitHotkeys();

		}

        private void buttonSearchFile_Click(object sender, EventArgs e)
        {
			string txt = textBoxSearchFile.Text.Trim();
			StringBuilder sb = new StringBuilder();
			Dictionary<long, string> loc  = FileUtils.SearchFile(FileUtils.CurFileName, txt);
			foreach (KeyValuePair<long, string> kvp in loc)
			{
				
				sb.AppendLine(String.Format("{0} {1}", kvp.Key, kvp.Value));
			}

			richTextBoxBottom.Text = sb.ToString();
						
		}


		private void ScrollToLineNumber(int ln)
		{
			int tot = TextArea.Lines.Count;
			if (ln <= tot)
			{
				var linesOnScreen = TextArea.LinesOnScreen - 2;

				var line = ln;

				var start = TextArea.Lines[line - (linesOnScreen / 2)].Position;
				var end = TextArea.Lines[line + (linesOnScreen / 2)].Position;
				TextArea.ScrollRange(start, end);
			}
			else
			{
                Console.WriteLine("Line " + ln.ToString() + " is out of range. Max Line is " + tot.ToString());
			}

		}
      

        private void richTextBoxBottom_DoubleClick(object sender, EventArgs e)
        {

			string str = richTextBoxBottom.SelectedText;
			Console.WriteLine("text is selected: " + str);
			//
            try
            {
				int loc = Int32.Parse(str);				
				int page = loc / getLimit();

				Console.WriteLine(String.Format("Double click page:{0:n0} ", page));

				buttonJumpToAnyPage(page);


			}
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
		}


		private void HighlightWord(string text)
		{
			if (string.IsNullOrEmpty(text))
				return;

			// Indicators 0-7 could be in use by a lexer
			// so we'll use indicator 8 to highlight words.
			const int NUM = 8;

			// Remove all uses of our indicator
			TextArea.IndicatorCurrent = NUM;
			TextArea.IndicatorClearRange(0, TextArea.TextLength);

			// Update indicator appearance
			TextArea.Indicators[NUM].Style = IndicatorStyle.StraightBox;
			TextArea.Indicators[NUM].Under = true;
			TextArea.Indicators[NUM].ForeColor = Color.Purple;
			TextArea.Indicators[NUM].OutlineAlpha = 90; //50;
			TextArea.Indicators[NUM].Alpha = 70; //30;

			// Search the document
			TextArea.TargetStart = 0;
			TextArea.TargetEnd = TextArea.TextLength;
			TextArea.SearchFlags = SearchFlags.None;
			while (TextArea.SearchInTarget(text) != -1)
			{
				// Mark the search results with the current indicator
				TextArea.IndicatorFillRange(TextArea.TargetStart, TextArea.TargetEnd - TextArea.TargetStart);

				// Search the remainder of the document
				TextArea.TargetStart = TextArea.TargetEnd;
				TextArea.TargetEnd = TextArea.TextLength;
			}
		}

	}
}
