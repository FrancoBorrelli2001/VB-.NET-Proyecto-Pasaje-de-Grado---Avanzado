﻿Imports CapaLogica
Imports Clases
Public Class FrmListadoAdministrativos
    Private Sub FrmListadoAdministrativos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ActualizarAdministrativos()
    End Sub

    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Me.Close()
    End Sub

    Private Sub btnVer_Click(sender As Object, e As EventArgs) Handles btnVer.Click
        If tblAdministrativo.SelectedRows.Count = 1 Then
            Dim Administrativo As Administrativo = tblAdministrativo.SelectedRows(0).Cells(0).Value
            Dim frm As New FrmVerAdministrativo(Administrativo)
            Me.Hide()
            frm.ShowDialog()
            Me.Show()
        Else
            MostrarMensaje(MsgBoxStyle.Critical, "Seleccione una sola fila para ver los detalles del administrativo.", "Error", "Select a single row to view details of the administrative staff.", "Error")
        End If
    End Sub

    Private Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        If tblAdministrativo.SelectedRows.Count = 1 Then
            Dim Administrativo As Administrativo = tblAdministrativo.SelectedRows(0).Cells(0).Value
            Dim frm As New FrmModificacionAdministrativo(Administrativo)
            Me.Hide()
            frm.ShowDialog()
            Me.Show()
        Else
            MostrarMensaje(MsgBoxStyle.Critical, "Seleccione una sola fila para modificar los datos del administrativo.", "Error", "Select a single row to modify the data of the administrative staff.", "Error")
        End If
        ActualizarAdministrativos()
    End Sub

    Private Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Dim frm As New FrmAltaAdministrativo
        Me.Hide()
        frm.ShowDialog()
        Me.Show()
        ActualizarAdministrativos()
    End Sub

    Private Sub ActualizarAdministrativos()
        tblAdministrativo.Rows.Clear()
        For Each Administrativo As Administrativo In CargarTodosLosAdministrativos()
            tblAdministrativo.Rows.Add(Administrativo, Administrativo.CI, Administrativo.Nombre, Administrativo.Apellido,
                                       Administrativo.Correo, Administrativo.Localidad, Administrativo.EsEncargado)
        Next
        tblAdministrativo.ClearSelection()
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If tblAdministrativo.SelectedRows.Count > 0 Then
            If MostrarMensaje(MsgBoxStyle.YesNo, "¿Confirma que desea eliminar este administrativo?" & vbNewLine & "Estos cambios no podrán deshacerse.", "Advertencia", "Are you sure to delete this administrative staff?" & vbNewLine & "These changes cannot be undone.", "Warning") = MsgBoxResult.Yes Then
                For Each r As DataGridViewRow In tblAdministrativo.SelectedRows
                    EliminarAdministrativo(r.Cells(0).Value)
                Next
                ActualizarAdministrativos()
            End If
        Else
            MostrarMensaje(MsgBoxStyle.Information, "Seleccione al menos una fila para eliminar el o los administrativos.", "", "Select at least one row to delete the administrative staff.", "")
        End If
    End Sub

    Private Sub lblTraducir_Click(sender As Object, e As EventArgs) Handles lblTraducir.Click
        TraducirAplicacion()
    End Sub

    Private Sub OcultarAdministrativo()
        tblAdministrativo.ClearSelection()

        For Each r As DataGridViewRow In tblAdministrativo.Rows

            If cbEs_Encargado.Checked = True Then
                If r.Cells(1).Value.ToString.ToLower Like ("*" & txtBA_CI.Text & "*").ToLower And r.Cells(5).Value.ToString.ToLower Like ("*" & txtBA_Localidad.Text & "*").ToLower And r.Cells(6).Value Like (True) Then
                    r.Visible = True
                Else
                    If Not r.Selected Then
                        r.Visible = False
                    End If
                End If
            Else
                If r.Cells(1).Value.ToString.ToLower Like ("*" & txtBA_CI.Text & "*").ToLower And r.Cells(5).Value.ToString.ToLower Like ("*" & txtBA_Localidad.Text & "*").ToLower And (r.Cells(6).Value Like (True) Or r.Cells(6).Value Like (False)) Then
                    r.Visible = True
                Else
                    If Not r.Selected Then
                        r.Visible = False
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub txtBA_Localidad_TextChanged(sender As Object, e As EventArgs) Handles txtBA_Localidad.TextChanged
        OcultarAdministrativo()
    End Sub

    Private Sub txtBA_CI_TextChanged(sender As Object, e As EventArgs) Handles txtBA_CI.TextChanged
        OcultarAdministrativo()
    End Sub

    Private Sub cbEs_Encargado_CheckedChanged(sender As Object, e As EventArgs) Handles cbEs_Encargado.CheckedChanged
        OcultarAdministrativo()
    End Sub

    Private Sub FrmListadoAdministrativos_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F1 Then
            AbrirAyuda(TiposUsuario.Administrativo, Me)
        End If
    End Sub
End Class