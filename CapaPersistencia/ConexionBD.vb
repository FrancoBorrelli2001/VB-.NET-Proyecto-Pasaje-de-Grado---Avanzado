﻿Imports MySql.Data.MySqlClient
Public NotInheritable Class ConexionBD
    Private Shared _Conexion As MySqlConnection
    Private Shared Adaptador As MySqlDataAdapter
    Private Shared ReadOnly _Relaciones As New List(Of RelacionBD)

    Public Shared ReadOnly Property Conexion() As MySqlConnection
        Get
            ' Si no se inicializó la conexión, lo hace con los datos de login en la BD
            If _Conexion Is Nothing Then
                Dim url As String = "hellocode.dynns.com"
                Dim nombreBD As String = "hellocare"
                Dim usuario As String = "root"
                Dim contrasena As String = "rootmariadb"

                Dim cadenaConexion As String = String.Format("server={0};database={1};userid={2};password={3}", url, nombreBD, usuario, contrasena)

                _Conexion = New MySqlConnection(cadenaConexion)
            End If

            Return _Conexion
        End Get
    End Property

    Public Shared Function EjecutarConsulta(comando As String, nombreTabla As String) As DataSet
        Dim datos As New DataSet                                        ' Declara un DataSet vacío
        Adaptador = New MySqlDataAdapter(comando, Conexion)             ' Reinicializa el adaptador para ejecutar el nuevo comando SQL
        Adaptador.Fill(datos, nombreTabla)                              ' Llena el DataSet
        Adaptador.FillSchema(datos, SchemaType.Mapped, nombreTabla)     ' Carga en la tabla del DataSet la clave primaria, de acuerdo con la BD
        Return datos
    End Function

    Public Shared Sub AplicarClavesExternas(datos As DataSet)
        ActualizarClavesExternas()

        ' Crea una lista con los nombres de las tablas
        Dim nombresTablas As New List(Of String)
        For Each t As DataTable In datos.Tables
            nombresTablas.Add(t.TableName)
        Next

        ' Para cada FK almacenada, se fija si la lista de los nombres de las tablas en el DataSet contiene los nombres de ambas tablas de la relación.
        ' En caso afirmativo, agrega la relación entre las dos tablas al DataSet.
        For Each relacion As RelacionBD In _Relaciones
            If nombresTablas.Contains(relacion.NombreTablaPrimaria) And nombresTablas.Contains(relacion.NombreTablaExterna) Then
                relacion.AgregarRelacion(datos)
            End If
        Next
    End Sub

    Private Shared Sub ActualizarClavesExternas()
        _Relaciones.Clear()
        ' Llena un DataSet con los datos de las claves externas de la BD

        'Dim datosClaves As New DataSet
        'Dim consultaClaves As String = "SELECT constraint_name, table_name, column_name, referenced_table_name, referenced_column_name
        '                                FROM information_schema.key_column_usage
        '                                WHERE table_schema='hellocare' AND referenced_table_name IS NOT NULL"
        'Dim adaptadorClaves As New MySqlDataAdapter(consultaClaves, Conexion)
        'adaptadorClaves.Fill(datosClaves, "key_column_usage")

        'Dim lista As New List(Of RelacionBD)

        Dim datos As DataSet = EjecutarConsulta("SELECT constraint_name, table_name, column_name, referenced_table_name, referenced_column_name
                                                 FROM information_schema.key_column_usage
                                                 WHERE table_schema='hellocare' AND referenced_table_name IS NOT NULL", "key_column_usage")

        Dim tabla As DataTable = datos.Tables("key_column_usage")

        ' Para cada fila de la tabla de FK, las agrega a la lista que es atributo de la clase
        For Each r As DataRow In tabla.Rows
            Dim nombre As String = r("constraint_name")
            Dim nombreTablaPrimaria As String = r("table_name")
            Dim nombreClavePrimaria As String = r("column_name")
            Dim nombreTablaExterna As String = r("referenced_table_name")
            Dim nombreClaveExterna As String = r("referenced_column_name")

            _Relaciones.Add(New RelacionBD(nombre, nombreTablaPrimaria, nombreClavePrimaria, nombreTablaExterna, nombreClaveExterna))
        Next
    End Sub
End Class
