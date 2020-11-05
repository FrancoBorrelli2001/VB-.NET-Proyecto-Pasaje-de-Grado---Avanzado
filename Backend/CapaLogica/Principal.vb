﻿Imports Clases
Imports CapaPersistencia
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports System.Text
Imports Microsoft.VisualBasic.FileIO

Public Module Principal
    Public Function AutenticarUsuarioPaciente(ci As String, contrasena As String) As ResultadosLogin
        If Not TienePersonaRegistrada(ci, TiposUsuario.Paciente) Then
            Return ResultadosLogin.PersonaNoExiste
        End If

        Dim paciente As Paciente = ObtenerPacientePorCI(ci)
        If Not TieneUsuarioRegistrado(paciente) Then
            pacienteLogeado = paciente
            Return ResultadosLogin.SinUsuario
        End If

        Dim usuarioPaciente As Usuario = ObtenerUsuarioPorPersona(paciente)
        If CifrarClave(contrasena) = usuarioPaciente.Contrasena Then
            pacienteLogeado = paciente
            Return ResultadosLogin.OK
        Else
            Return ResultadosLogin.ContrasenaIncorrecta
        End If
    End Function

    Public Function CargarPacientePorCI(ci As String)
        Return ObtenerPacientePorCI(ci)
    End Function

    Public Function AutenticarUsuarioMedico(ci As String, contrasena As String) As ResultadosLogin
        If Not TienePersonaRegistrada(ci, TiposUsuario.Medico) Then
            Return ResultadosLogin.PersonaNoExiste
        End If

        Dim medico As Medico = ObtenerMedicoPorCI(ci)

        If Not TieneUsuarioRegistrado(medico) Then
            medicoLogeado = medico
            Return ResultadosLogin.SinUsuario
        End If

        Dim usuarioMedico As Usuario = ObtenerUsuarioPorPersona(medico)
        If CifrarClave(contrasena) = usuarioMedico.Contrasena Then
            medicoLogeado = medico
            Return ResultadosLogin.OK
        Else
            Return ResultadosLogin.ContrasenaIncorrecta
        End If
    End Function

    Public Function CargarMedicoPorCI(ci As String)
        Return ObtenerMedicoPorCI(ci)
    End Function

    Public Function AutenticarUsuarioAdministrativo(ci As String, contrasena As String) As ResultadosLogin
        If Not TienePersonaRegistrada(ci, TiposUsuario.Administrativo) Then
            Return ResultadosLogin.PersonaNoExiste
        End If

        Dim administrativo As Administrativo = ObtenerAdministrativoPorCI(ci)
        If Not TieneUsuarioRegistrado(administrativo) Then
            administrativoLogeado = administrativo
            Return ResultadosLogin.SinUsuario
        End If

        Dim usuarioAdministrativo As Usuario = ObtenerUsuarioPorPersona(administrativo)
        If CifrarClave(contrasena) = usuarioAdministrativo.Contrasena Then
            administrativoLogeado = administrativo
            Return ResultadosLogin.OK
        Else
            Return ResultadosLogin.ContrasenaIncorrecta
        End If
    End Function

    Public Function CargarAdministrativoPorCI(ci As String)
        Return ObtenerAdministrativoPorCI(ci)
    End Function

    Public Function CargarTodosLosSintomas() As List(Of Sintoma)
        Return ObtenerListadoSintomas()
    End Function

    Public Function CargarTodasLasEnfermedades() As List(Of Enfermedad)
        Return ObtenerListadoEnfermedades()
    End Function

    Public Function CargarTodasLasEspecialidades() As List(Of Especialidad)
        Return ObtenerListadoEspecialidades()
    End Function

    Public Function CargarTodosLosPacientes() As List(Of Paciente)
        Return ObtenerListadoPacientes()
    End Function

    Public Function CargarTodosLosMedicos() As List(Of Medico)
        Return ObtenerListadoMedicos()
    End Function

    Public Function CargarTodosLosAdministrativos() As List(Of Administrativo)
        Return ObtenerListadoAdministrativos()
    End Function

    Public Function DuplicarFila(filaADuplicar As DataGridViewRow) As DataGridViewRow
        Dim nuevaFila As DataGridViewRow = filaADuplicar.Clone
        For i = 0 To filaADuplicar.Cells.Count - 1
            nuevaFila.Cells(i).Value = filaADuplicar.Cells(0).Value
        Next
        Return nuevaFila
    End Function

    Public Function RealizarDiagnostico(sintomasIngresados As List(Of Sintoma)) As EnfermedadesDiagnosticadas

        Dim listaEnfermedadesPosibles As New List(Of Enfermedad)
        Dim listaProbabilidades As New List(Of Decimal)

        For Each e As Enfermedad In CargarTodasLasEnfermedades()
            Dim porcentajeProbabilidad As Decimal = DeterminarProbabilidadEnfermedad(sintomasIngresados, e)

            If porcentajeProbabilidad > 0 Then
                listaEnfermedadesPosibles.Add(e)
                listaProbabilidades.Add(porcentajeProbabilidad)
            End If
        Next

        Dim listaEnfermedadesOrdenada As New List(Of Enfermedad)
        Dim listaProbabilidadesOrdenada As New List(Of Decimal)
        For i = 0 To listaEnfermedadesPosibles.Count - 1
            Dim enfermedadMasProbable As Enfermedad = listaEnfermedadesPosibles(0)
            For j = 0 To listaEnfermedadesPosibles.Count - 1
                If listaProbabilidades(j) > listaProbabilidades(listaEnfermedadesPosibles.IndexOf(enfermedadMasProbable)) Then
                    enfermedadMasProbable = listaEnfermedadesPosibles(j)
                End If
            Next
            listaProbabilidadesOrdenada.Add(listaProbabilidades(listaEnfermedadesPosibles.IndexOf(enfermedadMasProbable)))
            listaEnfermedadesOrdenada.Add(enfermedadMasProbable)
            listaProbabilidades.Remove(listaProbabilidades(listaEnfermedadesPosibles.IndexOf(enfermedadMasProbable)))
            listaEnfermedadesPosibles.Remove(enfermedadMasProbable)
        Next

        Dim enfermedadesDiagnosticadas As New EnfermedadesDiagnosticadas(listaEnfermedadesOrdenada, listaProbabilidadesOrdenada)
        Return enfermedadesDiagnosticadas
    End Function

    Private Function DeterminarProbabilidadEnfermedad(sintomasIngresados As List(Of Sintoma), enfermedad As Enfermedad) As Decimal
        Dim cantidadSintomasDeLaEnfermedad As Integer = enfermedad.Sintomas.Count
        Dim cantidadSintomasCoincidentes As Integer = 0
        Dim listaFrecuencias As New List(Of Decimal)
        For i = 0 To cantidadSintomasDeLaEnfermedad - 1
            For Each sintomaIngresado As Sintoma In sintomasIngresados
                If sintomaIngresado.ID = enfermedad.Sintomas(i).ID Then
                    cantidadSintomasCoincidentes += 1
                    listaFrecuencias.Add(enfermedad.FrecuenciaSintoma(i))
                Else
                    listaFrecuencias.Add(0)
                End If
            Next
        Next

        Dim porcentajeProbabilidad As Double = 0
        For Each frecuencia As Decimal In listaFrecuencias
            porcentajeProbabilidad += frecuencia
        Next
        porcentajeProbabilidad /= cantidadSintomasDeLaEnfermedad
        Return porcentajeProbabilidad
    End Function

    Public Function CargarUltimosMensajesDiagnostico(diagnosticoPrimarioConConsulta As DiagnosticoPrimarioConConsulta, cantidad As Integer) As List(Of Mensaje)
        Dim mensajes As List(Of Mensaje) = ObtenerUltimosMensajesPorDiagnosticoPrimarioConConsulta(diagnosticoPrimarioConConsulta, cantidad)
        mensajes.Reverse()
        Return mensajes
    End Function

    Public Function CargarUltimosMetadatosArchivosDiagnostico(diagnosticoPrimarioConConsulta As DiagnosticoPrimarioConConsulta, cantidad As Integer) As List(Of Mensaje)
        Dim metadatosArchivos As List(Of Mensaje) = ObtenerUltimosMetadatosArchivosPorDiagnosticoPrimarioConConsulta(diagnosticoPrimarioConConsulta, cantidad)
        metadatosArchivos.Reverse()
        Return metadatosArchivos
    End Function

    Public Function ContarMensajes(diagnosticoPrimarioConConsulta As DiagnosticoPrimarioConConsulta) As Integer
        Return ObtenerCantidadMensajesPorDiagnosticoPrimarioConConsulta(diagnosticoPrimarioConConsulta)
    End Function

    Public Function ContarArchivos(diagnosticoPrimarioConConsulta As DiagnosticoPrimarioConConsulta) As Integer
        Return ObtenerCantidadArchivosPorDiagnosticoPrimarioConConsulta(diagnosticoPrimarioConConsulta)
    End Function

    Public Function CargarContenidoArchivoPorID(idArchivo As Integer) As Byte()
        Return ObtenerArchivoPorID(idArchivo).Contenido
    End Function

    Public Function ContarDiagnosticosDiferenciales(diagnosticoPrimarioConConsulta As DiagnosticoPrimarioConConsulta) As Integer
        Return ObtenerCantidadDiagnosticosDiferencialesPorDiagnosticoPrimarioConConsulta(diagnosticoPrimarioConConsulta)
    End Function

    Public Function CargarDiagnosticosDiferenciales(diagnosticoPrimarioConConsulta As DiagnosticoPrimarioConConsulta) As List(Of DiagnosticoDiferencial)
        Return ObtenerDiagnosticosDiferencialesPorDiagnosticoPrimarioConConsulta(diagnosticoPrimarioConConsulta)
    End Function

    Public Function CargarUltimoDiagnosticoDiferencialConsulta(diagnosticoPrimarioConConsulta As DiagnosticoPrimarioConConsulta) As DiagnosticoDiferencial
        Dim todosLosDiferenciales As List(Of DiagnosticoDiferencial) = CargarDiagnosticosDiferenciales(diagnosticoPrimarioConConsulta)
        Dim ultimaFecha As Date = Date.MinValue
        Dim indiceUltimaFecha As Integer = Integer.MinValue
        For i = 0 To todosLosDiferenciales.Count - 1
            If todosLosDiferenciales(i).FechaHora > ultimaFecha Then
                ultimaFecha = todosLosDiferenciales(i).FechaHora
                indiceUltimaFecha = i
            End If
        Next
        Return todosLosDiferenciales(indiceUltimaFecha)
    End Function

    Public Function CargarDiagnosticosPrimariosDelPaciente(paciente As Paciente) As List(Of DiagnosticoPrimario)
        Return ObtenerDiagnosticosPrimariosPorPaciente(paciente)
    End Function

    Public Function CargarConsultasSinAtenderParaMedicoLogeado() As List(Of DiagnosticoPrimarioConConsulta)
        Dim consultasSinAtender As List(Of DiagnosticoPrimarioConConsulta) = ObtenerConsultasSinAtender()
        Dim consultasFiltradasPorEspecialidad As New List(Of DiagnosticoPrimarioConConsulta)
        For Each d As DiagnosticoPrimarioConConsulta In consultasSinAtender
            Dim enfermedad As Enfermedad = d.Enfermedades(d.IndiceEnfermedadMasProbable)
            For Each e As Especialidad In medicoLogeado.Especialidades
                If e.ID = enfermedad.Especialidad.ID Then
                    consultasFiltradasPorEspecialidad.Add(d)
                End If
            Next
        Next

        Dim consultasRecientes, consultasAtrasadas As New List(Of DiagnosticoPrimarioConConsulta)
        For i = 0 To consultasFiltradasPorEspecialidad.Count - 1
            Dim consulta As DiagnosticoPrimarioConConsulta = consultasFiltradasPorEspecialidad(i)
            If consulta.FechaHora.AddDays(1) < Now Then
                consultasAtrasadas.Add(consulta)
            Else
                consultasRecientes.Add(consulta)
            End If
        Next

        Dim listaOrdenada As New List(Of DiagnosticoPrimarioConConsulta)
        consultasAtrasadas.Reverse()
        listaOrdenada.AddRange(consultasAtrasadas)

        If consultasRecientes.Count > 0 Then
            Dim consultasRecientesSegunGravedad As New List(Of DiagnosticoPrimarioConConsulta)
            For i = 0 To consultasRecientes.Count - 1
                Dim consultaMasGrave As DiagnosticoPrimarioConConsulta = consultasRecientes(0)
                For j = 0 To consultasRecientes.Count - 1
                    Dim consulta As DiagnosticoPrimarioConConsulta = consultasRecientes(j)
                    If consulta.Enfermedades(consulta.IndiceEnfermedadMasProbable).Gravedad > consultaMasGrave.Enfermedades(consultaMasGrave.IndiceEnfermedadMasProbable).Gravedad Then
                        consultaMasGrave = consulta
                    End If
                Next
                consultasRecientesSegunGravedad.Add(consultaMasGrave)
                consultasRecientes.Remove(consultaMasGrave)
            Next
            listaOrdenada.AddRange(consultasRecientesSegunGravedad)
        End If

        Return listaOrdenada
    End Function

    Public Function AsignarMedicoLogeadoAConsulta(consulta As DiagnosticoPrimarioConConsulta) As DiagnosticoPrimarioConConsulta
        Dim consultaConMedico As New DiagnosticoPrimarioConConsulta(consulta.ID, consulta.Paciente, consulta.Sintomas,
                                                                    consulta.EnfermedadesDiagnosticadas, consulta.FechaHora, medicoLogeado,
                                                                    consulta.ComentariosAdicionales)

        AsignarIDMedicoAConsulta(medicoLogeado, consulta)
        Return consultaConMedico
    End Function

    Public Function CargarConsultasMedico() As List(Of DiagnosticoPrimarioConConsulta)
        Return ObtenerUltimosDiagnosticosPrimariosConConsultaPorMedico(medicoLogeado)
    End Function

    Public Sub CrearSintoma(nombre As String, descripcion As String, recomendaciones As String, urgencia As Integer,
                              listaEnfermedades As List(Of Enfermedad), listaFrecuencias As List(Of Decimal))

        Dim nuevoSintoma As New Sintoma(nombre, descripcion, recomendaciones, urgencia, New EnfermedadesAsociadas(listaEnfermedades, listaFrecuencias))
        Try
            InsertarObjeto(nuevoSintoma, TiposObjeto.Sintoma)
        Catch ex As MySqlException
            Select Case ex.Number
                Case 1062
                    Throw New Exception("Ya existe un síntoma con ese nombre.")
                Case Else
                    Throw ex
            End Select
        End Try
    End Sub

    Public Sub CrearEnfermedad(nombre As String, descripcion As String, recomendaciones As String, gravedad As Integer,
                               listaSintomas As List(Of Sintoma), listaFrecuencias As List(Of Decimal), especialidad As Especialidad)

        Dim nuevaEnfermedad As New Enfermedad(nombre, recomendaciones, gravedad, descripcion,
                                              New SintomasAsociados(listaSintomas, listaFrecuencias), especialidad)

        Try
            InsertarObjeto(nuevaEnfermedad, TiposObjeto.Enfermedad)
        Catch ex As MySqlException
            Select Case ex.Number
                Case 1062
                    Throw New Exception("Ya existe una enfermedad con ese nombre.")
                Case Else
                    Throw ex
            End Select
        End Try
    End Sub

    Public Sub CrearMedico(ci As String, nombre As String, apellido As String, correo As String, localidad As Localidad, especialidades As List(Of Especialidad))
        Dim nuevoMedico As New Medico(ci, nombre, apellido, correo, localidad, especialidades)
        Try
            InsertarObjeto(nuevoMedico, TiposObjeto.Medico)
        Catch ex As MySqlException
            Select Case ex.Number
                Case 1062
                    Throw New Exception("Ya existe un medico con esa cédula.")
                Case Else
                    Throw ex
            End Select
        End Try

        Try
            EnviarCorreoAlta(nuevoMedico)
        Catch ex As Exception
            Throw New Exception("No se pudo enviar el correo electrónico de alta al médico.")
        End Try

    End Sub

    Public Sub CrearAdministrativo(ci As String, nombre As String, apellido As String, correo As String, localidad As Localidad, esEncargado As Boolean)
        Dim nuevoAdministrativo As New Administrativo(ci, nombre, apellido, correo, localidad, esEncargado)

        Try
            InsertarObjeto(nuevoAdministrativo, TiposObjeto.Administrativo)
        Catch ex As MySqlException
            Select Case ex.Number
                Case 1062
                    Throw New Exception("Ya existe un administrativo con esa cédula.")
                Case Else
                    Throw ex
            End Select
        End Try

        Try
            EnviarCorreoAlta(nuevoAdministrativo)
        Catch ex As Exception
            Throw New Exception("No se pudo enviar el correo electrónico de alta al administrativo.")
        End Try

    End Sub

    Public Sub CrearPaciente(ci As String, nombre As String, apellido As String, correo As String, localidad As Localidad, telefonoMovil As String,
            telefonoFijo As String, sexo As TiposSexo, fechaNacimiento As Date, fechaDeFuncion As Date, calle As String, numeroPuerta As String, apartamento As String)
        Dim nuevoPaciente As New Paciente(ci, nombre, apellido, correo, localidad, telefonoMovil, telefonoFijo, sexo, fechaNacimiento, fechaDeFuncion, calle, numeroPuerta, apartamento)
        Try
            InsertarObjeto(nuevoPaciente, TiposObjeto.Paciente)
        Catch ex As MySqlException
            Select Case ex.Number
                Case 1062
                    Throw New Exception("Ya existe un paciente con esa cédula.")
                Case Else
                    Throw ex
            End Select
        End Try

        Try
            EnviarCorreoAlta(nuevoPaciente)
        Catch ex As Exception
            Throw New Exception("No se pudo enviar el correo electrónico de alta al paciente.")
        End Try

    End Sub

    Public Function CrearDiagnosticoPrimario(paciente As Paciente, sintomas As List(Of Sintoma), enfermedadesDiagnosticadas As EnfermedadesDiagnosticadas) As DiagnosticoPrimario
        Dim fechaHoraDiagnostico As Date = Now
        Dim nuevoDiagnostico As New DiagnosticoPrimario(paciente, sintomas, enfermedadesDiagnosticadas, fechaHoraDiagnostico,
                                                        TiposDiagnosticosPrimarios.Sin_Consulta)

        Dim idAsignado As Integer
        InsertarObjeto(nuevoDiagnostico, TiposObjeto.DiagnosticoPrimario, idAsignado)
        Dim diagnosticoInsertado As New DiagnosticoPrimario(idAsignado, paciente, sintomas, enfermedadesDiagnosticadas, fechaHoraDiagnostico,
                                                            TiposDiagnosticosPrimarios.Sin_Consulta)

        Return diagnosticoInsertado
    End Function

    Public Function CrearDiagnosticoDiferencial(consulta As DiagnosticoPrimarioConConsulta, enfermedadDiagnosticada As Enfermedad, conductaASeguir As String) As DiagnosticoDiferencial
        Dim nuevoDiagnostico As New DiagnosticoDiferencial(consulta, enfermedadDiagnosticada, conductaASeguir, Now)
        InsertarObjeto(nuevoDiagnostico, TiposObjeto.DiagnosticoDiferencial)
        Return nuevoDiagnostico
    End Function

    Public Function AgregarConsultaADiagnostico(diagnosticoPrimario As DiagnosticoPrimario, comentariosAdicionales As String) As DiagnosticoPrimarioConConsulta
        Dim nuevoDiagnosticoConConsulta As New DiagnosticoPrimarioConConsulta(diagnosticoPrimario, comentariosAdicionales)
        InsertarObjeto(nuevoDiagnosticoConConsulta, TiposObjeto.DiagnosticoPrimarioConConsulta)
        Return nuevoDiagnosticoConConsulta
    End Function

    Public Function EnviarMensaje(formatoMensaje As FormatosMensajeAdmitidos, contenido As Byte(), remitente As TiposRemitente,
                             diagnosticoEnCurso As DiagnosticoPrimarioConConsulta, Optional nombreArchivo As String = Nothing) As Mensaje

        Dim nuevoMensaje As New Mensaje(Now, formatoMensaje, contenido, nombreArchivo, remitente, diagnosticoEnCurso)
        InsertarObjeto(nuevoMensaje, TiposObjeto.Mensaje)
        Return nuevoMensaje
    End Function

    Public Sub ActualizarSintoma(sintomaViejo As Sintoma, nombre As String, descripcion As String, recomendaciones As String, urgencia As Integer,
                                 listaEnfermedades As List(Of Enfermedad), listaFrecuencias As List(Of Decimal))

        Dim nuevoSintoma As New Sintoma(sintomaViejo.ID, nombre, descripcion, recomendaciones, urgencia,
                                        New EnfermedadesAsociadas(listaEnfermedades, listaFrecuencias), sintomaViejo.Habilitado)
        ModificarObjeto(nuevoSintoma, TiposObjeto.Sintoma)
    End Sub

    Public Sub ActualizarEnfermedad(enfermedadVieja As Enfermedad, nombre As String, descripcion As String, recomendaciones As String, gravedad As Integer,
                                    listaSintomas As List(Of Sintoma), listaFrecuencias As List(Of Decimal), especialidad As Especialidad)

        Dim nuevaEnfermedad As New Enfermedad(enfermedadVieja.Id, nombre, recomendaciones, gravedad, descripcion,
                                              New SintomasAsociados(listaSintomas, listaFrecuencias), especialidad, enfermedadVieja.Habilitada)
        ModificarObjeto(nuevaEnfermedad, TiposObjeto.Enfermedad)
    End Sub

    Public Sub ActualizarPaciente(pacienteViejo As Paciente, ci As String, nombre As String, apellido As String, correo As String, localidad As Localidad,
                                  telefonoMovil As String, telefonoFijo As String, sexo As TiposSexo, fechaNacimiento As Date, fechaDeFuncion As Date,
                                  calle As String, numeroPuerta As String, apartamento As String)
        Dim nuevoPaciente As New Paciente(pacienteViejo.ID, ci, nombre, apellido, correo, localidad, telefonoMovil, telefonoFijo, sexo, fechaNacimiento, fechaDeFuncion, calle, numeroPuerta, apartamento)

        ModificarObjeto(nuevoPaciente, TiposObjeto.Paciente)
    End Sub

    Public Sub ActualizarAdministrativo(administrativoViejo As Administrativo, ci As String, nombre As String, apellido As String, correo As String, localidad As Localidad, EsEncargado As Boolean, habilitado As Boolean)

        Dim nuevoAdministrativo As New Administrativo(administrativoViejo.ID, ci, nombre, apellido, correo, localidad, EsEncargado, habilitado)
        ModificarObjeto(nuevoAdministrativo, TiposObjeto.Administrativo)
    End Sub


    Public Sub ActualizarMedico(medicoViejo As Medico, ci As String, nombre As String, apellido As String, correo As String, localidad As Localidad, especialidades As List(Of Especialidad))

        Dim nuevoMedico As New Medico(medicoViejo.ID, ci, nombre, apellido, correo, localidad, especialidades, True)

        ModificarObjeto(nuevoMedico, TiposObjeto.Medico)
    End Sub

    Public Sub EliminarSintoma(sintoma As Sintoma)
        EliminarObjeto(sintoma, TiposObjeto.Sintoma)
    End Sub

    Public Sub EliminarEnfermedad(enfermedad As Enfermedad)
        EliminarObjeto(enfermedad, TiposObjeto.Enfermedad)
    End Sub

    Public Sub EliminarPaciente(paciente As Paciente)
        EliminarObjeto(paciente, TiposObjeto.Paciente)
    End Sub
    Public Sub EliminarMedico(medico As Medico)
        EliminarObjeto(medico, TiposObjeto.Medico)
    End Sub

    Public Sub EliminarAdministrativo(administrativo As Administrativo)
        EliminarObjeto(administrativo, TiposObjeto.Administrativo)
    End Sub

    Public Sub ImportarEnfermedades(nombreArchivo As String)
        Dim filasParseadas As New List(Of String())
        Dim parser As New TextFieldParser(nombreArchivo, Encoding.UTF8)
        parser.TextFieldType = FieldType.Delimited
        parser.SetDelimiters(",")
        While Not parser.EndOfData
            filasParseadas.Add(parser.ReadFields)
        End While

        If filasParseadas.Count Mod 3 <> 0 Then
            Throw New Exception("Error de formato en el archivo importado. La cantidad de filas debe ser múltiplo de 3.")
        End If

        Dim especialidadesCargadas As List(Of Especialidad) = CargarTodasLasEspecialidades()
        Dim sintomasCargados As List(Of Sintoma) = CargarTodosLosSintomas()
        Dim listaEnfermedadesImportadas As New List(Of Enfermedad)

        For i = 0 To filasParseadas.Count - 1 Step 3
            Dim datosEnfermedad As String() = filasParseadas(i)
            Dim nombre As String = datosEnfermedad(0)
            Dim descripcion As String = datosEnfermedad(1)
            Dim recomendaciones As String = datosEnfermedad(2)
            Dim gravedad As Integer = datosEnfermedad(3)
            Dim especialidad As Especialidad = Nothing

            Dim nombreEspecialidad As String = datosEnfermedad(4)
            For Each e As Especialidad In especialidadesCargadas
                If e.Nombre = nombreEspecialidad Then
                    especialidad = e
                End If
            Next
            If especialidad Is Nothing Then
                Throw New Exception(String.Format("La especialidad indicada para la enfermedad ""{0}"" ({1}) no se encuentra registrada en el sistema.",
                                                  nombre, nombreEspecialidad))
            End If

            Dim j As Integer = 0
            While j < filasParseadas(i + 2).Length AndAlso filasParseadas(i + 2)(j) <> ""
                Dim valor As String = filasParseadas(i + 2)(j)
                Dim valorConvertido As Decimal
                If Decimal.TryParse(valor, valorConvertido) = False Then
                    Throw New Exception(String.Format("Error de formato: las frecuencias de los síntomas de la enfermedad ""{0}"" no tienen un formato válido.",
                                                      nombre))
                End If
                j += 1
            End While

            Dim cantSintomas As Integer = j
            Dim nombresSintomas As String() = filasParseadas(i + 1).Take(cantSintomas).ToArray
            Dim frecuenciasSintomas As String() = filasParseadas(i + 2).Take(cantSintomas).ToArray

            Dim listaSintomas As New List(Of Sintoma)
            Dim listaFrecuencias As New List(Of Decimal)

            For j = 0 To nombresSintomas.Length - 1
                Dim sintoma As Sintoma = Nothing
                For Each s In sintomasCargados
                    If nombresSintomas(j) = s.Nombre Then
                        sintoma = s
                    End If
                Next
                If sintoma Is Nothing Then
                    Throw New Exception(String.Format("El síntoma ""{0}"" de la enfermedad ""{1}"" no se encuentra registrado en el sistema.",
                                                       nombresSintomas(j), nombre))
                Else
                    listaSintomas.Add(sintoma)
                    listaFrecuencias.Add(CDec(frecuenciasSintomas(j)))
                End If
            Next

            Dim sintomasAsociados As New SintomasAsociados(listaSintomas, listaFrecuencias)
            listaEnfermedadesImportadas.Add(New Enfermedad(nombre, recomendaciones, gravedad, descripcion, sintomasAsociados, especialidad))
        Next

        InsertarEnfermedadesImportadas(listaEnfermedadesImportadas)
    End Sub

    Public Sub ImportarSintomas(nombreArchivo As String)
        Dim filasParseadas As New List(Of String())
        Dim parser As New TextFieldParser(nombreArchivo, Encoding.UTF8)
        parser.TextFieldType = FieldType.Delimited
        parser.SetDelimiters(",")
        While Not parser.EndOfData
            filasParseadas.Add(parser.ReadFields)
        End While

        If filasParseadas.Count Mod 3 <> 0 Then
            Throw New Exception("Error de formato en el archivo importado. La cantidad de filas debe ser múltiplo de 3.")
        End If

        Dim enfermedadesCargadas As List(Of Enfermedad) = CargarTodasLasEnfermedades()
        Dim listaSintomasImportados As New List(Of Sintoma)

        For i = 0 To filasParseadas.Count - 1 Step 3
            Dim datosSintoma As String() = filasParseadas(i)
            Dim nombre As String = datosSintoma(0)
            Dim descripcion As String = datosSintoma(1)
            Dim recomendaciones As String = datosSintoma(2)
            Dim urgencia As Integer = datosSintoma(3)

            Dim j As Integer = 0
            While j < filasParseadas(i + 2).Length AndAlso filasParseadas(i + 2)(j) <> ""
                Dim valor As String = filasParseadas(i + 2)(j)
                Dim valorConvertido As Decimal
                If Decimal.TryParse(valor, valorConvertido) = False Then
                    Throw New Exception(String.Format("Error de formato: las frecuencias de las enfermedades del síntoma ""{0}"" no tienen un formato válido.",
                                                      nombre))
                End If
                j += 1
            End While

            Dim cantEnfermedades As Integer = j
            Dim nombresEnfermedades As String() = filasParseadas(i + 1).Take(cantEnfermedades).ToArray
            Dim frecuenciasEnfermedades As String() = filasParseadas(i + 2).Take(cantEnfermedades).ToArray

            Dim listaEnfermedades As New List(Of Enfermedad)
            Dim listaFrecuencias As New List(Of Decimal)

            For j = 0 To nombresEnfermedades.Length - 1
                Dim enfermedad As Enfermedad = Nothing
                For Each e In enfermedadesCargadas
                    If nombresEnfermedades(j) = e.Nombre Then
                        enfermedad = e
                    End If
                Next
                If enfermedad Is Nothing Then
                    Throw New Exception(String.Format("La enfermedad ""{0}"" del síntoma ""{1}"" no se encuentra registrada en el sistema.",
                                                       nombresEnfermedades(j), nombre))
                Else
                    listaEnfermedades.Add(enfermedad)
                    listaFrecuencias.Add(frecuenciasEnfermedades(j))
                End If
            Next

            Dim enfermedadesAsociadas As New EnfermedadesAsociadas(listaEnfermedades, listaFrecuencias)
            listaSintomasImportados.Add(New Sintoma(nombre, descripcion, recomendaciones, urgencia, enfermedadesAsociadas))
        Next

        InsertarSintomasImportados(listaSintomasImportados)
    End Sub

    Public Sub RegistrarUsuario(persona As Persona, contrasena As String)
        Dim nuevoUsuario As New Usuario(CifrarClave(contrasena), persona)
        InsertarObjeto(nuevoUsuario, TiposObjeto.Usuario)
        Dim ventanaEspera As New FrmEsperar()
        ventanaEspera.Show()
        EnviarCorreoRegistro(persona, contrasena)
        ventanaEspera.Close()
    End Sub


    Public Function CargarTodosLosDepartamentos() As List(Of Departamento)
        Return ObtenerListadoDepartamentos()
    End Function

    Public Function CargarTodasLasLocalidades() As List(Of Localidad)
        Return ObtenerListadoLocalidades()
    End Function

    Public Function ExistenPersonasEnLocalidad(l As Localidad) As Boolean
        Return ObtenerCantidadPersonasEnLocalidad(l) > 0
    End Function

    Public Sub CrearDepartamento(nombre As String)
        Dim nuevoDepartamento As New Departamento(nombre)
        InsertarObjeto(nuevoDepartamento, TiposObjeto.Departamento)
    End Sub

    Public Sub CrearLocalidad(nombre As String, departamento As Departamento)
        Dim nuevaLocalidad As New Localidad(nombre, departamento)
        InsertarObjeto(nuevaLocalidad, TiposObjeto.Localidad)
    End Sub

    Public Sub CrearEspecialidad(nombre As String)
        Dim nuevaEspecialidad As New Especialidad(nombre, Nothing)
        InsertarObjeto(nuevaEspecialidad, TiposObjeto.Especialidad)
    End Sub

    Public Sub ActualizarDepartamento(departamentoViejo As Departamento, nombre As String)
        Dim nuevoDepartamento As New Departamento(departamentoViejo.ID, nombre)
        ModificarObjeto(nuevoDepartamento, TiposObjeto.Departamento)
    End Sub

    Public Sub ActualizarLocalidad(localidadVieja As Localidad, nombre As String, departamento As Departamento)
        Dim nuevaLocalidad As New Localidad(localidadVieja.ID, nombre, departamento)
        ModificarObjeto(nuevaLocalidad, TiposObjeto.Localidad)
    End Sub

    Public Sub ActualizarEspecialidad(especialidadVieja As Especialidad, nombre As String)
        Dim nuevaEspecialidad As New Especialidad(especialidadVieja.ID, nombre, Nothing, True)
        ModificarObjeto(nuevaEspecialidad, TiposObjeto.Especialidad)
    End Sub

    Public Sub EliminarDepartamento(departamento As Departamento)
        EliminarObjeto(departamento, TiposObjeto.Departamento)
    End Sub

    Public Sub EliminarLocalidad(localidad As Localidad)
        EliminarObjeto(localidad, TiposObjeto.Localidad)
    End Sub

    Public Sub EliminarEspecialidad(especialidad As Especialidad)
        EliminarObjeto(especialidad, TiposObjeto.Especialidad)
    End Sub

    Public Function ConsultaTieneMedico(consulta As DiagnosticoPrimarioConConsulta)
        Dim consultasSinAtender As List(Of DiagnosticoPrimarioConConsulta) = ObtenerConsultasSinAtender()
        For Each d As DiagnosticoPrimarioConConsulta In consultasSinAtender
            If consulta.ID = d.ID And d.Medico Is Nothing Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Sub CargarMedicoEnConsulta(ByRef consulta As DiagnosticoPrimarioConConsulta)
        Dim consultas As List(Of DiagnosticoPrimario) = ObtenerDiagnosticosPrimariosPorPaciente(consulta.Paciente)
        For Each c As DiagnosticoPrimario In consultas
            If consulta.ID = c.ID Then
                consulta = c
            End If
        Next
    End Sub
End Module
