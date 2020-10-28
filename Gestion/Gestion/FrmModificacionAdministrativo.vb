﻿Imports CapaLogica
Imports Clases
Public Class FrmModificacionAdministrativo
    Dim administrativoAModificar As Administrativo
    Sub New(Administrativo As Administrativo)

        InitializeComponent()

        txtCI.Text = Administrativo.CI
        txtNombre.Text = Administrativo.Nombre
        txtApellido.Text = Administrativo.Apellido
        txtCorreo.Text = Administrativo.Correo
        administrativoAModificar = Administrativo

        If Administrativo.EsEncargado = True Then
            checkEncargado.Checked = True
        End If



    End Sub
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        If MsgBox("Advertencia: no se guardaron los cambios." & vbNewLine & "¿Confirma que desea cerrar la ventana?", MsgBoxStyle.YesNo, "Salir") =
            MsgBoxResult.Yes Then
            Me.Close()
        End If
    End Sub

    Private Sub FrmModificacionAdministrativo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Se añade y se oculta las localidades 
        For Each localidad As Localidad In CargarTodasLasLocalidades()
            tblLocalidad.Rows.Add(localidad)
        Next

        For i = 0 To tblLocalidad.Rows.Count - 1
            tblLocalidad.Rows(i).Visible = False
        Next
        tblLocalidad.ClearSelection()

        'Se añade los departamentos al comboBox
        cmbDepartamento.Items.AddRange(CargarTodosLosDepartamentos.ToArray)

        For Each r As DataGridViewRow In tblLocalidad.Rows
            If CType(r.Cells(0).Value, Localidad).ID = administrativoAModificar.Localidad.ID Then
                r.Visible = True
            Else
                r.Visible = False
            End If
        Next
    End Sub

    Private Sub btnConfirmar_Click(sender As Object, e As EventArgs) Handles btnConfirmar.Click
        Try
            Dim localidad As Localidad

            Dim esEncargado As Boolean
            If checkEncargado.Checked = True Then
                esEncargado = True
            Else
                esEncargado = False
            End If

            Try
                localidad = tblLocalidad.SelectedRows(0).Cells(0).Value
            Catch ex As Exception
                Throw New Exception("No se selecciono ninguna localidad")
            End Try

            ActualizarAdministrativo(administrativoAModificar, txtCI.Text, txtNombre.Text, txtApellido.Text, txtCorreo.Text, localidad, esEncargado, True)
            MsgBox("Administrativo modificado con éxito.", MsgBoxStyle.OkOnly, "Éxito")
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")

        End Try
    End Sub

    Private Sub cmbDepartamento_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDepartamento.SelectedIndexChanged
        For Each r As DataGridViewRow In tblLocalidad.Rows
            If CType(r.Cells(0).Value, Localidad).Departamento = cmbDepartamento.SelectedItem Then
                r.Visible = True
            Else
                r.Visible = False
            End If
        Next
        tblLocalidad.ClearSelection()
    End Sub

    Private Sub lblTraducir_Click(sender As Object, e As EventArgs) Handles lblTraducir.Click
        TraducirAplicacion()
    End Sub
End Class