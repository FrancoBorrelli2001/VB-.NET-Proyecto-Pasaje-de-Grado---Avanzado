﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmChatMedico
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmChatMedico))
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.txtMensaje = New System.Windows.Forms.RichTextBox()
        Me.btnEnviar = New System.Windows.Forms.Button()
        Me.btnAdjuntar = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblNombrePaciente = New System.Windows.Forms.Label()
        Me.lstArchivos = New System.Windows.Forms.ListBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.expAdjuntar = New System.Windows.Forms.OpenFileDialog()
        Me.txtConversacion = New System.Windows.Forms.TextBox()
        Me.tmrActualizar = New System.Windows.Forms.Timer(Me.components)
        Me.lblTraducir = New System.Windows.Forms.Label()
        Me.btnDiagnosticar = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnSalir
        '
        resources.ApplyResources(Me.btnSalir, "btnSalir")
        Me.btnSalir.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.UseVisualStyleBackColor = True
        '
        'txtMensaje
        '
        resources.ApplyResources(Me.txtMensaje, "txtMensaje")
        Me.txtMensaje.Name = "txtMensaje"
        '
        'btnEnviar
        '
        resources.ApplyResources(Me.btnEnviar, "btnEnviar")
        Me.btnEnviar.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.btnEnviar.Name = "btnEnviar"
        Me.btnEnviar.UseVisualStyleBackColor = True
        '
        'btnAdjuntar
        '
        resources.ApplyResources(Me.btnAdjuntar, "btnAdjuntar")
        Me.btnAdjuntar.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.btnAdjuntar.Name = "btnAdjuntar"
        Me.btnAdjuntar.UseVisualStyleBackColor = True
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Name = "Label3"
        '
        'lblNombrePaciente
        '
        resources.ApplyResources(Me.lblNombrePaciente, "lblNombrePaciente")
        Me.lblNombrePaciente.Name = "lblNombrePaciente"
        '
        'lstArchivos
        '
        resources.ApplyResources(Me.lstArchivos, "lstArchivos")
        Me.lstArchivos.FormattingEnabled = True
        Me.lstArchivos.Name = "lstArchivos"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.Label2.Name = "Label2"
        '
        'expAdjuntar
        '
        Me.expAdjuntar.FileName = "OpenFileDialog1"
        resources.ApplyResources(Me.expAdjuntar, "expAdjuntar")
        '
        'txtConversacion
        '
        Me.txtConversacion.AcceptsReturn = True
        Me.txtConversacion.AcceptsTab = True
        resources.ApplyResources(Me.txtConversacion, "txtConversacion")
        Me.txtConversacion.BackColor = System.Drawing.SystemColors.Control
        Me.txtConversacion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtConversacion.Name = "txtConversacion"
        Me.txtConversacion.ReadOnly = True
        '
        'tmrActualizar
        '
        '
        'lblTraducir
        '
        resources.ApplyResources(Me.lblTraducir, "lblTraducir")
        Me.lblTraducir.BackColor = System.Drawing.Color.Transparent
        Me.lblTraducir.ForeColor = System.Drawing.Color.White
        Me.lblTraducir.Name = "lblTraducir"
        '
        'btnDiagnosticar
        '
        resources.ApplyResources(Me.btnDiagnosticar, "btnDiagnosticar")
        Me.btnDiagnosticar.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.btnDiagnosticar.Name = "btnDiagnosticar"
        Me.btnDiagnosticar.UseVisualStyleBackColor = True
        '
        'FrmChatMedico
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.btnDiagnosticar)
        Me.Controls.Add(Me.lblTraducir)
        Me.Controls.Add(Me.lstArchivos)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblNombrePaciente)
        Me.Controls.Add(Me.txtConversacion)
        Me.Controls.Add(Me.btnAdjuntar)
        Me.Controls.Add(Me.btnEnviar)
        Me.Controls.Add(Me.txtMensaje)
        Me.Controls.Add(Me.btnSalir)
        Me.DoubleBuffered = True
        Me.Name = "FrmChatMedico"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Splitter1 As Windows.Forms.Splitter
    Friend WithEvents btnSalir As Windows.Forms.Button
    Friend WithEvents txtMensaje As Windows.Forms.RichTextBox
    Friend WithEvents btnEnviar As Windows.Forms.Button
    Friend WithEvents btnAdjuntar As Windows.Forms.Button
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents lblNombrePaciente As Windows.Forms.Label
    Friend WithEvents lstArchivos As Windows.Forms.ListBox
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents expAdjuntar As Windows.Forms.OpenFileDialog
    Friend WithEvents txtConversacion As Windows.Forms.TextBox
    Friend WithEvents tmrActualizar As Timer
    Friend WithEvents lblTraducir As Label
    Friend WithEvents btnDiagnosticar As Button
End Class
