﻿Imports System.ComponentModel
Imports System.Globalization
Imports CapaLogica
Imports Clases

Public Class FrmListadoEnfermedades
    ' Cada vez que el formulario recibe el foco, actualiza las enfermedades del sistema
    Private Sub FrmListadoEnfermedades_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        ActualizarEnfermedades()
    End Sub

    Private Sub ActualizarEnfermedades()
        tblEnfermedades.Rows.Clear()
        For Each e As Enfermedad In CargarTodasLasEnfermedades()
            tblEnfermedades.Rows.Add(e, e.Nombre, e.Descripcion, e.Gravedad, e.Recomendaciones)
        Next
        tblEnfermedades.ClearSelection()
    End Sub

    Private Sub btnImportar_Click(sender As Object, e As EventArgs) Handles btnImportar.Click
        exploradorArchivos.FileName = ""
        exploradorArchivos.ShowDialog()
        Dim nombreArchivo As String = exploradorArchivos.FileName
        Dim hayErrores As Boolean = False

        If nombreArchivo = "" Then
            MsgBox("No se seleccionó ningún archivo CSV para importar.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If Not nombreArchivo.EndsWith(".csv") Then
            MsgBox("El archivo seleccionado no es de formato CSV.", MsgBoxStyle.Critical)
            Exit Sub
        End If

        If Not hayErrores Then
            ImportarEnfermedades(nombreArchivo)
            MsgBox("¡Importación finalizada!", MsgBoxStyle.Information)
        End If
    End Sub

    ' Abre un formulario con los detalles de una enfermedad
    Private Sub btnVer_Click(sender As Object, e As EventArgs) Handles btnVer.Click
        If tblEnfermedades.SelectedRows.Count = 1 Then
            Dim enfermedad As Enfermedad = tblEnfermedades.SelectedRows(0).Cells(0).Value
            Dim frm As New FrmVerEnfermedades(enfermedad)
            Me.Hide()
            frm.ShowDialog()
            Me.Show()
        Else
            MsgBox("Seleccione una sola fila para ver los detalles de la enfermedad.", MsgBoxStyle.Critical, "Error")
        End If
    End Sub

    ' Abre un formulario que permite al usuario ingresar una nueva patología
    Private Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Dim frm As New FrmAltaEnfermedades
        Me.Hide()
        frm.ShowDialog()
        Me.Show()
    End Sub

    ' Abre un formulario para que el usuario pueda ingresar nuevos datos para una enfermedad ya almacenada
    Private Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        If tblEnfermedades.SelectedRows.Count = 1 Then
            Dim enfermedad As Enfermedad = tblEnfermedades.SelectedRows(0).Cells(0).Value
            Dim frm As New FrmModificacionEnfermedades(enfermedad)
            Me.Hide()
            frm.ShowDialog()
            Me.Show()
        Else
            MsgBox("Seleccione una sola fila para modificar una enfermedad.", MsgBoxStyle.Critical, "Error")
        End If
    End Sub

    ' Permite eliminar una o varias de las enfermedades del sistema, luego de recibir confirmación del usuario
    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If tblEnfermedades.SelectedRows.Count > 0 Then
            If MsgBox("¿Confirma que desea eliminar esta(s) enfermedad(es)?" & vbNewLine &
                      "Estos cambios no podrán deshacerse.", MsgBoxStyle.YesNo, "Advertencia") = MsgBoxResult.Yes Then

                For Each r As DataGridViewRow In tblEnfermedades.SelectedRows
                    EliminarEnfermedad(r.Cells(0).Value)
                Next
                ActualizarEnfermedades()
            End If
        Else
            MsgBox("Seleccione al menos una fila para eliminar la(s) enfermedad(es).")
        End If
    End Sub

    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Me.Close()
    End Sub

    Private Sub lblTraducir_Click(sender As Object, e As EventArgs) Handles lblTraducir.Click
        Dim nombreIdioma As String = ""
        Select Case idiomaSeleccionado
            Case Idiomas.Espanol
                idiomaSeleccionado = Idiomas.Ingles
                nombreIdioma = "en"
            Case Idiomas.Ingles
                idiomaSeleccionado = Idiomas.Espanol
                nombreIdioma = "es"
        End Select

        Dim crmIdioma As New ComponentResourceManager(GetType(FrmListadoEnfermedades))
        For Each c As Control In Me.Controls
            crmIdioma.ApplyResources(c, c.Name, New CultureInfo(nombreIdioma))
        Next
    End Sub
End Class
