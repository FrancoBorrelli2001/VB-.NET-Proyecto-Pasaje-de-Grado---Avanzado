﻿Imports CapaLogica
Imports Clases

Public Class FrmAltaSintomas
    Private Sub FrmAltaSintomas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each enfermedad As Enfermedad In CargarTodasLasEnfermedades()
            tblPatologias.Rows.Add(enfermedad, False, enfermedad.Nombre, "")
        Next
        tblPatologias.ClearSelection()
    End Sub

    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Me.Close()
    End Sub

    Private Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        ' Determina si se asociaron patologías, si no se hizo mostrará una advertencia
        Dim hayPatologiasAsociadas As Boolean = False
        For Each r As DataGridViewRow In tblPatologias.Rows
            If r.Cells(1).Value = True Then
                hayPatologiasAsociadas = True
            End If
        Next

        ' Procede a insertar el nuevo síntoma si se ingresaron patologías o si se confirma que se desea insertarlo sin patologías asociadas
        If hayPatologiasAsociadas OrElse
           (Not hayPatologiasAsociadas And MsgBox("Advertencia: no se asoció el nuevo síntoma a ninguna patología del sistema." & vbNewLine &
                                                  "¿Desea continuar de todas formas?", MsgBoxStyle.YesNo, "Aviso") = MsgBoxResult.Yes) Then

            Try
                Dim listaEnfermedades As New List(Of Enfermedad)
                Dim listaFrecuencias As New List(Of Decimal)
                For Each r As DataGridViewRow In tblPatologias.Rows
                    If r.Cells(1).Value = True Then
                        listaEnfermedades.Add(CType(r.Cells(0).Value, Enfermedad))
                        Try
                            listaFrecuencias.Add(r.Cells(3).Value.ToString.Replace("%", ""))
                        Catch ex As Exception
                            Throw New Exception("Las frecuencias solo pueden ser valores numéricos.")
                        End Try
                    End If
                Next

                CrearSintoma(txtNombre.Text, txtDescripcion.Text, txtRecomendaciones.Text, txtUrgencia.Text, listaEnfermedades, listaFrecuencias)
                MsgBox("Síntoma agregado con éxito.", MsgBoxStyle.OkOnly, "Éxito")
                Me.Close()
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End If
    End Sub

    ' Si el formulario se cierra sin crear un síntoma, pide al usuario confirmación para abandonar la ventana.
    Private Sub FrmAltaSintomas_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If MsgBox("Advertencia: no se guardaron los cambios." & vbNewLine & "¿Confirma que desea cerrar la ventana?", MsgBoxStyle.YesNo, "Salir") =
            MsgBoxResult.No Then
            e.Cancel = True
        End If
    End Sub
End Class
