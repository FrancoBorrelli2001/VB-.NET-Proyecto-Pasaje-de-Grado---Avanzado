﻿Imports CapaLogica
Imports Clases

Public Class FrmPeticionesChat
    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        RefrescarPeticiones()
    End Sub

    Private Sub RefrescarPeticiones()
        tblPeticiones.Rows.Clear()
        For Each d As DiagnosticoPrimarioConConsulta In CargarConsultasSinAtenderParaMedicoLogeado()
            tblPeticiones.Rows.Add(d, d.Paciente, d.Enfermedades(d.IndiceEnfermedadMasProbable), d.ComentariosAdicionales, d.FechaHora)
        Next
    End Sub

    Private Sub btnAceptarConsulta_Click(sender As Object, e As EventArgs) Handles btnAceptarConsulta.Click
        If tblPeticiones.SelectedRows.Count = 1 Then
            ' marcar diagnostico como atendido, asignar medico
            Dim peticion As DiagnosticoPrimarioConConsulta = tblPeticiones.SelectedRows(0).Cells(0).Value
            Dim frm As New FrmChatMedico(peticion)
            Me.Hide()
            frm.ShowDialog()
            RefrescarPeticiones()
            Me.Show()
        Else
            MsgBox("Seleccione un diagnóstico para aceptar la petición e iniciar el chat.")
        End If
    End Sub

    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Me.Close()
    End Sub
End Class