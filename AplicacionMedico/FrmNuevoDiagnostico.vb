﻿Imports CapaLogica
Imports Clases

Public Class FrmNuevoDiagnostico
    Private consulta As DiagnosticoPrimarioConConsulta
    Public enfermedadDiagnosticada As Enfermedad

    Public Sub New(consultaEnCurso As DiagnosticoPrimarioConConsulta)

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        consulta = consultaEnCurso
    End Sub

    Private Sub FrmNuevoDiagnostico_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each enfermedad As Enfermedad In CargarTodasLasEnfermedades()
            tblEnfermedades.Rows.Add(enfermedad)
        Next
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        For Each r As DataGridViewRow In tblEnfermedades.Rows
            If r.Cells(0).Value.ToString.ToLower Like ("*" & txtBuscar.Text & "*").ToLower Then
                r.Visible = True
            Else
                r.Visible = False
            End If
        Next
    End Sub

    Private Sub btnEnviarDiagnostico_Click(sender As Object, e As EventArgs) Handles btnEnviarDiagnostico.Click
        If tblEnfermedades.SelectedRows.Count = 1 Then
            enfermedadDiagnosticada = tblEnfermedades.SelectedRows(0).Cells(0).Value
            CrearDiagnosticoDiferencial(consulta, enfermedadDiagnosticada, txtConductaASeguir.Text)
            Me.DialogResult = DialogResult.OK
            MsgBox("Diagnóstico enviado con éxito.")
            Me.Close()
        Else
            MsgBox("Seleccione una sola enfermedad para realizar el diagnóstico.")
        End If
    End Sub

    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Me.Close()
    End Sub
End Class