﻿Public Class DiagnosticoDiferencial
    Private ReadOnly _ID As Integer
    Private ReadOnly _DiagnosticoPrimarioConConsulta As DiagnosticoPrimarioConConsulta
    Private ReadOnly _EnfermedadDiagnosticada As Enfermedad
    Private ReadOnly _ConductaASeguir As String
    Private ReadOnly _FechaHora As Date

    Public ReadOnly Property ID As Integer
        Get
            Return _ID
        End Get
    End Property

    Public ReadOnly Property DiagnosticoPrimarioConConsulta As DiagnosticoPrimarioConConsulta
        Get
            Return _DiagnosticoPrimarioConConsulta
        End Get
    End Property

    Public ReadOnly Property EnfermedadDiagnosticada As Enfermedad
        Get
            Return _EnfermedadDiagnosticada
        End Get
    End Property

    Public ReadOnly Property ConductaASeguir As String
        Get
            Return _ConductaASeguir
        End Get
    End Property

    Public ReadOnly Property FechaHora As Date
        Get
            Return _FechaHora
        End Get
    End Property

    Public Sub New(diagnosticoPrimarioConConsulta As DiagnosticoPrimarioConConsulta, enfermedadDiagnosticada As Enfermedad, conductaASeguir As String,
                   fechaHora As Date)
        _ID = Integer.MinValue
        _DiagnosticoPrimarioConConsulta = diagnosticoPrimarioConConsulta
        _EnfermedadDiagnosticada = enfermedadDiagnosticada
        _ConductaASeguir = conductaASeguir
        _FechaHora = fechaHora
    End Sub

    Public Sub New(id As Integer, diagnosticoPrimarioConConsulta As DiagnosticoPrimarioConConsulta, enfermedadDiagnosticada As Enfermedad,
                   conductaASeguir As String, fechaHora As Date)
        _ID = id
        _DiagnosticoPrimarioConConsulta = diagnosticoPrimarioConConsulta
        _EnfermedadDiagnosticada = enfermedadDiagnosticada
        _ConductaASeguir = conductaASeguir
        _FechaHora = fechaHora
    End Sub
End Class
