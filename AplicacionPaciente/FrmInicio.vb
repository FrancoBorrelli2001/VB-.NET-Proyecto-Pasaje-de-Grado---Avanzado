﻿Imports System.Drawing

Public Class FrmInicio
    Dim appPacientes As FrmLogin
    Private Sub FrmInicio_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        picHelloCare.Image = Image.FromFile("..\..\logo_hellocare.jpg")
        picHelloCode.Image = Image.FromFile("..\..\logo_hellocode.jpg")
        appPacientes = New FrmLogin
    End Sub

    Private Sub tmrSplashWindow_Tick(sender As Object, e As EventArgs) Handles tmrSplashWindow.Tick
        tmrSplashWindow.Enabled = False
        appPacientes.Show()
        Me.Close()
    End Sub
End Class
