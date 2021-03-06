Imports System.IO
Imports System.Security.AccessControl
Imports System.Security.Cryptography
Imports System.Security.Principal
Imports System.Text
Imports Microsoft.Win32
Public Class Main
    Dim tripleDESCryptoServiceProvider_0 As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()
    Dim md5CryptoServiceProvider_0 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider()

    Dim Directorio1 As String = "C:\Users\" + Environment.UserName + "\AppData\Local\Worcome_Studios\Commons\MyChat_EX"
    Dim DB1 As String = Directorio1 & "\APP_DB.dat"
    Dim DB2 As String = Directorio1 & "\USR_DB.dat"

    Dim Registro As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\\Worcome_Studios\\" & "WorMyChatEX", True)

    Dim LlaveApp As String = "O91aHsTMe1fBCrRrp3YS"
    Dim LlaveLicencias As String = "APN5QIkEEFpe7gjenLtzJcv5Hx8ñOs"

    Dim ArchivoDeConfiguracionDelServidor As String

    'APP DB
    Dim LlaveAppDB As String
    Dim LicenseDB As String
    Dim UserIDDB As String
    Dim IsRegistered As String

    'USER DB
    Dim UserNameDB As String
    Dim PasswordDB As String
    Dim EmailDB As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'GetPrimeraDB() 'APP DB
        'GetSegundaDB() 'USER DB
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        GetPrimeraDB() 'APP DB
        GetSegundaDB() 'USER DB
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        UserNameDB = TextBox1.Text
        PasswordDB = TextBox2.Text
        EmailDB = TextBox3.Text

        LlaveAppDB = TextBox4.Text
        LicenseDB = TextBox5.Text
        UserIDDB = TextBox6.Text
        IsRegistered = TextBox7.Text

        SavePrimeraDB() 'APP DB
        SaveSegundaDB() 'USER DB
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'Licencia
        Try
            Dim LicenseString As String = "#Activated|" & DateTime.Now.ToString("yyyyMMdd") & "|WorMyChatEX|1.3.4.0|zhenboro@outlook.com|QwErTy|" & DateTime.Now.ToString("dd/MM/yyyy") & "|" & DateTime.Now.ToString("dd/MM/") & DateTime.Now.ToString("yyyy") + 1 & "|True|uwuowoeweawa"
            Dim Licencia = InputBox("Modifique los valores", "Licencia", LicenseString)
            If Licencia = Nothing Then
            Else
                Registro.SetValue("AppLicenser", Cifrar(Licencia, LlaveLicencias))
                RichTextBox1.AppendText(vbCrLf & "Licencia agregada")
                RichTextBox1.ScrollToCaret()
            End If
        Catch
        End Try
    End Sub

    Sub GetPrimeraDB() 'APP DB
        Try
            Dim RAWContent As String = My.Computer.FileSystem.ReadAllText(DB1)
            Dim DecryptContent As String = Descifrar(RAWContent, LlaveApp)
            Dim lines As String() = New TextBox() With {
            .Text = DecryptContent
            }.Lines
            RichTextBox1.AppendText(vbCrLf & "<--- DB Raw Content --->")
            RichTextBox1.AppendText(vbCrLf & RAWContent)
            RichTextBox1.AppendText(vbCrLf & "<--- DB Decrypted Content Loaded --->")
            RichTextBox1.AppendText(vbCrLf & DecryptContent)
            Me.LlaveAppDB = lines(1).Split(New Char() {">"c})(1).Trim()
            LicenseDB = lines(2).Split(New Char() {">"c})(1).Trim()
            UserIDDB = lines(3).Split(New Char() {">"c})(1).Trim()
            IsRegistered = lines(4).Split(New Char() {">"c})(1).Trim()

            TextBox4.Text = LlaveAppDB
            TextBox5.Text = LicenseDB
            TextBox6.Text = UserIDDB
            TextBox7.Text = IsRegistered
            Console.WriteLine("APP DB:" &
                              vbCrLf &
                              "    " & LlaveAppDB & vbCrLf &
                              "    " & LicenseDB & vbCrLf &
                              "    " & UserIDDB & vbCrLf &
                              "    " & IsRegistered)
            RichTextBox1.ScrollToCaret()
        Catch ex As Exception
            Console.WriteLine("[Debugger@GetAppData]Error: " + ex.Message)
        End Try
    End Sub

    Sub GetSegundaDB() 'USER DB
        Try
            Dim RAWContent As String = My.Computer.FileSystem.ReadAllText(DB2)
            Dim DecryptContent As String = Descifrar(RAWContent, Me.LlaveAppDB)
            Dim lines As String() = New TextBox() With {
            .Text = DecryptContent
            }.Lines
            RichTextBox1.AppendText(vbCrLf & "<--- DB Raw Content --->")
            RichTextBox1.AppendText(vbCrLf & RAWContent)
            RichTextBox1.AppendText(vbCrLf & "<--- DB Decrypted Content Loaded --->")
            RichTextBox1.AppendText(vbCrLf & DecryptContent)
            Me.UserNameDB = lines(1).Split(New Char() {">"c})(1).Trim()
            Me.PasswordDB = lines(2).Split(New Char() {">"c})(1).Trim()
            Me.EmailDB = lines(3).Split(New Char() {">"c})(1).Trim()

            TextBox1.Text = UserNameDB
            TextBox2.Text = PasswordDB
            TextBox3.Text = EmailDB
            Console.WriteLine("USER DB:" &
                              vbCrLf &
                              "    " & UserNameDB & vbCrLf &
                              "    " & PasswordDB & vbCrLf &
                              "    " & EmailDB)
            RichTextBox1.ScrollToCaret()
        Catch ex As Exception
            Console.WriteLine("[Debugger@GetUserData]Error: " + ex.Message)
        End Try
    End Sub

    Public Sub SavePrimeraDB()
        If My.Computer.FileSystem.FileExists(Me.DB1) = True Then
            My.Computer.FileSystem.DeleteFile(Me.DB1)
        End If
        Try
            Dim FormatoDB As String = "#Worcome MyChat EX App Data Base" &
            vbCrLf & "CryptoKey>" & LlaveAppDB &
            vbCrLf & "License>" & LicenseDB &
            vbCrLf & "UserID>" & UserIDDB &
            vbCrLf & "IsRegistered>" & IsRegistered
            Dim EncryptedContent As String = Cifrar(FormatoDB, Me.LlaveApp)
            RichTextBox1.AppendText(vbCrLf & "<--- DB Raw Content --->")
            RichTextBox1.AppendText(vbCrLf & FormatoDB)
            RichTextBox1.AppendText(vbCrLf & "<--- DB Encrypted Content Saved --->")
            RichTextBox1.AppendText(vbCrLf & EncryptedContent)
            My.Computer.FileSystem.WriteAllText(Me.DB1, EncryptedContent, False)
            RichTextBox1.ScrollToCaret()
            GetPrimeraDB()
        Catch ex As Exception
            Console.WriteLine("[Debugger@SaveAppData]Error: " + ex.Message)
        End Try
    End Sub

    Public Sub SaveSegundaDB()
        If My.Computer.FileSystem.FileExists(Me.DB2) = True Then
            My.Computer.FileSystem.DeleteFile(Me.DB2)
        End If
        Try
            Dim FormatoDB As String = "#Worcome MyChat EX User Data Base" &
                vbCrLf & "UserName>" & UserNameDB &
                vbCrLf & "Password>" & PasswordDB &
                vbCrLf & "Email>" & EmailDB
            Dim EncryptedContent As String = Cifrar(FormatoDB, Me.LlaveAppDB)
            RichTextBox1.AppendText(vbCrLf & "<--- DB Raw Content --->")
            RichTextBox1.AppendText(vbCrLf & FormatoDB)
            RichTextBox1.AppendText(vbCrLf & "<--- DB Encrypted Content Saved --->")
            RichTextBox1.AppendText(vbCrLf & EncryptedContent)
            My.Computer.FileSystem.WriteAllText(Me.DB2, EncryptedContent, False)
            RichTextBox1.ScrollToCaret()
            GetSegundaDB()
        Catch ex As Exception
            Console.WriteLine("[Debugger@SaveUserData]Error: " + ex.Message)
        End Try
    End Sub

    Public Function Cifrar(ByVal TextIn As String, ByVal llave As String) As String
        Dim result As String
        If TextIn = "" Then
            result = ""
        Else
            tripleDESCryptoServiceProvider_0.Key = md5CryptoServiceProvider_0.ComputeHash(New UnicodeEncoding().GetBytes(llave))
            tripleDESCryptoServiceProvider_0.Mode = CipherMode.ECB
            Dim cryptoTransform As ICryptoTransform = tripleDESCryptoServiceProvider_0.CreateEncryptor()
            Dim bytes As Byte() = Encoding.ASCII.GetBytes(TextIn)
            result = Convert.ToBase64String(cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length))
        End If
        Return result
    End Function
    Public Function Descifrar(ByVal TextIn As String, ByVal llave As String) As String
        Dim result As String
        If TextIn = "" Then
            result = ""
        Else
            tripleDESCryptoServiceProvider_0.Key = md5CryptoServiceProvider_0.ComputeHash(New UnicodeEncoding().GetBytes(llave))
            tripleDESCryptoServiceProvider_0.Mode = CipherMode.ECB
            Dim cryptoTransform As ICryptoTransform = tripleDESCryptoServiceProvider_0.CreateDecryptor()
            Dim array As Byte() = Convert.FromBase64String(TextIn)
            result = Encoding.ASCII.GetString(cryptoTransform.TransformFinalBlock(array, 0, array.Length))
        End If
        Return result
    End Function

    Sub DesbloquearDirectorio()
        Try
            Dim accessControl As FileSystemSecurity = File.GetAccessControl(Me.Directorio1)
            Dim identity As SecurityIdentifier = New SecurityIdentifier(WellKnownSidType.WorldSid, Nothing)
            accessControl.RemoveAccessRule(New FileSystemAccessRule(identity, FileSystemRights.FullControl, AccessControlType.Deny))
            File.SetAccessControl(Me.Directorio1, CType(accessControl, FileSecurity))
            RichTextBox1.AppendText(vbCrLf & "<--- Directorio Desbloqueado --->")
        Catch ex2 As Exception
            Console.WriteLine(ex2.Message)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        RichTextBox1.AppendText(vbCrLf & Descifrar(RichTextBox2.Text, InputBox("Ingrese una llave criptografica", "Llave criptografica", LlaveApp & " " & LlaveAppDB)))
    End Sub

    Dim NombreServidor, IDDueño, FechaCreado, Expira, Verificado, Contraseña, SoloLicenciados, Cifrado As String
    Dim ServerName, RoomName, Username, Password, Host, OwnerID As String
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim CodigoDeAcceso = InputBox("Ingrese su codigo de acceso del servidor propio", "Codigo de Acceso")
        'Solo para servidores propios (no de Worcome)

        If CodigoDeAcceso = Nothing Then
        Else
            'Obtener las credenciales del servidor para poder subir luego el archivo modificado

            Dim RAWContent As String = CodigoDeAcceso
            Dim DecryptContent As String = Descifrar(RAWContent, LlaveApp)
            RichTextBox1.AppendText(vbCrLf & "<--- Access Key Raw Content --->")
            RichTextBox1.AppendText(vbCrLf & RAWContent)
            RichTextBox1.AppendText(vbCrLf & "<--- Access Key Decrypted Content Loaded --->")
            RichTextBox1.AppendText(vbCrLf & DecryptContent)

            Dim array As String() = DecryptContent.Split(New Char() {">"c})
            ServerName = array(0)
            RoomName = array(1)
            Username = array(2)
            Password = array(3)
            Host = array(4)
            OwnerID = array(5)
            InputBox("El codigo de acceso contiene los siguientes elementos", "Codigo de Acceso", DecryptContent)
            ArchivoDeConfiguracionDelServidor = Directorio1 & "\MCEXC_" & ServerName & ".inf"
            'Obtener el archivo de configuracion del servidor para modificarlo
            If My.Computer.FileSystem.FileExists(ArchivoDeConfiguracionDelServidor) Then
                My.Computer.FileSystem.DeleteFile(ArchivoDeConfiguracionDelServidor)
            End If
            My.Computer.Network.DownloadFile(Host & "/" & ServerName & ".inf", ArchivoDeConfiguracionDelServidor, Username, Password)

            'Leer y modificar el archivo de configuracion del servidor
            Dim RAWContent2 As String = My.Computer.FileSystem.ReadAllText(ArchivoDeConfiguracionDelServidor)
            Dim DecryptContent2 As String = Descifrar(RAWContent2, LlaveApp)
            RichTextBox1.AppendText(vbCrLf & "<--- Server Config File Raw Content --->")
            RichTextBox1.AppendText(vbCrLf & RAWContent2)
            RichTextBox1.AppendText(vbCrLf & "<--- Server Config File Decrypted Content Loaded --->")
            RichTextBox1.AppendText(vbCrLf & DecryptContent2)

            Dim textBox As TextBox = New TextBox()
            textBox.Text = DecryptContent2

            Dim lines As String() = textBox.Lines
            NombreServidor = lines(1).Split(New Char() {">"c})(1).Trim()
            IDDueño = lines(2).Split(New Char() {">"c})(1).Trim()
            FechaCreado = lines(3).Split(New Char() {">"c})(1).Trim()
            Expira = lines(4).Split(New Char() {">"c})(1).Trim()
            Verificado = lines(5).Split(New Char() {">"c})(1).Trim()
            Contraseña = lines(6).Split(New Char() {">"c})(1).Trim()
            SoloLicenciados = lines(7).Split(New Char() {">"c})(1).Trim()

            TextBox8.Text = NombreServidor
            TextBox9.Text = IDDueño
            TextBox10.Text = FechaCreado
            TextBox11.Text = Expira
            TextBox12.Text = Verificado
            textBox13.Text = Contraseña
            TextBox14.Text = SoloLicenciados

            Try
                Cifrado = lines(8).Split(New Char() {">"c})(1).Trim()
                Dim array5 As String() = Cifrado.Split(New Char() {":"c})
                If array5(0) = "True" Then
                    TextBox15.Text = array5(0) & ":" & array5(1)
                Else
                    TextBox15.Text = "La sala no tiene cifrado"
                    TextBox15.Enabled = False
                End If
            Catch
            End Try
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If TextBox15.Text = "La sala no tiene cifrado" Then
            Cifrado = "False:None"
        End If

        NombreServidor = TextBox8.Text
        IDDueño = TextBox9.Text
        FechaCreado = TextBox10.Text
        Expira = TextBox11.Text
        Verificado = TextBox12.Text
        Contraseña = TextBox13.Text
        SoloLicenciados = TextBox14.Text

        Dim RAWContent As String = "#Worcome Server Services | MyChat EX - Global Party Information" &
            vbCrLf & "Server>" & NombreServidor &
            vbCrLf & "OwnerID>" & IDDueño &
            vbCrLf & "Created>" & FechaCreado &
            vbCrLf & "Expire>" & Expira &
            vbCrLf & "Verified>" & Verificado &
            vbCrLf & "Password>" & Contraseña &
            vbCrLf & "OnlyLicencedUsers>" & SoloLicenciados &
            vbCrLf & "Cipher>" & Cifrado
        Dim EncryptContent As String = Cifrar(RAWContent, LlaveApp)

        'Escribimos los datos modificados en el archivo
        My.Computer.FileSystem.WriteAllText(ArchivoDeConfiguracionDelServidor, EncryptContent, False)

        'Subimos el nuevo archivo al servidor
        My.Computer.Network.UploadFile(ArchivoDeConfiguracionDelServidor, Host & "/" & ServerName & ".inf", Username, Password)
    End Sub
End Class