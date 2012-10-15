Namespace Pinger

    Public Class HttpPinger : Implements IPinger

        Public Function PingUrl(ByVal url As Uri, ByVal pingTimeout As Integer, ByVal retryInterval As Integer, ByVal maxRetries As Integer) As Boolean Implements IPinger.PingUrl
            Dim ret As Boolean
            Dim tryCount As Integer

            Do While Not ret AndAlso tryCount < maxRetries
                Try
                    Dim request As Net.HttpWebRequest = DirectCast(Net.HttpWebRequest.CreateDefault(url), Net.HttpWebRequest)
                    request.AllowAutoRedirect = True
                    request.Timeout = pingTimeout
                    request.Method = "HEAD"

                    Dim response As Net.HttpWebResponse = DirectCast(request.GetResponse, Net.HttpWebResponse)

                    ret = (response.StatusCode = Net.HttpStatusCode.OK)

                    response.Close()

                Catch ex As Exception
                    ret = False
                Finally
                    tryCount += 1

                    If Not ret Then
                        System.Threading.Thread.Sleep(retryInterval)
                    End If
                End Try
            Loop

            Return ret
        End Function

    End Class

End Namespace
