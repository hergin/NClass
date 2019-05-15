﻿// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// Copyright (C) 2016 Georgi Baychev
// 
// This program is free software; you can redistribute it and/or modify it under 
// the terms of the GNU General Public License as published by the Free Software 
// Foundation; either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT 
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with 
// this program; if not, write to the Free Software Foundation, Inc., 
// 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using NClass.DiagramEditor.Diagrams;
using NClass.DiagramEditor.Diagrams.Editors;
using NClass.DiagramEditor.UseCaseDiagram.Shapes;

namespace NClass.DiagramEditor.UseCaseDiagram.Editors
{
    public partial class UseCaseEditor :
#if DEBUG
    DesignerHelperWindow
#else
    EditorWindow
#endif
    {
        private UseCaseShape shape = null;

        public UseCaseEditor()
        {
            InitializeComponent();
        }

        internal override void Init(DiagramElement element)
        {
            shape = (UseCaseShape)element;

            txtUseCase.BackColor = Style.CurrentStyle.UseCaseBackColor;
            txtUseCase.ForeColor = Style.CurrentStyle.UseCaseTextColor;
            txtUseCase.Text = shape.Text;

            Font font = Style.CurrentStyle.UseCaseFont;
            txtUseCase.Font = new Font(font.FontFamily, font.SizeInPoints * shape.Diagram.Zoom, font.Style);
        }

        internal override void Relocate(DiagramElement element)
        {
            IDiagram diagram = shape.Diagram;

            if (diagram == null) return;

            Rectangle absolute = shape.GetTextRectangle();

            // The following lines are required because of a .NET bug:
            // http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=380085
            if (!MonoHelper.IsRunningOnMono)
            {
                absolute.X -= 3;
                absolute.Width += 3;
            }

            this.SetBounds(
                (int)(absolute.X * diagram.Zoom) - diagram.Offset.X + ParentLocation.X,
                (int)(absolute.Y * diagram.Zoom) - diagram.Offset.Y + ParentLocation.Y,
                (int)(absolute.Width * diagram.Zoom),
                (int)(absolute.Height * diagram.Zoom));
        }

        public override void ValidateData()
        {
            this.shape.UseCase.Name = this.txtUseCase.Text;
        }

        private void txtComment_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (e.Modifiers == Keys.None)
                        shape.HideEditor();
                    else if (e.Modifiers == Keys.Control)
                        this.txtUseCase.AppendText(Environment.NewLine);
                    break;
                case Keys.Escape:
                    shape.HideEditor();
                    break;
            }
        }
    }
}