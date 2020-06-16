﻿Public Class Sintoma
    Private _Recomendaciones As String
    Private _Nombre As String
    Private _Descripcion As String
    Private _Urgencia As Integer
    Private _Frecuencia As Integer

    Public Property Recomendaciones As String
        Get
            Return _Recomendaciones
        End Get
        Set(value As String)
            Try
                If value.Length >= 1000 Then
                    Throw New Exception("El largo de las recomendaciones no debe superar los 1000 caracteres.")
                End If
                _Recomendaciones = value
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End Set
    End Property

    Public Property Nombre As String
        Get
            Return _Nombre
        End Get
        Set(value As String)
            Try
                If value = Nothing Or value = "" Then
                    Throw New Exception("El nombre se encuentra vacío.")
                End If
                If value.Length >= 100 Then
                    Throw New Exception("El largo del nombre no debe superar los 100 caracteres.")
                End If
                _Nombre = value
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End Set
    End Property

    Public Property Descripcion As String
        Get
            Return _Descripcion
        End Get
        Set(value As String)
            Try

                If value.Length >= 1000 Then
                    Throw New Exception("La cantidad de caracteres debe ser menor a 1000.")
                End If
                _Descripcion = value
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End Set
    End Property

    Public Property Urgencia As Integer
        Get
            Return _Urgencia
        End Get
        Set(value As Integer)
            Try
                If value < 0 Or value > 100 Then
                    Throw New Exception("El índice de urgencia debe ser un valor entero entre 0 y 100.")
                End If
                If value = Nothing Then
                    Throw New Exception("El índice de urgencia se encuentra vacío.")
                End If
                _Urgencia = value
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End Set
    End Property

    Public Property Frecuencia As Integer
        Get
            Return _Frecuencia
        End Get
        Set(value As Integer)
            Try
                If value < 0 Or value > 100 Then
                    Throw New Exception("La frecuencia de un síntoma en una enfermedad debe ser un valor entero entre 0 y 100.")
                End If
                _Urgencia = value
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
            End Try
        End Set
    End Property

    Public Sub New()
        _Frecuencia = Nothing
    End Sub

    Public Sub New(nombre As String, descripcion As String, recomendaciones As String, urgencia As Integer)
        Me.New()
        _Nombre = nombre
        _Descripcion = descripcion
        _Recomendaciones = recomendaciones
        _Urgencia = urgencia
    End Sub

    Public Overrides Function ToString() As String
        Return "Nombre: " + _Nombre.ToString() + " Descripción: " + _Descripcion.ToString() + " Urgencia: " + _Urgencia.ToString() + " Recomendaciones: " + _Recomendaciones.ToString()
    End Function
End Class