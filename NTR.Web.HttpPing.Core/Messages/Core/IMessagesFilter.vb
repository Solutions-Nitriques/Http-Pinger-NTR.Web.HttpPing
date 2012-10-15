Imports NTR.Web.HttpPing.Messages
Imports NTR.Web.HttpPing.Workers
Imports NTR.Web.HttpPing.Config

Public Interface IMessagesFilter

    Function FilterProcessStatusMessage(ByVal messageType As ProcessMessageType) As Boolean

    Function FilterWorkStatusMessage(ByVal workResult As IWorkResult, ByVal lastWorkResult As IWorkResult, ByVal config As IConfigModel) As Boolean

    Function FilterUrlStatusMessage(ByVal urlResult As IPingUrlResult) As Boolean

End Interface
